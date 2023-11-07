using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Tools
{
    public class KraftInequality
    {
        public static bool IsOptimalCode(string message)
        {
            Dictionary<char, int> frequencyTable = CalculateSymbolFrequencies(message);
            List<double> probabilities = CalculateProbabilities(frequencyTable);

            double sum = 0.0;
            foreach (double probability in probabilities)
            {
                sum += Math.Pow(2, -probability);
            }

            return sum <= 1.0;
        }

        private static Dictionary<char, int> CalculateSymbolFrequencies(string message)
        {
            Dictionary<char, int> frequencyTable = new Dictionary<char, int>();

            foreach (char symbol in message)
            {
                if (frequencyTable.ContainsKey(symbol))
                {
                    frequencyTable[symbol]++;
                }
                else
                {
                    frequencyTable[symbol] = 1;
                }
            }

            return frequencyTable;
        }

        private static List<double> CalculateProbabilities(Dictionary<char, int> frequencyTable)
        {
            List<double> probabilities = new List<double>();
            int totalSymbols = frequencyTable.Values.Sum();

            foreach (KeyValuePair<char, int> entry in frequencyTable)
            {
                double probability = (double)entry.Value / totalSymbols;
                probabilities.Add(probability);
            }

            return probabilities;
        }
    }
}
