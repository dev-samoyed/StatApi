using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace StatApi.Models
{
    public class Context : DbContext
    {
        public DbSet<Crime> Crimes { get; set; }
    }
}