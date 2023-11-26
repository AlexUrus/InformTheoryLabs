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
            int r = genPolynom.Length - 1;

            string result;

            infBytes = LeftShift(infBytes, r);

            string Rx = BinaryDivision(infBytes, genPolynom, r);

            result = XORBytes(infBytes, Rx);

            return result;
        }

        static string LeftShift(string input, int shifts)
        {
            return input + new string('0', shifts);
        }

        static string BinaryDivision(string dividend, string divisor, int r)
        {
            // Проверка на пустые строки или деление на ноль
            if (string.IsNullOrEmpty(dividend) || string.IsNullOrEmpty(divisor) || divisor == "0")
            {
                return "Ошибка: некорректные входные данные";
            }

            string divisorShifted = LeftShift(divisor, r);

            // Инициализация остатка
            string remainder = dividend;


            // Пока остаток больше или равен делителю
            while (true)
            {
                remainder = XORBytes(remainder, divisorShifted);
                r--;
                if(r != -1)
                {
                    divisorShifted = LeftShift(divisor, r);
                }
                else
                {
                    break;
                }
            }

            // Если остаток пуст, то делимое было меньше делителя
            if (string.IsNullOrEmpty(remainder))
            {
                return "0";
            }

            return remainder;
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
