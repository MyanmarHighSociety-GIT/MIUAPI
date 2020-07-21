using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;

namespace MIUWebAPI.Helper
{
    public class Utility
    {
        public static int FIRST_SUBMISSION_TYPE = 1;
        public static int FINAL_SUBMISSION_TYPE = 2;
        public static int User_TYPE_STUDENT = 1;
        public static int User_TYPE_LECTURE = 2;
        public static int User_TYPE_USERS = 3;
        public static int TERM_TYPE_FOUNDATION = 1;
        public static int TERM_TYPE_INTERNAL = 2;
        public static int TERM_TYPE_EXTERNAL = 3;

        public static int FEEDBACK_VERY_SATISFIED = 4;
        public static int FEEDBACK_SATISFIED = 3;
        public static int FEEDBACK_DASSATISFIED = 2;
        public static int FEEDBACK_VERY_DASSATISFIED = 1;

        public static int ENQUIRY_REQUIRED_RESPON = 1;
        public static int ENQUIRY_NORMAL = 2;

       

        public List<SelectListItem> GetEnquiryStatus()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Required Respond", Value = "1"},                
                new SelectListItem {Text = "Normal", Value = "2"},
               
            };
            return listItems;
        }
        public List<SelectListItem> GetRace()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Myanmar", Value = "1"},                
                new SelectListItem {Text = "Chin", Value = "2"},
                new SelectListItem {Text = "Chinese", Value = "3"},
                new SelectListItem {Text = "Indian", Value = "4"},
                new SelectListItem {Text = "Kachin",   Value = "5"},
                new SelectListItem {Text = "Kayah", Value = "6"},                                
                new SelectListItem {Text = "Kayin", Value = "7"},
                new SelectListItem {Text = "Mon", Value = "8"},
                new SelectListItem {Text = "Rakhine", Value = "9"},
                new SelectListItem {Text = "Others", Value = "10"}
            };
            return listItems;
        }

        public List<SelectListItem> GetJobType()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Full Time", Value = "1"},
                new SelectListItem {Text = "Part Time", Value = "2"},
            };
            return listItems;
        }

        public List<SelectListItem> GetStatus()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Active", Value = "1"},
                new SelectListItem {Text = "Terminated", Value = "2"},
                new SelectListItem {Text = "Quit", Value = "3"}
            };
            return listItems;
        }

        public List<SelectListItem> GetReligion()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Buddhism",   Value = "1"},
                new SelectListItem {Text = "Christianity", Value = "2"},
                new SelectListItem {Text = "Hinduism", Value = "3"},  
                new SelectListItem {Text = "Islam", Value = "4"},  
                new SelectListItem {Text = "Others", Value = "5"}  
            };
            return listItems;
        }
        
        public List<SelectListItem> GetMaritalStatus()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Single",   Value = "1"},
                new SelectListItem {Text = "Married", Value = "2"}                
            };
            return listItems;
        }

        public List<SelectListItem> GetGender()
        {
            var listItems = new List<SelectListItem>()
            {
                //new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Male",   Value = "1"},
                new SelectListItem {Text = "Female", Value = "2"}                
            };
            return listItems;
        }

        public List<SelectListItem> GetRelationship()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Mother",   Value = "1"},
                new SelectListItem {Text = "Father", Value = "2"},
                new SelectListItem {Text = "Sister", Value = "3"},
                new SelectListItem {Text = "Brother", Value = "4"},
                new SelectListItem {Text = "Husband", Value = "5"},
                new SelectListItem {Text = "Wife", Value = "5"},
                new SelectListItem {Text = "Relative", Value = "5"},
                new SelectListItem {Text = "Others", Value = "6"}
            };
            return listItems;
        }

        public List<SelectListItem> GetCountry()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select One",   Value = "0"},
                new SelectListItem {Text = "Myanmar",   Value = "1"},
                new SelectListItem {Text = "England", Value = "2"},
                new SelectListItem {Text = "United State", Value = "3"},
                new SelectListItem {Text = "Austriala", Value = "4"},
                new SelectListItem {Text = "Others", Value = "5"}
            };
            return listItems;
        }

        public List<SelectListItem> GetYear()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "Select Year",   Value = "0"}
            };
            for (int i = DateTime.Now.Year; i > 1900 ; i--)
            {
                listItems.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return listItems;
        }


        public List<SelectListItem> GetSkill()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "A", Value = "5"},
                new SelectListItem {Text = "B", Value = "4"},                
                new SelectListItem {Text = "C", Value = "3"},
                new SelectListItem {Text = "D", Value = "2"},
                new SelectListItem {Text = "E", Value = "1"},
                //new SelectListItem {Text = "Kachin",   Value = "5"},
                //new SelectListItem {Text = "Kayah", Value = "6"},                                
                //new SelectListItem {Text = "Kayin", Value = "7"},
                //new SelectListItem {Text = "Mon", Value = "8"},
                //new SelectListItem {Text = "Rakhine", Value = "9"},
                //new SelectListItem {Text = "Others", Value = "10"}
            };
            return listItems;
        }

        public string GetImageNameByFileType(string fileName)
        {
            string imageName = "";
            if (fileName != null)
            {
                var fileTypeArr = fileName.Split(new char[] { '.' });
                var fileType = fileTypeArr[fileTypeArr.Length - 1];

                if (fileType == "docx" || fileType == "doc")
                {
                    imageName = "/Images/word.png";
                }
                else if (fileType == "pdf")
                {
                    imageName = "/Images/pdf.png";
                }
                else if (fileType == "zip")
                {
                    imageName = "/Images/zip.png";
                }
                else if (fileType == "txt")
                {
                    imageName = "/Images/text.png";
                }
                else if (fileType == "pptx")
                {
                    imageName = "/Images/powerpoint.png";
                }
                else if (fileType == "jpg" || fileType == "png" || fileType == "gif" || fileType == "jpeg")
                {
                    imageName = "/Images/image.png";
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    imageName = "/Images/excel.png";
                }
                else
                {
                    imageName = "/Images/folder.png";
                }
            }

            return imageName;
        }

        public string GetImageNameByFileTypeGridView(string fileName)
        {
            string imageName = "";
            if (fileName != null)
            {
                var fileTypeArr = fileName.Split(new char[] { '.' });
                var fileType = fileTypeArr[fileTypeArr.Length - 1];

                if (fileType == "docx" || fileType == "doc")
                {
                    imageName = "/Images/wordLarge.png";
                }
                else if (fileType == "pdf")
                {
                    imageName = "/Images/pdfLarge.png";
                }
                else if (fileType == "zip")
                {
                    imageName = "/Images/zipLarge.png";
                }
                else if (fileType == "txt")
                {
                    imageName = "/Images/textLarge.png";
                }
                else if (fileType == "pptx")
                {
                    imageName = "/Images/powerpointLarge.png";
                }
                else if (fileType == "jpg" || fileType == "png" || fileType == "gif" || fileType == "jpeg")
                {
                    imageName = "/Images/imageLarge.png";
                }
                else if (fileType == "xls" || fileType == "xlsx")
                {
                    imageName = "/Images/excelLarge.png";
                }
                else
                {
                    imageName = "/Images/folderLarge.png";
                }
            }

            return imageName;
        }

        public string RemoveDoubleQuotes(string data)
        {
            string[] dataArray = data.Split('\"');
            string dataValue = String.Empty;
            for (int i = 0; i < dataArray.Length; i++)
            {
                if (i != 0 || i != dataArray.Length - 1)
                {
                    dataValue += dataArray[i];
                }
            }

            return dataValue;

        }


        public static int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            return (date - firstMonthMonday).Days / 7 + 1;
        }

        public static DateTime FirstDayOfMonth(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime LastDayOfMonth(int year, int month)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }

        public static DateTime First_n_DayofMonth(DateTime date, DayOfWeek dow = DayOfWeek.Monday)
        {
            if (dow == DayOfWeek.Monday)
            {
                if (date.DayOfWeek == DayOfWeek.Tuesday) { return date.AddDays(-1); }
                else if (date.DayOfWeek == DayOfWeek.Wednesday) { return date.AddDays(-2); }
            }

            for (int i = 0; i < 7; i++)
            {
                if (date.AddDays(i).DayOfWeek == dow)
                {
                    return date.AddDays(i);
                }
            }
            return date;
        }

        public static DateTime Last_n_DayofMonth(DateTime date, DayOfWeek dow = DayOfWeek.Friday)
        {

            if (dow == DayOfWeek.Friday)
            {
                if (date.DayOfWeek == DayOfWeek.Tuesday) { return date.AddDays(1); }
                else if (date.DayOfWeek == DayOfWeek.Wednesday) { return date.AddDays(2); }
            }

            for (int i = 0; i < 7; i++)
            {
                if (date.AddDays(-i).DayOfWeek == dow)
                {
                    return date.AddDays(-i);
                }
            }
            return date;
        }

        public static DateTime EndDateOfTerm(DateTime startDate, int TermDuration = 2)
        {
            int day = startDate.Day;
            int monthplus = 0;
            monthplus = TermDuration - 1;

            if (day >= 15 && day <= 31) { monthplus++; }

            DateTime nextMonth = startDate.AddMonths(monthplus);

            DateTime nextMonthEnd = LastdateOfMonth(nextMonth.Year, nextMonth.Month);
            return nextMonthEnd;

        }

        public static DateTime StartDateOfTerm(DateTime EndDate)
        {
            DateTime nextMonth = EndDate.AddMonths(1);
            DateTime nextMonthStart = FirstDayOfMonth(nextMonth.Year, nextMonth.Month);
            return nextMonthStart;
        }

        public static DateTime LastdateOfMonth(int year, int month)
        {
            var lastDayOfMonth = DateTime.DaysInMonth(year, month);
            DateTime lastdate = new DateTime(year, month, lastDayOfMonth);
            return lastdate;
        }

        public string[] DaysOfMonth()
        {
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            return days;
        }

        public List<SelectListItem> GetAttendanceDays()
        {
            var listItems = new List<SelectListItem>()
            {
                new SelectListItem {Text = "10",   Value = "10", Selected = true},
                new SelectListItem {Text = "30",   Value = "30"},
                new SelectListItem {Text = "All", Value = "60"}
            };
            return listItems;
        }

        public bool EqualDate(DateTime dt1, DateTime dt2)
        {
            DateTime date1 = new DateTime(dt1.Year, dt1.Month, dt1.Day);
            DateTime date2 = new DateTime(dt2.Year, dt2.Month, dt2.Day);
            bool result = DateTime.Equals(date1, date2);
            return result;
        }

        public bool LessThanOrEqual(DateTime dt1, DateTime dt2)
        {
            DateTime date1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            DateTime date2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 12, 0, 0);
            int result = DateTime.Compare(date1, date2);
           
            if (result < 0) //less than
                return true;
            else if (result == 0)
                return true;

            return false;
        }

        public bool GreaterThanOrEqual(DateTime dt1, DateTime dt2)
        {
            DateTime date1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            DateTime date2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 12, 0, 0);
            int result = DateTime.Compare(date1, date2);
           
            if (result > 0) //less than
                return true;
            else if (result == 0)
                return true;

            return false;
        }

        public bool GreaterThan(DateTime dt1, DateTime dt2)
        {
            DateTime date1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            DateTime date2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 12, 0, 0);
            int result = DateTime.Compare(date1, date2);
            
            if (result > 0) //less than
                return true;

            return false;
        }

        public bool LessThan(DateTime dt1, DateTime dt2)
        {
            DateTime date1 = new DateTime(dt1.Year, dt1.Month, dt1.Day, 0, 0, 0);
            DateTime date2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 12, 0, 0);
            int result = DateTime.Compare(date1, date2);
           
            if (result < 0) //less than
                return true;

            return false;
        }

        public bool IsPass(int gradingOverall)
        {
            return gradingOverall == 1 //Pass 
                ||
                        gradingOverall == 4 //(int)EntityEnum.Grading.Merit 
                        ||
                        gradingOverall == 5;// (int)EntityEnum.Grading.Distinction;
        }

        public bool IsResultOut(int gradingOverall)
        {
            return gradingOverall ==1// (int)EntityEnum.Grading.Pass 
                ||
                        gradingOverall == 4//(int)EntityEnum.Grading.Merit
                        ||
                        gradingOverall == 5//(int)EntityEnum.Grading.Distinction 
                        ||
                        gradingOverall == 2//(int)EntityEnum.Grading.Fail 
                        ||
                        gradingOverall == 3//(int)EntityEnum.Grading.Redo
                        ;
        }

        public static void DeleteFile(string filePath, string fileName)
        {
            string realFilePath = filePath + fileName;

            if (System.IO.File.Exists(realFilePath))
            {
                FileInfo file = new FileInfo(realFilePath);
                file.Delete();
            }
        }

        public static void RenameFile(string filePath, string newname, string oldfilename)
        {
            string realnewFilePath = filePath + newname;
            string realoldFilePath = filePath + oldfilename;

            if (System.IO.File.Exists(realoldFilePath))
            {
                File.Move(realoldFilePath, realnewFilePath);
            }
        }
    }
}