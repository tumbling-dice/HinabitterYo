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

using Sgml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HinabitterYo
{
    public class HtmlParser
    {
        public async static Task<XDocument> Load(string url)
        {
            using (var hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");

                return await hc.GetStreamAsync(url).ContinueWith(x =>
                 {
                     using (var httpStream = new StreamReader(x.Result))
                     using (var sgmlReader = new SgmlReader
                     {
                         InputStream = httpStream,
                         IgnoreDtd = true,
                         DocType = "HTML",
                         CaseFolding = CaseFolding.ToLower
                     })
                     {
                         return XDocument.Load(sgmlReader);
                     }
                 });
            }
        }
    }

    public static class XDocumentExtensions
    {
        private static readonly Func<string, string, Func<XElement, bool>> _attrFilter = (attr, value) =>
            (e) =>
                e.Attribute(attr) != null
                        && e.Attribute(attr).Value == value;


        public static IEnumerable<XElement> WhereClass(this XDocument xml, params string[] value)
        {
            return WhereClass(xml.Root.Descendants(), value);
        }

        public static IEnumerable<XElement> WhereClass(this IEnumerable<XElement> elements, params string[] value)
        {
            return elements.Where(_attrFilter("class", string.Join(" ", value)));
        }

        public static XElement WhereId(this XDocument xml, string name)
        {
            return WhereId(xml.Root.Descendants(), name);
        }

        public static XElement WhereId(this IEnumerable<XElement> elements, string value)
        {
            return elements.First(_attrFilter("id", value));
        }

    }
}
