using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysDataRightType.RequestModel
{
   public class SearchDataRightTypeRequest
    {
        public int limit { get; set; }
        public int page { get; set; }
        public string DatarightTypeCode { get; set; }
        public string DatarightTypeName { get; set; }
    }
}
