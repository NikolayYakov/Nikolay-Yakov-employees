using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikolay_Yakov_employees.Data
{
    internal class CommonProjectEmployees
    {
        public CommonProjectEmployees(int empAID, int empBID, int projectID, double totalDays)
        {
            EmpAID = empAID;
            EmpBID = empBID;
            ProjectID = projectID;
            DaysWorkedTogether = totalDays;
        }
        public int EmpAID { get; set; }
        public int EmpBID { get; set; }
        public int ProjectID { get; set; }
        public double DaysWorkedTogether { get; set; }

    }
}
