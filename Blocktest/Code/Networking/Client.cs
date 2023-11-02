using System.Net;
using Blocktest.Rendering;
using Blocktest.Scenes;
using Blocktest.UI;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Code;
using Shared.Code.Components;
using Shared.Code.Networking;
using Shared.Code.Packets;
namespace Blocktest.Networking;

public sealed class Client {
    private readonly Camera _camera;
    private readonly EventBasedNetListener _listener;
    private readonly NetManager _manager;
    private readonly Dictionary<int, Renderable> _playerRenderables = new();

    private readonly WorldState _worldState;
    public TickBuffer ClientTickBuffer;

    private readonly BlocktestGame _game;

    public Client(WorldState worldState, Camera camera, BlocktestGame game) {
        _worldState = worldState;
        _camera = camera;
        _game = game;
        ClientTickBuffer = new TickBuffer(0, _worldState);
        _listener = new EventBasedNetListener();
        _manager = new NetManager(_listener);
        _listener.NetworkReceiveEvent += NetworkReceiveEvent;
        _listener.PeerConnectedEvent += PeerConnected;
        _listener.PeerDisconnectedEvent += PeerDisconnected;
        _manager.Start();
    }

    public NetPeer? Server { get; private set; }

    public void Start(IPEndPoint ipEndPoint, string key) {
        _manager.Connect(ipEndPoint, key);
    }

    public void Stop() {
        _manager.Stop();
    }

    public void Update() {
        _manager.PollEvents();
    }

    private void PeerConnected(NetPeer peer) {
        Console.WriteLine("Connected to server");

        Transform newTransform = new(new Vector2Int(256, 128));
        Renderable newRenderable = new(newTransform, Layer.Player, Drawable.ErrorDrawable, Color.Orange);
        _worldState.PlayerPositions.Add(peer.RemoteId, newTransform);
        _playerRenderables.Add(peer.RemoteId, newRenderable);
        _camera.RenderedComponents.Add(newRenderable);
    }

    private void PeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
        Console.WriteLine("Disconnected from server");

        _game.SetScene(new MainMenuScene(_game,
            new DialogueWindow("Disconnected from server.", $"{disconnectInfo.Reason}")));
    }

    /// <summary>
    ///     Recieve network events from LiteNetLib
    /// </summary>
    /// <param name="peer">The server the packet is coming from.</param>
    /// <param name="packetReader">Contains the packet from the server.</param>
    /// <param name="channelNumber"></param>
    /// <param name="deliveryMethod">The delivery method used to deliver this packet.</param>
    private void NetworkReceiveEvent(NetPeer peer, NetPacketReader packetReader, byte channelNumber,
                                     DeliveryMethod deliveryMethod) {
        if (Server == null) {
            Server = peer;

            PacketType packetType = (PacketType)packetReader.GetByte();
            if (packetType != PacketType.WorldDownload) {
                Console.WriteLine("Bad packet!!!");
                return;
            }

            int sourceId = packetReader.GetInt();

            WorldDownload worldPacket = HandlePacket<WorldDownload>(packetReader, sourceId);
            worldPacket.Process(_worldState);
            ClientTickBuffer = new TickBuffer(worldPacket.TickNum, _worldState);

            SendPacket(new PeerEvent {
                SourceId = Server.RemoteId,
                TickNum = worldPacket.TickNum,
                Type = PeerEvent.PeerEventType.PlayerList
            });
        } else {
            HandlePackets(packetReader);
        }
    }

    /// <summary>
    ///     Handles packets after the first.
    /// </summary>
    /// <param name="packetReader">Contains the packet sent by the server.</param>
    /// ACCEPTS PacketType:SourceID:TickNum:Packet
    private void HandlePackets(NetDataReader packetReader) {
        byte packetByte = packetReader.GetByte();
        PacketType packetType = (PacketType)packetByte;

        int sourceId = packetReader.GetInt();

        switch (packetType) {
            case PacketType.TileChange:
                ClientTickBuffer.AddPacket(HandlePacket<TileChange>(packetReader, sourceId));
                break;
            case PacketType.BreakTile:
                ClientTickBuffer.AddPacket(HandlePacket<BreakTile>(packetReader, sourceId));
                break;
            case PacketType.MovePlayer:
                ClientTickBuffer.AddPacket(HandlePacket<MovePlayer>(packetReader, sourceId));
                break;
            case PacketType.PlayerList:
                ClientTickBuffer.AddPacket(HandlePacket<PlayerList>(packetReader, sourceId));
                break;
            case PacketType.PeerEvent:
                PeerEvent eventPacket = new() { SourceId = sourceId, TickNum = packetReader.GetUShort() };
                eventPacket.Deserialize(packetReader);
                HandleEvent(eventPacket);
                break;
            case PacketType.WorldDownload:
            default:
                Console.WriteLine("Bad packet!!!");
                break;
        }
    }

    private T HandlePacket<T>(NetDataReader packetReader, int sourceId) where T : IPacket, new() {
        T packet = new() { SourceId = sourceId, TickNum = packetReader.GetUShort() };
        packet.Deserialize(packetReader);
        return packet;
    }

    private void HandleEvent(PeerEvent peerEvent) {
        switch (peerEvent.Type) {
            case PeerEvent.PeerEventType.PeerDisconnect:
                Console.WriteLine("Player disconnected");

                _worldState.PlayerPositions.Remove(peerEvent.SourceId);
                _camera.RenderedComponents.Remove(_playerRenderables[peerEvent.SourceId]);
                _playerRenderables.Remove(peerEvent.SourceId);
                break;
            case PeerEvent.PeerEventType.PeerConnect:
                Console.WriteLine("New player connected");

                Transform newTransform = new(new Vector2Int(256, 128));
                Renderable newRenderable = new(newTransform, Layer.Player, Drawable.ErrorDrawable, Color.Orange);
                _worldState.PlayerPositions.Add(peerEvent.SourceId, newTransform);
                _playerRenderables.Add(peerEvent.SourceId, newRenderable);
                _camera.RenderedComponents.Add(newRenderable);
                break;
            case PeerEvent.PeerEventType.PlayerList:
            default:
                throw new ArgumentOutOfRangeException(nameof(peerEvent), peerEvent, null);
        }
    }

    // SENDS PacketType:TickNum:Packet
    public void SendPacket(IPacket packet, DeliveryMethod deliveryMethod = DeliveryMethod.ReliableUnordered) {
        NetDataWriter writer = new();

        writer.Put((byte)packet.GetPacketType());
        writer.Put(packet.TickNum);
        //ID is not needed for client to server packets
        writer.Put(packet);
        Server?.Send(writer, deliveryMethod);
    }
}