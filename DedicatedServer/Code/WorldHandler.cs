using System.Diagnostics;
using DedicatedServer.Code.Misc;
using DedicatedServer.Code.Networking;
using Shared;
using Shared.Code.Block_System;
using Shared.Code.Packets;
namespace DedicatedServer.Code;

internal sealed class WorldHandler {
    private readonly FrameCounter _frameCounter = new();
    private readonly object _locker = new();

    private readonly Server _server;
    private readonly Stopwatch _stopwatch;
    private readonly TimeSpan _targetTime = TimeSpan.FromMilliseconds(16);

    private readonly WorldState _worldState;

    private int _continueRun = 1;

#if DEBUG
    private long _currentTicks;
    private long _previousTicks;
#endif


    public WorldHandler() {
        BlockManagerShared.Initialize();
        _worldState = new WorldState();

        WorldDownload testDownload = WorldDownload.Default();
        testDownload.Process(_worldState);

        _server = new Server(_worldState);
        _stopwatch = new Stopwatch();
    }

    public void Run() {
        lock (_locker) {
            _server.StartServer(9050);
        }
        _stopwatch.Start();
        Loop();
    }

    private void Loop() {
        var timer = new Timer(Tick, _frameCounter, TimeSpan.Zero, _targetTime);

        while (Interlocked.Exchange(ref _continueRun, 1) == 1) {
            Thread.Sleep(1000);
        }
    }

    private void Tick(object? state) {
        lock (_locker) {
            #if DEBUG
            _currentTicks = _stopwatch.ElapsedTicks;
            long test = _currentTicks - _previousTicks;
            Console.WriteLine("CurrentMilliseconds = " + test / 1000000);
            Console.WriteLine("Current Tick = " + _server.LocalTickBuffer.CurrTick);
            _previousTicks = _currentTicks;
            #endif
            _server.Update();
        }
    }

    public void Stop() {
        Interlocked.Exchange(ref _continueRun, 0);
        lock (_locker) {
            _server.Stop();
        }
    }
}