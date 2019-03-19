using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JiaHang.Projects.Admin.Model
{
    public class UserGroupModel
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        [StringLength(30)]
        public string Name { get; set; }


        /// <summary>
        /// 模块父级Id 
        /// 递归获取  
        /// </summary>
        public string ParentId { get; set; }

    }
    
}
