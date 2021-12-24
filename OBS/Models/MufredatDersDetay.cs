using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OBS.Models
{
    public class MufredatDersDetay
    {
        public int MufredatID { get; set; }
        public int DersID { get; set; }
        public List<SelectListItem> DDLDers { get; set; }
        public List<SelectListItem> DDLMufredat { get; set; }
    }
}