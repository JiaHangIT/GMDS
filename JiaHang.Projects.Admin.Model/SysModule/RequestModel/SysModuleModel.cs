using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model.SysModule.RequestModel
{
    public class SysModuleModel
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        [StringLength(30)]
        public string ModuleName { get; set; }


        /// <summary>
        /// 模块父级Id 
        /// 递归获取  
        /// </summary>
        public string ParentId { get; set; }
        public int SortValue { get; set; }

    }
    
}
