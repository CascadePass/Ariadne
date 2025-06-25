using System;
using System.Windows;

namespace CascadePass.CascadeCore.UI
{
    public static class ResourceLocator
    {
        public static T GetResource<T>(string key) where T : class
        {
            if (Application.Current == null)
            {
                throw new InvalidOperationException("Application.Current is null. Ensure WPF app is properly initialized.");
            }

            if (Application.Current.Resources.Contains(key))
            {
                return Application.Current.Resources[key] as T;
            }

            foreach (var dictionary in Application.Current.Resources.MergedDictionaries)
            {
                if (dictionary.Contains(key))
                    return dictionary[key] as T;
            }

            return null;
        }

        public static object GetResource(string key)
        {
            return ResourceLocator.GetResource<object>(key);
        }
    }
}
