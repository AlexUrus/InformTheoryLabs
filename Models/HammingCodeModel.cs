using LR_1.Tools;
using LR_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LR_1.Models
{
    public class HammingCodeModel
    {
        public byte[,] H_Matrix = MatrixManager.HammingCodesMatrixWithParity;

        public byte[,] G_Matrix = MatrixManager.GeneratingMatrix;

        public ObservableCollection<SyndromeViewModel> SyndromeCollection;
        public ObservableCollection<CorrectionViewModel> Corrections;
        private HammingRepairTools _repairTools;
        public HammingCodeModel()
        {
            SyndromeCollection = new ObservableCollection<SyndromeViewModel>();
            Corrections = new ObservableCollection<CorrectionViewModel>();
        }

        public byte[][] GetEncodedMas(string text)
        {
            string strBits = ConvertToUnicode(text);
            string[] blocks = DivStrBy4bit(strBits);
            int blockLength = blocks[0].Length;
            var codematrix = new byte[blocks.Length][];

            for (var i = 0; i < blocks.Length; i++)
            {
                codematrix[i] = new byte[8];
            }

            for (var block = 0; block < blocks.Length; block++)
            {
                var bitblock = new byte[blockLength];
                for (var bit = 0; bit < blockLength; bit++)
                {
                    var b = blocks[block];
                    bitblock[bit] = (byte)char.GetNumericValue(b[bit]);
                }

                var vector = MultiplyVectorByGMatrix(bitblock, G_Matrix);

                for (var i = 0; i < vector.Length; i++)
                {
                    codematrix[block][i] = vector[i];
                }
            }

            return codematrix;
        }

        public string GetDecodedText(string encodedText)
        {
            SyndromeCollection.Clear();
            Corrections.Clear();

            _repairTools = new HammingRepairTools(MatrixManager.HammingCodesMatrixWithoutParity);

            List<byte[]> arrConstructions = ConvertEncodedTextToListConstructions(encodedText);

            var slist = _repairTools.GetSyndromeList(arrConstructions);
            foreach (var arr in slist)
            {
                SyndromeCollection.Add(new SyndromeViewModel(arr));
            }
            var arrcorrections = _repairTools.GetRepairedConstructions(arrConstructions, slist);
            for (var i = 0; i < arrcorrections.Count; i++)
            {
                Corrections.Add(new CorrectionViewModel(ConvertArrayToString(slist[i]), ConvertArrayToString(arrcorrections[i])));
            }

            return DecodeText(arrcorrections);
        }

        public string DecodeText(List<byte[]> arrcorrections)
        {
            List<byte[]> infBits = new List<byte[]>();
            foreach (var arr in arrcorrections)
            {
                byte[] infbit = { arr[1], arr[2], arr[3], arr[4]}; 
                infBits.Add(infbit);
            }

            var sb = new StringBuilder();

            foreach (var item in infBits)
            {
                foreach (var bit in item)
                {
                    sb.Append(bit);
                }
            }
            return DecodeBinaryToUnicode(sb.ToString());   

        }

        static string DecodeBinaryToUnicode(string binaryStr)
        {
            var sb = new StringBuilder();

            binaryStr = binaryStr.Trim();

            for (int i = 0; i < binaryStr.Length; i += 16) 
            {
                string byteStr = binaryStr.Substring(i, 16);
                int codepoint = Convert.ToInt32(byteStr, 2);
                sb.Append(char.ConvertFromUtf32(codepoint));
            }

            return sb.ToString();
        }

        private string ConvertArrayToString(byte[] arr)
        {
            var sb = new StringBuilder();
            foreach (var item in arr)
            {
                sb.Append(item);
            }
            return sb.ToString();
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

        private string[] DivStrBy4bit(string strBits)
        {
            string[] masBits = new string[(strBits.Length + 3) / 4];
            for (int i = 0; i < strBits.Length; i += 4)
            {
                string subStr = strBits.Substring(i, Math.Min(4, strBits.Length - i));
                masBits[i / 4] = subStr;
            }

            return masBits;
        }

        private byte[] MultiplyVectorByGMatrix(byte[] bitblock, byte[,] gmatrix)
        {
            var gColumns = gmatrix.GetLength(1);
            var codeconstr = new byte[gColumns + 1];
            for (var column = 0; column < gColumns; column++)
            {
                var xor = bitblock.Select((t, bit) => t * gmatrix[bit, column]).Sum();
                codeconstr[column + 1] = (byte)(xor % 2);
            }

            var allxor = codeconstr.Aggregate(0, (current, b) => current ^ b);
            codeconstr[0] = (byte)allxor;
            return codeconstr;
        }

        
    }
}
