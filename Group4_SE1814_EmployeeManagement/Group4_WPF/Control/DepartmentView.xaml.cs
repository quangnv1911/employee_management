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

namespace Group4_WPF.Control
{
    /// <summary>
    /// Interaction logic for EmployeeView.xaml
    /// </summary>
    public partial class DepartmentView : UserControl
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IJobService jobService;
        private readonly ILocationService locationService;

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

                dgDepartmentData.ItemsSource = null;
                var deparments = departmentService.GetDepartments();
                dgDepartmentData.ItemsSource = deparments;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error: Can not load Department's data");
            }
        }

        private void dgDepartmentData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.ItemsSource != null)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
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
                Department department = new Department();
                department.DepartmentId = int.Parse(tbDepartmetnID.Text);
                department.DepartmentName = tbDepartmentName.Text;
                //Location? location = locationService.GetLocationById(cbLocation.SelectedValue.ToString());
                //if (location != null)
                //{
                //    department.Location = location;
                //}
                department.LocationId = cbLocation.SelectedValue.ToString();



                departmentService.UpdateDepartment(department);
                MessageBox.Show($"Update Department {tbDepartmetnID.Text} successfully!!");
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
                Department department = new Department();
                department.DepartmentName = tbDepartmentName.Text;
                department.LocationId = cbLocation.SelectedValue.ToString();
                //Location? location = locationService.GetLocationById(cbLocation.SelectedValue.ToString());
                //if (location != null)
                //{
                //    department.Location = location;
                //}

                departmentService.InsertDepartment(department);
                MessageBox.Show($"Add Department successfully!!\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add new Department faild!!\n {ex}");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dgDepartmentData.ItemsSource = null;

                var departments = departmentService.FilterDepartments(
                    tbSearchName.Text,
                    tbSearchLocation.Text,
                    tbSearchManagerName.Text,
               cbOrderBy.SelectedValue?.ToString(),
               cbSortOrder.SelectedValue?.ToString());

                dgDepartmentData.ItemsSource = departments;
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
    }
}
