using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using System.Configuration;

namespace HinabitterYo
{
    class Program
    {
        static void Main(string[] args)
        {
            var facebook = new FacebookConnect();

            var hina = facebook.Hinabitter();
            var coco = facebook.Coconatsu();

            using (var yo = new Yo())
            {
                if (hina.ID > Properties.Settings.Default.HinabitterLastId)
                {
                    if (yo.YoAll(hina))
                    {
                        Properties.Settings.Default.HinabitterLastId = hina.ID;
                        Properties.Settings.Default.Save();
                    }
                }

                if (coco.ID > Properties.Settings.Default.CoconatsuLastId)
                {
                    if (yo.YoAll(coco))
                    {
                        Properties.Settings.Default.CoconatsuLastId = coco.ID;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }
    }
}
