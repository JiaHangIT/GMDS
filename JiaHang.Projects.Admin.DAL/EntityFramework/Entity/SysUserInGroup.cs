﻿using System;
using System.Collections.Generic;

namespace JiaHang.Projects.Admin.DAL.EntityFramework.Entity
{
    public partial class SysUserInGroup
    {
        public string UserId { get; set; }
        public string UserGroupId { get; set; }
        public int DeleteFlag { get; set; }
    }
}
