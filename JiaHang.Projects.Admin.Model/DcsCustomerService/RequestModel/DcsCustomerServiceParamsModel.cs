using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.DcsCustomerServiceParams.RequestModel
{
    public class DcsCustomerServiceParamsModel
    {
       
       
        public string CustomerId { get; set; }
        public List<ServiceInfo> ServiceInfos { get; set; }
       
    }

    public class ServiceInfo
    {
        public string ServiceId { get; set; }
        public List<string> FieldIds { get; set; }
    }
}
