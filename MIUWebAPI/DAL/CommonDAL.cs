using MIUWebAPI.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MIUWebAPI.ViewModels.ReportAttendanceInfo;

namespace MIUWebAPI.DAL
{
    public class CommonDAL
    {
        private MIUEntities db = new MIUEntities();
        public AttRateAndPercent GetAttRateAndPercent(int batchID, int studentID)
        {
            using (MIUEntities db = new MIUEntities())
            {
                AttRateAndPercent res = new AttRateAndPercent();
                try
                {
                    List<int> ids = db.Terms.Where(x => x.BatchID == batchID).Select(x => x.ID).ToList<int>();
                    IQueryable<TermDetail> termDetailInfo = null;
                    termDetailInfo = db.TermDetails.Where(x => ids.Contains(x.TermID));

                    var firstSubmissionDates = termDetailInfo.Where(x => x.Term.StartDate < DateTime.Now && x.FirstSubmission >= DateTime.Now);
                    var finalSubmissionDates = termDetailInfo.Where(x => x.FirstResult < DateTime.Now && x.FinalSubmission >= DateTime.Now);
                    //var firstSubmissionDates = tds.Where(x => x.Term.StartDate < DateTime.Now && x.FirstSubmission >= DateTime.Now);
                    //var finalSubmissionDates = tds.Where(x => x.FirstResult < DateTime.Now && x.FinalSubmission >= DateTime.Now);

                    if (firstSubmissionDates != null && firstSubmissionDates.Count() > 0)
                    {
                        var submission = firstSubmissionDates.FirstOrDefault();
                        res.AttendanceRateTermName = submission.Term.TermName;
                        res.CurrentTermId = submission.Term.ID;

                        res.AttenddanceRatePercent = GetAttendanceRateForDashboard(submission.Term.ID, studentID);
                    }
                    else if (finalSubmissionDates != null && finalSubmissionDates.Count() > 0)
                    {
                        var submission = finalSubmissionDates.FirstOrDefault();
                        res.AttendanceRateTermName = submission.Term.TermName;
                        res.CurrentTermId = submission.Term.ID;

                        res.AttenddanceRatePercent = GetAttendanceRateForDashboard(submission.Term.ID, studentID);
                    }
                }
                catch (Exception)
                {

                }
                return res;
            }
        }

        public int GetAttendanceRateForDashboard(int termID, int studentID)
        {
            using (MIUEntities db = new MIUEntities())
            {
                int attendanceRate = 0;

                try
                {
                    var totalTerms = db.Attendances.Where(x => x.TermID == termID);


                    if (studentID != 0)
                        totalTerms = db.Attendances.Where(x => x.TermID == termID && x.StudentID == studentID);

                    if (totalTerms != null)
                    {
                        var totalAttendance = totalTerms.Where(x => x.IsAttending == true);
                        if (totalAttendance != null && totalAttendance.Count() != 0 && totalTerms.Count() != 0)
                        {
                            int val1 = totalAttendance.Count();
                            double val2 = totalTerms.Count();

                            attendanceRate = (int)Math.Round((val1 / val2) * 100);
                        }
                    }
                    return attendanceRate;
                }
                catch (Exception)
                {
                    return attendanceRate;
                }

            }

        }
        
        public IQueryable<TermDetail> GetTermDetailsByBatch(int batchID)
        {
            IQueryable<TermDetail> termDetailInfo = null;
            List<int> ids = db.Terms.Where(x => x.BatchID == batchID).Select(x => x.ID).ToList<int>();
            termDetailInfo = db.TermDetails.Where(x => ids.Contains(x.TermID));
            return termDetailInfo;
        }
    }
}