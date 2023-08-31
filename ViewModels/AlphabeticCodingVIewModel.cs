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
    public class AlphabeticCodingVIewModel : BaseViewModel
    {
        public ReactiveCommand<Unit, Unit> CheckKraftInequalityCommand { get; }
        public ReactiveCommand<Unit, Unit> HilbertMooreEncodeCommand { get; }
        
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

        private string _testText;
        public string TestText
        {
            get => _testText;
            set => this.RaiseAndSetIfChanged(ref _testText, value);
        }
        
        private string _codeIsOptimal;
        public string CodeIsOptimal
        {
            get => _codeIsOptimal;
            set => this.RaiseAndSetIfChanged(ref _codeIsOptimal, value);
        }

        private string _redundancy;
        public string Redundancy
        {
            get => _redundancy;
            set => this.RaiseAndSetIfChanged(ref _redundancy, value);
        }

        private ObservableCollection<HilbertMooreField> _mooreFields;
        public ObservableCollection<HilbertMooreField> MooreFields
        {
            get => _mooreFields;
            set => this.RaiseAndSetIfChanged(ref _mooreFields, value);
        }

        public AlphabeticCodingVIewModel()
        {
            //CheckKraftInequalityCommand = ReactiveCommand.Create(CheckKraftInequality);
            HilbertMooreEncodeCommand = ReactiveCommand.Create(HilbertMooreEncode);
            MooreFields = new ObservableCollection<HilbertMooreField>();
        }

        private void CheckKraftInequality()
        {
            if (KraftInequality.IsOptimalCode(_messageText))
            {
                CodeIsOptimal = "Оптимальный";
            }
            else
            {
                CodeIsOptimal = "Не Оптимальный";
            }
        }
        private void HilbertMooreEncode()
        {
            HilbertMooreEncoding mooreEncoding = new HilbertMooreEncoding(_messageText);
            mooreEncoding.EncodingMessage();
            MooreFields = mooreEncoding.hilbertMooreFields;
            EncodeText = mooreEncoding.EncodeText;
            Redundancy = mooreEncoding.Redundancy.ToString();
            CheckKraftInequality();
        }


    }
}
