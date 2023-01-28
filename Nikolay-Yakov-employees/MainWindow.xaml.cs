using Microsoft.Win32;
using Nikolay_Yakov_employees.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
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

namespace Nikolay_Yakov_employees
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PickFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                openFileDialog.ShowDialog();
                string path =  openFileDialog.FileName;
                
                var employees = ReadCsvFile(path);
                var employeesToProjects = employees.GroupBy(x => x.ProjectID).ToList();
                var commonProjectsEmployees = new List<CommonProjectEmployees>();
                foreach (var employeesToProject in employeesToProjects)
                {
                    commonProjectsEmployees.AddRange(GetPairs(employeesToProject.ToList(), employeesToProject.Key));
                }
                var orderedList = commonProjectsEmployees.OrderByDescending(x => x.DaysWorkedTogether);
                var mostTimeSameProject = orderedList.FirstOrDefault();

                ShowResult(mostTimeSameProject);

                ObservableCollection<CommonProjectEmployees> commonEmployees = new ObservableCollection<CommonProjectEmployees>(orderedList);
                DG1.DataContext = commonEmployees;

            }
            catch(Exception ex)
            {
                string caption = "Error!";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;

                result = MessageBox.Show(ex.Message, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private List<Employee> ReadCsvFile(string path)
        {
            List<Employee> employees = new List<Employee>();
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] columns = line.Split(',');
                employees.Add(new Employee(columns));
            }
            return employees;
        }

        private List<CommonProjectEmployees> GetPairs(List<Employee> employees, int projectId)
        {
            var employeesCommonProjects = new List<CommonProjectEmployees>();
            for(int i = 0; i < employees.Count; i++)
            {
                for(int j = i+1; j < employees.Count; j++)
                { 
                    if (employees[i].DateFrom < employees[j].DateTo && employees[j].DateFrom < employees[i].DateTo)
                    {
                        DateTime start = (employees[i].DateFrom > employees[j].DateFrom) ? employees[i].DateFrom : employees[j].DateFrom;
                        DateTime end = (employees[i].DateTo < employees[j].DateTo) ? employees[i].DateTo : employees[j].DateTo;
                        double difference = (end - start).TotalDays + 1;
                        employeesCommonProjects.Add(new CommonProjectEmployees(employees[i].EmpId, employees[j].EmpId, projectId, difference));
                    }
                }
            }
            return employeesCommonProjects;
        }

        private void ShowResult(CommonProjectEmployees? mostTimeSameProject)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Information;

            string messageBoxText;
            string caption;

            if (mostTimeSameProject != null)
            {
                messageBoxText = $"Employees {mostTimeSameProject.EmpAID} and {mostTimeSameProject.EmpBID} have worked together the most: {mostTimeSameProject.DaysWorkedTogether} days." +
                                     $" They worked on project {mostTimeSameProject.ProjectID}";
                caption = "Employees working together the most.";  
            }
            else
            {
                messageBoxText = "There are not a pair of employees who worked together on the same project.";
                caption = "no result";
            }
           

            MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
        }

    }
}
