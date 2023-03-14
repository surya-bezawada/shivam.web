using Sivam.web.Data;
using Sivam.web.Models;
using Sivam.web.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;

namespace Sivam.web.Services
{
    public class SMSService : ISMSService
    {

        //public void SendSMS(tblBooking booking, int Type)
        //{
        //    String message = HttpUtility.UrlEncode("OTP for your mobile number verification is " + OTP + ". The code will be valid for 10 minutes. %n Thanks - Shivam Clinic.");
        //}

        public void SendOTP(tblBooking booking)
        {
            string userMobile = booking.Mobile;
            //String message = HttpUtility.UrlEncode("OTP for your mobile number verification is " + booking.OTP + ". The code will be valid for 10 minutes. %n Thanks - Shivam Clinic.");
            string message = HttpUtility.UrlEncode("OTP for your mobile number verification is " + booking.OTP + ". The code will be valid for 10 minutes. Thanks - Shivam Clinic");
            SendSMS(userMobile, (int)OTPType.UserOTP, message, booking.RecId);
        }

        public void SendConfirmations(int recId)
        {
            using (var db = new ShivamDBEntities())
            {
                var data = (from b in db.Bookings
                            join d in db.DeptMasters on b.DeptId equals d.DeptId
                            join t in db.TimesMasters on b.TimeSlotId equals t.TimeSlotId
                            where b.RecId == recId
                            select new { b.Name, b.Mobile, t.Timing, d.DeptName, d.MobileNumber }).FirstOrDefault();

                string userConfMsg = HttpUtility.UrlEncode("Thank you!%nyour appointment booked with " + data.DeptName + " with the timeslot " + data.Timing + "%nThanks - Shivam");
                string docConfMsg = HttpUtility.UrlEncode("You have an appointment for " + data.DeptName + " at the time " + data.Timing + " with " + data.Name + "%nThanks - Shivam");
                SendSMS(data.Mobile, (int)OTPType.UserConfirmation, userConfMsg, recId);
                SendSMS(data.MobileNumber, (int)OTPType.UserConfirmation, docConfMsg, recId);
            }

        }

        private void SendSMS(string mobile, int type, string message, int bookingId = 0)
        {
            string APIKey = string.Empty;
            if (type == 1)
                APIKey = "PN5HT8T/bIA-QYyuiVHUE8qyXPr3Am0rVTadylMPIZ";
            else
                APIKey = "MekLk436AXg-cbhI43AbgPVUUTWsq4Dz702Y5mlK7q";
            using (var wb = new WebClient())
            {
                var queryParams = new NameValueCollection()
                {
                    {"apikey" , APIKey},
                    {"numbers" , mobile},
                    {"message" , message},
                    {"sender" , "SSHVMM"}
                };
                var response = wb.UploadValues("https://api.textlocal.in/send/", queryParams);
                string result = System.Text.Encoding.UTF8.GetString(response);

                SaveAPIResponse(mobile, type, ToQueryString(queryParams), result, bookingId);
            }
        }
        public string ToQueryString(NameValueCollection nameValueCollection)
        {
            NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(String.Empty);
            httpValueCollection.Add(nameValueCollection);
            return httpValueCollection.ToString();
        }
        public void UpdateOTPStatus(int recId, bool isExpired, bool status)
        {
            using (var db = new ShivamDBEntities())
            {
                var data = db.Bookings.Find(recId);
                if (data != null)
                {
                    data.IsExpired = isExpired;
                    data.Status = status;
                }
                db.SaveChanges();
            }
        }
        private void SaveAPIResponse(string mobile, int type, string request, string response, int bookingId = 0)
        {
            tblOTPDetail otpDts = new tblOTPDetail()
            {
                Mobile = mobile,
                Type = type,
                APIRequest = request,
                APIResponse = response,
                BookingId = bookingId,
                CreatedOn = DateTime.Now

            };
            using (var db = new ShivamDBEntities())
            {
                db.OTPDetails.Add(otpDts);
                db.SaveChanges();
            }

        }
    }
}