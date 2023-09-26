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
                EncodeText = ConvertToUnicode(MessageText);
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

        private static string SimplifyEncoding(string kod2, List<YourCollectionType> collection2)
        {
            StringBuilder kod3 = new StringBuilder();

            for (int i = 0; i < kod2.Length; i += 4)
            {
                foreach (var collectionItem in collection2)
                {
                    int result = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        int kod2Bit = Convert.ToInt32(kod2[i + j]);
                        int collectionBit = Convert.ToInt32(collectionItem.First[j]);

                        result += (kod2Bit & collectionBit);
                    }
                    kod3.Append(result % 2);
                }
            }

            return kod3.ToString();
        }
    }
}
