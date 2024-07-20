using BusinessObjects;
using Microsoft.Win32;
using Services;
using Services.Impl;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Group4_WPF.Control
{
    public partial class DepartmentView : UserControl
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IJobService jobService;
        private readonly ILocationService locationService;

        private const int RecordsPerPage = 10;
        private int _currentPage = 1;
        private int _totalPages = 1;

        private string _currentSearchName = string.Empty;
        private string _currentSearchLocation = string.Empty;
        private string _currentSearchManagerName = string.Empty;
        private string _currentOrderBy = string.Empty;
        private string _currentSortOrder = string.Empty;

        public DepartmentView()
        {
            employeeService = new EmployeeService();
            departmentService = new DepartmentService();
            jobService = new JobService();
            locationService = new LocationService();
            InitializeComponent();
        }

        public void Department_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDepartmentList();
            LoadLocation();
        }

        private void LoadLocation()
        {
            cbLocation.ItemsSource = locationService.GetLocations();
            cbLocation.SelectedValuePath = "LocationId";
        }

        private void LoadDepartmentList()
        {
            try
            {
                int totalRecords = departmentService.GetDepartments().Count();
                txtQuantity.Text = totalRecords.ToString();
                _totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
                UpdatePaginationButtons();

                LoadPagedDepartments();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error: Can not load Department's data");
            }
        }

        private void LoadPagedDepartments()
        {
            try
            {
                dgDepartmentData.ItemsSource = null;
                var departments = departmentService.GetDepartments()
                                                  .Skip((_currentPage - 1) * RecordsPerPage)
                                                  .Take(RecordsPerPage)
                                                  .ToList();
                dgDepartmentData.ItemsSource = departments;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error: Can not load Department's data");
            }
        }

        private void LoadFilteredPagedDepartments()
        {
            try
            {
                dgDepartmentData.ItemsSource = null;

                var departments = departmentService.FilterDepartments(
                    _currentSearchName,
                    _currentSearchLocation,
                    _currentSearchManagerName,
                    _currentOrderBy,
                    _currentSortOrder)
                    .Skip((_currentPage - 1) * RecordsPerPage)
                    .Take(RecordsPerPage)
                    .ToList();

                dgDepartmentData.ItemsSource = departments;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filter failed \n {ex}");
            }
        }

        private void dgDepartmentData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.ItemsSource != null)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
                if (row == null) return;

                DataGridCell cell = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string departmentId = ((TextBlock)cell.Content).Text;
                if (!departmentId.Equals(""))
                {
                    Department? department = departmentService.GetDepartmentById(int.Parse(departmentId));
                    if (department != null)
                    {
                        tbDepartmetnID.Text = department.DepartmentId.ToString();
                        tbDepartmentName.Text = department.DepartmentName;
                        cbLocation.SelectedValue = department.LocationId;
                        tbManagerName.Text = department.Manager?.FirstName;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbDepartmetnID.Text = "";
            tbDepartmentName.Text = "";
            cbOrderBy.SelectedIndex = -1;
            tbManagerName.Text = "";
        }

        private void btnDeleteDepartment_Click(object sender, RoutedEventArgs e)
        {
            int departmentId;

            try
            {
                int.TryParse(tbDepartmetnID.Text, out departmentId);
                employeeService.DeleteEmployee(departmentId);
                MessageBox.Show($"Delete Department with {departmentId} successfully!");
                LoadDepartmentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete Department {tbDepartmetnID.Text} failed!!\n{ex}");
            }
        }

        private void btnUpdateDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Department department = new Department
                {
                    DepartmentId = int.Parse(tbDepartmetnID.Text),
                    DepartmentName = tbDepartmentName.Text,
                    LocationId = cbLocation.SelectedValue.ToString()
                };

                departmentService.UpdateDepartment(department);
                MessageBox.Show($"Update Department {tbDepartmetnID.Text} successfully!!");
                LoadDepartmentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update Department {tbDepartmetnID.Text} failed!!\n{ex}");
            }
        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Department department = new Department
                {
                    DepartmentName = tbDepartmentName.Text,
                    LocationId = cbLocation.SelectedValue.ToString()
                };

                departmentService.InsertDepartment(department);
                MessageBox.Show($"Add Department successfully!!\n");
                LoadDepartmentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add new Department failed!!\n {ex}");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            _currentSearchName = tbSearchName.Text;
            _currentSearchLocation = tbSearchLocation.Text;
            _currentSearchManagerName = tbSearchManagerName.Text;
            _currentOrderBy = cbOrderBy.SelectedValue?.ToString();
            _currentSortOrder = cbSortOrder.SelectedValue?.ToString();

            _currentPage = 1;
            lblPageInfo.Content = $"Page {_currentPage} of {_totalPages}";
            LoadFilteredPagedDepartments();
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
                    string filePath = openFileDialog.FileName;
                    departmentService.ImportExcelFile(filePath);
                    LoadDepartmentList();
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
                await departmentService.ExportExcel(tbSearchName.Text,
                    tbSearchLocation.Text,
                    tbSearchManagerName.Text,
                    cbOrderBy.SelectedValue?.ToString(),
                    cbSortOrder.SelectedValue?.ToString());
                MessageBox.Show("Export successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export fail\n{ex}");
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadFilteredPagedDepartments();
                UpdatePaginationButtons();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadFilteredPagedDepartments();
                UpdatePaginationButtons();
            }
        }

        private void UpdatePaginationButtons()
        {
            btnPrevious.IsEnabled = _currentPage > 1;
            btnNext.IsEnabled = _currentPage < _totalPages;
            lblPageInfo.Content = $"Page {_currentPage} of {_totalPages}";
        }
    }
}
