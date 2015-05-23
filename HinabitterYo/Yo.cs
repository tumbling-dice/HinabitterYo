using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Web;

namespace HinabitterYo
{
    class Yo : IDisposable
    {

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

            var resp = _client.PostAsync(URI_YO_ALL, new StringContent(query.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            return resp.IsSuccessStatusCode;
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
