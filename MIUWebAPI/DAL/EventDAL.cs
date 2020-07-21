using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class EventDAL
    {
        public Task<List<EventInfo>> GetEvent(int Take)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<EventInfo> infoList = new List<EventInfo>();
                    List<Event> List = new List<Event>();
                    try
                    {
                        List = db.Events.Where(x => x.Status != true).OrderByDescending(x => x.ModifiedDate != null ? x.ModifiedDate : x.CreatedDate).ToList();

                        if (Take != 0)
                        {
                            List = List.Take(Take).ToList();
                        }

                        foreach (var data in List)
                        {
                            EventInfo info = new EventInfo();
                            PropertyCopier<Event, EventInfo>.Copy(data, info);
                            //data.EventDetails = db.EventDetails.Where(x => x.EventID == data.ID).ToList();

                            foreach (var detail in data.EventDetails)
                            {
                                EventDetailInfo detailinfo = new EventDetailInfo();
                                PropertyCopier<EventDetail, EventDetailInfo>.Copy(detail, detailinfo);
                                detail.FileName = MIUFileServer.GetFileUrl("EventCalendar", detail.FileName);
                            }
                            infoList.Add(info);
                        }
                    }
                    catch (Exception)
                    {
                        
                    }

                    return infoList;
                }
                    
            });
        }
}
}