using System;
using System.Collections.Generic;
using System.Text;

namespace JiaHang.Projects.Admin.Model.Enumerations.SysModule
{
    /// <summary>
    /// 权限类别
    /// 位运算区分 该用户有哪些权限
    /// </summary>
    public enum PermissionType
    {
        query = 1,
        add = 2,
        modify = 4,
        delete = 8
    }
}
