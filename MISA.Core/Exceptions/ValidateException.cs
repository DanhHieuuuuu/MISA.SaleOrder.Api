using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Exceptions
{
    public class ValidateException : Exception
    {
        private readonly string _errorMessage;
        public ValidateException(string msg) 
        {
            _errorMessage = msg; 
        }
        public override string Message => _errorMessage;
    }
}
