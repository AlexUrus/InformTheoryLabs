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

        int[,] H = new int[3, 7]
        {
            { 1, 1, 0, 1, 1, 0, 0},
            { 1, 1, 1, 0, 0, 1, 0},
            { 1, 1, 0, 1, 0, 0, 1}
        };

        /*
        int[,] G = new int[4, 7]
        {
            { 1, 0, 0, 0, 1, 1, 1},
            { 0, 1, 0, 0, 1, 1, 0},
            { 0, 0, 1, 0, 0, 1, 1},
            { 0, 0, 0, 1, 1, 0, 1}
        };*/

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
        public ReactiveCommand<Unit, Unit> CheckKraftInequalityCommand { get; }
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
            MooreFields = new ObservableCollection<HilbertMooreField>();
        }

        public void EncodingText()
        {
            if(MessageText != null && MessageText != string.Empty) 
            {

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

        int[] G = { 8, 4, 2, 1, 11, 13, 7 };

        public string BinaryCode(string message, string alphabet, string[] code)
        {
            string binaryMessage = "";

            for (int i = 0; i < message.Length; i++)
            {
                int j = 0;
                while (message[i] != alphabet[j])
                {
                    j++;
                }
                binaryMessage += code[j];
            }
            binaryMessage = Padding(binaryMessage);

            return binaryMessage;
        }

        public string Padding(string binaryMessage)
        {
            binaryMessage += "1";
            while (binaryMessage.Length % 4 != 0)
            {
                binaryMessage += "0";
            }

            return binaryMessage;
        }

        public int[] InfBits(string binaryCode)  //инф биты в группы по 4
        {
            int[] infBits = new int[binaryCode.Length / 4];
            int i = 0;
            while (binaryCode.Length != 0)
            {
                infBits[i] = Convert.ToInt32(binaryCode.Substring(0, 4), 2);
                binaryCode = binaryCode.Substring(4);
                i++;
            }

            return infBits;
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

        public int[] CheckBits(int[] informativeBits)              //проверочный бит
        {
            int[] checkBits = new int[informativeBits.Length];


            for (int i = 0; i < checkBits.Length; i++)
            {
                checkBits[i] = (SumXor(informativeBits[i], G[4]) << 2) + (SumXor(informativeBits[i], G[5]) << 1) + SumXor(informativeBits[i], G[6]);
            }

            return checkBits;
        }

        public int[] ParityBit(int[] infBits, int[] checkBits)   // бит четности
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

        public int[] infBits { get; private set; }
        public int[] checkBits { get; private set; }
        public int[] parityBit { get; private set; }

        public int[] getFullCode(string message, string alphabet, string[] code)
        {
            infBits = InfBits(BinaryCode(message, alphabet, code));
            checkBits = CheckBits(infBits);
            parityBit = ParityBit(infBits, checkBits);

            int[] fullCode = new int[infBits.Length];

            for (int i = 0; i < fullCode.Length; i++)
            {
                fullCode[i] += (infBits[i] << 4) + (checkBits[i] << 1) + (parityBit[i]);
            }

            return fullCode;
        }
    }
}
