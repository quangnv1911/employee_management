using BusinessObjects;
using Microsoft.Win32;
using Services;
using Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Group4_WPF.Control
{
    public partial class EmployeeView : UserControl
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IJobService jobService;

        private const int RecordsPerPage = 10;
        private int _currentPage = 1;
        private int _totalPages = 1;

        private string _filterName;
        private string _filterEmail;
        private string _filterPhone;
        private string _filterSalary;
        private string _filterDepartment;
        private string _filterJob;
        private string _orderBy;
        private string _sortOrder;

        public EmployeeView()
        {
            employeeService = new EmployeeService();
            departmentService = new DepartmentService();
            jobService = new JobService();
            InitializeComponent();

            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            if (threadPrincipal != null)
            {
                if (threadPrincipal.Identity.Name != "admin")
                {
                    btnAddEmployee.IsEnabled = false;
                    btnUpdateEmployee.IsEnabled = false;
                    btnUpdateEmployee.IsEnabled = false;
                    btnImportFile.IsEnabled = false;
                    btnExportFile.IsEnabled = false;
                }
            }
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
            LoadPagedEmployees();
            
        }

        private void LoadPagedEmployees()
        {
            try
            {
                IPrincipal threadPrincipal = Thread.CurrentPrincipal;
                dgEmployeeData.ItemsSource = null;

                List<Employee> employees = GetFilteredEmployees(threadPrincipal);
                txtQuantity.Text = employees.Count.ToString();
                _totalPages = (int)Math.Ceiling((double)employees.Count / RecordsPerPage);
                UpdatePaginationButtons();
                lblPageInfo.Content = $"Page {_currentPage} of {_totalPages}";
                dgEmployeeData.ItemsSource = employees.Skip((_currentPage - 1) * RecordsPerPage).Take(RecordsPerPage).ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error: Can not load employee's data");
            }
        }

        private List<Employee> GetFilteredEmployees(IPrincipal threadPrincipal)
        {
            List<Employee> employees = new List<Employee>();

            if (threadPrincipal != null)
            {
                if (threadPrincipal.Identity.Name == "admin")
                {
                    employees = employeeService.GetEmployees();
                }
                else if (threadPrincipal.IsInRole("manager"))
                {
                    if (int.TryParse(threadPrincipal.Identity.Name, out int userID))
                    {
                        employees = employeeService.GetEmployeesManage(userID);
                    }
                }
            }

            if (!string.IsNullOrEmpty(_filterName) ||
                !string.IsNullOrEmpty(_filterEmail) ||
                !string.IsNullOrEmpty(_filterPhone) ||
                !string.IsNullOrEmpty(_filterSalary) ||
                !string.IsNullOrEmpty(_filterDepartment) ||
                !string.IsNullOrEmpty(_filterJob) ||
                !string.IsNullOrEmpty(_orderBy) ||
                 !string.IsNullOrEmpty(_sortOrder))
            {
                employees = employeeService.FilterEmployee(_filterName, _filterEmail, _filterPhone, _filterSalary, _filterDepartment, _filterJob, _orderBy, _sortOrder);
            }

            return employees;
        }

        private void UpdatePaginationButtons()
        {
            btnPrevious.IsEnabled = _currentPage > 1;
            btnNext.IsEnabled = _currentPage < _totalPages;
        }

        private void btnEmployeeID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbEmployeeID.Text))
            {
                tbEmployeeID.IsEnabled = false;
                if (int.TryParse(tbEmployeeID.Text, out int employeeID))
                {
                    var employee = employeeService.GetEmployeeById(employeeID);
                    if (employee != null)
                    {
                        PopulateEmployeeDetails(employee);
                    }
                }
            }
        }

        private void PopulateEmployeeDetails(Employee employee)
        {
            tbEmployeeFirstName.Text = employee.FirstName;
            tbEmployeeLastName.Text = employee.LastName;
            tbEmployeeEmail.Text = employee.Email;
            tbEmployeePhone.Text = employee.Phone;
            dpEmployeeHire.SelectedDate = DateTime.Parse(employee.HireDate.ToString());
            tbSalary.Text = employee.Salary.ToString();
            tbComiPct.Text = employee.CommissionPct.ToString();
            cbEmployeeJob.SelectedValue = employee.Job?.JobId;
            cbManager.SelectedValue = employee.Manager?.EmployeeId;
            cbManager.IsEnabled = false;
            cbDepartment.SelectedValue = employee.Department?.DepartmentId;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearEmployeeDetails();
        }

        private void ClearEmployeeDetails()
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
            try
            {
                if (int.TryParse(tbEmployeeID.Text, out int employeeID))
                {
                    employeeService.DeleteEmployee(employeeID);
                    MessageBox.Show($"Delete employee with ID {employeeID} successfully!");
                    LoadEmployeeList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete employee failed!!\n{ex}");
            }
        }

        private void btnUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dpEmployeeHire.SelectedDate == null || dpEmployeeHire.SelectedDate > DateTime.Now)
                {
                    MessageBox.Show("Invalid date");
                    return;
                }
                Employee employee = CreateEmployeeFromInput();
                employeeService.UpdateEmployee(employee);
                MessageBox.Show($"Update employee {tbEmployeeID.Text} successfully!!");
                LoadEmployeeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update employee failed!!\n{ex}");
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(dpEmployeeHire.SelectedDate == null || dpEmployeeHire.SelectedDate > DateTime.Now)
                {
                    MessageBox.Show("Invalid date");
                    return;
                }

                Employee employee = CreateEmployeeFromInput();
                employeeService.InsertEmployee(employee);
                MessageBox.Show($"Add new employee successfully!!");
                LoadEmployeeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add new employee failed!!\n{ex}");
            }
        }

        private Employee CreateEmployeeFromInput()
        {
            return new Employee
            {
                EmployeeId = int.TryParse(tbEmployeeID.Text, out int empId) ? empId : 0,
                FirstName = tbEmployeeFirstName.Text,
                LastName = tbEmployeeLastName.Text,
                Email = tbEmployeeEmail.Text,
                Phone = tbEmployeePhone.Text,
                HireDate = DateOnly.Parse(dpEmployeeHire.Text),
                Salary = double.Parse(tbSalary.Text),
                CommissionPct = double.Parse(tbComiPct.Text),
                ManagerId = int.Parse(cbManager.SelectedValue.ToString()),
                DepartmentId = int.Parse(cbDepartment.SelectedValue.ToString()),
                JobId = cbEmployeeJob.SelectedValue.ToString()
            };
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentPage = 1;
                ApplyFilters();
                LoadPagedEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filter failed \n {ex}");
            }
        }

        private void ApplyFilters()
        {
            _filterName = tbSearchName.Text;
            _filterEmail = tbSearchEmail.Text;
            _filterPhone = tbSearchPhone.Text;
            _filterSalary = tbSearchSalary.Text;
            _filterDepartment = tbSearchDepartment.Text;
            _filterJob = tbSearchJob.Text;
            _orderBy = cbOrderBy.SelectedValue?.ToString();
            _sortOrder = cbSortOrder.SelectedValue?.ToString();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    employeeService.ImportExcelFile(openFileDialog.FileName);
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
                await employeeService.ExportExcel(_filterName, _filterEmail, _filterPhone, _filterSalary, _filterDepartment, _filterJob, _orderBy, _sortOrder);
                MessageBox.Show("Export successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export failed\n{ex}");
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPagedEmployees();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadPagedEmployees();
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

                        cbEmployeeJob.SelectedValue = employee.Job?.JobId;
                        cbManager.SelectedValue = employee.Manager?.EmployeeId;

                        cbManager.IsEnabled = false;
                        cbDepartment.SelectedValue = employee.Department?.DepartmentId;
                    }
                }
            }
        }

    }
}
