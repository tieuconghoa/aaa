using System.Collections;
using System.Globalization;
using System;
using System.Reflection;
using log4net;
using Bussiness;
using SqlDataProvider.Data;
using Game.Base;
using Game.Base.Config;

namespace Bussiness
{
	public abstract class GameProperties
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [ConfigProperty("Edition","��ǰ��Ϸ�汾","11000")]
        public static readonly string EDITION;

        [ConfigProperty("MustComposeGold", "�ϳ����Ľ�Ҽ۸�", 1000)]
        public static readonly int PRICE_COMPOSE_GOLD;

        [ConfigProperty("MustFusionGold", "�������Ľ�Ҽ۸�", 1000)]
        public static readonly int PRICE_FUSION_GOLD;

        [ConfigProperty("MustStrengthenGold", "ǿ��������ļ۸�", 1000)]
        public static readonly int PRICE_STRENGHTN_GOLD;

        [ConfigProperty("CheckRewardItem", "��֤�뽱����Ʒ", 11001)]
        public static readonly int CHECK_REWARD_ITEM;

        [ConfigProperty("CheckCount", "�����֤��ʧ�ܴ���", 2)]
        public static readonly int CHECK_MAX_FAILED_COUNT;

        [ConfigProperty("HymenealMoney", "���ļ۸�", 1000)]
        public static readonly int PRICE_PROPOSE;

        [ConfigProperty("DivorcedMoney", "���ļ۸�", 1000)]
        public static readonly int PRICE_DIVORCED;

        [ConfigProperty("MarryRoomCreateMoney", "��鷿��ļ۸�,2Сʱ��3Сʱ��4Сʱ�ö��ŷָ�", "2000,2700,3400")]
        public static readonly string PRICE_MARRY_ROOM;

        [ConfigProperty("BoxAppearCondition", "������Ʒ��ʾ�ĵȼ�", 4)]
        public static readonly int BOX_APPEAR_CONDITION;

        [ConfigProperty("DisableCommands", "��ֹʹ�õ�����", "")]
        public static readonly string DISABLED_COMMANDS;

        [ConfigProperty("AssState","������ϵͳ�Ŀ���,True��,False�ر�",false)]
        public static bool ASS_STATE;

        [ConfigProperty("DailyAwardState","ÿ�ս�������,True��,False�ر�",true)]
        public static bool DAILY_AWARD_STATE;

        [ConfigProperty("Cess","���׿�˰",0.10)]
        public static readonly double Cess;

        [ConfigProperty("BeginAuction", "����ʱ��ʼ���ʱ��", 20)]
        public static int BeginAuction;

        [ConfigProperty("EndAuction", "����ʱ�������ʱ��", 40)]
        public static int EndAuction;

		private static void Load(Type type)
		{
            using (ServiceBussiness sb = new ServiceBussiness())
            {
                foreach (FieldInfo f in type.GetFields())
                {
                    if (!f.IsStatic)
                        continue;
                    object[] attribs = f.GetCustomAttributes(typeof(ConfigPropertyAttribute), false);
                    if (attribs.Length == 0)
                        continue;
                    ConfigPropertyAttribute attrib = (ConfigPropertyAttribute)attribs[0];
                    f.SetValue(null, GameProperties.LoadProperty(attrib, sb));
                }
            }
		}

        private static void Save(Type type)
        {
            using (ServiceBussiness sb = new ServiceBussiness())
            {
                foreach (FieldInfo f in type.GetFields())
                {
                    if (!f.IsStatic)
                        continue;
                    object[] attribs = f.GetCustomAttributes(typeof(ConfigPropertyAttribute), false);
                    if (attribs.Length == 0)
                        continue;
                    ConfigPropertyAttribute attrib = (ConfigPropertyAttribute)attribs[0];
                    SaveProperty(attrib, sb, f.GetValue(null));
                }
            }
        }

        private static object LoadProperty(ConfigPropertyAttribute attrib, ServiceBussiness sb)
        {
            String key = attrib.Key;
            ServerProperty property = sb.GetServerPropertyByKey(key);
            if (property == null)
            {
                property = new ServerProperty();
                property.Key = key;
                property.Value = attrib.DefaultValue.ToString();
                log.Error("Cannot find server property " + key + ",keep it default value!");
            }
            log.Debug("Loading " + key + " Value is " + property.Value);
            try
            {
                return Convert.ChangeType(property.Value, attrib.DefaultValue.GetType());
            }
            catch (Exception e)
            {
                log.Error("Exception in GameProperties Load: ", e);
                return null;
            }
        }

        private static void SaveProperty(ConfigPropertyAttribute attrib, ServiceBussiness sb,object value)
        {
            try
            {
                sb.UpdateServerPropertyByKey(attrib.Key, value.ToString());
            }
            catch (Exception ex)
            {
                log.Error("Exception in GameProperties Save: ", ex);
            }
        }

		public static void Refresh()
		{
            log.Info("Refreshing game properties!");
            Load(typeof(GameProperties));
		}

        public static void Save()
        {
            log.Info("Saving game properties into db!");
            Save(typeof(GameProperties));
        }
	}
}
