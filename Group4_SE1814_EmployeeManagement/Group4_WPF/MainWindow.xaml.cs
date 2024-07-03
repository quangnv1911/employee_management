using BusinessObjects;
using Group4_WPF.Control;
using Services;
using Services.Impl;
using System.Collections.ObjectModel;
using System.Security.Principal;
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

namespace Group4_WPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEmployeeService employeeService;
        public MainWindow()
        {
            InitializeComponent();
            employeeService = new EmployeeService();
            ccDisplayContent.Content = new HomeView();
        } 

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUserName();
            UpdateLasLogin();
         
        }
        private void UpdateUserName()
        {
            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            if (threadPrincipal != null)
            {
                if (threadPrincipal.Identity.Name == "admin")
                {
                    lbUserName.Content = "Hello " + "ADMIN";
                }
                else
                {
                    if (threadPrincipal.Identity.Name != null)
                    {
                        int id;
                        int.TryParse(threadPrincipal.Identity.Name, out id);
                        lbUserName.Content = "Hello" +employeeService.GetEmployeeName(id);
                    }
                    else
                    {
                        return;
                    }

                }
            }
        }

        //private void nvListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (nvListView.SelectedItem is ListBoxItem selectedItem)
        //    {
        //        string tag = selectedItem.Tag as string;
        //        switch (tag)
        //        {
        //            case "Employee":
        //                ccDisplayContent.Content = new EmployeeView();
        //                break;
        //            case "Job":
        //                ccDisplayContent.Content = new JobView();
        //                break;
        //            case "Department":
        //                ccDisplayContent.Content = new DepartmentView();
        //                break;
        //            default:
        //                ccDisplayContent.Content = new HomeView();
        //                break;
        //        }
        //    }
        //}

        private void UpdateLasLogin()
        {
            lbLastLogin.Content = DateTime.Now.ToString();
        }



        private void ImgLogout_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Thread.CurrentPrincipal = null;

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        

        private void btnEmployeeClick_Click(object sender, RoutedEventArgs e)
        {
            ccDisplayContent.Content = new EmployeeView();
        }

        private void btnJobClick_Click(object sender, RoutedEventArgs e)
        {
            ccDisplayContent.Content = new JobView();
        }

        private void btnDepartmentClick_Click(object sender, RoutedEventArgs e)
        {
            ccDisplayContent.Content = new DepartmentView();
        }

        private void btnHomeClick_Click(object sender, RoutedEventArgs e)
        {
            ccDisplayContent.Content = new HomeView();
        }
    }
}