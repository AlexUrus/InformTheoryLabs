using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR_1.Models
{
    public class FieldMatrix : ReactiveObject
    {
        private int _value;
        public int Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }
        public FieldMatrix(int x)
        {
            Value = x;
        }
    }
}
