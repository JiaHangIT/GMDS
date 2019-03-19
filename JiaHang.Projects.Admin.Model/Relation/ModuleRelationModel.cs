using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model
{


    public class ModuleRouteRelationRequestModel
    {
        public string Id { get; set; }
        public string ModuleId { get; set; }
        public string ControllerId { get; set; }
    }

    public class ModuleRouteRelationResponseModel
    {
        public string id { get; set; }

        public string label { get; set; }
        public int ModuleLevel { get; set; }
        public string ModuleId { get; set; }
        public string ModuleParentId { get; set; }
        public List<AreaResponseModel> Areas { get; set; }
        public List<ControllerResponseModel> Controllers { get; set; }
        public List<ModuleRouteRelationResponseModel> children { get; set; }
        public ModuleRouteRelationResponseModel()
        {
            children = new List<ModuleRouteRelationResponseModel>();
        }
    }
    public class AreaResponseModel
    {
        public string AreaId { get; set; }
        public string AreaAlias { get; set; }
        public string AreaPath { get; set; }
        public List<ControllerResponseModel> Controllers { get; set; }
    }
    public class ControllerResponseModel
    {
        public string RelationId { get; set; }
        public string ControllerId { get; set; }
        public string ControllerPath { get; set; }
        public string ControllerAlias { get; set; }
        public bool ControllerIsApi { get; set; }
        public List<MethodResponseModel> Methods { get; set; }
    }
    public class MethodResponseModel
    {

        public string MethodId { get; set; }
        public string MethodAlias { get; set; }
        public string MethodPath { get; set; }
        public string MethodType { get; set; }
        public string CompletePath { get; set; }
    }
}
