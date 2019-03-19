using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JiaHang.Projects.Admin.DAL.EntityFramework;
using JiaHang.Projects.Admin.DAL.EntityFramework.Entity;
using JiaHang.Projects.Admin.Model;
using JiaHang.Projects.Admin.Model.SysRoute;

namespace JiaHang.Projects.Admin.BLL
{
    public class SysRouteBLL
    {
        private readonly DataContext _context;
        public SysRouteBLL(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<FuncResult> Select()
        {

            var controllers = from a in _context.SysControllerRoute
                              where string.IsNullOrWhiteSpace(a.AreaId)
                              join b in _context.SysMethodRoute
                              on a.SysControllerRouteId equals b.ControllerId
                              into b_temp
                              from b_ifnull in b_temp.DefaultIfEmpty()
                              orderby a.ControllerAlias
                              select new
                              {
                                  MethodIfNull = b_ifnull == null,
                                  MethodId = b_ifnull != null ? b_ifnull.Id : null,
                                  MethodPath = b_ifnull != null ? b_ifnull.MethodPath : null,
                                  MethodAlias = b_ifnull != null ? b_ifnull.MethodAlias : null,
                                  MethodType = b_ifnull != null ? b_ifnull.MethodType : null,
                                  MethodSortValue = b_ifnull != null ? b_ifnull.SortValue : 0,
                                  a.IsApi,
                                  a.ControllerPath,
                                  a.ControllerAlias,
                                  ControllerId = a.SysControllerRouteId,
                                  ControllerSortValue = a.SortValue
                              };
         

            object data = null;
            await Task.Run(() =>
            {
                data = new
                {
                    //ApiRoute = new
                    //{
                    //    Areas = areas.GroupBy(e => e.AreaId).Select(r => new
                    //    {
                    //        AreaId = r.Key,
                    //        r.First().AreaPath,
                    //        r.First().AreaAlias,
                    //        Controllers = r.Where(cr => !cr.ControllerIfNull && cr.IsApi).
                    //        GroupBy(cg => cg.ControllerId).Select(controller => new
                    //        {
                    //            controller.First().ControllerId,
                    //            controller.First().ControllerAlias,
                    //            controller.First().ControllerPath,
                    //            controller.First().IsApi,
                    //            Methods = controller.Where(n => !n.MethodIfNull).Select(method => new
                    //            {
                    //                method.MethodId,
                    //                method.MethodPath,
                    //                method.MethodType,
                    //                method.MethodAlias,
                    //                CompletePath = (controller.First().IsApi?"/api":"")+ $"/{r.First().AreaPath}/{controller.First().ControllerPath}/{method.MethodPath}"
                    //            })
                    //        })
                    //    }),
                    //    Controllers = controllers.Where(e => e.IsApi).GroupBy(cg => cg.ControllerId).Select(controller => new
                    //    {
                    //        controller.First().ControllerId,
                    //        controller.First().ControllerAlias,
                    //        controller.First().ControllerPath,
                    //        controller.First().IsApi,
                    //        SortValue=controller.First().ControllerSortValue,
                    //        Methods = controller.Where(n => !n.MethodIfNull).Select(method => new
                    //        {
                    //            method.MethodId,
                    //            method.MethodPath,
                    //            method.MethodType,
                    //            method.MethodAlias,
                    //            SortValue=method.MethodSortValue,
                    //            CompletePath = (controller.First().IsApi ? "/api" : "") + $"/{controller.First().ControllerPath}/{method.MethodPath}"
                    //        })
                    //    })
                    //},
                    ViewRoute = new
                    {
                        //Areas = areas.GroupBy(e => e.AreaId).Select(r => new
                        //{
                        //    AreaId = r.Key,
                        //    r.First().AreaPath,
                        //    r.First().AreaAlias,
                        //    Controllers = r.Where(cr => !cr.ControllerIfNull && !cr.IsApi).GroupBy(cg => cg.ControllerId).Select(controller => new
                        //    {
                        //        controller.First().ControllerId,
                        //        controller.First().ControllerAlias,
                        //        controller.First().ControllerPath,
                        //        controller.First().IsApi,

                        //        Methods = controller.Where(n => !n.MethodIfNull).Select(method => new
                        //        {
                        //            method.MethodId,
                        //            method.MethodPath,
                        //            method.MethodType,
                        //            method.MethodAlias,

                        //            CompletePath = (controller.First().IsApi ? "/api" : "") + $"/{r.First().AreaPath}/{controller.First().ControllerPath}/{method.MethodPath}"
                        //        })
                        //    })
                        //}),
                        Controllers = controllers.Where(e => !(e.IsApi==1)).GroupBy(cg => cg.ControllerId).Select(controller => new
                        {
                            controller.First().ControllerId,
                            controller.First().ControllerAlias,
                            controller.First().ControllerPath,
                            controller.First().IsApi,
                            SortValue = controller.First().ControllerSortValue,
                            Methods = controller.Where(n => !n.MethodIfNull).OrderByDescending(me => me.MethodSortValue).Select(method => new
                            {
                                method.MethodId,
                                method.MethodPath,
                                method.MethodType,
                                method.MethodAlias,
                                SortValue = method.MethodSortValue,
                                CompletePath = ((controller.First().IsApi==1) ? "/api" : "") + $"/{controller.First().ControllerPath}/{method.MethodPath}"
                            })
                        }).OrderByDescending(cce => cce.SortValue)
                    },
                };
            });

            return new FuncResult() { IsSuccess = true, Content = data };
        }


      
        public async Task<FuncResult> AddOrUpdateController(ControllerRouteModel model, string currentUserId)
        {



            if (string.IsNullOrWhiteSpace(model.ControllerPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "Controller路径不能为空" };
            }
            if (!Regex.Match(model.ControllerPath, @"^[a-zA-Z0-9]+$").Success)
            {
                return new FuncResult() { IsSuccess = false, Message = "ControllerPath只能输入字母、数字" };
            }
            if (string.IsNullOrWhiteSpace(model.ControllerAlias))
            {
                model.ControllerAlias = model.ControllerPath;
            }
            if (_context.SysControllerRoute.Where(e => e.SysControllerRouteId != model.Id).Select(e => e.ControllerPath.ToLower()).Contains(model.ControllerPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "路径重复" };
            }
            if (_context.SysControllerRoute.Where(e => e.SysControllerRouteId != model.Id).Select(e => e.ControllerAlias.ToLower()).Contains(model.ControllerAlias))
            {
                return new FuncResult() { IsSuccess = false, Message = "别名重复" };
            }



            SysControllerRoute entity = new SysControllerRoute();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity = await _context.SysControllerRoute.FindAsync(model.Id);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "id错误" };
                }
                entity.ControllerAlias = model.ControllerAlias;
                entity.ControllerPath = model.ControllerPath;
                entity.AreaId = model.AreaId;
                entity.IsApi = model.IsApi?1:0;
                entity.SortValue = model.SortValue;

