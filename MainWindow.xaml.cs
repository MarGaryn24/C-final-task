using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Final_Project
{
    public partial class MainWindow : Window
    {
        private Password_Manager _passwordManager;
        public MainWindow()
        {
            InitializeComponent();
            _passwordManager = new Password_Manager();
        }
        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            var password = PasswordTextBox.Text;
            if (!string.IsNullOrEmpty(password))
            {
                _passwordManager.EncryptAndSavePassword(password);
                ResultTextBlock.Text = "Password encrypted and saved.";
            }
            else
            {
                ResultTextBlock.Text = "Please enter a password.";
            }
        }
        private async void BruteForceButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBlock.Text = "Brute force attack in progress...";
            await Task.Run(() =>
            {
                var result = _passwordManager.BruteForceAttack();
                Dispatcher.Invoke(() => ResultTextBlock.Text = result);
            });
        }
    }
}