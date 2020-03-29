using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eatm
{
    class Program
    {
        static void Main(string[] args)
        {
            Eatm eatm = new Eatm(); // Creating an object of class Eatm. Creating Eatm default constructor.
            eatm.Init(); // object.Init method is called. Call ends on line 99 in Eatm.cs
            eatm.Start(); // object.Start method is called. 
        }
    }
}