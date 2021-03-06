﻿using JiaHang.Projects.Admin.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JiaHang.Projects.Admin.BLL.Relation.CurrentUserRouteBLL;

namespace JiaHang.Projects.Admin.Web.WebApiIdentityAuth
{
    /// <summary>
    /// 登录凭据管理
    /// 登录成功时 将用户信息-token  储存到内存中
    /// 登录时保存
    /// 中间件读取
    /// </summary>
    public class CredentialsManage
    {

        private IMemoryCache _cache;
        public CredentialsManage(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string SetAccount(AccountModel account, bool remember)
        {
            var token = Guid.NewGuid().ToString("N");
            _cache.Set<AccountModel>(token, account, DateTimeOffset.Now.AddDays(remember ? 7 : 1));
            return token;
        }
        public AccountModel GetAccount(string token)
        {
            var account = _cache.Get<AccountModel>(token);
            return account;
        }
        public void SetAccountRoute(List<UserRouteModel> routes, string token)
        {
            _cache.Set<List<UserRouteModel>>(token+"_route", routes, DateTimeOffset.Now.AddDays(7));
        }

        public List<UserRouteModel> GetAccountRoute(string token) {
            var routes = _cache.Get<List<UserRouteModel>>(token + "_route");
            return routes;
        }

        

        public void DeleteAccount(string token)
        {
            _cache.Remove(token);
        }
    }
}
