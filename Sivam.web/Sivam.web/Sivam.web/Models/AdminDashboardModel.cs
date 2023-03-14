using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sivam.web.Models
{
    public class AdminDashboardModel
    {
        public int TodayBookings { get; set; }
        public int MonthBookings { get; set; }
        public int TotalPatients { get; set; }
    }

    public class FilterModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int Filter { get; set; }
        public bool Status { get; set; }
    }

    public class ReportModel
    {
        public int RecId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string BookingDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DeptName { get; set; }
        public string Timing { get; set; }
        public bool Status { get; set; }
    }
}