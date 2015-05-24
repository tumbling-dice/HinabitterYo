﻿#region License
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
