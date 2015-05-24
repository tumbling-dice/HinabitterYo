using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace HinabitterYo
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            _logger.Info("connect to Facebook.");
            var facebook = new FacebookConnect();

            var tasks = new Task[] {
                facebook.Hinabitter().ContinueWith(_sendYo),
                facebook.Coconatsu().ContinueWith(_sendYo),
            };

            if (Task.WaitAll(tasks, -1))
            {
                _logger.Info("end task.");
            }
        }

        private static Action<Task<FacebookItem>> _sendYo = t =>
        {
            if (t.IsFaulted) return;

            using (var yo = new Yo())
            {
                var r = t.Result;
                var props = Properties.Settings.Default;
                var accountName = r.FromAccount.GetAccountName();

                _logger.InfoFormat("{0}: check last id.", accountName);

                var lastId = Account.hinabitter == r.FromAccount
                            ? props.HinabitterLastId
                            : props.CoconatsuLastId;

                _logger.InfoFormat("{0}: newly id:{1}, last id:{2}", accountName, r.ID, lastId);

                if (r.ID > lastId && yo.YoAll(r))
                {
                    _logger.InfoFormat("{0}: sent yo.", accountName);
                    switch (r.FromAccount)
                    {
                        case Account.hinabitter:
                            props.HinabitterLastId = r.ID;
                            break;
                        case Account.coconatsu5572:
                            props.CoconatsuLastId = r.ID;
                            break;
                        default:
                            _logger.WarnFormat("{0} is unknown account.", accountName);
                            break;
                    }

                    props.Save();
                }
            }
        };
    }
}
