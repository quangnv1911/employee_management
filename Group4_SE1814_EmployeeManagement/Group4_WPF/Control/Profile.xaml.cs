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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessObjects;
namespace Group4_WPF.Control
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IJobService jobService;
        private readonly ILocationService locationService;
        public Profile()
        {
           
            InitializeComponent();
            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            employeeService = new EmployeeService();
            departmentService = new DepartmentService();
            jobService = new JobService();
            locationService = new LocationService();
            load();
        }

        public void load()
        {

            IPrincipal threadPrincipal = Thread.CurrentPrincipal;
            if (threadPrincipal != null)
            {
                if (threadPrincipal.Identity.Name == "admin")
                {
                    txtName.Text = "Admin";
                    txtRole.Text = "Admin";
                    
                }
                else
                {
                    if (threadPrincipal.Identity.Name != null)
                    {
                         int id;

                        int.TryParse(threadPrincipal.Identity.Name, out id);

                        Employee? e = employeeService.GetEmployeeById(id);

                        int? departmentIdNullable = e.DepartmentId;
                        int departmentId = departmentIdNullable.GetValueOrDefault();

                        Job? j = jobService.GetJobById(e.JobId);
                        Department? d = departmentService.GetDepartmentById(departmentId);

                        txtName.Text = $"{e.FirstName} {e.LastName}";
                        txtPhone.Text = e.Phone;
                        txtEmployeeID.Text = e.EmployeeId.ToString();
                        txtEmail.Text = e.Email;
                        txtJob.Text = j.JobTitle;
                        txtSalary.Text = $"{e.Salary.ToString()}$";
                        bool isManager = employeeService.GetEmployees().Any(e => e.ManagerId == id);
                        txtRole.Text = isManager ? "Manager" : "Employee";
                        txtDepartment.Text = d.DepartmentName;

                    }
                    else
                    {
                        return;
                    }

                }
            }
        }
    }
}
