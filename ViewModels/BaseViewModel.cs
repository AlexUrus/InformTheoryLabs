using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.ViewModels
{
    public class BaseViewModel : ReactiveObject
    {
        public string ConvertToUnicode(string str)
        {
            StringBuilder binaryStringBuilder = new StringBuilder();
            foreach (char c in str)
            {
                string binaryChar = Convert.ToString(c, 2).PadLeft(16, '0');
                binaryStringBuilder.Append(binaryChar);
            }
            return binaryStringBuilder.ToString();
        }
    }
}
