using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Tools
{
    public static class CyclicCodingTools
    {
        public static string Encode(string infBytes /*A(x)*/, string genPolynom)
        {

            string result;

            infBytes = LeftShift(infBytes, infBytes.Length - genPolynom.Length);

            string Rx = BinaryDivision(infBytes, genPolynom);

            result = XORBytes(infBytes, Rx);

            return result;
        }

        public static string Decode(string encodedMessage, string genPolynom, int countErrors)
        {
            int w = int.MaxValue;
            string Rx;
            int countCyclics = 0;

            while(true)
            {
                
                Rx = BinaryDivision(encodedMessage, genPolynom);
                w = Rx.Count(c => c == '1');

                if(w<countErrors)
                {
                    break;
                }
                else
                {
                   encodedMessage = CyclicLeftShift(encodedMessage, 1);
                }
                countCyclics++;
            }

            return RemoveErrors(encodedMessage, w, countCyclics);
        }

        static string RemoveErrors(string badMessage, int w, int countCyclics)
        {
            badMessage = XORBytes(badMessage, Convert.ToString(w,2));
            return CyclicRightShift(badMessage, countCyclics);
        }

        static string LeftShift(string input, int shifts)
        {
            return input + new string('0', shifts);
        }

        static string CyclicLeftShift(string input, int shifts)
        {
            int length = input.Length;
            int shiftCount = shifts % length;

            char[] output = new char[length];

            for (int i = 0; i < length - shiftCount; i++)
            {
                output[i] = input[i + shiftCount];
            }

            for (int i = 0; i < shiftCount; i++)
            {
                output[length - shiftCount + i] = input[i];
            }

            return new string(output);
        }

        static string CyclicRightShift(string input, int shifts)
        {
            int length = input.Length;
            int shiftCount = shifts % length;

            char[] output = new char[length];

            for (int i = 0; i < shiftCount; i++)
            {
                output[i] = input[length - shiftCount + i];
            }

            for (int i = shiftCount; i < length; i++)
            {
                output[i] = input[i - shiftCount];
            }

            return new string(output);
        }


        static string BinaryDivision(string dividend, string divisor)
        {
            // Проверка на пустые строки или деление на ноль
            if (string.IsNullOrEmpty(dividend) || string.IsNullOrEmpty(divisor) || divisor == "0")
            {
                return "Ошибка: некорректные входные данные";
            }

            dividend = dividend.TrimStart('0');

            int shift = dividend.Length - divisor.Length;

            string divisorShifted = LeftShift(divisor, dividend.Length - divisor.Length);

            // Инициализация остатка
            string remainder = dividend;

            while (CompareBinaryStrings(remainder, divisor))
            {
                remainder = XORBytes(remainder, divisorShifted);

                shift--;
                if(shift == -1) break;

                divisorShifted = LeftShift(divisor, shift);
            }

            // Если остаток пуст, то делимое было меньше делителя
            if (string.IsNullOrEmpty(remainder))
            {
                return "0";
            }

            return remainder;
        }

        static bool CompareBinaryStrings(string binary1, string binary2)
        {
            int decimal1 = Convert.ToInt32(binary1, 2);
            int decimal2 = Convert.ToInt32(binary2, 2);
            return decimal1 >= decimal2;
        }


        static string XORBytes(string byteString1, string byteString2)
        {
            // Проверка на пустые строки или разные длины
            if (string.IsNullOrEmpty(byteString1) || string.IsNullOrEmpty(byteString2)) 
            {
                return "Ошибка: некорректные входные данные";
            }

            while (byteString1.Length > byteString2.Length)
            {
                byteString2 = '0' + byteString2;
            }

            // Выполнение XOR для каждого символа
            char[] resultChars = new char[byteString1.Length];

            for (int i = 0; i < byteString1.Length; i++)
            {
                resultChars[i] = (byteString1[i] == byteString2[i]) ? '0' : '1';
            }

            // Преобразование результата в строку
            return new string(resultChars);
        }
    }
}
