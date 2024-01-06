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
using System.Windows.Media;

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

        private double _averageMutualInf;
        public double AverageMutualInf
        {
            get => _averageMutualInf;
            set => this.RaiseAndSetIfChanged(ref _averageMutualInf, value);
        }

        private double _ansambl;
        public double Ansambl
        {
            get => _ansambl;
            set => this.RaiseAndSetIfChanged(ref _ansambl, value);
        }

        public ObservableCollection<ObservableCollection<double>> Matrix { get; set; }

        public Dictionary<string, double> AnsamblDictionary {get; set;}

        public ReactiveCommand<Unit, Unit> CalcAllFieldsCommand { get; }
        public NoiseCodeViewModel()
        {
            _model = new EntropyCalculation();
            CalcAllFieldsCommand = ReactiveCommand.Create(CalcAllFields);
            Matrix = new ObservableCollection<ObservableCollection<double>>()
            {
                new ObservableCollection<double> {  0.9, 0.1},
                new ObservableCollection<double> {  0.2, 0.8 },
            };
        }

        public void CalcAllFields()
        {
            _model.CalcAllFields();
            EntropyFirstStage = _model.EntropyFirstStage;
            CountSelfInfList = new (_model.CountSelfInfList);
            MaxEntropy = _model.MaxEntropy;
            Ansambl = _model.Ansambl;
            AnsamblDictionary = GenerateAnsambl(Message);
            AverageMutualInf = CalcAverageMutualInf(AnsamblDictionary,Matrix);
        }

        private static Dictionary<string, double> GenerateAnsambl(string message)
        {
            var ansambl = new Dictionary<string, double>();
            for (int i = 0; i < message.Length; i++)
            {
                if (!ansambl.ContainsKey(message[i].ToString()))
                {
                    ansambl.Add(message[i].ToString(), ProbabilityOfOccurrence(message, i));
                }
            }
            return ansambl;
        }
        private static double ProbabilityOfOccurrence(string message, int index)
        {
            return (double)message.Count(x => x == message[index]) / message.Length;
        }

        public static double CalcAverageMutualInf(Dictionary<string, double> ansambl, ObservableCollection<ObservableCollection<double>> matrix)
        {
            double I = 0.0;
            List<double> marginalY = new List<double>();

            for (int y = 0; y < matrix[0].Count; y++)
            {
                double marginalProb = 0.0;
                for (int x = 0; x < matrix.Count; x++)
                {
                    marginalProb += ansambl.ElementAt(x).Value * matrix[x][y];
                }
                marginalY.Add(marginalProb);
            }
            double[,] Pxy = MatrixTransition(ansambl, matrix);

            for (int i = 0; i < Pxy.GetLength(0); i++)
            {
                for (int j = 0; j < Pxy.GetLength(1); j++)
                {
                    I += Pxy[i, j] * (Math.Log10(matrix[j][i] / marginalY.ElementAt(j)));
                }

            }
            return I;
        }

        private static double[,] MatrixTransition(Dictionary<string, double> Ansambl, ObservableCollection<ObservableCollection<double>> matrix)
        {
            double[,] Pxy = new double[matrix.Count, matrix[0].Count];
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    var item = Ansambl.ElementAt(i);
                    var itemValue = item.Value;
                    Pxy[i, j] = matrix[i][j] * itemValue;
                }
            }
            return Pxy;
        }
    }
}
