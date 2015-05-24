using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using System.Configuration;
using log4net;

namespace HinabitterYo
{
    class FacebookConnect
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly FacebookClient _client;

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
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    foreach (var e in t.Exception.InnerExceptions)
                    {
                        _logger.Warn("extend access_token expire is failed.", e);
                    }
                }
            });
        }

        public Task<FacebookItem> Hinabitter()
        {
            return CreateItem(Account.hinabitter);
        }

        public Task<FacebookItem> Coconatsu()
        {
            return CreateItem(Account.coconatsu5572);
        }

        private Task<FacebookItem> CreateItem(Account account)
        {
            var accountName = account.GetAccountName();

            return _client.GetTaskAsync(string.Format("{0}/feed", accountName)).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    foreach (var e in t.Exception.InnerExceptions)
                    {
                        _logger.Fatal(string.Format("to get {0}'s feed is failed.", accountName), e);
                    }

                    throw t.Exception;
                }

                dynamic feed = t.Result;
                dynamic data = feed.data[0];

                var item = new FacebookItem
                {
                    ID = long.Parse(((string)data.id).Split('_')[1]),
                    From = ((string)data.message).Split('\n').Last(),
                    FromAccount = account,
                };

                item.Link = string.Format("https://www.facebook.com/{0}/posts/{1}", accountName, item.ID);

                return item;
            });

        }

    }
}
