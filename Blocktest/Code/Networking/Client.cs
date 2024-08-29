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

public sealed class Client : NetworkInterface {
    private readonly Camera _camera;

    private readonly BlocktestGame _game;
    private readonly Dictionary<int, Renderable> _playerRenderables = new();
    private bool _initialized;
    public bool WorldDownloaded;

    public Client(WorldState worldState, Camera camera, BlocktestGame game) : base(worldState) {
        _camera = camera;
        _game = game;

        Listener.PeerConnectedEvent += PeerConnected;
        Listener.PeerDisconnectedEvent += PeerDisconnected;
    }

    private void PeerConnected(NetPeer peer) {
        Console.WriteLine($"Connected to server {peer.Address}:{peer.Port} as {peer.RemoteId}");

        Transform newTransform = new(new Vector2Int(256, 128));
        Renderable newRenderable = new(newTransform, Layer.Player, Drawable.PlaceholderDrawable);
        LocalWorldState.PlayerPositions.Add(peer.RemoteId, newTransform);
        _playerRenderables.Add(peer.RemoteId, newRenderable);
        _camera.RenderedComponents.Add(newRenderable);
    }

    private void PeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
        Console.WriteLine("Disconnected from server");

        _game.SetScene(new MainMenuScene(_game,
            new DialogueWindow("Disconnected from server.", $"{disconnectInfo.Reason}")));
    }

    protected override void HandlePackets(NetDataReader packetReader, int sourceId, PacketType packetType,
                                          ushort tickNum, NetPeer peer) {
        switch (packetType) {
            case PacketType.TileChange:
                LocalTickBuffer.AddPacket(HandlePacket<TileChange>(packetReader, sourceId, tickNum));
                break;
            case PacketType.BreakTile:
                LocalTickBuffer.AddPacket(HandlePacket<BreakTile>(packetReader, sourceId, tickNum));
                break;
            case PacketType.MovePlayer:
                LocalTickBuffer.AddPacket(HandlePacket<MovePlayer>(packetReader, sourceId, tickNum));
                break;
            case PacketType.PlayerList:
                LocalTickBuffer.AddPacket(HandlePacket<PlayerList>(packetReader, sourceId, tickNum));
                break;
            case PacketType.PeerEvent:
                HandleEvent(HandlePacket<PeerEvent>(packetReader, sourceId, tickNum));
                break;
            case PacketType.WorldDownload:
                if (_initialized) {
                    Console.WriteLine("Bad packet!!!");
                    break;
                }

                WorldDownload worldPacket = HandlePacket<WorldDownload>(packetReader, sourceId, tickNum);
                worldPacket.Process(LocalWorldState);
                LocalTickBuffer = new TickBuffer(worldPacket.TickNum, LocalWorldState);

                SendPacket(new PeerEvent {
                    SourceId = Server.RemoteId,
                    TickNum = worldPacket.TickNum,
                    Type = PeerEvent.PeerEventType.PlayerList
                }, Server);

                _initialized = true;

                if (!WorldDownloaded)
                {
                    WorldDownloaded = true; // nyehhh i dont know what i'm doing with this
                }
                break;
            default:
                Console.WriteLine("Bad packet!!!");
                break;
        }
    }

    protected override void HandleEvent(PeerEvent peerEvent) {
        switch (peerEvent.Type) {
            case PeerEvent.PeerEventType.PeerDisconnect:
                Console.WriteLine("Player disconnected");

                LocalWorldState.PlayerPositions.Remove(peerEvent.SourceId);
                _camera.RenderedComponents.Remove(_playerRenderables[peerEvent.SourceId]);
                _playerRenderables.Remove(peerEvent.SourceId);
                break;
            case PeerEvent.PeerEventType.PeerConnect:
                Console.WriteLine("New player connected");

                Transform newTransform = new(new Vector2Int(256, 128));
                Renderable newRenderable = new(newTransform, Layer.Player, Drawable.PlaceholderDrawable);
                LocalWorldState.PlayerPositions.Add(peerEvent.SourceId, newTransform);
                _playerRenderables.Add(peerEvent.SourceId, newRenderable);
                _camera.RenderedComponents.Add(newRenderable);
                break;
            case PeerEvent.PeerEventType.PlayerList:
            default:
                throw new ArgumentOutOfRangeException(nameof(peerEvent), peerEvent, null);
        }
    }
}