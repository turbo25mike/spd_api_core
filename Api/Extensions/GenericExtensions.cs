using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Api.DataStore;

namespace Api.Extensions
{
    public static class GenericExtensions
    {
        public static object GetValue<T>(this T src, string propertyName)
        {
            return src.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)?.GetValue(src);
        }

        public static void SetValue<T>(this T src, string propertyName, object val)
        {
            src.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)?.SetValue(src, val);
        }

        public static Dictionary<string, object> CreateSet<T>(this T src, string[] setProps = null)
        {
            if (setProps == null)
            {
                setProps = (from prop in src.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            where
                              prop.CanWrite && prop.GetSetMethod(true).IsPublic &&
                              prop.Name != "CreatedBy" && prop.Name != "CreatedDate" &&
                              prop.Name != "UpdatedBy" && prop.Name != "UpdatedDate" &&
                              prop.Name != "RemovedBy" && prop.Name != "RemovedDate"
                            select prop.Name).ToArray();
            }
            var set = new Dictionary<string, object>();
            if (setProps == null) return null;
            foreach (var prop in setProps)
                set.Add(prop, GetValue(src, prop));
            return set;
        }

        public static string SplitNameOnUppercase(this object obj, string seperator = "_")
        {
            var objectName = obj.GetType().Name;
            return SplitNameOnUppercase(objectName, seperator);
        }
        
        public static string SplitNameOnUppercase(this string objectName, string seperator = "_")
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(objectName, seperator).ToLower();
        }

        
    }
}
