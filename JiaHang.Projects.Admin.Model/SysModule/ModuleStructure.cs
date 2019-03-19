using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model
{
    public class ModuleStructure
    {
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public int ModuleLevel { get; set; }
        public List<ModuleStructure> ListChilds { get; set; }
        public ModuleStructure()
        {
            ListChilds = new List<ModuleStructure>();
        }
    }

}
