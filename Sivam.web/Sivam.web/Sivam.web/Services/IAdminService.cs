using Sivam.web.Data;
using Sivam.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sivam.web.Services
{
    public interface IAdminService
    {
        //string InsertUpdateBanner(BannersModel obj);

        //IEnumerable<BannersModel> GetAllBanners();

        AdminDashboardModel GetDashBoard();
        List<ReportModel> GetBookingReports(FilterModel filters);
        string SaveUpdateVideURL(tblVideo model);
        //string DeleteBanner(int Bid);
    }
}