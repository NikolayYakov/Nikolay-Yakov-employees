using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikolay_Yakov_employees.Data
{
    internal class Employee
    {
        public Employee(string[] employeeData)
        {
            EmpId = int.Parse(employeeData[0]);
            ProjectID = int.Parse(employeeData[1]);
            DateFrom = employeeData[2] == "NULL" ? DateTime.UtcNow : DateTime.Parse(employeeData[2]);
            DateTo = employeeData[3] == "NULL" ? DateTime.UtcNow : DateTime.Parse(employeeData[3]);
        }
        public int EmpId { get; set; }
        public int ProjectID { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

    }
}
