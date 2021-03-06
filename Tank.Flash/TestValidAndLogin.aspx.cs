using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Bussiness.Interface;

namespace Tank.Flash
{
    public partial class TestValidAndLogin : System.Web.UI.Page
    {
        public static string FlashUrl
        {
            get
            {
                return ConfigurationSettings.AppSettings["FlashUrl"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string result = "";
            try
            {
                string site = "";
                string id = Request["id"] == null ? "" : Request["id"]; //"A";
                string name = Request["username"];
                string password = Request["password"] == null ? "" : Request["password"];
                password = Guid.NewGuid().ToString();
                string time = "1236319807954";// BaseInterface.ConvertDateTimeInt(DateTime.Now);

                string key = string.Empty;
                if (string.IsNullOrEmpty(key))
                {
                    key = BaseInterface.GetLoginKey;
                }

                string v = BaseInterface.md5(name + password + time.ToString() + key);
                string Url = BaseInterface.LoginUrl + "?content=" + HttpUtility.UrlEncode(name + "|" + password + "|" + time.ToString() + "|" + v);
                Url += "&site=" + site;
                result = BaseInterface.RequestContent(Url);

                if (result == "0")
                {
                    string flashUrl = FlashUrl + "?user=" + HttpUtility.UrlEncode(name) + "&key=" + HttpUtility.UrlEncode(password)
                        + "&site=" + site + "&sitename=" + site;                     
                    Response.Write(flashUrl);
                }
                else
                {
                    Response.Write(result);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
    }
}
