using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.Enumerations.SysModule
{
    /// <summary>
    /// 模块-用户 /用户组枚举
    /// </summary>
    public enum ModuleUserRelation
    {
        None=0,
        ModuleUser = 1, //根据userid从user 表取用户
        ModuleUserGroup = 2//根据userid 从user_group取模块组
    }
}
