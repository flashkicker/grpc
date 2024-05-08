using COS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2
{
    [COSModule]
    internal class HelloWorld
    {
        [COSMethod]
        public string SayHello(string arg1, string arg2)
        {
            return arg1 + arg2;
        }
    }
}
