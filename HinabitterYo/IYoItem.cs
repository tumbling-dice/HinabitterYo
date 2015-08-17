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

namespace HinabitterYo
{
    public interface IYoItem
    {
        long ID { get; set; }
        string Link { get; set; }
        string From { get; set; }
        Account FromAccount { get; set; }
    }

    public enum Account
    {
        hinabitter,
        coconatsu5572,
        kuranogawa,
        kuranogawa_app,
    }

    public static class AccountExtention
    {
        public static string GetAccountName(this Account account)
        {
            return Enum.GetName(typeof(Account), account);
        }
    }
}
