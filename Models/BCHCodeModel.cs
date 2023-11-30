using LR_1.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LR_1.Models
{
    public class BCHCodeModel
    {
        public int R { get;set;}
        public int K { get;set;}
        public int N { get;set;}
        public string GenPolynom { get; set; }

        private int t;
        private int d0;

        public BCHCodeModel(int n, int k, int r, string genPolynom)
        {
            R = r; K = k; N = n; GenPolynom = genPolynom;
            d0 = genPolynom.Count(c => c == '1');
            t = GetQuotient(d0);
        }

        private int GetQuotient(int n)
        {
            int i = 0;
            while (true)
            {
                n /= 2;
                i++;
                if (n == 1) break;
            }
            return i;
        }

        public string EncodeMessage(string infCodeBytes)
        {
            return CyclicCodingTools.Encode(infCodeBytes, GenPolynom);
        }

        public string TryDecodeMessage(string encodedBytes)
        {
            return CyclicCodingTools.Decode(encodedBytes, GenPolynom,t);
        }
    }
}
