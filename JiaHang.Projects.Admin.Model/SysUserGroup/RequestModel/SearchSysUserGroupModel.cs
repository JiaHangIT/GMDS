﻿using System;
using System.Collections.Generic;
using System.Text;


namespace JiaHang.Projects.Admin.Model
{
    public class SearchSysUserGroupModel
    {
        public int limit { get; set; }
        public int page { get; set; }
        /// <summary>
        /// 模块级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }
    }
}
