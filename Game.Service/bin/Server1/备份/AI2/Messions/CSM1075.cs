﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using Game.Logic.Effects;
namespace GameServerScript.AI.Messions
{
    public class CSM1075 : AMissionControl
    {
        private List<PhysicalObj> m_bord = new List<PhysicalObj>();

        private List<PhysicalObj> m_key = new List<PhysicalObj>();

        private PhysicalObj m_door = null;

        private string keyIndex = null;

        private int m_count = 0;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            else if (score > 825)
            {
                return 2;
            }
            else if (score > 725)
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
            Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Board001");
            Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.CrystalDoor001");
            Game.AddLoadingFile(2, "image/map/1075/objects/1075Object.swf", "game.crazytank.assetmap.Key");
            Game.AddLoadingFile(2, "image/map/1075/objects/wordtip75.swf", "game.view.wordtip75");
            Game.SetMap(1075);
        }

        public override void OnPrepareStartGame()
        {
            base.OnPrepareStartGame();
            Game.TotalCount = Game.PlayerCount;
            Game.TotalTurn = Game.PlayerCount * 6;
            Game.SendMissionInfo();
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();

            //动态设置关卡回合数上线

            Game.TotalCount = Game.PlayerCount;
            Game.TotalTurn = Game.PlayerCount * 6;
            Game.SendMissionInfo();

            //这里需要随机，有多少个人，就随机多少次，设置State=1的木板
            // CreatePhysicalObj(int x, int y,string name, string model, string defaultAction, int scale, int rotation)
            m_bord.Add(Game.CreatePhysicalObj(76, 167, "board1", "game.crazytank.assetmap.Board001", "1", 1, 1, 336));
            m_bord.Add(Game.CreatePhysicalObj(403, 159, "board2", "game.crazytank.assetmap.Board001", "1", 1, 1, 23));
            m_bord.Add(Game.CreatePhysicalObj(699, 156, "board3", "game.crazytank.assetmap.Board001", "1", 1, 1, 350));
            m_bord.Add(Game.CreatePhysicalObj(959, 148, "board4", "game.crazytank.assetmap.Board001", "1", 1, 1, 325));

            m_bord.Add(Game.CreatePhysicalObj(177, 261, "board5", "game.crazytank.assetmap.Board001", "1", 1, 1, 22));
            m_bord.Add(Game.CreatePhysicalObj(514, 277, "board6", "game.crazytank.assetmap.Board001", "1", 1, 1, 336));
            m_bord.Add(Game.CreatePhysicalObj(782, 285, "board7", "game.crazytank.assetmap.Board001", "1", 1, 1, 23));
            m_bord.Add(Game.CreatePhysicalObj(1061, 280, "board8", "game.crazytank.assetmap.Board001", "1", 1, 1, 22));

            m_bord.Add(Game.CreatePhysicalObj(274, 406, "board9", "game.crazytank.assetmap.Board001", "1", 1, 1, 350));
            m_bord.Add(Game.CreatePhysicalObj(621, 409, "board10", "game.crazytank.assetmap.Board001", "1", 1, 1, 23));
            m_bord.Add(Game.CreatePhysicalObj(873, 414, "board11", "game.crazytank.assetmap.Board001", "1", 1, 1, 336));
            m_bord.Add(Game.CreatePhysicalObj(1155, 428, "board12", "game.crazytank.assetmap.Board001", "1", 1, 1, 336));


            m_door = Game.CreatePhysicalObj(1275, 556, "door", "game.crazytank.assetmap.CrystalDoor001", "start", 1, 1, 0);

            int[] num = new int[] { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };

            for (int i = 0; i < Game.TotalCount; i++)
            {
                int index = Game.Random.Next(0, 12);
                if (num[index] == index)
                {
                    i--;
                }
                else
                {
                    num[index] = index;
                    m_bord.ToArray()[index].PlayMovie("2", 0, 0);
                    keyIndex = string.Format("Key{0}", index);
                    m_key.Add(Game.CreatePhysicalObj(m_bord.ToArray()[index].X, m_bord.ToArray()[index].Y - 8, keyIndex, "game.crazytank.assetmap.Key", "1", 1, 1, 0));
                    Game.SendGameObjectFocus(1, m_bord.ToArray()[index].Name, 0, 0);
                }
            }

            Game.SendGameObjectFocus(1, "door", 1000, 0);


            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "sound/Sound201.swf", "Sound201"));
            loadingFileInfos.Add(new LoadingFileInfo(2, "sound/Sound202.swf", "Sound202"));
            Game.SendLoadResource(loadingFileInfos);
            Game.GameOverResources.Add("game.crazytank.assetmap.CrystalDoor001");
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (Game.CurrentLiving != null)
            {
                ((Player)Game.CurrentLiving).Seal((Player)Game.CurrentLiving, 0, 0);
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            if (Game.CurrentLiving != null)
            {
                ((Player)Game.CurrentLiving).SetBall(3);
            }
        }

        public override bool CanGameOver()
        {

            for (int i = 0; i < 12; i++)
            {
                foreach (Player player in Game.GetAllFightPlayers())
                {
                    if (player.X > m_bord[i].X - 40 && player.X < m_bord[i].X + 40 && player.Y < m_bord[i].Y + 40 && player.Y > m_bord[i].Y - 40)
                    {
                        if (m_bord[i].CurrentAction == "2")
                        {
                            m_bord[i].PlayMovie("3", 0, 0);
                            keyIndex = string.Format("Key{0}", i);
                            Game.RemovePhysicalObj(Game.FindPhysicalObjByName(keyIndex)[0], true);
                            m_count++;
                            Game.SendUpdateUiData();
                        }
                    }
                }
            }

            if (m_count == Game.TotalCount)
            {
                return true;
            }
            return false;
        }

        public override void OnPrepareGameOver()
        {
            if (Game.TurnIndex == Game.TotalTurn)
            {
                for (int i = 0; i < 12; i++)
                {
                    foreach (Player player in Game.GetAllFightPlayers())
                    {
                        if (player.X > m_bord[i].X - 40 && player.X < m_bord[i].X + 40 && player.Y < m_bord[i].Y + 40 && player.Y > m_bord[i].Y - 40)
                        {
                            if (m_bord[i].CurrentAction == "2")
                            {
                                m_bord[i].PlayMovie("3", 0, 0);
                                keyIndex = string.Format("Key{0}", i);
                                Game.RemovePhysicalObj(Game.FindPhysicalObjByName(keyIndex)[0], true);
                                m_count++;
                                Game.SendUpdateUiData();
                            }
                        }
                    }
                }
            }

            if (m_count == Game.TotalCount)
            {
                Game.SendGameObjectFocus(2, "door", 0, 6000);
                Game.AddAction(new PlaySoundAction("201", 0));
                m_door.PlayMovie("end", 4000, 5000);
                Game.AddAction(new PlaySoundAction("202", 3000));
            }
        }

        public override int UpdateUIData()
        {
            return m_count;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();

            //所有木板都被占领，则IsWin为true
            if (m_count == Game.TotalCount)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }

            foreach (Player player in Game.GetAllFightPlayers())
            {
                player.OffSeal(player, 0);
            }

            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/show6.jpg", ""));
            Game.SendLoadResource(loadingFileInfos);
        }
    }
}
