﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using SqlDataProvider.Data;

namespace GameServerScript.AI.Messions
{
    public class CTM1376 : AMissionControl
    {
        private PhysicalObj m_kingMoive;

        private PhysicalObj m_kingFront;

        private SimpleBoss m_king = null;

        private SimpleBoss m_secondKing = null;

        private PhysicalObj[] m_leftWall = null;

        private PhysicalObj[] m_rightWall = null;

        private int m_kill = 0;

        private int m_state = 1305;

        private int turn = 0;

        private int IsSay = 0;

        private int firstBossID = 1305;

        private int secondBossID = 1306;

        private int npcID = 1309;

        private static string[] KillChat = new string[]{
            "马迪亚斯不要再控制我！",

            "这就是挑战我的下场！",

            "不！！这不是我的意愿… "
        };

        private static string[] ShootedChat = new string[]{
            "哎呀~~你们为什么要攻击我？<br/>我在干什么？",
                   
            "噢~~好痛!我为什么要战斗？<br/>我必须战斗…"
        };

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
            Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_left");
            Game.AddLoadingFile(2, "image/map/1076/objects/1076MapAsset.swf", "com.mapobject.asset.WaveAsset_01_right");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");

            int[] resources = { firstBossID, secondBossID, npcID };
            Game.LoadResources(resources);
            int[] gameOverResources = { firstBossID, npcID };
            Game.LoadNpcGameOverResources(gameOverResources);

            Game.SetMap(1076);
            Game.IsBossWar = "啵咕国王";
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            m_kingMoive = Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 0);
            m_kingFront = Game.Createlayer(610, 380, "font", "game.asset.living.boguoKingAsset", "out", 1, 0);
            m_king = Game.CreateBoss(m_state, 750, 668, -1, 0);

            m_king.FallFrom(750, 640, "fall", 0, 2, 1000);
            m_king.SetRelateDemagemRect(-21, -79, 72, 51);
            m_king.AddDelay(10);

            m_king.Say("你们这些低等的庶民，竟敢来到我的王国放肆！", 0, 3000);
            m_kingMoive.PlayMovie("in", 9000, 0);
            m_kingFront.PlayMovie("in", 9000, 0);
            m_kingMoive.PlayMovie("out", 13000, 0);
            m_kingFront.PlayMovie("out", 13400, 0);
            turn = Game.TurnIndex;
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            IsSay = 0;
            if (Game.TurnIndex > turn + 1)
            {
                if (m_kingMoive != null)
                {
                    Game.RemovePhysicalObj(m_kingMoive, true);
                    m_kingMoive = null;
                }
                if (m_kingFront != null)
                {
                    Game.RemovePhysicalObj(m_kingFront, true);
                    m_kingFront = null;
                }
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            //回合数到100的时候结束游戏
            if (Game.TurnIndex > Game.MissionInfo.TotalTurn - 1)
            {
                return true;
            }

            if (m_king.IsLiving == false)
            {
                if (m_state == firstBossID)
                {
                    m_state++;
                }
            }

            if (m_state == secondBossID && m_secondKing == null)
            {
                m_kingMoive = Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
                m_secondKing = Game.CreateBoss(m_state, m_king.X, m_king.Y, m_king.Direction, 1);
                Game.RemoveLiving(m_king.Id);

                if (m_secondKing.Direction == -1)
                {
                    m_secondKing.SetRectBomb(24, -159, 66, 38);
                    m_secondKing.SetRelateDemagemRect(58, -142, 5, 3);
                }
                else
                {
                    m_secondKing.SetRectBomb(-90, -159, 66, 38);
                    m_secondKing.SetRelateDemagemRect(-63, -142, 5, 3);
                }

                m_secondKing.Say("<span class='red'>你们把我激怒了，我饶不了你们！</span>", 0,  3000);
                m_kingMoive.PlayMovie("in", 5000, 0);
                m_secondKing.PlayMovie("weakness", 6100, 0);
                m_kingMoive.PlayMovie("out", 12000, 0);

                List<Player> players = Game.GetAllFightPlayers();
                int minDelay = Game.FindRandomPlayer().Delay;
                foreach (Player player in players)
                {
                    if (player.Delay < minDelay)
                    {
                        minDelay = player.Delay;
                    }
                }

                m_secondKing.AddDelay(minDelay - 2000);
                turn = Game.TurnIndex;
            }

            if (m_secondKing != null && m_secondKing.IsLiving == false)
            {
                m_leftWall = Game.FindPhysicalObjByName("wallLeft", false);
                m_rightWall = Game.FindPhysicalObjByName("wallRight", false);

                if (m_leftWall != null)
                {
                    Game.RemovePhysicalObj(m_leftWall[0], true);
                }

                if (m_rightWall != null)
                {
                    Game.RemovePhysicalObj(m_rightWall[0], true);
                }
                m_secondKing.PlayMovie("Die", 0, 2000);
                m_kill++;
                return true;
            }

            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return m_kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (m_state == 30 && m_secondKing.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }

            List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
            loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/show7.jpg", ""));
            Game.SendLoadResource(loadingFileInfos);
        }

        public override void DoOther()
        {
            base.DoOther();
            if (m_king.IsLiving)
            {
                int index = Game.Random.Next(0, KillChat.Length);
                m_king.Say(KillChat[index], 0, 0);
            }
            else
            {
                int index = Game.Random.Next(0, KillChat.Length);
                m_king.Say(KillChat[index], 0, 0);
            }
        }

        public override void OnShooted()
        {
            if (IsSay == 0)
            {
                if (m_king.IsLiving)
                {
                    int index = Game.Random.Next(0, ShootedChat.Length);
                    m_king.Say(ShootedChat[index], 0, 1500);
                }
                else
                {
                    int index = Game.Random.Next(0, ShootedChat.Length);
                    m_secondKing.Say(ShootedChat[index], 0, 1500);
                }
                IsSay = 1;
            }
        }
    }
}
