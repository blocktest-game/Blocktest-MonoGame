using Shared.Code.Packets;
namespace Shared.Code.Networking;

public sealed class TickBuffer {
    private readonly Tick[] _tickBuffer = new Tick[GlobalsShared.MaxTicksStored];
    private int _currentDistance;
    private ushort _currentRecent;
    public ushort CurrTick;

    public TickBuffer(ushort newTick, WorldState worldState) {
        CurrTick = newTick;
        Array.Fill(_tickBuffer, new Tick(worldState.Foreground, worldState.Background));
        _currentDistance = 0;
        _currentRecent = CurrTick;
    }

    /// <summary>
    ///     Add additional tick
    /// </summary>
    public void IncrCurrTick(WorldState worldState) {
        //tickBuffer[currTick].ProcessStartTick();
        ProcessTicks(_currentRecent, worldState);
        Tick newTick = new(worldState.Foreground, worldState.Foreground);
        CurrTick++;
        if (CurrTick == GlobalsShared.MaxTicksStored) {
            CurrTick = 0;
        }
        _tickBuffer[CurrTick] = newTick;
        _currentDistance = 0;
        _currentRecent = CurrTick;
    }

    private bool CheckFurthestTick(ushort newTickNum) {
        int newTickDistance;
        if (newTickNum > CurrTick) {
            newTickDistance = GlobalsShared.MaxTicksStored - newTickNum + CurrTick;
        } else {
            newTickDistance = CurrTick - newTickNum;
        }
        if (newTickDistance <= _currentDistance) {
            return false;
        }
        _currentDistance = newTickDistance;
        _currentRecent = newTickNum;
        return true;
    }

    public void ProcessTicks(ushort startTick, WorldState worldState) {
        Tick tick = _tickBuffer[startTick];
        tick.ProcessStartTick(worldState);
        for (int i = startTick + 1; i != CurrTick + 1; i++) {
            if (i == GlobalsShared.MaxTicksStored) {
                i = 0;
            }
            tick = _tickBuffer[i];
            tick.ProcessTick(worldState);
        }
    }

    public void AddPacket(IPacket newPacket) {
        CheckFurthestTick(newPacket.TickNum);
        ushort tickNum = newPacket.TickNum;
        _tickBuffer[tickNum].Packets.Add(newPacket);
    }
}