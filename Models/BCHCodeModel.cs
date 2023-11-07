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
        public int T { get;set;}
        public int K { get;set;}
        public int N { get;set;}
        public string GenPolynom { get; set; }

        public BCHCodeModel(int t, int k, int n, string genPolynom)
        {
            T = t; K = k; N = n; GenPolynom = genPolynom;
        }

        public string EncodeMessage(string infCodeBytes)
        {
            string encodedBytes;
            Polynom infCodeBytesPolynom = new Polynom(infCodeBytes);
            Polynom genPolynom = new Polynom(GenPolynom);

            string infCodeString = genPolynom;
            StringBuilder s3 = new StringBuilder(Polynom.GetStringZerosByLength(infCodeString.Length));

            s3[0] = '1';

            encodedBytes = infCodeBytesPolynom * new Polynom(s3.ToString());
            encodedBytes += Polynom.Mod(infCodeBytesPolynom, genPolynom);

            return encodedBytes;
        }

        public string TryDecodeMessage(string encodedBytes, out bool isCorrectDecoding)
        {
            Polynom p1 = new Polynom(encodedBytes);
            Polynom p2 = new Polynom(GenPolynom);
            Polynom remainder = Polynom.Mod(p1, p2);
            if (remainder == "")
                isCorrectDecoding = true;
            else
                isCorrectDecoding = false;

            return p1.PolynomString.Substring(0, K);
        }

        public string RepairBytes(string badDecodedMessage)
        {
            Polynom p1 = new Polynom(badDecodedMessage);
            int n1 = p1.PolynomString.Length;
            Polynom p2 = new Polynom(GenPolynom);

            int shifts = 0;

            while (true)
            {
                Polynom polyRemainder = Polynom.Mod(p1, p2); 
                string remainder = polyRemainder;

                if (OnesCount(remainder) > T)
                {
                    string s = p1.PolynomString;
                    s = s.Substring(1) + s[0];
                    shifts++;
                }
                else
                {
                    p1 = p1 + polyRemainder;
                    break;
                }
            }

            for (int i = 0; i < shifts; i++)
            {
                string s = p1;
                if (s.Length < n1)
                {
                    while (s.Length < n1)
                    {
                        s = '0' + s;
                    }
                }
                s = s[s.Length - 1] + s.Substring(0, s.Length - 1);
                p1 = new Polynom(s);
            }
            return p1.PolynomString.Substring(0, K);
        }

        int OnesCount(string s)
        {
            int sum = 0;
            foreach (char c in s)
            {
                if (c == '1') sum++;
            }
            return sum;
        }
    }
}
