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
        private HammingCodeModel _model;

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

        public string G_MatrixString
        {
            get 
            {
                return ConvertMatrixtoString(_model.G_Matrix);
            }
        }
        public string H_MatrixString
        {
            get
            {
                return ConvertMatrixtoString(_model.H_Matrix);
            }
        }

        private ObservableCollection<SyndromeViewModel> _syndromeCollection;
        public ObservableCollection<SyndromeViewModel> SyndromeCollection
        {
            get => _syndromeCollection;
            set => this.RaiseAndSetIfChanged(ref _syndromeCollection, value);
        }

        private ObservableCollection<CorrectionViewModel> _corrections;
        public ObservableCollection<CorrectionViewModel> Corrections
        {
            get => _corrections;
            set => this.RaiseAndSetIfChanged(ref _corrections, value);
        }

        public ReactiveCommand<Unit,Unit> EncodeTextCommand { get; }
        public ReactiveCommand<Unit, Unit> DecodeTextCommand { get; }

        private ObservableCollection<HilbertMooreField> _mooreFields;
        public ObservableCollection<HilbertMooreField> MooreFields
        {
            get => _mooreFields;
            set => this.RaiseAndSetIfChanged(ref _mooreFields, value);
        }

        public HammingCodeViewModel()
        {
            EncodeTextCommand = ReactiveCommand.Create(EncodingText);
            DecodeTextCommand = ReactiveCommand.Create(DecodingText );
            _model = new HammingCodeModel();
        }

        public void EncodingText()
        {
            if(MessageText != null && MessageText != string.Empty) 
            {
                byte[][] encodedBits = _model.GetEncodedMas(MessageText);
                EncodeText = ConvertMasIntToStringBytes(encodedBits);
            }
        }

        public void DecodingText()
        {
            if(EncodeText != null && EncodeText != string.Empty)
            {
                DecodeText = _model.GetDecodedText(EncodeText); 
            }
            Corrections = _model.Corrections;
            SyndromeCollection = _model.SyndromeCollection;
        }

        private string ConvertMatrixtoString(byte[,] matrix)
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

        private string ConvertMasIntToStringBytes(byte[][] masBits)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte[] block in masBits)
            {
                foreach (byte bit in block)
                {
                    sb.Append(Convert.ToString(bit, 2));
                }
            }
            return sb.ToString();
        }
    }
}
