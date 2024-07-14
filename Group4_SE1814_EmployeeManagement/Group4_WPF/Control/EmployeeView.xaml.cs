using BusinessObjects;
using Microsoft.Win32;
using Services;
using Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
using System.Xml.Linq;

namespace Group4_WPF.Control
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IJobService jobService;

        public EmployeeView()
        {
            employeeService = new EmployeeService();
            departmentService = new DepartmentService();
            jobService = new JobService();
            InitializeComponent();
        }
        public void Employee_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEmployeeList();
            LoadDepartmentList();
            LoadManager();
            LoadJob();
        }

        private void LoadDepartmentList()
        {
            cbDepartment.ItemsSource = departmentService.GetDepartments();
            cbDepartment.DisplayMemberPath = "DepartmentName";
            cbDepartment.SelectedValuePath = "DepartmentId";
        }

        private void LoadManager()
        {
            cbManager.ItemsSource = employeeService.GetEmployees();
            cbManager.DisplayMemberPath = "FirstName";
            cbManager.SelectedValuePath = "EmployeeId";
        }

        private void LoadJob()
        {
            cbEmployeeJob.ItemsSource = jobService.GetJobs();
            cbEmployeeJob.DisplayMemberPath = "JobTitle";
            cbEmployeeJob.SelectedValuePath = "JobId";
        }

        private void LoadEmployeeList()
        {
            try
            {
                IPrincipal threadPrincipal = Thread.CurrentPrincipal;

                dgEmployeeData.ItemsSource = null;

                if (threadPrincipal != null)
                {
                    if (threadPrincipal.Identity.Name == "admin")
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

            }
            catch (Exception e)
            {
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
                    if (employee != null)
                    {
                        tbEmployeeID.Text = employee.EmployeeId.ToString();
                        tbEmployeeID.IsEnabled = false;
                        tbEmployeeFirstName.Text = employee.FirstName;
                        tbEmployeeLastName.Text = employee.LastName;
                        tbEmployeeEmail.Text = employee.Email;
                        tbEmployeePhone.Text = employee.Phone;
                        dpEmployeeHire.Text = employee.HireDate.ToString();

                        tbSalary.Text = employee.Salary.ToString();
                        tbComiPct.Text = employee.CommissionPct.ToString();

                        cbEmployeeJob.SelectedValue = employee.Job.JobId;
                        cbManager.SelectedValue = employee.Manager?.EmployeeId;
                        
                        cbManager.IsEnabled = false;
                        cbDepartment.SelectedValue = employee.Department.DepartmentId;
                    }
                }
            }
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {

            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            MessageBox.Show($"Name: {threadPrincipal.Identity.Name} \n IsAuthenticated: {threadPrincipal.Identity.IsAuthenticated}\nAuthenticationType: {threadPrincipal.Identity.AuthenticationType}");

            MessageBox.Show($"Role: {threadPrincipal.IsInRole("admin")}");
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
            cbEmployeeJob.SelectedValue = null;
            tbSalary.Text = "";
            tbComiPct.Text = "";
            cbManager.SelectedValue = null;
            cbDepartment.SelectedValue = null;
        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            int employeeID;

            try
            {
                int.TryParse(tbEmployeeID.Text, out employeeID);
                employeeService.DeleteEmployee(employeeID);
                MessageBox.Show($"Delete employee with {employeeID} successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete employee {tbEmployeeID.Text} failed!!\n{ex}");
            }
        }

        private void btnUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Employee employee = new Employee();
                employee.EmployeeId = int.Parse(tbEmployeeID.Text);
                employee.FirstName = tbEmployeeFirstName.Text;
                employee.LastName = tbEmployeeLastName.Text;
                employee.Email = tbEmployeeEmail.Text;
                employee.Phone = tbEmployeePhone.Text;
                employee.HireDate = DateOnly.Parse(dpEmployeeHire.Text);
                employee.Salary = double.Parse(tbSalary.Text);
                employee.CommissionPct = double.Parse(tbComiPct.Text);
                //Employee manager = null;
                //if (cbManager.SelectedValue != null)
                //{
                //    manager = employeeService.GetEmployeeById(int.Parse(cbManager.SelectedValue.ToString()));
                //}
                employee.ManagerId = int.Parse(cbManager.SelectedValue.ToString());
                //employee.Manager = manager;

                //Department department = departmentService.GetDepartmentById(int.Parse(cbDepartment.SelectedValue.ToString()));
                //employee.Department = department;
                employee.DepartmentId = int.Parse(cbDepartment.SelectedValue.ToString());
                //Job job = jobService.GetJobById(tbEmployeeJob.Text);
                //employee.Job = job;
                employee.JobId = cbEmployeeJob.SelectedValue.ToString();

                employeeService.UpdateEmployee(employee);
                MessageBox.Show($"Update employee {tbEmployeeID.Text} successfully!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update employee {tbEmployeeID.Text} failed!!\n{ex}");
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Employee employee = new Employee();
                employee.FirstName = tbEmployeeFirstName.Text;
                employee.LastName = tbEmployeeLastName.Text;
                employee.Email = tbEmployeeEmail.Text;
                employee.Phone = tbEmployeePhone.Text;
                employee.HireDate = DateOnly.Parse(dpEmployeeHire.Text);
                employee.Salary = double.Parse(tbSalary.Text);
                employee.CommissionPct = double.Parse(tbComiPct.Text);
                //Employee manager = null;
                //if (cbManager.SelectedValue != null)
                //{
                //    manager = employeeService.GetEmployeeById(int.Parse(cbManager.SelectedValue.ToString()));
                //}

                //employee.Manager = manager;
                employee.ManagerId = int.Parse(cbManager.SelectedValue.ToString());
                //Department department = departmentService.GetDepartmentById(int.Parse(cbDepartment.SelectedValue.ToString()));
                employee.DepartmentId = int.Parse(cbDepartment.SelectedValue.ToString());

                employee.JobId = null;

                employeeService.InsertEmployee(employee);
                MessageBox.Show($"Add new employee {tbEmployeeID.Text} successfully!!\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add new employee faild!!\n {ex}");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPrincipal threadPrincipal = Thread.CurrentPrincipal;

                dgEmployeeData.ItemsSource = null;

                var employees = employeeService.FilterEmployee(tbSearchName.Text,
               tbSearchEmail.Text,
               tbSearchPhone.Text,
               tbSearchSalary.Text,
               tbSearchDepartment.Text,
               tbSearchJob.Text,
               cbOrderBy.SelectedValue?.ToString(),
               cbSortOrder.SelectedValue?.ToString());

                var employeeFilterList = employeeService.FilterListByRole(threadPrincipal.Identity.Name, employees);
                dgEmployeeData.ItemsSource = employeeFilterList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filter faile \n {ex}");
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    employeeService.ImportExcelFile(filePath);
                    LoadEmployeeList();
                    MessageBox.Show("Import success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import failed!!\n{ex}");
            }
        }

        private async void btnExport_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                await employeeService.ExportExcel(tbSearchName.Text,
               tbSearchEmail.Text,
               tbSearchPhone.Text,
               tbSearchSalary.Text,
               tbSearchDepartment.Text,
               tbSearchJob.Text,
               cbOrderBy.SelectedValue?.ToString(),
               cbSortOrder.SelectedValue?.ToString());
                MessageBox.Show("Export successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export fail\n{ex}");
            }
        }

        
    }
}
