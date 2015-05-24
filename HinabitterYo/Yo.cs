using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Http;
using System.Configuration;
using System.Web;
using log4net;

namespace HinabitterYo
{
    class Yo : IDisposable
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string URI_YO_ALL = "http://api.justyo.co/yoall/";

        private readonly string API_KEY;
        private readonly HttpClient _client;

        public Yo()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            API_KEY = ConfigurationManager.AppSettings["YoKey"];
        }

        public void YoAll(List<FacebookItem> items)
        {

            foreach (var item in items)
            {
                var query = HttpUtility.ParseQueryString("");
                query["api_token"] = API_KEY;
                query["link"] = item.Link;

                _client.PostAsync(URI_YO_ALL, new StringContent(query.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"))
                        .ContinueWith(t =>
                        {
                            if (t.Exception != null)
                            {
                                foreach (var e in t.Exception.InnerExceptions)
                                {
                                    _logger.Fatal("yo is failed.", e);
                                }
                                return;
                            }

                            var r = t.Result;

                            if (r.IsSuccessStatusCode)
                            {
                                _logger.InfoFormat("{0}: sent yo. id:{1}", item.FromAccount.GetAccountName(), item.ID);
                            }
                            else
                            {
                                _logger.Error(string.Format("yo is failed. status code:{0} return message:{1}"
                                    , r.StatusCode, r.Content.ReadAsStringAsync().Result));
                            }
                        });


                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

    }
}
