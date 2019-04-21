using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysOperRightParamsModel.RequestModel
{
    public class SysOperRightParamsModel
    {
        public string ModelId { get; set; }
        public List<string> userIds { get; set; }
        public List<string> userGroupIds { get; set; }       
    }
    public class SysOperRightDeleteParamsModel
    {
        public string ModelId { get; set; }
        public List<string> operids { get; set; }
    }
}
