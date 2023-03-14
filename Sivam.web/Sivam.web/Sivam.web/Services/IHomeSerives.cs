using Sivam.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sivam.web.Services
{
    public interface IHomeSerives
    {
        SelectList GetDepartmentsList();
        SelectList GetTimingsList();
        Object GenerateBooking(BookingModel obj);
        Object ValidateOTP(int OTP, int RecId, string secureKey);
        Object ResendOTP(int RecId, string secureKey);
    }
}