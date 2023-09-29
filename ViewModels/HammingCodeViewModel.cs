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


        int[,] G = new int[4, 7]
        {
            { 1, 0, 0, 0, 1, 1, 1},
            { 0, 1, 0, 0, 1, 1, 0},
            { 0, 0, 1, 0, 0, 1, 1},
            { 0, 0, 0, 1, 1, 0, 1}
        };

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
                EncodeText = ApplyExtendedHammingCode(MessageText);
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

        private string ApplyExtendedHammingCode(string input)
        {
            StringBuilder encodedString = new StringBuilder();

            // Пройдемся по каждому символу входной строки
            foreach (char c in input)
            {
                // Конвертируем символ в его двоичное представление и добавляем нули слева до 16 бит
                string binaryChar = Convert.ToString(c, 2).PadLeft(16, '0');

                // Пройдемся по каждым 4 битам и применим расширенный код Хемминга
                for (int i = 0; i < 16; i += 4)
                {
                    StringBuilder encodedGroup = new StringBuilder();
                    for (int j = 0; j < 3; j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < 7; k++)
                        {
                            int bit = (binaryChar[i + k] - '0') & G[j, k];
                            sum += bit;
                        }
                        encodedGroup.Append((sum % 2).ToString());
                    }
                    encodedString.Append(encodedGroup.ToString());
                }
            }

            return encodedString.ToString();
        }

        private string DecodeHammingCode(string input, int[,] hammingMatrix)
        {
            StringBuilder decodedString = new StringBuilder();
            for (int i = 0; i < input.Length; i += 8)
            {
                int[] syndrome = new int[3];
                int[] receivedData = new int[7];

                // Заполняем массив приемных данных и вычисляем синдромы.
                for (int j = 0; j < 7; j++)
                {
                    receivedData[j] = input[i + j + 1] - '0';
                    for (int k = 0; k < 3; k++)
                    {
                        syndrome[k] += receivedData[j] & hammingMatrix[k, j];
                    }
                }

                // Проверяем синдромы.
                bool hasError = false;
                for (int j = 0; j < 3; j++)
                {
                    syndrome[j] %= 2;
                    if (syndrome[j] != 0)
                    {
                        hasError = true;
                        break;
                    }
                }

                // Если есть ошибка, пытаемся исправить ее.
                if (hasError)
                {
                    int errorBit = syndrome[0] * 1 + syndrome[1] * 2 + syndrome[2] * 4;
                    receivedData[errorBit - 1] ^= 1; // Инвертируем бит с ошибкой.
                }

                // Добавляем биты данных к результату.
                for (int j = 0; j < 7; j++)
                {
                    decodedString.Append(receivedData[j]);
                }
            }
            return decodedString.ToString();
        }
    }
}
