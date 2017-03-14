using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapayaX2.Database;

namespace PapayaX2.Models
{
    public class MenuModel
    {
        public IEnumerable<rs_module> module { get; set; }
        public IEnumerable<rs_useracl> acl { get; set; }
    }
}