using LR_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LR_1.Models
{
    public class HammingCodeModel
    {
        private MatrixManager matrixManager;

        public byte[,] H_Matrix = new byte[3, 7]
        {
            { 1, 1, 0, 1, 1, 0, 0},
            { 1, 1, 1, 0, 0, 1, 0},
            { 1, 1, 0, 1, 0, 0, 1}
        };

        public byte[,] G_Matrix = new byte[4, 7]
        {
            { 1, 0, 0, 0, 1, 1, 1},
            { 0, 1, 0, 0, 1, 1, 0},
            { 0, 0, 1, 0, 0, 1, 1},
            { 0, 0, 0, 1, 1, 0, 1}
        };
        int[] G = { 8, 4, 2, 1, 13, 14, 11 };

        public List<SyndromeViewModel> SyndromeCollection;
        public List<CorrectionViewModel> Corrections;
        public HammingCodeModel()
        {
            matrixManager = new MatrixManager();
            SyndromeCollection = new List<SyndromeViewModel>();
            Corrections = new List<CorrectionViewModel>();
        }

        public int[] GetEncodedMas(string text)
        {
            string strBits = ConvertToUnicode(text);
            int[] masBits = DivStrBy4bit(strBits);
            int[] checkedmasBits = GenerateBits(masBits);
            int[] parityBit = ParityBit(masBits, checkedmasBits);

            int[] fullCode = new int[masBits.Length];

            for (int i = 0; i < fullCode.Length; i++)
            {
                fullCode[i] += (masBits[i] << 4) + (checkedmasBits[i] << 1) + (parityBit[i]);
            }

            return fullCode;
        }

        public string GetDecodedText(string encodedText)
        {
            List<byte[]> arrConstructions = ConvertEncodedTextToListConstructions(encodedText);

            var slist = GetSyndromeList(arrConstructions);
            foreach (var arr in slist)
            {
                SyndromeCollection.Add(new SyndromeViewModel(arr));
            }
            var arrcorrections = GetRepairedConstructions(arrConstructions, slist);
            for (var i = 0; i < arrcorrections.Count; i++)
            {
                Corrections.Add(new CorrectionViewModel(slist[i], arrcorrections[i]));
            }

            return "gg";
        }

        private byte[,] MultiplyMatrices(byte[,] matrixA, byte[,] matrixB)
        {
            int rowsA = matrixA.GetLength(0);
            int columnsA = matrixA.GetLength(1);
            int rowsB = matrixB.GetLength(0);
            int columnsB = matrixB.GetLength(1);

            if (columnsA != rowsB)
            {
                throw new ArgumentException("Невозможно умножить матрицы. Количество столбцов в первой матрице должно быть равно количеству строк во второй матрице.");
            }

            byte[,] resultMatrix = new byte[rowsA, columnsB];

            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < columnsB; j++)
                {
                    byte sum = 0;
                    for (int k = 0; k < columnsA; k++)
                    {
                        sum += (byte)(matrixA[i, k] * matrixB[k, j]);
                    }
                    resultMatrix[i, j] = sum;
                }
            }

            return resultMatrix;
        }

        private List<byte[]> ConvertEncodedTextToListConstructions(string encodedText)
        {
            List<byte[]> listBytes = new List<byte[]>();

            for (int i = 0; i < encodedText.Length; i += 8)
            {
                string subString = encodedText.Substring(i, 8);
                byte[] subArray = new byte[8];

                for (int j = 0; j < subString.Length; j++)
                {
                    subArray[j] = Convert.ToByte(subString[j].ToString());
                }

                listBytes.Add(subArray);
            }

            return listBytes;
        }

        private List<byte[]> ConvertMatrixToListOfByteArray(byte[,] matrix)
        {
            List<byte[]> listBytes = new List<byte[]>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                byte[] rowBytes = new byte[matrix.GetLength(1)];

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    rowBytes[j] = matrix[i, j];
                }

                listBytes.Add(rowBytes);
            }

            return listBytes; 
        }

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

        private int[] DivStrBy4bit(string strBits)
        {
            int[] masBits = new int[(strBits.Length + 3) / 4];
            for (int i = 0; i < strBits.Length; i += 4)
            {
                string subStr = strBits.Substring(i, Math.Min(4, strBits.Length - i));
                masBits[i / 4] = Convert.ToInt32(subStr, 2);
            }
            return masBits;
        }

        private static int SumXor(int x, int gMatrix)
        {
            int m = 0b1000;
            int result = 0;
            for (int i = 3; i >= 0; i--)
            {
                result ^= (x & gMatrix & (m >> (3 - i))) >> i;
            }
            return result;
        }

        public int[] GenerateBits(int[] informativeBits)
        {
            int[] checkBits = new int[informativeBits.Length];

            for (int i = 0; i < checkBits.Length; i++)
            {
                checkBits[i] = (SumXor(informativeBits[i], G[4]) << 2) + (SumXor(informativeBits[i], G[5]) << 1) + SumXor(informativeBits[i], G[6]);
            }

            return checkBits;
        }

        public int[] ParityBit(int[] infBits, int[] checkBits)
        {
            int[] parityBit = new int[checkBits.Length];

            for (int i = 0; i < parityBit.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                    parityBit[i] ^= (infBits[i] & (1 << j)) >> j;

                for (int j = 0; j < 3; j++)
                    parityBit[i] ^= (checkBits[i] & (1 << j)) >> j;
            }

            return parityBit;
        }

        private byte[] SyndromeOf(byte[] u, byte[,] extendedMatrix)
        {
            var rows = extendedMatrix.GetLength(0);
            var columns = extendedMatrix.GetLength(1);
            var codeconstr = new byte[rows];

            for (var row = 0; row < rows; row++)
            {
                var xor = 0;

                for (var bit = 0; bit < columns; bit++)
                {
                    xor += u[bit] * extendedMatrix[row, bit];
                }

                codeconstr[row] = (byte)(xor % 2);
            }

            return codeconstr;
        }

        public List<byte[]> GetSyndromeList(IList<byte[]> constructions)
        {
            var result = new List<byte[]>();
            foreach (var construction in constructions)
            {
                result.Add(SyndromeOf(construction, H_Matrix));
            }

            return result;
        }

        public List<byte[]> GetRepairedConstructions(IList<byte[]> constructions, IList<byte[]> syndromes)
        {
            if (constructions.Count != syndromes.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            var repairedConstructions = new List<byte[]>();

            var constructionsAndSyndromes = constructions.Zip(syndromes, (c, s) => new { Construction = c, Syndrome = s });

            foreach (var cs in constructionsAndSyndromes)
            {
                if (CheckSyndromeForError(cs.Syndrome))
                {
                    var errorBit = FindErrorBitNumber(cs.Syndrome);
                    if (errorBit == -1)
                    {
                        repairedConstructions.Add(cs.Construction);
                    }
                    else
                    {
                        var _ = cs.Construction[errorBit] == 0 ? 1 : 0;
                        cs.Construction[errorBit] = (byte)_;
                        repairedConstructions.Add(cs.Construction);
                    }
                }
                else
                {
                    repairedConstructions.Add(cs.Construction);
                }

            }

            return repairedConstructions;
        }

        /// <summary>
        /// Проверяет код на ошибки по синдрому: true, если есть ошибки
        /// </summary>
        /// <param name="optionalSyndrome">S доп.+ синдром из 3 бит</param>
        /// <returns>True - есть ошибки</returns>
        private bool CheckSyndromeForError(byte[] optionalSyndrome)
        {
            if (optionalSyndrome.All(b => b == 0))
            {
                return false;
            }
            else if (optionalSyndrome.First() == 1)
            {
                return true;
            }

            return false;//четное число ошибок
        }

        private int FindErrorBitNumber(byte[] optionalSyndrome)
        {
            var num = -1;

            var bitcolumn = new byte[optionalSyndrome.Length];

            for (var col = 0; col < G_Matrix.GetLength(1); col++)
            {
                for (var row = 0; row < G_Matrix.GetLength(0); row++)
                {
                    bitcolumn[row] = G_Matrix[row, col];
                }

                if (!optionalSyndrome.SequenceEqual(bitcolumn)) continue;
                num = col;
                break;
            }

            return num;

        }

    }
}
