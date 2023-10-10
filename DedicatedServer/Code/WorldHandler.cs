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

    private int _continueRun = 1;
    private bool _continueWait = true;
    private int _counter = 0;
    private long _currentTicks = 0;
    private TimeSpan _currentTime = TimeSpan.Zero;
    private long _previousTicks = 0;

    public WorldHandler() {
        BlockManagerShared.Initialize();
        GlobalsShared.BackgroundTilemap = new TilemapShared(GlobalsShared.MaxX, GlobalsShared.MaxY, true);
        GlobalsShared.ForegroundTilemap = new TilemapShared(GlobalsShared.MaxX, GlobalsShared.MaxY, false);
        
        var testDownload = WorldDownload.Default();
        testDownload.Process();

        _server = new Server();
        _stopwatch = new Stopwatch();
    }

    public void Run() {
        lock (_locker) {
            _server.Start();
        }
        _stopwatch.Start();
        Loop();
    }

    private void Loop() {
        Timer timer = new(Tick, _frameCounter, TimeSpan.Zero, _targetTime);
        //System.Threading.Timer timer = new(Tick, _frameCounter, 16, 16);
        while (Interlocked.Exchange(ref _continueRun, 1) == 1) {
            //Tick();
            //WaitHandler();
            Thread.Sleep(1000);
        }
    }

    private void Tick(object? state) {
        lock (_locker) {
            /*currentTicks = stopwatch.ElapsedTicks;
            long test = currentTicks - previousTicks;
            Console.WriteLine("CurrentMilliseconds = " + test / 1000000);
            Console.WriteLine("Current Tick = " + GlobalsServer.serverTickBuffer.currTick);
            previousTicks = currentTicks;
            counter++;*/
            _server.Update();
            _server.ServerTickBuffer.IncrCurrTick();
        }
    }

    public void Stop() {
        Interlocked.Exchange(ref _continueRun, 0);
    }
}