using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaskManager.Utilities
{
    public class Validate
    {
        static public bool ValidateString(string inputString, int length)
        {
            if (String.IsNullOrEmpty(inputString)) return false;
            if(inputString.Length < length) return false;
            return true;
        }

        static public string TrimInputString(string inputString)
        {
            return inputString.Trim();
        }
    }
}
