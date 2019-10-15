using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using JiaHang.Projects.Admin.Model.Enumerations.Sys_User;

namespace JiaHang.Projects.Admin.Model.SysUserInfo.RequestModel
{
    public class SysUserInfoModel
    {
        [StringLength(30)]
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [StringLength(30)]
        public string UserPassword { get; set; }

        /// <summary>
        /// 用户组织ID
        /// </summary>
        public string UserOrgId { get; set; }

        /// <summary>
        /// 用户组别名称列表，用逗号分割
        /// </summary>
        public string UserGroupNames { get; set; }

        /// <summary>
        /// 用户电子邮件地址
        /// </summary>
        [StringLength(60)]
        public string UserEmail { get; set; }

        /// <summary>
        /// 用户手机号码
        /// </summary>
        [StringLength(30)]
        public string UserMobileNo { get; set; }





    }
}
