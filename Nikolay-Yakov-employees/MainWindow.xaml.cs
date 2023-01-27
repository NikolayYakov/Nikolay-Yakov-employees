using Microsoft.Win32;
using Nikolay_Yakov_employees.Data;
using System;
using System.Collections.Generic;
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
                var mostTimeSameProject =  commonProjectsEmployees.OrderByDescending(x => x.DaysWorkedTogether).FirstOrDefault();

            }
            catch(Exception ex)
            {

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

    }
}
