using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysUserInfo.RequestModel
{
    public class SearchSysUserInfo
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string User_Account { get; set; }
        public string User_Name { get; set; }
        public int? User_Is_Ldap { get; set; }
        public int? User_Ower { get; set; }
       
    }
}
