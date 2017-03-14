using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapayaX2.Database;

namespace PapayaX2.Models
{
    public class AclManageModel
    {
        public IEnumerable<rs_useracl> user_acl { get; set; }
        public IEnumerable<rs_action> action { get; set; }
    }
}