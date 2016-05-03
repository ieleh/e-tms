﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

//using e_tms.Web.Models;
namespace e_tms.Web
{
    public static class AttributeHelper
    {
        public static string GetDisplayName(object obj, string propertyName)
        {
            if (obj == null) return null;
            return GetDisplayName(obj.GetType(), propertyName);

        }

        public static string GetDisplayName(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null) return null;

            return GetDisplayName(property);
        }

        public static string GetDisplayName(PropertyInfo property)
        {
            var attrName = GetAttributeDisplayName(property);
            if (!string.IsNullOrEmpty(attrName))
                return attrName;

            var metaName = GetMetaDisplayName(property);
            if (!string.IsNullOrEmpty(metaName))
                return metaName;

            return property.Name.ToString();
        }

        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as System.ComponentModel.DataAnnotations.DisplayAttribute).Name;
        }

        private static string GetMetaDisplayName(PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return null;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return null;
            return GetAttributeDisplayName(metaProperty);
        }
        //---------------------------------------------------------------------
        public static bool GetRequired(object obj, string propertyName)
        {
            if (obj == null) return false;
            return GetRequired(obj.GetType(), propertyName);

        }

        public static bool GetRequired(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null) return false;

            return GetRequired(property);
        }
        public static bool GetRequired(PropertyInfo property)
        {
            var required = GetAttributeRequired(property);
            if (required)
                return required;

            required = GetMetaRequired(property);
            return required;

        }
        private static bool GetAttributeRequired(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.RequiredAttribute), true);
            if (atts.Length == 0)
                return false;
            return true;
        }
        private static bool GetMetaRequired(PropertyInfo property)
        {
            var atts = property.DeclaringType.GetCustomAttributes(
                    typeof(MetadataTypeAttribute), true);
            if (atts.Length == 0)
                return false;

            var metaAttr = atts[0] as MetadataTypeAttribute;
            var metaProperty =
                metaAttr.MetadataClassType.GetProperty(property.Name);
            if (metaProperty == null)
                return false;
            return GetAttributeRequired(metaProperty);
        }
    }

    public class ButtonAttribute : ActionMethodSelectorAttribute
    {
        public string Name { get; set; }
        public ButtonAttribute(string name)
        {
            Name = name;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, System.Reflection.MethodInfo methodInfo)
        {
            return controllerContext.Controller.ValueProvider.GetValue(Name) != null;
        }
    }

    public static class CacheExtensions
    {
        static object sync = new object();

        public static T Data<T>(this Cache cache, string cacheKey, int expirationSeconds, Func<T> method)
        {
            var data = cache == null ? default(T) : (T)cache[cacheKey];
            if (data == null)
            {
                data = method();

                if (expirationSeconds > 0 && data != null)
                {
                    lock (sync)
                    {
                        cache.Insert(cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), Cache.NoSlidingExpiration);
                    }
                }
            }
            return data;
        }
    }
    public static class HMTLHelperExtensions
    {

        public static bool IsAuthorize(this HtmlHelper html, string menu)
        {
            //string userid = html.ViewContext.HttpContext.User.Identity.GetUserId();
            //string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            //string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            //string key = userid + currentAction + currentController;
            //// var data= html.ViewContext.HttpContext.Cache.Data<IList<e_tms.Web.Models.RoleMenu>>(key,10000,()=>{
            //var rolemanager = html.ViewContext.HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            //var usermanager = html.ViewContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            //var roles = usermanager.GetRoles(userid);
            //StoreContext db = new StoreContext();
            //var authorize = db.RoleMenus.Where(x => roles.Contains(x.RoleName) && x.MenuItem.Action == currentAction && x.MenuItem.Controller == currentController).ToList();
            ////return authorize;
            ////});
            //var data = authorize;
            //if (menu == "Create")
            //{
            //    return data.Where(x => x.Create == true).Any();
            //}
            //if (menu == "Edit")
            //{
            //    return data.Where(x => x.Edit == true).Any();
            //}
            //if (menu == "Delete")
            //{
            //    return data.Where(x => x.Delete == true).Any();
            //}
            //if (menu == "Import")
            //{
            //    return data.Where(x => x.Import == true).Any();
            //}




            return false;
        }
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.HttpContext.Request.RequestContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
            {
                controller = currentController;
            }

            if (String.IsNullOrEmpty(action))
                action = currentAction;
            var ctrs = controller.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (ctrs.Length > 1)
            {
                return ctrs.Contains(currentController) ? cssClass : String.Empty;
            }
            return controller == currentController && action == currentAction ? cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }

    public static class HtmlExtensions
    {
        public static MvcHtmlString ActionButton(this HtmlHelper html, string linkText, string action, string controllerName, string iconClass)
        {
            //<a href="/@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText"><i class="@lLink.IconClass"></i><span class="">@lLink.LinkText</span></a>
            var lURL = new UrlHelper(html.ViewContext.RequestContext);

            // build the <span class="">@lLink.LinkText</span> tag
            var lSpanBuilder = new TagBuilder("span");
            lSpanBuilder.MergeAttribute("class", "");
            lSpanBuilder.SetInnerText(linkText);
            string lSpanHtml = lSpanBuilder.ToString(TagRenderMode.Normal);

            // build the <i class="@lLink.IconClass"></i> tag
            var lIconBuilder = new TagBuilder("i");
            lIconBuilder.MergeAttribute("class", iconClass);
            string lIconHtml = lIconBuilder.ToString(TagRenderMode.Normal);

            // build the <a href="@lLink.ControllerName/@lLink.ActionName" title="@lLink.LinkText">...</a> tag
            var lAnchorBuilder = new TagBuilder("a");
            lAnchorBuilder.MergeAttribute("href", lURL.Action(action, controllerName));
            lAnchorBuilder.InnerHtml = lIconHtml + lSpanHtml; // include the <i> and <span> tags inside
            string lAnchorHtml = lAnchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(lAnchorHtml);
        }
    }
}