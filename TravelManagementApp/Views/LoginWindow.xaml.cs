using System.Windows;
using TravelManagementApp.Services;
using TravelDataAccess.Models;

namespace TravelManagementApp.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthenticationService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthenticationService();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = string.Empty;

            string code = txtCode.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(password))
            {
                lblError.Content = "Please enter Code and Password";
                return;
            }

            Customer? customer = _authService.Login(code, password);

            if (customer != null)
            {
                // Login success
                MainWindow mainWindow = new MainWindow(customer);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                // Login failed
                lblError.Content = "Invalid Code or Password";
            }
        }
    }
}