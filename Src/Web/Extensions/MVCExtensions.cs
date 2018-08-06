using Kasanova.FaldoneFoto.ApplicationCore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public static class MVCExtensions
    {
        /// <summary>
        /// Extension method to convert collection to SelectedListItem MVC
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="selectedItems"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> coll) where T : BaseEntity<string>
        {
            return ToSelectListItems<T>(coll, null, null);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> coll, string selectedItems) where T : BaseEntity<string>
        {
            return ToSelectListItems<T>(coll, selectedItems, null);
        }

        /// <summary>
        /// Extension method to convert collection to SelectedListItem MVC
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="selectedItems"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(this IEnumerable<T> coll, string selectedItems, SelectListItem emptyItem) where T : BaseEntity<string>
        {
            var items = new List<string>();
            if (!string.IsNullOrEmpty(selectedItems))
            {
                items = selectedItems.Split(',').ToList();
            }
            var results = new List<SelectListItem>();
            coll.ToList().ForEach(s =>
            {
                SelectListItem item = null;
                item = new SelectListItem()
                {
                    Value = s.Id,
                    Text = s.Id
                };
                item.Selected = items.Contains(s.Id);
                results.Add(item);
            });
            if (emptyItem != null)
            {
                results.Insert(0, emptyItem);
            }
            return results;
        }

        /// <summary>
        /// Extension method to convert collection to SelectedListItem MVC
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="selectedItems"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectedListItems<T>(this IEnumerable<T> coll) where T : KeyItemValue
        {
            return ToSelectedListItems<T>(coll, null, null);
        }

        /// <summary>
        /// Extension method to convert collection to SelectedListItem MVC
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="selectedItems"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectedListItems<T>(this IEnumerable<T> coll, string selectedItems, SelectListItem emptyItem) where T : KeyItemValue
        {
            var items = new List<string>();
            if (!string.IsNullOrEmpty(selectedItems))
            {
                items = selectedItems.Split(',').ToList();
            }
            var results = new List<SelectListItem>();
            coll.ToList().ForEach(s =>
            {
                SelectListItem item = null;
                item = new SelectListItem()
                {
                    Value = s.Id,
                    Text = s.Description
                };
                item.Selected = items.Contains(s.Id);
                results.Add(item);
            });
            if (emptyItem != null)
            {
                results.Insert(0, emptyItem);
            }
            return results;
        }

        public static T RegisterForDispose<T>(this T disposable, HttpContext context) where T : IDisposable
        {
            context.Response.RegisterForDispose(disposable);
            return disposable;
        }
    }
}
