using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Models
{
    public class MatrixManager
    {
        public byte[,] HammingCodesMatrix { get; set; }
        public byte[,] GeneratingMatrix { get; set; }

        public MatrixManager()
        {
            HammingCodesMatrix = new byte[3, 7]
            {
                { 1, 1, 0, 1, 1, 0, 0}, //216 
                { 1, 1, 1, 0, 0, 1, 0}, //228
                { 1, 1, 0, 1, 0, 0, 1}  //210
            };
            GeneratingMatrix = new byte[4, 7]
            {
                { 1, 0, 0, 0, 1, 1, 1},
                { 0, 1, 0, 0, 1, 1, 0},
                { 0, 0, 1, 0, 0, 1, 1},
                { 0, 0, 0, 1, 1, 0, 1}
            };
        }

        public static T[,] TransposeMatrix<T>(T[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            T[,] Tmatrix = new T[columns, rows];

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    Tmatrix[j, i] = matrix[i, j];
                }
            }

            return Tmatrix;
        }

        public byte[][] GetCodeConstructionsFromCipher(string cipher)
        {
            var blocks = SplitIntoBlocks(cipher, 4);
            var blockLength = blocks[0].Length;
            var codematrix = new byte[blocks.Length][];

            for (var i = 0; i < blocks.Length; i++)
            {
                codematrix[i] = new byte[8];
            }

            //get block, parse it to byte array and * matrix
            for (var block = 0; block < blocks.Length; block++)
            {
                var bitblock = new byte[blockLength];
                for (var bit = 0; bit < blockLength; bit++)
                {
                    var b = blocks[block];
                    bitblock[bit] = (byte)char.GetNumericValue(b[bit]);
                }

                var vector = MultiplyVectorByGMatrix(bitblock, GeneratingMatrix);

                for (var i = 0; i < vector.Length; i++)
                {
                    codematrix[block][i] = vector[i];
                }
            }

            return codematrix;
        }

        private byte[] MultiplyVectorByGMatrix(byte[] bitblock, byte[,] gmatrix)
        {
            var gColumns = gmatrix.GetLength(1);
            var codeconstr = new byte[gColumns + 1];
            for (var column = 0; column < gColumns; column++)//columns G (7)
            {
                var xor = bitblock.Select((t, bit) => t * gmatrix[bit, column]).Sum();
                codeconstr[column + 1] = (byte)(xor % 2);
            }
            //xor all bits
            var allxor = codeconstr.Aggregate(0, (current, b) => current ^ b);
            codeconstr[0] = (byte)allxor;
            return codeconstr;
        }

        private string[] SplitIntoBlocks(string input, int key)
        {
            for (var i = 0; i < input.Length % key; i++)
            {
                if (i < 1)
                {
                    input += '1';
                }
                else
                {
                    input += '0';
                }
            }

            var stringSplit = input.Select((c, index) => new { c, index })
                .GroupBy(x => x.index / key)
                .Select(group => group.Select(item => item.c))
                .Select(chars => new string(chars.ToArray()));
            return stringSplit.ToArray();
        }
    }
}
