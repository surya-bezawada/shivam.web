using Sivam.web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sivam.web.Services
{
    public interface ISMSService
    {
        void SendOTP(tblBooking booking);
        void UpdateOTPStatus(int recId, bool isExpired, bool status);
        void SendConfirmations(int recId);
       // void SendSMS(int bookingId, int Type);
    }
}