using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using System.Configuration;

namespace HinabitterYo
{
    class FacebookConnect
    {
        private readonly FacebookClient _client;
        private const string ACC_HINABITETR = "hinabitter";
        private const string ACC_COCONATSU = "coconatsu5572";

        public FacebookConnect()
        {
            _client = new FacebookClient
            {
                AppId = ConfigurationManager.AppSettings["AppId"],
                AppSecret = ConfigurationManager.AppSettings["SecretKey"],
                AccessToken = ConfigurationManager.AppSettings["AccessToken"]
            };

            // auto extend access_token expire
            _client.GetTaskAsync("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["AppId"],
                client_secret = ConfigurationManager.AppSettings["SecretKey"],
                grant_type = "fb_exchange_token",
                fb_exchange_token = ConfigurationManager.AppSettings["AccessToken"],
            });
        }

        public FacebookItem Hinabitter()
        {
            return CreateItem(ACC_HINABITETR);
        }

        public FacebookItem Coconatsu()
        {
            return CreateItem(ACC_COCONATSU);
        }

        private FacebookItem CreateItem(string account)
        {
            dynamic feed = _client.Get(string.Format("{0}/feed", account));
            dynamic data = feed.data[0];

            var item = new FacebookItem
            {
                ID = ((string)data.id).Split('_')[1],
                From = ((string)data.message).Split('\n').Last()
            };

            item.Link = string.Format("https://www.facebook.com/{0}/posts/{1}", account, item.ID);

            return item;
        }
    }
}
