using BusinessObjects;
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
            
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hl");
            LoadEmployeeList();
            UpdateUserName();
            
        }
        private void UpdateUserName()
        {
            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            if (threadPrincipal != null)
            {
                if (threadPrincipal.Identity.Name == "admin")
                {
                    lbUserName.Content = "ADMIN";
                }
                else
                {
                    if(threadPrincipal.Identity.Name != null)
                    {
                        int id;
                        int.TryParse(threadPrincipal.Identity.Name, out id);
                        lbUserName.Content = employeeService.GetEmployeeName(id);
                    }
                    else
                    {
                        return;
                    }
                    
                }
            }
        }

        private void LoadEmployeeList()
        {
            try
            {
                IPrincipal threadPrincipal = Thread.CurrentPrincipal;
                MessageBox.Show($"Name: {threadPrincipal.Identity.Name} \n IsAuthenticated: {threadPrincipal.Identity.IsAuthenticated}\nAuthenticationType: {threadPrincipal.Identity.AuthenticationType}");
                dgEmployeeData.ItemsSource = null;
                
                if(threadPrincipal != null)
                {
                    if(threadPrincipal.Identity.Name == "admin")
                    {
                        var employees = employeeService.GetEmployees();
                        dgEmployeeData.ItemsSource = employees;
                    }
                    else
                    {
                        if (threadPrincipal.IsInRole("manager"))
                        {
                            int userID;
                            int.TryParse(threadPrincipal.Identity.Name, out userID);
                            var employees = employeeService.GetEmployeesManage(userID);
                            dgEmployeeData.ItemsSource = employees;
                        }
                    }
                }
                
            }catch (Exception e){
                MessageBox.Show(e.Message.ToString(), "Error: Can not load employee's data");
            }
        }

        private void dgEmployeeData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.ItemsSource != null)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
                DataGridCell cell = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string employeeId = ((TextBlock)cell.Content).Text;
                if (!employeeId.Equals(""))
                {
                    Employee? employee = employeeService.GetEmployeeById(Int32.Parse(employeeId));
                    if(employee != null)
                    {
                        tbEmployeeID.Text = employee.EmployeeId.ToString();
                        tbEmployeeID.IsEnabled = false;
                        tbEmployeeFirstName.Text = employee.FirstName;
                        tbEmployeeLastName.Text = employee.LastName;
                        tbEmployeeEmail.Text = employee.Email;
                        tbEmployeePhone.Text = employee.Phone;
                        dpEmployeeHire.Text = employee.Phone;
                        tbEmployeeJob.Text = employee.Job.JobTitle;
                        tbSalary.Text = employee.Salary.ToString();
                        tbComiPct.Text = employee.CommissionPct.ToString();

                        cbManager.SelectedValue = employee.Manager.FirstName;
                        cbManager.IsEnabled = false;
                        tbDepartment.Text = employee.Department.DepartmentName;
                    }
                    //Product product = iProductService.GetProductById(Int32.Parse(productId));
                    //txtProductID.Text = product.ProductId.ToString();
                    //txtProductName.Text = product.ProductName.ToString();
                    //txtPrice.Text = product.UnitPrice.ToString();
                    //txtUnitsInStock.Text = product.UnitsInStock.ToString();
                    //cboCategory.SelectedValue = product.CategoryId;
                }
            }
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {

            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            MessageBox.Show($"Name: {threadPrincipal.Identity.Name} \n IsAuthenticated: {threadPrincipal.Identity.IsAuthenticated}\nAuthenticationType: {threadPrincipal.Identity.AuthenticationType}");

            MessageBox.Show($"Role: {threadPrincipal.IsInRole("admin")}");
        }



        private void ImgLogout_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Thread.CurrentPrincipal = null;

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }



        private void tbEmployeeID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbEmployeeFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbEmployeeEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbEmployeePhone_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbEmployeeJob_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbSalary_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbComiPct_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbManager_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbDepartment_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dpEmployeeHire_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void tbEmployeeLastName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbEmployeeID.Text = "";
            tbEmployeeID.IsEnabled = true;
            tbEmployeeFirstName.Text = "";
            tbEmployeeLastName.Text = "";
            tbEmployeeEmail.Text = "";
            tbEmployeePhone.Text = "";
            dpEmployeeHire.SelectedDate = null;
            tbEmployeeJob.Text = "";
            tbSalary.Text = "";
            tbComiPct.Text = "";
            cbManager.SelectedValue = null;
            tbDepartment.Text = "";
        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            int employeeID;
            int.TryParse(tbEmployeeID.Text, out employeeID);
            employeeService.DeleteEmployee(employeeID);
            try
            {

            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}