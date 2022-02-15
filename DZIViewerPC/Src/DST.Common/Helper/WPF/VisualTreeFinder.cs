using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace DST.Common.Helper
{
    public static class VisualTreeFinder
    {
        /// <summary>
        /// 获取控件下的某一类型的子控件
        /// </summary>
        /// <typeparam name="T">子控件类型</typeparam>
        public static List<T> GetChildObjects<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }

        /// <summary>
        /// 获取指定名字的控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }

        /// <summary>
        /// 根据控件名称，查找子控件集合
        /// elementName为空时，查找指定类型的所有子控件
        /// </summary>
        public static List<T> GetChildsByName<T>(DependencyObject obj, string elementName) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == elementName) || (string.IsNullOrEmpty(elementName)))
                {
                    childList.Add((T)child);
                }
                else
                {
                    List<T> grandChildList = GetChildsByName<T>(child, elementName);
                    if (grandChildList != null)
                    {
                        childList.AddRange(grandChildList);
                    }
                }
            }
            return childList;
        }

        public static T GetParent<T>(Visual obj) where T : Visual
        {
            DependencyObject parent = obj;
            while (parent != null)
            {
                if (parent is T)
                {
                    break;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
            }

            return parent as T;
        }
        /// <summary>
        /// 获取控件下的某一类型的子控件
        /// </summary>
        /// <typeparam name="T">子控件类型</typeparam>
        public static List<T> FindChildren<T>(this DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(child.FindChildren<T>());
            }
            return childList;
        }
    }
}