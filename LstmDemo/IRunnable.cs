using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LstmDemo
{
    public interface IRunnable
    {
        Action Run { get; set; }
    }
}
