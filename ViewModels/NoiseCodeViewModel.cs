using LR_1.Tools;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.ViewModels
{
    public class NoiseCodeViewModel: BaseViewModel
    {
        private EntropyCalculation _model;
        public ReactiveCommand<Unit, Unit> CalcEntropyCommand { get; }
        public NoiseCodeViewModel()
        {
            CalcEntropyCommand = ReactiveCommand.Create(CalcEntropy);
            _model = new EntropyCalculation();
        }

        public void CalcEntropy()
        {
            _model.CalcAllFields();
        }
    }
}
