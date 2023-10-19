using DynamicData.Kernel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.ViewModels
{
    public class CorrectionViewModel : BaseViewModel
    {
        public CorrectionViewModel(byte[] errorType, byte[] correction)
        {
            ErrorType = errorType;
            CorrectConstruction = correction;
        }

        private byte[] _correctConstruction;
        public byte[] CorrectConstruction
        {
            get => _correctConstruction; 
            set => this.RaiseAndSetIfChanged(ref _correctConstruction, value);
        }

        private byte[] _errorType;
        public byte[] ErrorType
        {
            get => _errorType; 
            set => this.RaiseAndSetIfChanged(ref _errorType, value);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CorrectConstr: ");
            foreach (var item in _correctConstruction)
            {
                stringBuilder.Append(item.ToString() + " ");
            }
            stringBuilder.Append(";");
            stringBuilder.Append("ErrorType: ");
            foreach (var item in _errorType)
            {
                stringBuilder.Append(item.ToString() + " ");
            }

            return stringBuilder.ToString();
        }
    }
}
