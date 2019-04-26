using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model
{
    public class UserRouteModel
    {
        public string ModelGroupId { get; set; }
        public string ModelGroupName { get; set; }
        public string ModelGroupUrl { get; set; }
        public bool OutUrlFlag { get; set; }
        public List<UserModuleRoute> Models { get; set;  }
        public List<UserRouteModel> ModelGroups { get; set; }
        public UserRouteModel() {
            Models = new List<UserModuleRoute>();
            ModelGroups = new List<UserRouteModel>();
        }
    }
    public class UserModuleRoute
    {
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        public string ModelUrl { get; set; }
        public bool OutUrlFlag { get; set; }
    }
}
