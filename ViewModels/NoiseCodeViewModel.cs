using LR_1.Models;
using LR_1.Tools;
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
    public class NoiseCodeViewModel : BaseViewModel
    {
        private EntropyCalculation _model;

        private ObservableCollection<CountSelfInfField> _countSelfInfList;
        public ObservableCollection<CountSelfInfField> CountSelfInfList
        {
            get => _countSelfInfList;
            set => this.RaiseAndSetIfChanged(ref _countSelfInfList, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this.RaiseAndSetIfChanged(ref _message, value);
                _model.Message = value;
            }
        }

        private double _entropyFirstStage;
        public double EntropyFirstStage 
        { 
            get => _entropyFirstStage;
            set => this.RaiseAndSetIfChanged(ref _entropyFirstStage, value);
        }

        private double _maxEntropy;
        public double MaxEntropy
        {
            get => _maxEntropy;
            set => this.RaiseAndSetIfChanged(ref _maxEntropy, value);
        }

        private double _ansambl;
        public double Ansambl
        {
            get => _ansambl;
            set => this.RaiseAndSetIfChanged(ref _ansambl, value);
        }

        public ReactiveCommand<Unit, Unit> CalcAllFieldsCommand { get; }
        public NoiseCodeViewModel()
        {
            _model = new EntropyCalculation();
            CalcAllFieldsCommand = ReactiveCommand.Create(CalcAllFields);
        }

        public void CalcAllFields()
        {
            _model.CalcAllFields();
            EntropyFirstStage = _model.EntropyFirstStage;
            CountSelfInfList = new (_model.CountSelfInfList);
            MaxEntropy = _model.MaxEntropy;
            Ansambl = _model.Ansambl;
        }
    }
}
