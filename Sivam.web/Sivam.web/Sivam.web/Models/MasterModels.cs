using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sivam.web.Models
{
    public class MasterModels
    {
    }

    [Table("tblTimesMaster")]
    public class TimeSlots
    {
        [Key]
        public int TimeSlotId { get; set; }
        public string Timing { get; set; }
    }

    [Table("tblDeptMaster")]
    public class Departments
    {
        [Key]
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string MobileNumber { get; set; }
    }

    enum OTPType
    {
        UserOTP = 1,
        Doctor = 2,
        UserConfirmation = 3
    }
}