#region Licence
/**
 * HinabitaYo
 *
 * Copyright (c) 2015 @inujini_ (https://twitter.com/inujini_)
 *
 * This software is released under the MIT License.
 * http://opensource.org/licenses/mit-license.php
 */
#endregion

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
        }

        public Task<List<IYoItem>> Hinabitter(long lastId)
        {
            return CreateItems(Account.hinabitter, lastId);
        }

        public Task<List<IYoItem>> Coconatsu(long lastId)
        {
            return CreateItems(Account.coconatsu5572, lastId);
        }

        private Task<List<IYoItem>> CreateItems(Account account, long lastId)
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
                dynamic datas = feed.data;

                var items = new List<IYoItem>();

                foreach(var data in datas)
                {
                    var id = long.Parse(((string)data.id).Split('_')[1]);

                    if (lastId >= id) continue;

                    var message = (string)data.message;

                    if(message == null)
                    {
                        continue;
                    }

                    var item = new FacebookItem
                    {
                        ID = id,
                        From = message.Split('\n').Last(),
                        FromAccount = account,
                    };

                    item.Link = string.Format("https://www.facebook.com/{0}/posts/{1}", accountName, item.ID);

                    items.Add(item);

                    _logger.InfoFormat("{0}: add yo queue. id:{1}", accountName, item.ID);
                }

                return items;
            });

        }

    }
}
