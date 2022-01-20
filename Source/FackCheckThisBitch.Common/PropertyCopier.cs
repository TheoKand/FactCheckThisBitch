using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FackCheckThisBitch.Common
{
    public class PropertyCopier<TParent, TChild> where TParent : class
        where TChild : class
    {
        public static void Copy(TParent parent, TChild child)
        {
            if (parent == null || child == null) return;
            var parentProperties = parent.GetType().GetProperties(BindingFlags.Instance 
                                                                  | BindingFlags.Public 
                                                                  | BindingFlags.DeclaredOnly);
            var childProperties = child.GetType().GetProperties(BindingFlags.Instance 
                                                                | BindingFlags.Public 
                                                                | BindingFlags.DeclaredOnly);

            foreach (var parentProperty in parentProperties)
            {
                foreach (var childProperty in childProperties)
                {
                    if (parentProperty.Name == childProperty.Name &&
                        parentProperty.PropertyType == childProperty.PropertyType)
                    {
                        childProperty.SetValue(child, parentProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }
    }
}
