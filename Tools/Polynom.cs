﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace LR_1.Tools
{
    class Polynom
    {
        public string PolynomString;

        public Polynom(string polynomString)
        {
            PolynomString = polynomString;
        }

        public static Polynom operator *(Polynom poly1, Polynom poly2)
        {
            string p1 = poly1, p2 = poly2;
            int s1 = p1.Count();
            int s2 = p2.Count();
            int sr = p1.Count() + p2.Count() - 1;
            StringBuilder result = new StringBuilder();
            result.Length = sr;
            result.Replace('\0', '0');
            for (int i = 0; i < s1; i++)
            {
                for (int j = 0; j < s2; j++)
                {
                    char mul = Mul(p1[s1 - i - 1], p2[s2 - j - 1]);
                    int f = i + j;
                    result[sr - f - 1] = Plus(result[sr - f - 1], mul);
                }
            }
            return new Polynom(result.ToString());
        }

        public static Polynom Mod(Polynom poly1, Polynom poly2)
        {
            string polynom1 = poly1, polynom2 = poly2;
            int polynomSymbCount2 = polynom2.Count();

            StringBuilder polynomSb1 = new StringBuilder(polynom1);
            Polynom result = new Polynom("");

            while (polynomSb1.Length >= polynomSymbCount2)
            {
                int length = polynomSb1.Length - (polynomSymbCount2 - 1);
                StringBuilder a = new StringBuilder(GetStringZerosByLength(length));
                a[0] = '1';

                Polynom ap = new Polynom(a.ToString());
                result = result + ap;
                Polynom m = poly2 * ap;
                polynomSb1 = new StringBuilder(new Polynom(polynomSb1.ToString()) + m);
            }

            string sresult = polynomSb1.ToString();
            if (sresult.Length == 0) sresult = "0";
            return new Polynom(sresult.ToString());
        }

        public static Polynom operator +(Polynom poly1, Polynom poly2)
        {
            string p1 = poly1, p2 = poly2;
            int s1 = p1.Count();
            int s2 = p2.Count();
            if (s1 > s2)
            {
                p2 = GetStringZerosByLength(s1 - s2) + p2;
                s2 = s1;
            }
            if (s2 > s1)
            {
                p1 = GetStringZerosByLength(s2 - s1) + p1;
                s1 = s2;
            }
            StringBuilder r = new StringBuilder();
            r.Length = s1;
            for (int i = 0; i < s1; i++)
            {
                r[i] = Plus(p1[i], p2[i]);
            }
            int index = 0;
            for (; index < s1; index++)
            {
                if (r[index] != '0') break;
            }
            r.Remove(0, index);
            return new Polynom(r.ToString());
        }

        public static string GetStringZerosByLength(int len)
        {
            StringBuilder r = new StringBuilder();
            r.Length = len;
            r.Replace('\0', '0');
            return r.ToString();
        }

        static char Plus(char x, char y)
        {
            if (x != y) return '1';
            else return '0';
        }

        static char Mul(char x, char y)
        {
            if (x == '0' || y == '0') return '0';
            else return '1';
        }

        public static implicit operator string(Polynom poly)
        {
            return poly.PolynomString;
        }
    }
}
