using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatApi.Models
{
    public class Crime
    {
        public Guid Id { get; set; }
        public string Request { get; set; }
        public string Url { get; set; }
        public string PathToFile { get; set; }
    }
}