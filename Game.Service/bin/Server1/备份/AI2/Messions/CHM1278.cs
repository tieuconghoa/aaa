﻿using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Bussiness;

namespace GameServerScript.AI.Messions
{
    public class CHM1278 : AMissionControl
    {
        private SimpleBoss m_king = null;

        private int m_kill = 0;

        private int bossID = 1208;

        private int npcID = 1211;

        private PhysicalObj m_kingMoive;

        private PhysicalObj m_front;

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
            int[] resources = { npcID, bossID };
            int[] gameOverResource = { bossID};
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BombKingAsset");
            Game.SetMap(1076);
            Game.IsBossWar = LanguageMgr.GetTranslation("GameServerScript.AI.Messions.CHM1278.msg1");
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

        }

        public override void OnPrepareNewGame()
        {
            base.OnPrepareNewGame();
            m_kingMoive = Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1, 0);
            m_front = Game.Createlayer(710, 380, "font", "game.asset.living.BombKingAsset", "out", 1, 1, 0);
            m_king = Game.CreateBoss(bossID, 888, 590, -1, 0);
            m_king.FallFrom(888, 690, "fall", 0, 2, 1000);
            m_king.SetRelateDemagemRect(-41, -187, 83, 140);
            m_kingMoive.PlayMovie("in", 1000, 0);
            m_front.PlayMovie("in", 2000, 2000);
            m_king.AddDelay(16);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();

            if (m_king.State == 0)
            {
                m_king.SetRelateDemagemRect(-41, -187, 83, 140);
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (m_kingMoive != null)
            {
                Game.RemovePhysicalObj(m_kingMoive, true);
                m_kingMoive = null;
            }
            if (m_front != null)
            {
                Game.RemovePhysicalObj(m_front, true);
                m_front = null;
            }
        }

        public override bool CanGameOver()
        {


            if (m_king.IsLiving == false)
            {
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

        public override void OnPrepareGameOver()
        {
            base.OnPrepareGameOver();
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            bool IsAllPlayerDie = true;
            foreach (Player player in Game.GetAllFightPlayers())
            {
                if (player.IsLiving == true)
                {
                    IsAllPlayerDie = false;
                }
            }
            if (m_king.IsLiving == false && IsAllPlayerDie == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
        }
    }
}
