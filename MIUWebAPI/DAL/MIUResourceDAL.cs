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
    public class MIUResourceDAL
    {
        //public Task<List<string>> GetMIUResourceKeyword()
        //{
        //    return Task.Run(() =>
        //    {
        //        using (MIUEntities db = new MIUEntities())
        //        {
        //            try
        //            {
        //                List<string> infoList = new List<string>();

        //                var KeywordList = db.MICResourceFiles.GroupBy(x => x.Keyword).Select(x => x.Key == null ? "Other" : x.Key).ToList();

        //                return KeywordList;
        //            }
        //            catch (Exception)
        //            {
        //                return null;
        //            }
                    
        //        }
        //    });
        //}

        public Task<List<MIUResourceInfo>> GetMIUResource(string Sorting, string Name)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<MIUResourceInfo> infoList = new List<MIUResourceInfo>();
                    List<MICResourceFile> dataList = db.MICResourceFiles.ToList();
                    try
                    {
                        if (string.IsNullOrEmpty(Sorting) || Sorting == "\"\"")
                        {
                            dataList = dataList.OrderByDescending(x => x.CreatedDate != null ? x.CreatedDate : x.ModifiedDate).OrderByDescending(x => x.IsFolder).ToList();

                            if (!String.IsNullOrEmpty(Name) && Name != "\"\"")
                            {
                                dataList = dataList.Where(x => x.Name.Contains(Name)).ToList();
                            }
                        }
                        else if (Sorting == "ascending")
                        {
                            dataList = dataList.OrderBy(x => x.Name).OrderByDescending(x => x.IsFolder).ToList();

                            if (!String.IsNullOrEmpty(Name) && Name != "\"\"")
                            {
                                dataList = dataList.Where(x => x.Name.Contains(Name)).ToList();
                            }
                        }
                        else if (Sorting == "descending")
                        {
                            dataList = dataList.OrderByDescending(x => x.Name).OrderByDescending(x => x.IsFolder).ToList();

                            if (!String.IsNullOrEmpty(Name) && Name != "\"\"")
                            {
                                dataList = dataList.Where(x => x.Name.Contains(Name)).ToList();
                            }
                        }
                        //if(!String.IsNullOrEmpty(Name) && Name != "\"\"")
                        //{
                        //    dataList = dataList.Where(x => x.Name.Contains(Name)).ToList();
                        //}

                        foreach (var data in dataList)
                        {
                            MIUResourceInfo info = new MIUResourceInfo();

                            PropertyCopier<MICResourceFile, MIUResourceInfo>.Copy(data, info);
                            info.FilePath = MIUFileServer.GetFileUrl("Resources/MIUResource", data.Name);
                            if (data.IsFolder == 1)
                            {
                                info.FileCount = db.MIUResourceFolderDetails.Where(x => x.MIUResourceID == data.ID).Count();
                            }
                            infoList.Add(info);
                        }

                        return infoList;
                    }
                    catch (Exception)
                    {
                        return infoList;
                    }
                }
            });
        }

        public Task<List<MIUResourceFolderDetailInfo>> GetMIUResourceFolderDetail(int MIUResourceID, string Sorting, string Name)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<MIUResourceFolderDetailInfo> infoList = new List<MIUResourceFolderDetailInfo>();
                    List<MIUResourceFolderDetail> dataList = db.MIUResourceFolderDetails.Where(x => x.MIUResourceID == MIUResourceID).ToList();
                    var folderName = db.MICResourceFiles.Where(x => x.ID == MIUResourceID && x.IsFolder == 1).Select(x => x.Name).SingleOrDefault();
                    try
                    {
                        if (string.IsNullOrEmpty(Sorting) || Sorting == "\"\"")
                        {
                            dataList = dataList.OrderByDescending(x => x.CreatedDate != null ? x.CreatedDate : x.ModifiedDate).ToList();

                            if (!String.IsNullOrEmpty(Name) && Name != "\"\"")
                            {
                                dataList = dataList.Where(x => x.Name.Contains(Name)).ToList();
                            }
                        }
                        else if(Sorting == "ascending")
                        {
                            dataList = dataList.OrderBy(x => x.Name).ToList();

                            if (!String.IsNullOrEmpty(Name) && Name != "\"\"")
                            {
                                dataList = dataList.Where(x => x.Name.Contains(Name)).ToList();
                            }
                        }
                        else if(Sorting == "descending")
                        {
                            dataList = dataList.OrderByDescending(x => x.Name).ToList();

                            if (!String.IsNullOrEmpty(Name) && Name != "\"\"")
                            {
                                dataList = dataList.Where(x => x.Name.Contains(Name)).ToList();
                            }
                        }

                        foreach (var data in dataList)
                        {
                            MIUResourceFolderDetailInfo info = new MIUResourceFolderDetailInfo();

                            PropertyCopier<MIUResourceFolderDetail, MIUResourceFolderDetailInfo>.Copy(data, info);
                            info.FilePath = MIUFileServer.GetFileUrl("Resources/MIUResource/" + folderName, data.Name);
                            infoList.Add(info);
                        }

                        return infoList;
                    }
                    catch (Exception)
                    {
                        return infoList;
                    }
                }
            });
        }


    }
}