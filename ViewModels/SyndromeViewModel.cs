using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.ViewModels
{
    public class SyndromeViewModel : BaseViewModel
    {
        public SyndromeViewModel(byte[] vector)
        {
            SOptional = vector.FirstOrDefault();
            Syndrome = string.Join(" ", vector.Skip(1));
        }

        private byte _soptional;
        public byte SOptional
        {
            get => _soptional; 
            set => this.RaiseAndSetIfChanged(ref _soptional, value);
        }

        private string _syndrome;
        public string Syndrome
        {
            get => _syndrome; 
            set => this.RaiseAndSetIfChanged(ref _syndrome, value);
        }
    }
}
