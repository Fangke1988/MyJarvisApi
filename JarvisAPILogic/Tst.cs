using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarvisAPILogic
{
    public class Tst
    {
        public static int count = 0;
        public string GetTstStr()
        {
            return count++.ToString();
        }
    }
}
