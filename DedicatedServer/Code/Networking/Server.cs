using LiteNetLib;
using LiteNetLib.Utils;
using Shared;
using Shared.Code;
using Shared.Code.Components;
using Shared.Code.Networking;
using Shared.Code.Packets;
namespace DedicatedServer.Code.Networking;

public sealed class Server : NetworkInterface {
    private readonly ServerPlayerManager _playerManager;

    public Server(WorldState worldState) : base(worldState) {
        _playerManager = new ServerPlayerManager(GlobalsShared.MaxPlayers);

        Listener.ConnectionRequestEvent += NewConnection;
        Listener.PeerConnectedEvent += NewPeer;
        Listener.PeerDisconnectedEvent += PeerDisconnected;
    }

    private void NewConnection(ConnectionRequest request) {
        Console.WriteLine("New Connection");
        if (_playerManager.PlayerCount < GlobalsShared.MaxPlayers) {
            request.AcceptIfKey("testKey");
        } else {
            request.Reject();
        }
    }

    /// <summary>
    ///     Adds a new player to the playerManager and sends them the current world.
    /// </summary>
    /// <param name="peer">The new player</param>
    private void NewPeer(NetPeer peer) {
        Console.WriteLine("New Peer");
        _playerManager.AddPlayer(peer);
        LocalWorldState.PlayerPositions.Add(peer.Id, new Transform(Vector2Int.Zero));

        WorldDownload worldDownload = new() {
            World = LocalWorldState.CurrentWorld,
            TickNum = LocalTickBuffer.CurrTick,
            SourceId = -1
        };
        SendPacket(worldDownload, peer, DeliveryMethod.ReliableOrdered);

        SendPacketAllPeers(new PeerEvent {
            SourceId = peer.Id,
            TickNum = LocalTickBuffer.CurrTick,
            Type = PeerEvent.PeerEventType.PeerConnect
        }, peer);
    }

    /// <summary>
    ///     Removes a player from the playerManager.
    /// </summary>
    /// <param name="peer">The player to remove</param>
    /// <param name="disconnectInfo">The reason the player disconnected</param>
    private void PeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo) {
        Console.WriteLine($"Peer Disconnected: {disconnectInfo.Reason}");
        _playerManager.RemovePlayer(peer);
        LocalWorldState.PlayerPositions.Remove(peer.Id);

        SendPacketAllPeers(new PeerEvent {
            SourceId = peer.Id,
            TickNum = LocalTickBuffer.CurrTick,
            Type = PeerEvent.PeerEventType.PeerDisconnect
        }, peer);
    }

    protected override void HandlePackets(NetDataReader packetReader, int sourceId, PacketType packetType,
                                          ushort tickNum, NetPeer peer) {
        IPacket newPacket;
        switch (packetType) {
            case PacketType.TileChange:
                newPacket = HandlePacket<TileChange>(packetReader, sourceId, tickNum);
                break;
            case PacketType.BreakTile:
                newPacket = HandlePacket<BreakTile>(packetReader, sourceId, tickNum);
                break;
            case PacketType.MovePlayer:
                newPacket = HandlePacket<MovePlayer>(packetReader, sourceId, tickNum);
                break;
            case PacketType.PeerEvent:
                HandleEvent(HandlePacket<PeerEvent>(packetReader, sourceId, tickNum));
                return;
            case PacketType.WorldDownload:
            case PacketType.PlayerList:
            default:
                Console.WriteLine("Bad packet!!!");
                return;
        }
        LocalTickBuffer.AddPacket(newPacket);
        SendPacketAllPeers(newPacket, peer);
    }

    protected override void HandleEvent(PeerEvent peerEvent) {
        switch (peerEvent.Type) {
            case PeerEvent.PeerEventType.PlayerList:
                Console.WriteLine("Player List requested");
                NetPeer? peer = Manager.GetPeerById(peerEvent.SourceId);
                foreach (var player in _playerManager.PlayerList) {
                    if (player.Key == peerEvent.SourceId) {
                        continue;
                    }
                    SendPacket(
                        new PeerEvent {
                            SourceId = player.Key, Type = PeerEvent.PeerEventType.PeerConnect,
                            TickNum = LocalTickBuffer.CurrTick
                        }, peer);
                }

                //SendPacket(new PlayerList { PlayerIds = _playerManager.PlayerList.Keys.ToArray(), TickNum = ServerTickBuffer.CurrTick }, peer);
                break;
            case PeerEvent.PeerEventType.PeerDisconnect:
            case PeerEvent.PeerEventType.PeerConnect:
            default:
                throw new ArgumentOutOfRangeException(nameof(peerEvent), peerEvent, null);
        }
    }
}