using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinabitterYo
{
    class FacebookItem
    {
        public long ID { get; set; }
        public string Link { get; set; }
        public string From { get; set; }
        public Account FromAccount { get; set; }
    }

    enum Account
    {
        hinabitter,
        coconatsu5572,
    }

    static class AccountExtention
    {
        public static string GetAccountName(this Account account)
        {
            return Enum.GetName(typeof(Account), account);
        }
    }
}
