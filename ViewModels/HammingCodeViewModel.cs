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

        private HammingCodeModel _model;

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
                //EncodeText = 
                EncodeText = _model.GetHammingEncode(_messageText);
            }
        }

        

    }
}
