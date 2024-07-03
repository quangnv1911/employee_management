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
    public partial class JobView : UserControl
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly IJobService jobService;

        public JobView()
        {
            employeeService = new EmployeeService();
            departmentService = new DepartmentService();
            jobService = new JobService();
            InitializeComponent();
        }
        public void Job_Loaded(object sender, RoutedEventArgs e)
        {
            LoadJobList();
        }


        private void LoadJobList()
        {
            try
            {

                dgJobData.ItemsSource = null;
                var jobs = jobService.GetJobs();
                dgJobData.ItemsSource = jobs;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error: Can not load Job's data");
            }
        }

        private void ClearFilter()
        {
            tbSearchName.Text = "";
            tbSearchMaxSalary.Text = "";
            tbSearchMinSalary.Text = "";
            cbOrderBy.SelectedIndex = -1;
            cbSortOrder.SelectedIndex = -1;
        }

        private void dgJobData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.ItemsSource != null)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
                DataGridCell cell = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string jobId = ((TextBlock)cell.Content).Text;
                if (!jobId.Equals(""))
                {
                    Job? Job = jobService.GetJobById(jobId);
                    if (Job != null)
                    {
                        tbJobId.Text = jobId;
                        tbName.Text = Job.JobTitle;
                        tbMinSalary.Text = Job.MinSalary.ToString();
                        tbMaxSalary.Text = Job.MaxSalary.ToString();
                    }
                }
            }
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbJobId.Text = "";
            tbName.Text = "";
            tbMinSalary.Text = "";
            tbMaxSalary.Text = "";
            ClearFilter();
        }

        private void btnDeleteJob_Click(object sender, RoutedEventArgs e)
        {


            try
            {

                jobService.DeleteJob(tbJobId.Text);

                MessageBox.Show($"Delete Job with {tbJobId.Text} successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Delete Job {tbJobId.Text} failed!!\n{ex}");
            }
        }

        private void btnUpdateJob_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Job job = new Job();
                job.JobId = tbJobId.Text;
                job.JobTitle = tbName.Text;
                job.MinSalary = int.Parse(tbMinSalary.Text);
                job.MaxSalary = int.Parse(tbMaxSalary.Text);


                jobService.UpdateJob(job);
                MessageBox.Show($"Update Job {tbJobId.Text} successfully!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update Job {tbJobId.Text} failed!!\n{ex}");
            }
        }

        private void btnAddJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Job job = new Job();
                job.JobTitle = tbName.Text;
                job.MinSalary = int.Parse(tbMinSalary.Text);
                job.MaxSalary = int.Parse(tbMaxSalary.Text);
                jobService.InsertJob(job);
                MessageBox.Show($"Add Job successfully!!\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add new Job faild!!\n {ex}");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPrincipal threadPrincipal = Thread.CurrentPrincipal;

                dgJobData.ItemsSource = null;

                var jobs = jobService.FilterJob(
                    tbName.Text,
                    tbMinSalary.Text,
                    tbMaxSalary.Text,
               cbOrderBy.SelectedValue?.ToString(),
               cbSortOrder.SelectedValue?.ToString());

                dgJobData.ItemsSource = jobs;
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
                    jobService.ImportExcelFile(filePath);
                    LoadJobList();
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
                await jobService.ExportExcel(tbName.Text,
                                    tbMinSalary.Text,
                                    tbMaxSalary.Text,
                               cbOrderBy.SelectedValue?.ToString(),
                               cbSortOrder.SelectedValue?.ToString());
                MessageBox.Show("Export successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export fail\n {ex}");
            }
        }
    }
}
