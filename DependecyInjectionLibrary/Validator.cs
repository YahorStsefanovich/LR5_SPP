using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependecyInjectionLibrary
{
     public class Validator
     {
          private IEnumerable<Dependency> dependencies;

          public bool Validate(Configuration config)
          {
               dependencies = config.Dependencies;
               foreach (Dependency dependency in dependencies)
               {
                    bool result = false;
                    if (dependency.pair.Key.IsGenericTypeDefinition &&
                         !dependency.pair.Value.IsGenericTypeDefinition)
                    {
                         return result;
                    }

                    if (dependency.pair.Key != dependency.pair.Value &&
                         !dependency.pair.Key.IsAssignableFrom(dependency.pair.Value))
                         return result;

                    if (dependency.pair.Value.IsAbstract)
                         return result;
               }

               return true;
          }

          //получить последнего наследника от типа t
          private Type GetUpperHeritor(Type t, bool isParent = true)
          {
               foreach (Dependency dependency in dependencies)
               {
                    if (dependency.pair.Key == t)
                    {
                         if (dependency.pair.Value != t)
                              return GetUpperHeritor(dependency.pair.Value, false);
                         else
                              return dependency.pair.Value;
                    }
               }

               if (isParent)
                    return null;
               else
                    return t;
          }
     }
}
