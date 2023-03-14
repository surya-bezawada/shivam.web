using Sivam.web.Data;
using Sivam.web.Models;
using Sivam.web.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Sivam.web.Services
{
    public class HomeSerives : IHomeSerives
    {
        private readonly ISMSService _smsService;
        public HomeSerives(ISMSService smsService)
        {
            this._smsService = smsService;
        }
        public SelectList GetDepartmentsList()
        {
            try
            {
                using (var db = new ShivamDBEntities())
                {
                    var states = (from slist in db.DeptMasters
                                  select new { slist.DeptId, slist.DeptName }).ToList();
                    return new SelectList(states, "DeptId", "DeptName");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SelectList GetTimingsList()
        {
            try
            {
                using (var db = new ShivamDBEntities())
                {
                    var states = (from slist in db.TimesMasters
                                  select new { slist.TimeSlotId, slist.Timing }).ToList();
                    return new SelectList(states, "TimeSlotId", "Timing");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Object GenerateBooking(BookingModel obj)
        {
            try
            {
                using (var db = new ShivamDBEntities())
                {
                    Guid g = Guid.NewGuid();
                    tblBooking booking = new tblBooking()
                    {
                        Name = obj.Name,
                        Email = obj.Email,
                        Mobile = obj.Mobile,
                        BookingDate = obj.BookingDate,
                        TimeSlotId = obj.TimeSlotId,
                        DeptId = obj.DeptId,
                        IPAddress = CommonUtils.GetIp(),
                        DeviceInfo = CommonUtils.GetDeviceInfor(),
                        MACAddress = CommonUtils.GetMACAddress(),
                        SecureKey = g.ToString(),
                        CreatedOn = DateTime.Now,
                        OTP = CommonUtils.GenerateNumericOTP(),
                        IsExpired = false,
                        Status = false

                    };
                    db.Bookings.Add(booking);
                    db.SaveChanges();

                    _smsService.SendOTP(booking);
                    var res = new
                    {
                        recid = booking.RecId,
                        securekey = booking.SecureKey,
                        success = true,
                        message = "OTP has been sent to your given mobile number " + booking.Mobile.Substring(0, 3) + "XXXX " + booking.Mobile.Substring(7, 3) + ". OTP is valid for 10 minutes only."
                    };
                    return res;

                }
            }

            catch (Exception ex)
            {
                var res = new
                {
                    success = false,
                    message = "Ex: Something went wrong."
                };
                return res;
            }
        }

        public Object ValidateOTP(int OTP, int RecId, string secureKey)
        {
            try
            {
                using (var db = new ShivamDBEntities())
                {
                    var booking = db.Bookings.Where(x => x.RecId == RecId && x.SecureKey == secureKey).FirstOrDefault();
                    if (booking == null)
                    {
                        var res1 = new
                        {
                            code = 100,
                            status = true,
                            message = "Something went wrong: Record Not Found."
                        };
                        return res1;
                    }
                    if (booking.IsExpired || booking.Status)
                    {
                        var res11 = new
                        {
                            code = 100,
                            status = true,
                            message = "OTP already expired/used, please try with new OTP."
                        };
                        return res11;
                    }
                    System.DateTime currentTime = DateTime.Now;
                    var timeDiff = currentTime - booking.CreatedOn;
                    TimeSpan span = booking.CreatedOn.Subtract(currentTime);
                    if (span.Minutes > 10)
                    {
                        _smsService.UpdateOTPStatus(RecId, true, false);
                        var res2 = new
                        {
                            code = 100,
                            status = true,
                            message = "OTP has been expired, please try again."
                        };
                        return res2;
                    }
                    if (booking.OTP != OTP)
                    {
                        var res33 = new
                        {
                            code = 100,
                            status = true,
                            message = "Incorrect OTP Details."
                        };
                        return res33;
                    }
                    bool isValid = (booking.OTP == OTP && !booking.Status);
                    if (isValid)
                    {
                        _smsService.UpdateOTPStatus(RecId, false, true);
                        _smsService.SendConfirmations(RecId);
                        var res3 = new
                        {
                            code = 200,
                            status = true,
                            message = "You have booked an appointment."
                        };
                        return res3;
                    }
                    var res = new
                    {
                        code = 100,
                        status = false,
                        message = "Failed in Execultion"
                    };
                    return res;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;

            }
            catch (DbUpdateException e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (var eve in e.Entries)
                {
                    sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                }
                var a = sb.ToString();

                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Object ResendOTP(int RecId, string secureKey)
        {
            try
            {
                using (var db = new ShivamDBEntities())
                {
                    var booking = db.Bookings.Where(x => x.RecId == RecId && x.SecureKey == secureKey).FirstOrDefault();

                    if (booking == null)
                    {
                        var res1 = new
                        {
                            code = 100,
                            status = true,
                            message = "Something went wrong: Record Not Found."
                        };
                        return res1;
                    }
                    _smsService.SendOTP(booking);
                    var res = new
                    {
                        code = 200,
                        recid = booking.RecId,
                        securekey = booking.SecureKey,
                        success = true,
                        message = "OTP has been resent to your given mobile number " + booking.Mobile.Substring(0, 4) + "XXXXXX. OTP is valid for 10 minutes only."
                    };
                    return res;
                }

            }
            catch (Exception ex)
            {
                var res = new
                {
                    success = false,
                    message = "Caught Exception"
                };
                return res;
            }
        }
        private void SendOTP(tblBooking booking)
        {
            string userMobile = booking.Mobile;
            string docMobile = string.Empty;
            using (var db = new ShivamDBEntities())
            {
                docMobile = db.DeptMasters.Where(x => x.DeptId == booking.DeptId)?.FirstOrDefault()?.MobileNumber;
            }
            String message = HttpUtility.UrlEncode("OTP for your mobile number verification is 1212. The code will be valid for 10 minutes. %n Thanks - Shivam Clinic.");
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "PN5HT8T/bIA-QYyuiVHUE8qyXPr3Am0rVTadylMPIZ"},
                {"numbers" , docMobile},
                {"message" , message},
                {"sender" , "SSHVMM"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
            }
        }
    }
}