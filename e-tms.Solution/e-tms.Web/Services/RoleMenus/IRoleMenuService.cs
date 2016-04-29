
     
 
 
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;
using e_tms.Web.Models;
using e_tms.Web.Repositories;

namespace e_tms.Web.Services
{
    public interface IRoleMenuService:IService<RoleMenu>
    {

                  IEnumerable<RoleMenu> GetByMenuId(int  menuid);

                  IEnumerable<RoleMenu> GetByRoleName(string roleName);
                  void Authorize(RoleMenusView[] list);

                  IEnumerable<MenuItem> RenderMenus(string[] roleNames);
      
 
	}
}