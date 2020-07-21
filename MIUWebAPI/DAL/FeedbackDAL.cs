using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MIUWebAPI.ViewModels.FeedbackInfo;

namespace MIUWebAPI.DAL
{
    public class FeedbackDAL
    {

        public Task<List<FeedbackReasonList>> GetFeedbackReasonList()
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        List<FeedbackReason> dataList = db.FeedbackReasons.ToList();
                        List<FeedbackReasonList> infoList = new List<FeedbackReasonList>();
                        foreach (var data in dataList)
                        {
                            FeedbackReasonList info = new FeedbackReasonList();
                            PropertyCopier<FeedbackReason, FeedbackReasonList>.Copy(data, info);
                            infoList.Add(info);
                        }
                        return infoList;

                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
            });
        }

        public Task<JsonResponse> PostFeedback(int reasonID, string comment, int react, string fullname)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        StudentFeedback feedback = new StudentFeedback()
                        {
                            ReasonID = reasonID,
                            Comment = comment,
                            React = react,
                            CreatedBy = fullname,
                            CreatedDate = DateTime.Now
                        };
                        db.StudentFeedbacks.Add(feedback);
                        db.SaveChanges();
                        return new JsonResponse() { Flag = true, Message = "Successfully Saved" };
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = true, Message = ex.Message };
                    }
                }
            });
        }

    }
}