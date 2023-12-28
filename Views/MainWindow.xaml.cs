using LR_1.Views;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LR_1.ViewModels
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Task_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Задание:\r\n 1 Изучить алгоритм побуквенного кодирования:\r\nМетод Гилберта-Мура.\r\n\r\n2 Разработать тестовые примеры.\r\n\r\n3 Исследовать полученный код на оптимальность:\r\nС помощью неравенства Крафта\r\n\r\n4 Оценить избыточность сообщения\r\nИзбыточность кода в бит/символ или относительную избыточность кода в процентах.\r\n\r\n5 Изучить алгоритм помехоустойчивого кодирования: матричный способ расширенного кода Хэмминга (7, 4).\r\n Рассмотреть случаи: нет ошибки; 1 ошибка в бите проверки четности, в информативном и проверочном битах; 2, 3 ошибки в произвольных битах.\r\nПримечание: подробно вычислить расширенный синдром для кодовой комбинации с однократной ошибкой в информативном бите; продемонстрировать, как определяется местоположение ошибки и как ошибка устраняется.\r\n\r\n6 Оценить статистические характеристики полученного кода:\r\nВычислите границу Варшамова-Гильберта.\r\n\r\n7 Разработать графическое приложение метода побуквенного и помехоустойчивого кодирования с имитацией передачи информации по открытому, зашумленному каналу связи (окно кодирования и окно декодирования).\r\n");
        }

        private void AboutDev_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Разработчик - Урусов Александр Андреевич гр. Бист-311");
        }
    }
}
