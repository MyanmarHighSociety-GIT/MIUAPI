using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MIUWebAPI.Helper
{
    public class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ErrorLog(DbEntityValidationException error,string controllerName,string actionName)
        {
            string msg = "";
            foreach (DbEntityValidationResult item in error.EntityValidationErrors)
            {
                DbEntityEntry entry = item.Entry;
                string entityTypeName = entry.Entity.GetType().Name;
                foreach (DbValidationError subItem in item.ValidationErrors)
                {
                    string message = string.Format("Error '{0}' occurred in {1} at {2}",subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                    msg += message;
                }
            }
            string route =string.Format("Error occurred at controller ({0}) and action ({1}) - ", controllerName, actionName);
            log.Error(string.Format(route,msg));
        }      
    }
}