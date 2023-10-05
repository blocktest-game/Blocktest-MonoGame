using System;
using System.Diagnostics;
using System.Threading;
using Shared;
using Shared.Networking;
using Blocktest;
using Blocktest.Networking;

namespace Blocktest
{
    class WorldHandler
    {
        private readonly object locker = new();
        private int counter = 0;
        private Stopwatch stopwatch;
        private TimeSpan targetTime = TimeSpan.FromMilliseconds(16);
        private TimeSpan currentTime = TimeSpan.Zero;
        private FrameCounter _frameCounter = new FrameCounter();

        private Server server;

        private int continueRun = 1;
        private long previousTicks = 0;
        private long currentTicks = 0;
        private bool continueWait = true;

        public WorldHandler()
        {
            BlockManagerShared.Initialize();
            GlobalsShared.BackgroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);
            GlobalsShared.ForegroundTilemap = new TilemapShared(GlobalsShared.maxX, GlobalsShared.maxY);

            WorldDownload testDownload = new();
            int[,,] newWorld = new int[GlobalsShared.maxX, GlobalsShared.maxY, 2];
            for (int i = 0; i < GlobalsShared.maxX; i++) {
                newWorld[i, 59, 1] = 4;
                newWorld[i, 58, 1] = 2;
                newWorld[i, 57, 1] = 2;
                newWorld[i, 56, 1] = 2;
                newWorld[i, 55, 1] = 2;
                newWorld[i, 54, 1] = 3;
            }
            testDownload.world = newWorld;
            testDownload.tickNum = 1;
            testDownload.Process();

            server = new();
            stopwatch = new();
            
        }

        public void Run()
        {
            lock (locker) {
                server.Start();
            }
            stopwatch.Start();
            Loop();
        }

        protected void Loop()
        {
            Timer timer = new(Tick, _frameCounter, TimeSpan.Zero, targetTime);
            //System.Threading.Timer timer = new(Tick, _frameCounter, 16, 16);
            while(Interlocked.Exchange(ref continueRun, 1) == 1)
            {
                //Tick();
                //WaitHandler();
                Thread.Sleep(1000);
            }
        }

        protected void Tick(Object? state)
        {
            lock(locker)
            {
                /*currentTicks = stopwatch.ElapsedTicks;
                long test = currentTicks - previousTicks;
                Console.WriteLine("CurrentMilliseconds = " + test / 1000000);
                Console.WriteLine("Current Tick = " + GlobalsServer.serverTickBuffer.currTick);
                previousTicks = currentTicks;
                counter++;*/
                server.Update();
                server.serverTickBuffer.IncrCurrTick();
            }
        }

        public void Stop()
        {
            Interlocked.Exchange(ref continueRun, 0);
        }
    }
}