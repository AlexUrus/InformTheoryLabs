using LR_1.Models;
using ReactiveUI;
using Splat.ModeDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.ViewModels
{
    public class ReseiverViewModel : BaseViewModel
    {
        private HammingCodeModel _hammingCodeModel;
        public string _recievedMessage;
        public string RecievedMessage
        {
            get => _recievedMessage;
            set => this.RaiseAndSetIfChanged(ref _recievedMessage, value);
        }

        private string _decodedText;
        public string DecodedText
        {
            get => _decodedText;
            set => this.RaiseAndSetIfChanged(ref _decodedText, value);
        }
        public ReactiveCommand<Unit, Unit> DecodeTextCommand { get; }

        public ReseiverViewModel(string recievedMessage)
        {
            _hammingCodeModel = new HammingCodeModel();
            RecievedMessage = recievedMessage;
            DecodeTextCommand = ReactiveCommand.Create(DecodingText);
        }

        public void DecodingText()
        {
            DecodedText = _hammingCodeModel.GetDecodedText(RecievedMessage);
            DecodedText = HilbertMooreEncoding.DecodingMessage(DecodedText);
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
