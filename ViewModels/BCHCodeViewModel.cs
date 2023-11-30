using LR_1.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.ViewModels
{
    public class BCHCodeViewModel : BaseViewModel
    {
        private BCHCodeModel _model;

        private string _encodedMessage;
        public string EncodedMessage
        {
            get => _encodedMessage;
            set => this.RaiseAndSetIfChanged(ref _encodedMessage, value);
        }
        private string _messageInfBytes;
        public string MessageInfBytes
        {
            get => _messageInfBytes;
            set => this.RaiseAndSetIfChanged(ref _messageInfBytes, value);
        }

        private string _decodedMessage;
        public string DecodedMessage
        {
            get => _decodedMessage;
            set => this.RaiseAndSetIfChanged(ref _decodedMessage, value);
        }

        public string GenPolynom
        {
            get
            {
                return _model.GenPolynom;
            }
        }

        public ReactiveCommand<Unit, Unit> EncodeCommand { get; }
        public ReactiveCommand<Unit, Unit> DecodeCommand { get; }

        public BCHCodeViewModel()
        {
            _model = new BCHCodeModel(31, 16, 15, "1000111110101111");
            EncodeCommand = ReactiveCommand.Create(EncodingText);
            DecodeCommand = ReactiveCommand.Create(DecodingText);
        }

        public void EncodingText()
        {
            if (MessageInfBytes != "")
            {
                EncodedMessage = _model.EncodeMessage(MessageInfBytes);
            }
        }

        public void DecodingText()
        {
            if (EncodedMessage != "")
            {
                DecodedMessage = _model.TryDecodeMessage(EncodedMessage);
            }
        }
    }
}
