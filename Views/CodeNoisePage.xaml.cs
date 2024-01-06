using LR_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LR_1.Views
{
    /// <summary>
    /// Логика взаимодействия для CodeNoisePage.xaml
    /// </summary>
    public partial class CodeNoisePage : Page
    {
        NoiseCodeViewModel NoiseCodeViewModel { get; set; }
        int[] size = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        ObservableCollection<ObservableCollection<double>> Matrix
        {
            get => NoiseCodeViewModel.Matrix;
            set => NoiseCodeViewModel.Matrix = value;
        }

        public CodeNoisePage()
        {
            InitializeComponent();
            NoiseCodeViewModel = new NoiseCodeViewModel();
            DataContext = NoiseCodeViewModel;
            cbSize.ItemsSource = size;

            dgMatrix.ItemsSource = Matrix;
            if (Matrix.Count > 0)
            {
                dgMatrix = CreateDataGrid(dgMatrix, Matrix[0].Count);
                tbResult.Text = Summ(Matrix).ToString();
            }

        }

        private void dgMatrix_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private ObservableCollection<ObservableCollection<double>> CreateMatrix(int countRow, int countColumn, int left, int right)
        {
            Matrix.Clear();
            if (countRow > 0 && countColumn > 0)
            {
                Random rnd = new Random();
                if (left > right) { int swap = left; left = right; right = swap; }
                for (int i = 0; i < countRow; i++)
                {
                    Matrix.Add(new ObservableCollection<double>()); 
                    for (int j = 0; j < countColumn; j++)
                        Matrix[i].Add(rnd.Next(left, right));

                    Matrix[i][i] = 0;
                }
            }
            return Matrix;
        }

        private void CbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Matrix.Clear();
            dgMatrix.Columns.Clear();
            tbResult.Clear();
            if (cbSize.SelectedIndex > -1)
            {
                if (int.TryParse(leftBorder.Text, out int left) && int.TryParse(rightBorder.Text, out int right))
                {
                    int count = size[cbSize.SelectedIndex];
                    Matrix = CreateMatrix(count, count, left, right);
                    dgMatrix = CreateDataGrid(dgMatrix, count);
  
                    tbResult.Text = Summ(Matrix).ToString();
                }
                else
                {
                    leftBorder.Clear();
                    rightBorder.Clear();
                    cbSize.SelectedIndex = -1;
                }
            }
        }

        private DataGrid CreateDataGrid(DataGrid dgMatrix, int countColumn)
        {
            dgMatrix.Columns.Clear();
            if (countColumn > 0)
            {
                for (int i = 0; i < countColumn; i++)
                {

                    DataGridTextColumn column = new DataGridTextColumn
                    {
                        Header = (i + 1).ToString(),
                        Binding = new Binding(String.Format("[{0}]", i))
                    };
                    dgMatrix.Columns.Add(column);
                }
            }
            return dgMatrix;
        }

        private double Summ(ObservableCollection<ObservableCollection<double>> matrix)
        {
            double sum = 0;
            for (int i = 0; i < matrix.Count; i++)
                for (int j = 0; j < matrix[i].Count; j++)
                    sum += matrix[i][j];
            return sum;
        }

        private void dgMatrix_LostFocus(object sender, RoutedEventArgs e)
        {
            tbResult.Text = Summ(Matrix).ToString(); 
        }

        private void dgMatrix_KeyUp(object sender, KeyEventArgs e)
        {
            dgMatrix_LostFocus(this, null);
        }
    }
}
