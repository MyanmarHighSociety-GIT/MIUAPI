using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.Models
{
    public class JsonResponse
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public string ReferenceKey { get; set; }
    }

    public class JsonResult
    {
        public bool Flag { get; set; }
        public int Result { get; set; }
    }
}