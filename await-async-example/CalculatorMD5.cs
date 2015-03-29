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
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Diagnostics;

namespace awaitasyncexample
{
    public class CalculatorMD5 : CalculatorInterface
    {
        readonly Mutex mutex;
        readonly Dictionary<string, Task<string>> cache;

        public CalculatorMD5 ()
        {
            mutex = new Mutex ();
            cache = new Dictionary<string, Task<string>> ();
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        #region CalculatorInterface implementation

        public Task<string> GetHashAsync (string text)
        {
            mutex.WaitOne ();
            if(cache.ContainsKey (text)) {
                Debug.WriteLine ("From cache: " + text);
                return cache [text];
            }
            mutex.ReleaseMutex ();

            var t = Task<string>.Factory.StartNew (() => {
                MD5 md5Hash = MD5.Create();
                return GetMd5Hash(md5Hash, text);
            });

            mutex.WaitOne ();
            cache.Add (text, t);
            mutex.ReleaseMutex ();
            return t;
        }

        #endregion
    }
}

