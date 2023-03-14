using Sivam.web.Data;
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
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            this._adminService = adminService;
        }
        // GET: Admin
        public ActionResult Index()
        {
            var model = _adminService.GetDashBoard();
            return View(model);
        }
        public ActionResult Reports()
        {
            var model = _adminService.GetBookingReports(new FilterModel());
            return View(model);
        }

        public ActionResult AddVideoUrl()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddVideoUrl(tblVideo model)
        {
            var res = _adminService.SaveUpdateVideURL(model);
            if (res == "insert")
            {
                TempData["message"] = "Video URL saved successfully.";
            }
            else if (res == "update")
            {
                TempData["message"] = "Video URL has been updated successfully.";
            }
            else
            {
                TempData["message"] = "Error : " + res + "!";
            }
            return RedirectToAction("AddVideoUrl", new { msg = res });
        }
        public ActionResult ManageVideoURLs()
        {
            return View();
        }
        //public ActionResult AddBanner(int bid = 0, string msg = "")
        //{
        //    var model = new BannersModel();
        //    if (bid != 0)
        //    {
        //        model = _adminService.GetBanner(bid);
        //    }
        //    return View(model);
        //}

        //public ActionResult AddUpdateBanner(BannersModel obj)
        //{
        //    obj.ImgUrl = CommonUtils.UploadImage(obj.ImgUrl, CommonUtils.GenerateCode());
        //    string res = _adminService.InsertUpdateBanner(obj);
        //    if (res == "insert")
        //    {
        //        TempData["message"] = "Banner saved successfully.";
        //    }
        //    else if (res == "update")
        //    {
        //        TempData["message"] = "Banner has been updated successfully.";
        //    }
        //    else
        //    {
        //        TempData["message"] = "Error : " + res + "!";
        //    }
        //    return RedirectToAction("AddBanner", new { msg= res});

        //}

        public JsonResult GetFilteredReport(FilterModel obj)
        {
            var model = _adminService.GetBookingReports(obj);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}