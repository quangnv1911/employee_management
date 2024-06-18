using Services.Impl;
using Services;
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
using System.Windows.Shapes;
using BusinessObjects;
using System.Security.Principal;

namespace Group4_WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private readonly IAccountService accountService;

        public LoginWindow()
        {
            InitializeComponent();
            accountService = new AccountService();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AccountMember? accountMember = accountService.GetAccountByEmailAndPass(txtUser.Text, txtPass.Password);
            if (accountMember != null)
            {
                MainWindow mainWindow = new MainWindow();

                // Lấy tên người dùng. Nếu người dùng là admin thì tự động đổi thành admin
                string userId;
                if (accountMember.EmployeeId == null)
                {
                    userId = "admin";
                }
                else
                {
                    userId = accountMember.EmployeeId.ToString();
                }
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(userId), [accountMember.Role]);

                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong login information!");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
