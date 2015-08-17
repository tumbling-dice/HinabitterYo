#region License
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
using log4net;

namespace HinabitterYo
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            _logger.Info("start connect.");
            var facebook = new FacebookConnect();

            var props = Properties.Settings.Default;

            var tasks = new Task<List<IYoItem>>[] {
                facebook.Hinabitter(props.HinabitterLastId),
                facebook.Coconatsu(props.CoconatsuLastId),
                Kuranogawa.FromLetter(props.KuranogawaLastDay),
            };

            if (Task.WaitAll(tasks, -1))
            {
                var yoQueues = tasks.Where(x => !x.IsFaulted)
                                         .SelectMany(x => x.Result)
                                         .OrderBy(x => x.ID)
                                         .ToList();

                if (!yoQueues.Any())
                {
                    _logger.Info("no yoQueues. finish.");
                    return;
                }


                using (var yo = new Yo())
                {
                    if (args.Length == 0) yo.YoAll(yoQueues);

                    foreach (var ids in yoQueues.GroupBy(x => x.FromAccount, y => y.ID).ToDictionary(x => x.Key, y => y.Max()))
                    {
                        var accountName = ids.Key.GetAccountName();

                        switch (ids.Key)
                        {
                            case Account.hinabitter:
                                props.HinabitterLastId = ids.Value;
                                break;
                            case Account.coconatsu5572:
                                props.CoconatsuLastId = ids.Value;
                                break;
                            case Account.kuranogawa:
                                props.KuranogawaLastDay = ids.Value;
                                break;
                            default:
                                _logger.WarnFormat("{0} is unknown account.", accountName);
                                break;
                        }
                    }

                    props.Save();

                    _logger.Info("finish!");
                }


            }
        }
    }
}
