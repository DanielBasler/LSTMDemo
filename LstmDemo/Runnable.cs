using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public class Runnable : IRunnable
    {
        public Action Run { get; set; }
    }
}