                entity.LastUpdatedBy = currentUserId;
                entity.LastUpdateDate = DateTime.Now;
                _context.SysControllerRoute.Update(entity);
                await _context.SaveChangesAsync();
                return new FuncResult() { IsSuccess = true, Content = entity, Message = "编辑成功" };
            }

            entity.SysControllerRouteId = Guid.NewGuid().ToString("N");
            entity.ControllerAlias = model.ControllerAlias;
            entity.ControllerPath = model.ControllerPath;
            entity.AreaId = model.AreaId;
            entity.IsApi = model.IsApi?1:0;
            entity.SortValue = model.SortValue;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;
            entity.CreatedBy = currentUserId;
            entity.CreationDate = DateTime.Now;
            try
            {
                await _context.SysControllerRoute.AddAsync(entity);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                LogService.WriteError(ex);
                return new FuncResult() { IsSuccess = false, Message = "发生了预料之外的错误" };
            }
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }


        public async Task<FuncResult> AddOrUpdateMethod(MethodRouteModel model, string currentUserId)
        {
            if (await _context.SysControllerRoute.FindAsync(model.ControllerId) == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "ControllerId错误!" };
            }
            if (string.IsNullOrWhiteSpace(model.MethodType))
            {
                return new FuncResult() { IsSuccess = false, Message = "Method路径不能为空" };
            }

            if (!Regex.Match(model.MethodPath, @"^[a-zA-Z0-9]+$").Success)
            {
                return new FuncResult() { IsSuccess = false, Message = "MethodPath只能输入字母、数字" };
            }

            if (string.IsNullOrWhiteSpace(model.MethodAlias))
            {
                model.MethodAlias = model.MethodAlias;
            }
            if (string.IsNullOrWhiteSpace(model.MethodType))
            {
                model.MethodType = "HttpGet";
            }
            if (_context.SysMethodRoute.Where(e => e.ControllerId == model.ControllerId && e.Id != model.Id).Select(e => e.MethodPath.ToLower()).Contains(model.MethodPath))
            {
                return new FuncResult() { IsSuccess = false, Message = "路径重复" };
            }
            if (_context.SysMethodRoute.Where(e => e.ControllerId == model.ControllerId && e.Id != model.Id).Select(e => e.MethodAlias.ToLower()).Contains(model.MethodAlias))
            {
                return new FuncResult() { IsSuccess = false, Message = "别名重复" };
            }



            SysMethodRoute entity = new SysMethodRoute();
            if (!string.IsNullOrWhiteSpace(model.Id))
            {
                entity = await _context.SysMethodRoute.FindAsync(model.Id);
                if (entity == null)
                {
                    return new FuncResult() { IsSuccess = false, Message = "id错误" };
                }
                entity.MethodAlias = model.MethodAlias;
                entity.MethodPath = model.MethodPath;
                entity.ControllerId = model.ControllerId;
                entity.MethodType = model.MethodType;
                entity.SortValue = model.SortValue;

                entity.LastUpdatedBy = currentUserId;
                entity.LastUpdateDate = DateTime.Now;
                _context.SysMethodRoute.Update(entity);
                await _context.SaveChangesAsync();
                return new FuncResult() { IsSuccess = true, Content = entity, Message = "编辑成功" };
            }

            entity.Id = Guid.NewGuid().ToString("N");
            entity.MethodAlias = model.MethodAlias;
            entity.MethodPath = model.MethodPath;
            entity.ControllerId = model.ControllerId;
            entity.MethodType = model.MethodType;
            entity.SortValue = model.SortValue;
            entity.LastUpdatedBy = currentUserId;
            entity.LastUpdateDate = DateTime.Now;
            entity.CreatedBy = currentUserId;
            entity.CreationDate = DateTime.Now;

            await _context.SysMethodRoute.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "添加成功" };
        }



        public async Task<FuncResult> DeleteControllerRoute(string id, string currentUserId)
        {
            SysControllerRoute entity = await _context.SysControllerRoute.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "ControllerId错误" };
            }
            if (_context.SysMethodRoute.Count(e => e.ControllerId == id) > 0)
            {
                return new FuncResult() { IsSuccess = false, Message = "该Controller下还存在Method,请先删除其所有的Method" };
            }
            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteTime = DateTime.Now;
            _context.SysControllerRoute.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }

        public async Task<FuncResult> DeleteMethodRoute(string id, string currentUserId)
        {
            SysMethodRoute entity = await _context.SysMethodRoute.FindAsync(id);
            if (entity == null)
            {
                return new FuncResult() { IsSuccess = false, Message = "MethodId错误" };
            }

            entity.DeleteFlag = 1;
            entity.DeleteBy = currentUserId;
            entity.DeleteTime = DateTime.Now;
            _context.SysMethodRoute.Update(entity);
            await _context.SaveChangesAsync();
            return new FuncResult() { IsSuccess = true, Content = entity, Message = "删除成功" };
        }
    }
}
