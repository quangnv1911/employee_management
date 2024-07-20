using Services;
using Services.Impl;
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

namespace Group4_WPF.Control
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IJobService jobService;
        public HomeView()
        {
            InitializeComponent();
            employeeService = new EmployeeService();
            departmentService = new DepartmentService();
            jobService = new JobService();
        }

        public void Home_Loaded(object sender, RoutedEventArgs e)
        {
            tbDepartment.Text = departmentService.CountTotalDepartment().ToString();
            tbEmployee.Text = employeeService.CountTotalEmployee().ToString();
            tbJob.Text = jobService.CountTotalJob().ToString();
            var listJob = jobService.GetJobMaxMony();
            var from = dpFrom.SelectedDate;
            var to = dpTo.SelectedDate;

            DateOnly? fromDate = from.HasValue ? DateOnly.FromDateTime(from.Value) : (DateOnly?)null;
            DateOnly? toDate = to.HasValue ? DateOnly.FromDateTime(to.Value) : (DateOnly?)null;

            var listEmployee = employeeService.GetEmployeeMaxSalary(fromDate, toDate);

            // Xử lý danh sách nhân viên (listEmployee) ở đây
            string listJobValue = "";
            foreach (var item in listJob)
            {
                listJobValue += $"{item.JobTitle?.ToString()}-{item.JobId}-{item.MaxSalary}\n";
            }
            tbListBestJob.Text = listJobValue;

            string listEmployeeLuxury = "";
            foreach(var item in listEmployee)
            {
                listEmployeeLuxury += $"{item.FirstName?.ToString()}-${item.Salary}-{item.Department?.DepartmentName}\n";
            }
            tbListBestEmployee.Text = listEmployeeLuxury;

            tbBestEmployee.Text = $"{listEmployee.FirstOrDefault()?.EmployeeId?.ToString()} - {listEmployee.FirstOrDefault()?.FirstName?.ToString()}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tbDepartment.Text = departmentService.CountTotalDepartment().ToString();
            tbEmployee.Text = employeeService.CountTotalEmployee().ToString();
            tbJob.Text = jobService.CountTotalJob().ToString();
            var listJob = jobService.GetJobMaxMony();
            var from = dpFrom.SelectedDate;
            var to = dpTo.SelectedDate;

            DateOnly? fromDate = from.HasValue ? DateOnly.FromDateTime(from.Value) : (DateOnly?)null;
            DateOnly? toDate = to.HasValue ? DateOnly.FromDateTime(to.Value) : (DateOnly?)null;

            var listEmployee = employeeService.GetEmployeeMaxSalary(fromDate, toDate);

            // Xử lý danh sách nhân viên (listEmployee) ở đây
            string listJobValue = "";
            foreach (var item in listJob)
            {
                listJobValue += $"{item.JobTitle?.ToString()}-{item.JobId}-{item.MaxSalary}\n";
            }
            tbListBestJob.Text = listJobValue;

            string listEmployeeLuxury = "";
            foreach (var item in listEmployee)
            {
                listEmployeeLuxury += $"{item.FirstName?.ToString()}-${item.Salary}-{item.Department?.DepartmentName}\n";
            }
            tbListBestEmployee.Text = listEmployeeLuxury;

            tbBestEmployee.Text = $"{listEmployee.FirstOrDefault()?.EmployeeId?.ToString()} - {listEmployee.FirstOrDefault()?.FirstName?.ToString()}";
        }
    }
}
