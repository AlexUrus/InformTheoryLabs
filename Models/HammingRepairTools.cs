using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Models
{
    public class HammingRepairTools
    {
        public byte[,] HammingCodesMatrix { get; private set; }//исходная проверочная матрица
        public byte[,] ExtendedMatrix { get; private set; }//расширенная матрица Хэмминга

        public HammingRepairTools(byte[,] checkcodesMatrix)
        {
            HammingCodesMatrix = checkcodesMatrix;
            ExtendedMatrix = GetExtendedMatrix(HammingCodesMatrix);
        }

        private byte[,] GetExtendedMatrix(byte[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var colums = matrix.GetLength(1);
            var extended = new byte[rows + 1, colums + 1];
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < colums; col++)
                {
                    extended[row + 1, col + 1] = matrix[row, col];
                }
            }

            for (var i = 1; i < extended.GetLength(0); i++)
            {
                extended[i, 0] = 0;
            }

            for (var i = 0; i < extended.GetLength(1); i++)
            {
                extended[0, i] = 1;
            }
            return extended;
        }

        private byte[] SyndromeOf(byte[] u, byte[,] extendedMatrix)
        {
            var rows = extendedMatrix.GetLength(0);
            var columns = extendedMatrix.GetLength(1);
            var codeconstr = new byte[rows];
            for (var row = 0; row < rows; row++)
            {
                var xor = u.Select((t, bit) => t * extendedMatrix[row, bit]).Sum();
                codeconstr[row] = (byte)(xor % 2);
            }

            return codeconstr;
        }

        public List<byte[]> GetSyndromeList(IList<byte[]> constructions)
        {
            var result = new List<byte[]>();
            foreach (var construction in constructions)
            {
                result.Add(SyndromeOf(construction, ExtendedMatrix));
            }

            return result;
        }

        /// <summary>
        /// Получает восстановленные кодовые конструкции по расширенным синдромам
        /// </summary>
        /// <param name="constructions">список кодовых конструкций</param>
        /// <param name="syndromes">список расширенных синдромов</param>
        /// <returns></returns>
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

            for (var col = 0; col < ExtendedMatrix.GetLength(1); col++)
            {
                for (var row = 0; row < ExtendedMatrix.GetLength(0); row++)
                {
                    bitcolumn[row] = ExtendedMatrix[row, col];
                }

                if (!optionalSyndrome.SequenceEqual(bitcolumn)) continue;
                num = col;
                break;
            }

            return num;

        }
    }
}
