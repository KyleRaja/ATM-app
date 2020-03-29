using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eatm
{
    class Account
    {
        //all initialized with the collection initializers in Eatm.cs.
        public string FullName { get; set; } // Jack shepard, Dough Stamper, and Rose dawson
        public int CardNumber { get; set; }  // 123,456,789
        public int PinCode { get; set; }     // 1111,2222,3333
        public double Balance { get; set; }  // 20000, 15000, 29000
    }
}
