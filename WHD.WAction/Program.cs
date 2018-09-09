using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHD.WAction
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new WProcess();
            if (process.IsVailed)
            {
                process.StartWork();
            }
            else
            {
                Console.Write("Cannot Load Process");
            }
        }
    }
}
