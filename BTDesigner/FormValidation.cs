using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTDesigner
{
    internal static class FormValidation
    {

        internal static bool CheckProfileString(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            if (name.Length < 8 || name.Length > 32)
                return false;
            return true;
        }

        public static bool StringsMatches(string text1, string text2)
        {
            bool res = false;
if(text1==text2)
            {
                res = true;
            }
            return res;
        }
    }
}
