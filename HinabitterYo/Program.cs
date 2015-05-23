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
            Console.WriteLine("id:{0}, from:{1}", hina.ID, hina.From);
            Console.WriteLine("id:{0}, from:{1}", coco.ID, coco.From);

        }
    }
}
