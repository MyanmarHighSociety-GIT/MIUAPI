using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class HolidayWrap
    {
        public int TotalHoliday { get; set; }

        public List<HolidayInfo> HolidayInfo { get; set; }
    }

    public class HolidayInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime Date { get; set; }
        public string Day { get; set; }
        public string Week { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
    }
}