using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
               bool result = false;
               foreach (Dependency dependency in dependencies)
               {
                    if ((dependency.pair.Key.IsGenericTypeDefinition &&
                         !dependency.pair.Value.IsGenericTypeDefinition) ||
                         (dependency.pair.Key != dependency.pair.Value &&
                         !dependency.pair.Key.IsAssignableFrom(dependency.pair.Value)) ||
                         dependency.pair.Value.IsAbstract)
                    {
                         return result;
                    }
                    else
                    {
                         Type createType = GetUpperHeritor(dependency.pair.Value) ?? dependency.pair.Value;
                         List<Type> notAllowedTypes = new List<Type>();

                         notAllowedTypes.Add(createType);
                         foreach (ConstructorInfo constructorInfo in createType.GetConstructors())
                         {
                              if (result = CheckParameters(constructorInfo, notAllowedTypes))
                              {
                                   break;
                              }
                         }

                         if (!result)
                              return false;
                    }
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

          //проверить рекурсивно все типы всех параметров всех конструкторов на возможность создания
          private bool CheckParameters(ConstructorInfo constructor, List<Type> notAllowedTypes)
          {
               foreach (ParameterInfo param in constructor.GetParameters())
               {
                    bool result = false;
                    List<Type> types;
                    Type t = GetUpperHeritor(param.ParameterType);
                    Type[] genericArgs = null;

                    if (t == null && param.ParameterType.IsGenericType)
                    {
                         genericArgs = param.ParameterType.GenericTypeArguments;
                         t = GetUpperHeritor(param.ParameterType.GetGenericTypeDefinition());
                    }

                    if (t == null || notAllowedTypes.Contains(t))
                         return false;

                    if (t.IsGenericTypeDefinition)
                    {
                         try
                         {
                              t = t.MakeGenericType(genericArgs);
                         }
                         catch
                         {
                              throw new InvalidOperationException();
                         }
                    }

                    foreach (ConstructorInfo constructorInfo in t.GetConstructors())
                    {
                         types = new List<Type>(notAllowedTypes);
                         types.Add(t);
                         if (result = CheckParameters(constructorInfo, types))
                              break;
                    }

                    if (!result)
                         return false;
               }

               return true;
          }
     }
}
