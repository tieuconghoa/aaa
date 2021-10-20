﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;

namespace GameServerScript.AI.Messions
{
    public class CHM1272 : AMissionControl
    {
        private List<SimpleNpc> redNpc = new List<SimpleNpc>();
        
        private List<SimpleNpc> blueNpc = new List<SimpleNpc>();
        
        private int redCount = 0;
        
        private int blueCount = 0;
        
        private int redTotalCount = 0;
        
        private int blueTotalCount = 0;
        
        private int dieRedCount = 0;
        
        private int dieBlueCount = 0;

        private int redNpcID = 1201;

        private int blueNpcID = 1202;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 930)
            {
                return 3;
            }
            else if (score > 850)
            {
                return 2;
            }
            else if (score > 775)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = {redNpcID, blueNpcID};
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(resources);
            Game.SetMap(1072);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

            for (int i = 0; i < 4; i++)
            {
                redTotalCount++;

                if (i < 1)
                {
                    redNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                }
                else if (i < 3)
                {
                    redNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                }
                else
                {
                    redNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                }
            }

            blueTotalCount++;
            blueNpc.Add(Game.CreateNpc(blueNpcID, 1467, 495, 1));
        }

        public override void OnNewTurnStarted()
        {
            redCount = redTotalCount - dieRedCount;
            blueCount = blueTotalCount - dieBlueCount;

            if (Game.GetLivedLivings().Count == 0)
            {
                Game.PveGameDelay = 0;
            }


            if (Game.TurnIndex > 1 && Game.CurrentPlayer.Delay > Game.PveGameDelay)
            {
                if (blueCount == 3 && redCount == 12)
                {
                    return;
                }

                if (redTotalCount < 12 && blueTotalCount < 3)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        redTotalCount++;

                        if (i < 1)
                        {
                            redNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                        }
                        else if (i < 3)
                        {
                            redNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                        }
                        else
                        {
                            redNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                        }
                    }

                    blueTotalCount++;
                    blueNpc.Add(Game.CreateNpc(blueNpcID, 1467, 495, 1));
                }
                else
                {
                    if (redCount < 12)
                    {
                        if (12 - redCount >= 4)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (redTotalCount < 20 && redCount != 12)
                                {
                                    redTotalCount++;

                                    if (i < 1)
                                    {
                                        redNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                                    }
                                    else if (i < 3)
                                    {
                                        redNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                                    }
                                    else
                                    {
                                        redNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                                    }
                                }
                            }
                        }
                        else if (12 - redCount > 0)
                        {
                            for (int i = 0; i < (12 - redCount); i++)
                            {
                                if (redTotalCount < 20 && redCount != 12)
                                {
                                    redTotalCount++;

                                    if (i < 1)
                                    {
                                        redNpc.Add(Game.CreateNpc(redNpcID, 900 + (i + 1) * 100, 505, 1));
                                    }
                                    else if (i < 3)
                                    {
                                        redNpc.Add(Game.CreateNpc(redNpcID, 920 + (i + 1) * 100, 505, 1));
                                    }
                                    else
                                    {
                                        redNpc.Add(Game.CreateNpc(redNpcID, 1000 + (i + 1) * 100, 515, 1));
                                    }
                                }
                            }
                        }

                        if (blueCount < 3 && blueTotalCount < 5)
                        {
                            blueTotalCount++;
                            blueNpc.Add(Game.CreateNpc(blueNpcID, 1467, 495, 1));
                        }
                    }
                }

            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            bool result = true;
            dieRedCount = 0;
            dieBlueCount = 0;

           

            foreach (SimpleNpc npc in redNpc)
            {
                if (npc.IsLiving)
                {
                    result = false;
                }
                else
                {
                    dieRedCount++;
                }
            }

            foreach (SimpleNpc blueNpcSingle in blueNpc)
            {
                if (blueNpcSingle.IsLiving)
                {
                    result = false;
                }
                else
                {
                    dieBlueCount++;
                }
            }

            if (result && redTotalCount == 20 && blueTotalCount == 5)
            {
                Game.IsWin = true;
                return true;
            }
            
            if (Game.TurnIndex > Game.MissionInfo.TotalTurn - 1)
            {
                return true;
            }

            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return Game.TotalKillCount;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (Game.GetLivedLivings().Count == 0)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/3", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
