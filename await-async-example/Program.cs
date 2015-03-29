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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace awaitasyncexample
{
    class MainClass
    {
        CalculatorMD5 calc;
        List<Task<string>> tasks;

        const int threadCount = 100;

        MainClass() {
            calc = new CalculatorMD5 ();
            tasks = new List<Task<string>> ();


            int i = threadCount;
                
            while(i-- > 0) {
                tasks.Add (calc.GetHashAsync ("Hello " + (i % 20)));
            }
        }

        void WaitAll() {
            foreach(var t in tasks) {
                Console.WriteLine ("MD5 == " + t.Result);
            }
        }

        public static void Main (string[] args)
        {
            MainClass main = new MainClass ();

            Console.WriteLine ("Hello World!");
            main.WaitAll ();
        }
    }
}
