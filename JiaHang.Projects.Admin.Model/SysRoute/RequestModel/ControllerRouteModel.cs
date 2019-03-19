namespace JiaHang.Projects.Admin.Model.SysRoute
{
    public class ControllerRouteModel
    {
        public string Id { get; set; }
        public string AreaId { get; set; }
        public bool IsApi { get; set; }
        public string ControllerPath { get; set; }
        public string ControllerAlias { get; set; }
        public int SortValue { get; set; }
    }
}
