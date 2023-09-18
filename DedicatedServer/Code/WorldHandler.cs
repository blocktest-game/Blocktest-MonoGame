using System;
using System.Diagnostics;
using System.Threading;
using Blocktest.Networking;

namespace Blocktest
{
    class WorldHandler
    {
        private readonly object locker = new();
        private int counter = 0;
        private Stopwatch stopwatch;
        private TimeSpan targetTime = TimeSpan.FromTicks(166667);
        private TimeSpan currentTime = TimeSpan.Zero;
        private FrameCounter _frameCounter = new FrameCounter();

        private Server server;

        private int continueRun = 1;
        private long previousTicks = 0;
        private long currentTicks = 0;
        private bool continueWait = true;

        public WorldHandler()
        {
            server = new();
            stopwatch = new();
        }

        public void Run()
        {
            server.Start();
            stopwatch.Start();
            Loop();
        }

        protected void Loop()
        {
            System.Threading.Timer timer = new(Tick, _frameCounter, TimeSpan.Zero, targetTime);
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
                GlobalsServer.serverTickBuffer.IncrCurrTick();
            }
        }

        public void Stop()
        {
            Interlocked.Exchange(ref continueRun, 0);
        }
    }
}