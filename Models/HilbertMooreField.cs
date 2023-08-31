using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Models
{
    public class HilbertMooreField
    {
        public char Symbol { get; set; }
        public double Probability { get; set; }
        public double CumulativeProbability { get; set; }
        public double SigmaM { get; set; }
        public int LengthDigits { get; set; }
        public string SigmaMBinary { get; set; }
        public string BinaryCode { get; set; }
    }
}
