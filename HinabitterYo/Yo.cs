using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public bool YoAll(FacebookItem item)
        {
            var query = HttpUtility.ParseQueryString("");
            query["api_token"] = API_KEY;
            query["link"] = item.Link;

            return _client.PostAsync(URI_YO_ALL, new StringContent(query.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"))
                .ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        foreach (var e in t.Exception.InnerExceptions)
                        {
                            _logger.Fatal("yo is failed.", e);
                        }
                        return false;
                    }

                    var r = t.Result;

                    if (r.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        _logger.Error(string.Format("yo is failed. status code:{0} return message:{1}"
                            , r.StatusCode, r.Content.ReadAsStringAsync().Result));
                        return false;
                    }
                }).Result;
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
