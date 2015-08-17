using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinabitterYo
{
    public class Kuranogawa
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string KURANOGAWA_LETTER_URL = "http://p.eagate.573.jp/game/bemani/hinabita/p/kuranogawa/";

        public static Task<List<IYoItem>> FromLetter(long lastId)
        {
            return HtmlParser.Load(KURANOGAWA_LETTER_URL).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    foreach (var e in t.Exception.InnerExceptions)
                    {
                        _logger.Fatal("to get kuranogawa letter is failed.", e);
                    }

                    throw t.Exception;
                }

                var html = t.Result;
                var ns = html.Root.Name.Namespace;

                var targets = html.Descendants(ns + "div").WhereClass("blog_date")
                                  .Where(x => x.Value != null)
                                  .Select(x => long.Parse(DateTime.Parse(x.Value).ToString("yyyyMMddHHmmss")))
                                  .Where(x => x > lastId);

                if (!targets.Any())
                {
                    return new List<IYoItem>();
                }
                else
                {
                    var accountName = Account.kuranogawa.GetAccountName();
                    foreach (var target in targets) _logger.InfoFormat("{0}: add yo queue. id:{1}", accountName, target);
                }

                return targets.Select(x => new KuranogawaItem
                {
                    ID = x,
                    Link = KURANOGAWA_LETTER_URL,
                    From = "久領堤纏",
                    FromAccount = Account.kuranogawa
                }).Select(x => x as IYoItem).ToList();

            });
        }
    }
}
