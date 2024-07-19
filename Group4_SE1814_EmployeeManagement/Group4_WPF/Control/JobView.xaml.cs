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
    public partial class JobView : UserControl
    {
        private readonly IJobService jobService;
        private const int RecordsPerPage = 10;
        private int _currentPage = 1;
        private int _totalPages = 1;

        private string _currentSearchName = string.Empty;
        private string _currentSearchMaxSalary = string.Empty;
        private string _currentSearchMinSalary = string.Empty;
        private string _currentOrderBy = string.Empty;
        private string _currentSortOrder = string.Empty;

        public JobView()
        {
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
                int totalRecords = jobService.GetJobs().Count();
                _totalPages = (int)Math.Ceiling((double)totalRecords / RecordsPerPage);
                UpdatePaginationButtons();
                LoadPagedJobs();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error: Can not load Job's data");
            }
        }

        private void LoadPagedJobs()
        {
            try
            {
                dgJobData.ItemsSource = null;
                var jobs = jobService.GetJobs()
                                     .Skip((_currentPage - 1) * RecordsPerPage)
                                     .Take(RecordsPerPage)
                                     .ToList();
                dgJobData.ItemsSource = jobs;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Error: Can not load Job's data");
            }
        }

        private void LoadFilteredPagedJobs()
        {
            try
            {
                dgJobData.ItemsSource = null;
                var jobs = jobService.FilterJob(
                    _currentSearchName,
                    _currentSearchMinSalary,
                    _currentSearchMaxSalary,
                    _currentOrderBy,
                    _currentSortOrder)
                    .Skip((_currentPage - 1) * RecordsPerPage)
                    .Take(RecordsPerPage)
                    .ToList();
                dgJobData.ItemsSource = jobs;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Filter failed \n {ex}");
            }
        }

        private void dgJobData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.ItemsSource != null)
            {
                DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex) as DataGridRow;
                if (row == null) return;

                DataGridCell cell = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                string jobId = ((TextBlock)cell.Content).Text;
                if (!jobId.Equals(""))
                {
                    Job? job = jobService.GetJobById(jobId);
                    if (job != null)
                    {
                        tbJobId.Text = job.JobId;
                        tbName.Text = job.JobTitle;
                        tbMinSalary.Text = job.MinSalary.ToString();
                        tbMaxSalary.Text = job.MaxSalary.ToString();
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

        private void ClearFilter()
        {
            tbSearchName.Text = "";
            tbSearchMaxSalary.Text = "";
            tbSearchMinSalary.Text = "";
            cbOrderBy.SelectedIndex = -1;
            cbSortOrder.SelectedIndex = -1;
        }

        private void btnDeleteJob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                jobService.DeleteJob(tbJobId.Text);
                MessageBox.Show($"Delete Job with {tbJobId.Text} successfully!");
                LoadJobList();
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
                Job job = new Job
                {
                    JobId = tbJobId.Text,
                    JobTitle = tbName.Text,
                    MinSalary = int.Parse(tbMinSalary.Text),
                    MaxSalary = int.Parse(tbMaxSalary.Text)
                };

                jobService.UpdateJob(job);
                MessageBox.Show($"Update Job {tbJobId.Text} successfully!!");
                LoadJobList();
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
                Job job = new Job
                {
                    JobTitle = tbName.Text,
                    MinSalary = int.Parse(tbMinSalary.Text),
                    MaxSalary = int.Parse(tbMaxSalary.Text)
                };

                jobService.InsertJob(job);
                MessageBox.Show($"Add Job successfully!!\n");
                LoadJobList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add new Job failed!!\n {ex}");
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            _currentSearchName = tbSearchName.Text;
            _currentSearchMaxSalary = tbSearchMaxSalary.Text;
            _currentSearchMinSalary = tbSearchMinSalary.Text;
            _currentOrderBy = cbOrderBy.SelectedValue?.ToString();
            _currentSortOrder = cbSortOrder.SelectedValue?.ToString();

            _currentPage = 1;
            lblPageInfo.Content = $"Page {_currentPage} of {_totalPages}";
            LoadFilteredPagedJobs();
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
                await jobService.ExportExcel(tbSearchName.Text,
                    tbSearchMinSalary.Text,
                    tbSearchMaxSalary.Text,
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
                LoadFilteredPagedJobs();
                UpdatePaginationButtons();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                LoadFilteredPagedJobs();
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
