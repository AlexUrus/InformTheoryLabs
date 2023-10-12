using LR_1.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.ViewModels
{
    public class HammingCodeViewModel : BaseViewModel
    {
        private string _encodeText;
        public string EncodeText
        {
            get => _encodeText;
            set => this.RaiseAndSetIfChanged(ref _encodeText, value);
        }

        int[,] H_Matrix = new int[4, 8]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1},
            { 0, 1, 1, 0, 1, 1, 0, 0},
            { 0, 1, 1, 1, 0, 0, 1, 0},
            { 0, 1, 1, 0, 1, 0, 0, 1}
        };
        
        public int[,] G_Matrix = new int[4, 7]
        {
            { 1, 0, 0, 0, 1, 1, 1},
            { 0, 1, 0, 0, 1, 1, 0},
            { 0, 0, 1, 0, 0, 1, 1},
            { 0, 0, 0, 1, 1, 0, 1}
        };
        int[] G = {8, 4, 2, 1, 13, 14, 11};

        public string G_MatrixString
        {
            get 
            {
                return ConvertMatrixtoString(G_Matrix);
            }
        }
        public string H_MatrixString
        {
            get
            {
                return ConvertMatrixtoString(H_Matrix);
            }
        }


        private string _messageText;
        public string MessageText
        {
            get => _messageText;
            set => this.RaiseAndSetIfChanged(ref _messageText, value);
        }

        private string _decodeText;
        public string DecodeText
        {
            get => _decodeText;
            set => this.RaiseAndSetIfChanged(ref _decodeText, value);
        }
        public ReactiveCommand<Unit,Unit> EncodeTextCommand { get; }

        private ObservableCollection<HilbertMooreField> _mooreFields;
        public ObservableCollection<HilbertMooreField> MooreFields
        {
            get => _mooreFields;
            set => this.RaiseAndSetIfChanged(ref _mooreFields, value);
        }

        public HammingCodeViewModel()
        {
            EncodeTextCommand = ReactiveCommand.Create(EncodingText);
            _model = new HammingCodeModel();
        }

        public void EncodingText()
        {
            if(MessageText != null && MessageText != string.Empty) 
            {
                int[] encodedBits = GetFullCode();
                EncodeText = ConvertMasIntToStringBytes(encodedBits);
            }
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

        private string ConvertMatrixtoString(int[,] matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sb.Append(matrix[i, j].ToString() + " ");
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        private string ConvertMasIntToStringBytes(int[] masBits)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int bits in masBits)
            {
                sb.Append(Convert.ToString(bits, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }

        static int SumXor(int x, int gMatrix)
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
        private int[] GetFullCode()
        {
            string strBits = ConvertToUnicode(MessageText);
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
    }
}
