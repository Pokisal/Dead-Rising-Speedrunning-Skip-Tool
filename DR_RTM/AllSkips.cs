using System;
using System.Diagnostics;
using System.Dynamic;
using System.Timers;
using ReadWriteMemory;

namespace DR_RTM
{

    public static class AllSkips
    {
        public static double TimerInterval = 16.666666666666668;

        public static Timer UpdateTimer = new Timer(TimerInterval);

        public static Process GameProcess;

        public static Form1 form;

        public static int skipMode = 0;

        private static ReadWriteMemory.ProcessMemory gameMemory;

        private static IntPtr gameTimePtr;

        private const int gameTimeOffset = 408;

        private static uint gameTime;

        private static uint campaignProgress;

        private static bool inCutsceneOrLoad;

        private static int loadingRoomId;

        private static byte caseMenuState;

        private static int cutsceneID;

        private static bool cletusDefeatFlag = false;

        private static string Version = "US";

        private static dynamic old = new ExpandoObject();

        public static void Init()
        {
        }

        public static void CheckVersion()
        {
            string FilePath = GameProcess.MainModule.FileName;
            long length = new System.IO.FileInfo(FilePath).Length;
            if (length == 28647632)
            {
                Version = "US";
            }
            if (length == 28648144)
            {
                Version = "JPN";
            }
        }

        public static string StringTime(uint time)
        {
            uint num = time / 108000u % 24u;
            uint num2 = time / 1800u % 60u;
            uint num3 = time / 30u % 60u;
            string text = "AM";
            if (num >= 12)
            {
                text = "PM";
                num %= 12u;
            }
            if (num == 0)
            {
                num = 12u;
            }
            return string.Format("{0}:{1}:{2} {3}", num.ToString("D2"), num2.ToString("D2"), num3.ToString("D2"), text);
        }

