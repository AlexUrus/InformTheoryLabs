using LR_1.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Models
{
    public class HilbertMooreEncoding
    {
        public static ObservableCollection<HilbertMooreField> HilbertMooreFields = new ObservableCollection<HilbertMooreField>();
        string Message { get; set; }
        public string EncodeText { get; set; }
        public double Redundancy { get; set; }
        public bool IsOptimalCode { get; set; }

        private Dictionary<char, double> SymbolProbabilities;

        public HilbertMooreEncoding(string message)
        {
            Message = message;
            HilbertMooreFields.Clear();
        }
        public void EncodingMessage()
        {
            EncodeText = "";
            CalcSymbolProbabilities();
            foreach (var field in HilbertMooreFields)
            {
                EncodeText += field.BinaryCode;
            }
            Dictionary<char, string> codewords = MappingCodeWords();
            Redundancy = CalculateRedundancy(codewords);
            IsOptimalCode = KraftInequality.IsOptimalCode(Message);
        }

        public static string DecodingMessage(string encodedMessage)
        {
            string[] encodedSymbols = encodedMessage.Chunk(HilbertMooreFields.Count).Select(x => new string(x)).ToArray();

            StringBuilder decodedSB = new StringBuilder();

            foreach (var encodedSymbol in encodedSymbols)
            {
                var field = HilbertMooreFields.FirstOrDefault(field => field.BinaryCode == encodedSymbol);

                if (field != null)
                {
                    decodedSB.Append(field.Symbol);
                }
                else
                {
                    decodedSB.Append("*");
                }
            }
           
            return decodedSB.ToString();
        }

        private Dictionary<char, string> MappingCodeWords()
        {
            Dictionary<char, string> codewords = new Dictionary<char, string>();
            foreach (var field in HilbertMooreFields)
            {
                codewords[field.Symbol] = field.BinaryCode;
            }
            return codewords;
        }

        public void CalcSymbolProbabilities()
        {
            double q = 0.0;
            SymbolProbabilities = new Dictionary<char, double>();
            Dictionary<char, int> symbolCounts = OneSymbCountContains();

            foreach (KeyValuePair<char, int> kvp in symbolCounts)
            {
                double probability = (double)kvp.Value / Message.Length;
                SymbolProbabilities.Add(kvp.Key, probability);

                double sigmaM = q + probability / 2;
                string sigmaMBinary = DecimalToBinary(sigmaM);
                int lengthDigits = (int)Math.Ceiling(-Math.Log2(probability / 2));
                string binaryCode = BinaryCode(sigmaMBinary, lengthDigits);

                HilbertMooreFields.Add(new HilbertMooreField()
                {
                    Probability = probability,
                    Symbol = kvp.Key,
                    CumulativeProbability = q,
                    SigmaM = sigmaM,
                    LengthDigits = lengthDigits,
                    SigmaMBinary = sigmaMBinary,
                    BinaryCode = binaryCode
                });
                q += probability;

            }
        }

        private Dictionary<char, int> OneSymbCountContains()
        {
            Dictionary<char, int> symbolCounts = new Dictionary<char, int>();
            List<char> symbols = ProcessMessage(Message);
            foreach (char symbol in symbols)
            {
                if (symbolCounts.ContainsKey(symbol))
                {
                    symbolCounts[symbol]++;
                }
                else
                {
                    symbolCounts[symbol] = 1;
                }
            }
            return symbolCounts;
        }

        private string BinaryCode(string sigmaMBinary, int lengthDigits)
        {
            if (sigmaMBinary.Length >= 2)
            {
                string substring = sigmaMBinary.Substring(2);
                if (substring.Length >= lengthDigits)
                {
                    return substring.Substring(0, lengthDigits);
                }
            }
            return string.Empty;
        }


        public static string DecimalToBinary(double number)
        {
            int integralPart = (int)number;
            double fractionalPart = number - integralPart;

            string integralBinary = Convert.ToString(integralPart, 2);

            StringBuilder fractionalBinary = new StringBuilder();
            const int maxFractionalDigits = 32;

            for (int i = 0; i < maxFractionalDigits; i++)
            {
                fractionalPart *= 2;
                int bit = (int)fractionalPart;
                fractionalBinary.Append(bit);
                fractionalPart -= bit;

                if (fractionalPart == 0)
                    break;
            }

            string binaryNumber = integralBinary;

            if (fractionalBinary.Length > 0)
                binaryNumber += "." + fractionalBinary.ToString();

            return binaryNumber + "000000";
        }

        private static List<char> ProcessMessage(string message)
        {
            List<char> uniqueSortedChars = message
                .OrderBy(c => c == ' ' ? 0 : 1)
                .ThenBy(c => c)
                .ToList();

            return uniqueSortedChars;
        }

        public double CalculateRedundancy(Dictionary<char, string> codewords)
        {
            double entropy = 0;
            double avgLength = 0;

            foreach (var kvp in SymbolProbabilities)
            {
                double probability = kvp.Value;
                string codeword = codewords[kvp.Key];

                entropy += -probability * Math.Log2(probability);
                avgLength += probability * codeword.Length;
            }

            double redundancy = 1 - entropy / avgLength;
            return redundancy;
        }
    }
}
