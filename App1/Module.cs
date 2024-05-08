using COS.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    [COSModule]
    internal class Module
    {
        [COSMethod]
        public string ExampleMethod()
        {
            return "test Message";
        }
    }
}
