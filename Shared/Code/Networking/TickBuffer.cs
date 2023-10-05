using Shared.Code.Packets;
namespace Shared.Code.Networking;

public class TickBuffer {
    private readonly Tick[] _tickBuffer = new Tick[GlobalsShared.MaxTicksStored];
    private int _currentDistance;
    private ushort _currentRecent;
    public ushort CurrTick;

    public TickBuffer(ushort newTick) {
        CurrTick = newTick;
        for (int i = 0; i < GlobalsShared.MaxTicksStored; i++) {
            _tickBuffer[i] = new Tick(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
        }
        _currentDistance = 0;
        _currentRecent = CurrTick;
    }

    /// <summary>
    ///     Add additional tick
    /// </summary>
    public void IncrCurrTick() {
        //tickBuffer[currTick].ProcessStartTick();
        ProcessTicks(_currentRecent);
        Tick newTick = new(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
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
        if (newTickDistance > _currentDistance) {
            _currentDistance = newTickDistance;
            _currentRecent = newTickNum;
            return true;
        }
        return false;
    }

    public void ProcessTicks(ushort startTick) {
        Tick tick = _tickBuffer[startTick];
        tick.ProcessStartTick();
        for (int i = startTick + 1; i != CurrTick + 1; i++) {
            if (i == GlobalsShared.MaxTicksStored) {
                i = 0;
            }
            tick = _tickBuffer[i];
            tick.ProcessTick();
        }
    }

    public void AddPacket(IPacket newPacket) {
        CheckFurthestTick(newPacket.GetTickNum());
        ushort tickNum = newPacket.GetTickNum();
        _tickBuffer[tickNum] ??= new Tick(GlobalsShared.ForegroundTilemap, GlobalsShared.BackgroundTilemap);
        _tickBuffer[tickNum].Packets.Add(newPacket);
    }
}