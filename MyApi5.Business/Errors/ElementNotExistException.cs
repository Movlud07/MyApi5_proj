using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi5.Business.Errors
{
    public class ElementNotExistException : Exception
    {
        public ElementNotExistException()
        {
            
        }
        public ElementNotExistException(string message) : base(message) { }
    }
}
