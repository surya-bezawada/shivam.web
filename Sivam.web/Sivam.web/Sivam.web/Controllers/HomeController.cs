using Sivam.web.Models;
using Sivam.web.Services;
using Sivam.web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sivam.web.Controllers
{
    [SessionExpire]
   
    public class HomeController : Controller
    {
        private readonly IHomeSerives _homeService;
        public HomeController(IHomeSerives homeService)
        {
            this._homeService = homeService;
        }
        public ActionResult Index()
        {
            ViewBag.TimigsList = GetTimingsList();
            ViewBag.DeptsList = GetDepartmentsList();
            Session["validtOtpSe"] = "Valid";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AdminLogin(LoginModel obj)
        {
            if (obj.UserName == "ShivamAdmin" && obj.Password == "Admin@Shivam")
                return RedirectToAction("Index", "Admin");
            ViewBag.Error = "Invalid Credentials";
            return View("Login");
        }

        public void validtemobile()
        {
            //cSession["uniqueID"] = "Valid";
        }

        [Route("GetDetails")]
        [HttpPost]
        public JsonResult GetDetails(BookingModel obj)
        {
            //obj.IPAddress = CommonUtils.GetIp();
            //obj.DeviceInfo = CommonUtils.GetDeviceInfor();
            //obj.MACAddress = CommonUtils.GetMACAddress();
            //obj.SecureKey = new Guid().ToString();
            //obj.CreatedOn = DateTime.Now;
            var a = _homeService.GenerateBooking(obj);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateDetails(int OTP, int RecId, string secureKey)
        {
            //obj.IPAddress = CommonUtils.GetIp();
            //obj.DeviceInfo = CommonUtils.GetDeviceInfor();
            //obj.MACAddress = CommonUtils.GetMACAddress();
            //obj.SecureKey = new Guid().ToString();
            //obj.CreatedOn = DateTime.Now;
            var a = _homeService.ValidateOTP(OTP, RecId, secureKey);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReWriteDetails(int RecId, string secureKey)
        {
            var a = _homeService.ResendOTP(RecId, secureKey);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public SelectList GetDepartmentsList()
        {
            var res = _homeService.GetDepartmentsList();
            return res;
        }

        public SelectList GetTimingsList()
        {

            var res = _homeService.GetTimingsList();

            //res = new SelectList(res
            //                 .Where(x => (Convert.ToDateTime(x.Text) > DateTime.Now))
            //                 .ToList(),
            //                 "Value",
            //                 "Text");
            return res;
        }
    }
}