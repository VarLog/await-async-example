// /* 
//  * foo() - bar
//  *
//  * Copyright (C) Maxim Fedorenko <varlllog@gmail.com>
//  *
//  * This program is free software; you can redistribute it and/or
//  * modify it under the terms of the GNU General Public License
//  * as published by the Free Software Foundation; either version 2
//  * of the License, or (at your option) any later version.
//  * 
//  * This program is distributed in the hope that it will be useful,
//  * but WITHOUT ANY WARRANTY; without even the implied warranty of
//  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  * GNU General Public License for more details.
//  *
//  * You should have received a copy of the GNU General Public License
//  * along with this program; if not, write to the Free Software
//  * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
//  */
//
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace awaitasyncexample
{
    public class ContentGetter : ContentGetterInterface
    {
        Dictionary<string, Task<string>> cache;

        public ContentGetter ()
        {
            cache = new Dictionary<string, Task<string>> ();
        }

        #region ContentGetterInterface implementation

        public Task<string> GetContentAsync (string urlAddress)
        {
            if(cache.ContainsKey (urlAddress)) {
                return cache [urlAddress];
            }

            var t = Task<string>.Factory.StartNew (() => {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create (urlAddress);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse ();

                string data = null;
                if (response.StatusCode == HttpStatusCode.OK) {
                    Stream receiveStream = response.GetResponseStream ();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null) {
                        readStream = new StreamReader (receiveStream);
                    } else {
                        readStream = new StreamReader (receiveStream, Encoding.GetEncoding (response.CharacterSet));
                    }

                    data = readStream.ReadToEnd ();

                    response.Close ();
                    readStream.Close ();
                }

                return data;
            });
            t.Start ();

            cache.Add (urlAddress, t);
            return t;
        }

        public Task<string> GetCachedContentAsync (string urlAddress)
        {
            if(cache.ContainsKey (urlAddress)) {
                return cache [urlAddress];
            } 

            return null;
        }

        #endregion
    }
}

