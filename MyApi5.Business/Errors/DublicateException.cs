using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Errors
{
    public class DublicateException : Exception
    {
        public DublicateException()
        {
            
        }
        public DublicateException(string message) : base(message)
        {
            
        }
    }
}