        public static void UpdateEvent(object source, ElapsedEventArgs e)
        {
            if (gameMemory != null && !gameMemory.CheckProcess())
            {
                gameMemory = null;
                UpdateTimer.Enabled = false;
                return;
            }
            if (gameMemory == null)
            {
                gameMemory = new ReadWriteMemory.ProcessMemory(GameProcess);
            }
            if (!gameMemory.IsProcessStarted())
            {
                gameMemory.StartProcess();
            }
            gameTimePtr = gameMemory.Pointer("DeadRising.exe", 26496472, 134592);
            if (gameTimePtr == IntPtr.Zero)
            {
                if (!form.IsDisposed)
                {
                    form.TimeDisplayUpdate("<missing>");
                }
                return;
            }
            if (Version == "US")
            {
                old.gameTime = gameTime;
                old.campaignProgress = campaignProgress;
                old.inCutsceneOrLoad = inCutsceneOrLoad;
                old.loadingRoomId = loadingRoomId;
                old.caseMenuState = caseMenuState;
                old.cutsceneID = cutsceneID;
                gameTime = gameMemory.ReadUInt(IntPtr.Add(gameTimePtr, 408));
                campaignProgress = gameMemory.ReadUInt(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26496472, 134592), 336));
                inCutsceneOrLoad = (gameMemory.ReadByte(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26500976), 112)) & 1) == 1;
                loadingRoomId = gameMemory.ReadInt(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26500976), 72));
                caseMenuState = gameMemory.ReadByte(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26505152, 192600), 386));
                cutsceneID = gameMemory.ReadInt(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26496472, 134592), 33544));
            }
            else if (Version == "JPN")
            {
                old.gameTime = gameTime;
                old.campaignProgress = campaignProgress;
                old.inCutsceneOrLoad = inCutsceneOrLoad;
                old.loadingRoomId = loadingRoomId;
                old.caseMenuState = caseMenuState;
                gameTime = gameMemory.ReadUInt(IntPtr.Add(gameTimePtr, 408));
                campaignProgress = gameMemory.ReadUInt(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26496472, 134592), 336));
                inCutsceneOrLoad = (gameMemory.ReadByte(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26496472, 134592), -2120912)) & 1) == 1;
                loadingRoomId = gameMemory.ReadInt(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26496472, 134592), -2120952));
                caseMenuState = gameMemory.ReadByte(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26496472, 134592), 386));
                cutsceneID = gameMemory.ReadInt(IntPtr.Add(gameMemory.Pointer("DeadRising.exe", 26496472, 134592), 33544));
            }
            form.TimeDisplayUpdate(StringTime(gameTime));
            if (skipMode == 0)
            {
                if ((old.caseMenuState == 2 || old.caseMenuState == 19) && caseMenuState == 0)
                {
                    if (campaignProgress == 140)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5832000u);
                    }
                    if (campaignProgress == 215)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6372000u);
                    }
                    if (campaignProgress == 220)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6804000u);
                    }
                    if (campaignProgress == 250)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7776000u);
                    }
                    if (campaignProgress == 290)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8100000u);
                    }
                    if (campaignProgress == 300)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8964000u);
                    }
                    if (campaignProgress == 340)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 9612000u);
                    }
                    if (campaignProgress == 390)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10152000u);
                    }
                }
                if (old.campaignProgress == 230 && campaignProgress == 240 && cutsceneID == 26)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7128000u);
                }
                if (old.campaignProgress == 240 && campaignProgress == 250 && cutsceneID == 27)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7452000u);
                }
                if (campaignProgress == 400 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10260000u);
                }
                if (campaignProgress == 402 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), gameTime + 18000 + 1);
                }
                if (campaignProgress == 404 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10368001u);
                }
                if (campaignProgress == 406 || campaignProgress == 410 || campaignProgress == 415)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), gameMemory.ReadUInt(IntPtr.Add(gameTimePtr, 408)) + 1);
                }
                if (old.campaignProgress != 415 && campaignProgress == 415)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 11448000u);
                }
                if (campaignProgress == 420 && loadingRoomId == 288 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 11664501u);
                }
            }
            else if (skipMode == 1)
            {
                if ((old.caseMenuState == 2 || old.caseMenuState == 19) && caseMenuState == 0 && campaignProgress == 80 && gameTime < 4104000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4104000u);
                }
                if (campaignProgress == 80 && loadingRoomId == 512 && gameTime < 4536000 && cutsceneID == 112)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4536000u);
                }
                if (campaignProgress == 80 && loadingRoomId == 1792 && gameTime < 6048000 && cutsceneID == 63)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6048000u);
                }
                if (campaignProgress == 80 && loadingRoomId == 768 && gameTime < 6480000 && cutsceneID == 99)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6480000u);
                }
                if (campaignProgress == 80 && loadingRoomId == 512 && gameTime < 6804000 && cutsceneID == 117)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6804000u);
                }
                if (campaignProgress == 80 && loadingRoomId == 768 && gameTime < 7236000 && cutsceneID == 75)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7236000u);
                }
                if (campaignProgress == 1100 && loadingRoomId == 256 && gameTime < 7776000 && cutsceneID == 64)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7776000u);
                }
                if (campaignProgress == 1100 && loadingRoomId == 1283 && gameTime < 8316000 && cutsceneID == 73)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8316000u);
                }
                if (campaignProgress == 1100 && loadingRoomId == 512 && gameTime < 9072000 && !inCutsceneOrLoad && (cutsceneID == 0 || cutsceneID == 70 || cutsceneID == 76 || cutsceneID == 113))
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 9072000u);
                }
            }
            else if (skipMode == 2)
            {
                if ((old.caseMenuState == 2 || old.caseMenuState == 19) && caseMenuState == 0)
                {
                    if (campaignProgress == 80 && gameTime < 4104000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4104000u);
                    }
                    if ((campaignProgress == 140 || campaignProgress == 150) && gameTime < 5832000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5832000u);
                    }
                    if (campaignProgress == 180 && gameTime < 6048000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6048000u);
                    }
                    if (campaignProgress == 215 && gameTime < 6372000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6372000u);
                    }
                    if (campaignProgress == 220 && gameTime < 6480000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6480000u);
                    }
                    if (campaignProgress == 230 && gameTime < 7020000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7020000u);
                    }
                    if (campaignProgress == 250 && gameTime < 7776000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7344000u);
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7776000u);
                    }
                    if (campaignProgress == 290)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8100000u);
                    }
                    if (campaignProgress == 300)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8532000u);
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8964000u);
                    }
                    if (campaignProgress == 340)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 9612000u);
                    }
                    if (campaignProgress == 390)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10152000u);
                    }
                }
                if (campaignProgress == 215 && loadingRoomId == 768 && gameTime < 6270000 && cutsceneID == 99 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6270000u);
                }
                if (campaignProgress == 220 && loadingRoomId == 768 && gameTime > 6480000 && gameTime < 6804000 && cutsceneID == 75)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6804000u);
                }
                if (old.campaignProgress == 240 && campaignProgress == 250 && cutsceneID == 27)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7452000u);
                }
                if (campaignProgress == 250 && loadingRoomId == 1024 && gameTime > 7128000 && gameTime < 7776000 && cutsceneID == 72)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7776000u);
                }
                if (campaignProgress == 320 && loadingRoomId == 512 && gameTime > 8964000 && gameTime < 9072000 && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 9072000u);
                }
                if (campaignProgress == 400 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10260000u);
                }
                if (campaignProgress == 402 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), gameTime + 18000 + 1);
                }
                if (campaignProgress == 404 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10368001u);
                }
                if (campaignProgress == 406 || campaignProgress == 410 || campaignProgress == 415)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), gameMemory.ReadUInt(IntPtr.Add(gameTimePtr, 408)) + 1);
                }
                if (old.campaignProgress != 415 && campaignProgress == 415)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 11448000u);
                }
                if (campaignProgress == 420 && loadingRoomId == 288 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 11664501u);
                }
            }
            else if (skipMode == 3)
            {
                if ((old.caseMenuState == 2 || old.caseMenuState == 19) && caseMenuState == 0)
                {
                    if (campaignProgress == 80 && Form1.skipFlag)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4104000u);
                    }
                    if (campaignProgress == 110 && Form1.skipFlag)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4428000u);
                    }
                    if (campaignProgress == 130 && gameTime < 4392000 && !Form1.skipFlag)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4392000u);
                    }
                    if ((campaignProgress == 140 || campaignProgress == 150) && gameTime < 5832000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5832000u);
                    }
                    if (campaignProgress == 180 && gameTime < 5940000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5940000u);
                    }
                    if (campaignProgress == 215 && gameTime < 6372000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6372000u);
                    }
                    if (campaignProgress == 250 && gameTime < 7776000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7776000u);
                    }
                }
                if (campaignProgress == 130 && inCutsceneOrLoad && cutsceneID == 7 && gameTime < 5076000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5076000u);
                }
                if (campaignProgress == 210 && gameTime < 6048000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6048000u);
                }
                if (campaignProgress == 220 && loadingRoomId == 512 && old.loadingRoomId == 256 && gameTime < 6480000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6480000u);
                }
                if (campaignProgress == 220 && loadingRoomId == 288 && old.inCutsceneOrLoad && !inCutsceneOrLoad && gameTime > 6480000 && gameTime < 6804000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6804000u);
                }
                if (campaignProgress == 230 && loadingRoomId == 288 && gameTime > 6804000 && gameTime < 7020000 && cutsceneID == 75)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7020000u);
                }
                if (campaignProgress == 240 && cutsceneID == 26 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7128000u);
                }
                if (campaignProgress == 250 && cutsceneID == 27 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7452000u);
                }
                if (campaignProgress == 290 && loadingRoomId == 288 && gameTime > 7776000 && gameTime < 8100000 && cutsceneID != 33)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8100000u);
                }
                if (campaignProgress == 280 && loadingRoomId == 288 && gameTime > 7776000 && gameTime < 8100000 && cutsceneID == 33)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8100000u);
                }
                if (campaignProgress == 300 && loadingRoomId == 288 && gameTime > 8200000 && gameTime < 8640000 && cutsceneID != 33)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8640000u);
                }
                if (loadingRoomId == 512 && gameTime > 8640000 && gameTime < 9072000 && !inCutsceneOrLoad && (cutsceneID == 70 || cutsceneID == 76 || cutsceneID == 113))
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 9072000u);
                }
                if (cutsceneID == 70 && loadingRoomId == 287 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 11664501u);
                }
            }
            else if (skipMode == 4)
            {
                if ((old.caseMenuState == 2 || old.caseMenuState == 19) && caseMenuState == 0)
                {
                    if (campaignProgress == 80 && Form1.skipFlag)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4104000u);
                    }
                    if (campaignProgress == 110 && Form1.skipFlag)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4428000u);
                    }
                    if (campaignProgress == 130 && gameTime < 4392000 && !Form1.skipFlag)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 4392000u);
                    }
                    if (campaignProgress == 140 || campaignProgress == 150)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5832000u);
                    }
                    if (campaignProgress == 180 && gameTime < 5940000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5940000u);
                    }
                    if (campaignProgress == 215 && gameTime < 6372000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6372000u);
                    }
                    if (campaignProgress == 250 && cletusDefeatFlag)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7776000u);
                    }
                    if (campaignProgress == 340)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 9612000u);
                    }
                    if (campaignProgress == 360)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10044000u);
                    }
                    if (campaignProgress == 390 && gameTime < 10152000)
                    {
                        gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10152000u);
                    }
                }
                if (cutsceneID == 8)
                {
                    cletusDefeatFlag = false;
                }
                if (cutsceneID == 72)
                {
                    cletusDefeatFlag = true;
                }
                if (campaignProgress == 130 && inCutsceneOrLoad && cutsceneID == 7 && gameTime < 5076000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 5076000u);
                }
                if (campaignProgress == 210 && gameTime < 6048000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6048000u);
                }
                if (campaignProgress == 220 && loadingRoomId == 512 && old.loadingRoomId == 256 && gameTime < 6480000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6480000u);
                }
                if (campaignProgress == 220 && loadingRoomId == 288 && old.inCutsceneOrLoad && !inCutsceneOrLoad && gameTime > 6480000 && gameTime < 6804000)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 6804000u);
                }
                if (campaignProgress == 230 && loadingRoomId == 288 && gameTime > 6804000 && gameTime < 7020000 && cutsceneID == 75)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7020000u);
                }
                if (campaignProgress == 230 && loadingRoomId == 256 && gameTime > 7020000 && gameTime < 7236000 && cutsceneID == 64)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7236000u);
                }
                if (campaignProgress == 250 && loadingRoomId == 1024 && gameTime > 7128000 && gameTime < 7776000 && cutsceneID == 72)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 7776000u);
                }
                if (campaignProgress == 290 && loadingRoomId == 288 && gameTime > 7776000 && gameTime < 8100000 && cutsceneID != 33)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8100000u);
                }
                if (campaignProgress == 280 && loadingRoomId == 288 && gameTime > 7776000 && gameTime < 8100000 && cutsceneID == 33)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8100000u);
                }
                if (campaignProgress == 300 && loadingRoomId == 288 && gameTime > 8200000 && gameTime < 8640000 && cutsceneID != 33)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8640000u);
                }
                if (campaignProgress == 300 && loadingRoomId == 288 && gameTime > 8748000 && gameTime < 8964000 && cutsceneID == 76)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 8964000u);
                }
                if (campaignProgress == 320 && loadingRoomId == 512 && gameTime > 8964000 && gameTime < 9072000 && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 9072000u);
                }
                if (campaignProgress == 400 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10260000u);
                }
                if (campaignProgress == 402 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), gameTime + 18000 + 1);
                }
                if (campaignProgress == 404 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 10368001u);
                }
                if (campaignProgress == 406 || campaignProgress == 410 || campaignProgress == 415)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), gameMemory.ReadUInt(IntPtr.Add(gameTimePtr, 408)) + 1);
                }
                if (old.campaignProgress != 415 && campaignProgress == 415)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 11448000u);
                }
                if (campaignProgress == 420 && loadingRoomId == 288 && old.inCutsceneOrLoad && !inCutsceneOrLoad)
                {
                    gameMemory.WriteUInt(IntPtr.Add(gameTimePtr, 408), 11664501u);
                }
            }
        }
    }
}
