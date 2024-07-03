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
        }
    }
}
