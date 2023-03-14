using Sivam.web.Data;
using Sivam.web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Web;

namespace Sivam.web.Services
{
    public class AdminService : IAdminService
    {
        public AdminDashboardModel GetDashBoard()
        {
            try
            {
                using (var context = new ShivamDBEntities())
                {
                    var source = context.Database.SqlQuery<AdminDashboardModel>("sp_getAdminDashboard").FirstOrDefault();
                    return source;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ReportModel> GetBookingReports(FilterModel filters)
        {
            try
            {
                using (var context = new ShivamDBEntities())
                {
                    //SqlParameter sDate = new SqlParameter("@startdate", SqlDbType.Text);
                    //sDate.Value = filters.StartDate ?? string.Empty;

                    //SqlParameter eDate = new SqlParameter("@enddate", SqlDbType.DateTime);
                    //eDate.IsNullable = true;
                    //eDate.Value = filters.EndDate ?? SqlDateTime.Null;

                    var sDate = new SqlParameter("@startdate", filters.StartDate ?? "");
                    var eDate = new SqlParameter("@enddate", filters.EndDate ?? "");
                    var sts = new SqlParameter("@status", filters.Status);
                    var fltr = new SqlParameter("@filter", filters.Filter);
                    var source = context.Database.SqlQuery<ReportModel>("sp_getReport @startdate, @enddate, @status, @filter",
                        sDate, eDate, sts, fltr).ToList();
                    return source;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaveUpdateVideURL(tblVideo obj)
        {
            string optype = string.Empty;
            try
            {
                using (var context = new ShivamDBEntities())
                {
                    if (obj.RecId == 0)
                    {
                        obj.CreatedOn = DateTime.Now;
                        obj.ModifiedOn = DateTime.Now;
                        //obj.IsActive = true;

                        context.Videos.Add(obj);
                        context.SaveChanges();
                        optype = "insert";
                    }
                    else
                    {
                        var data = context.Videos.Find(obj.RecId);
                        if (data != null)
                        {
                            data.Title = obj.Title;
                            data.ModifiedOn = DateTime.Now;
                            data.VideoUrl = obj.VideoUrl;
                            data.Description = obj.Description;
                            data.IsActive = obj.IsActive;
                        }
                        context.SaveChanges();
                        optype = "update";
                    }
                }
                return optype;
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
                return raise.ToString();

            }
            catch (DbUpdateException e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

                foreach (var eve in e.Entries)
                {
                    sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
                }


                return sb.ToString();
            }
            catch (Exception dbEx)
            {
                return dbEx.Message.ToString();
            }
        }
        //public string InsertUpdateBanner(BannersModel obj)
        //{
        //    string optype = string.Empty;
        //    try
        //    {
        //        using (var context = new ShivamDBContext())
        //        {
        //            if (obj.Bid == 0)
        //            {
        //                BannersModel bnr = new BannersModel();
        //                //{
        //                bnr.Title = obj.Title;
        //                bnr.CreatedOn = DateTime.Now;
        //                bnr.ModifiedOn = DateTime.Now;
        //                bnr.IsActive = true;
        //                bnr.ImgUrl = obj.ImgUrl;
        //                //};

        //                context.Banner.Add(bnr);
        //                context.SaveChanges();
        //                optype = "insert";
        //            }
        //            else
        //            {
        //                var data = context.Banner.Find(obj.Bid);
        //                if (data != null)
        //                {
        //                    data.Title = obj.Title;
        //                    data.ModifiedOn = DateTime.Now;
        //                    data.ImgUrl = obj.ImgUrl;
        //                    data.IsActive = obj.IsActive;
        //                }
        //                context.SaveChanges();
        //                optype = "update";
        //            }
        //        }
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        Exception raise = dbEx;
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                string message = string.Format("{0}:{1}",
        //                    validationErrors.Entry.Entity.ToString(),
        //                    validationError.ErrorMessage);
        //                // raise a new exception nesting  
        //                // the current instance as InnerException  
        //                raise = new InvalidOperationException(message, raise);
        //            }
        //        }
        //        throw raise;

        //    }
        //    catch (DbUpdateException e)
        //    {
        //        var sb = new StringBuilder();
        //        sb.AppendLine($"DbUpdateException error details - {e?.InnerException?.InnerException?.Message}");

        //        foreach (var eve in e.Entries)
        //        {
        //            sb.AppendLine($"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated");
        //        }


        //        optype = sb.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        optype = "Error !";
        //    }
        //    return optype;
        //}

        //public IEnumerable<BannersModel> GetAllBanners()
        //{
        //    try
        //    {
        //        using (var dbContext = new ShivamDBContext())
        //        {
        //            return dbContext.Banner?.OrderByDescending(x => x.Bid).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public BannersModel GetBanner(int id)
        //{
        //    try
        //    {
        //        using (var dbContext = new ShivamDBContext())
        //        {
        //            return dbContext.Banner.Where(x => x.Bid == id).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}