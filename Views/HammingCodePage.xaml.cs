using LR_1.ViewModels;
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

namespace LR_1.Views
{
    /// <summary>
    /// Логика взаимодействия для HammingCodePage.xaml
    /// </summary>
    public partial class HammingCodePage : Page
    {
        public HammingCodePage()
        {
            InitializeComponent();
        }

        private void SendEncodedText_Click(object sender, RoutedEventArgs e)
        {
            var receiverWindow = new RecieverWindow();
            receiverWindow.DataContext = new ReseiverViewModel(HammingEncodeText.Text);
            receiverWindow.Show();
        }
    }
}
