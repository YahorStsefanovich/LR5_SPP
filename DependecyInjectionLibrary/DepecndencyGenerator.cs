using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DependecyInjectionLibrary
{
     public class DepecndencyGenerator
     {
          private Validator validator;
          private List<Dependency> dependecies;
          private Dictionary<KeyValuePair<Type, Type>, object> singltonList;
          

          public DepecndencyGenerator(Configuration config)
          {
               singltonList = new Dictionary<KeyValuePair<Type, Type>, object>();
               dependecies = new List<Dependency>(config.Dependencies);
               validator = new Validator();

               if (!validator.Validate(config))
                    throw new ArgumentException("Invalid argument");

          }

          public T Resolve<T>()
               where T : class
          {
               T result = null;
               if (!typeof(T).IsGenericTypeDefinition)
               {
                    foreach  (Dependency dependency in dependecies)
                    {
                         if (dependency.pair.Key == typeof(T))
                         {
                              return (T)Generate(Validator.GetUpperHeritor(dependecies, dependency.pair.Value) ??
                                   dependency.pair.Value, dependency);
                         }
                    }

                    if (typeof(T).IsGenericType)
                    {
                         foreach (Dependency dependency in dependecies)
                         {
                              if (dependency.pair.Key == typeof(T).GetGenericTypeDefinition())
                              {
                                   try
                                   {
                                        Type generic = (Validator.GetUpperHeritor(dependecies, dependency.pair.Value) ?? dependency.pair.Value)
                                             .MakeGenericType(typeof(T).GenericTypeArguments);
                                        result = (T)Generate(generic,
                                             new Dependency(new KeyValuePair<Type, Type>(typeof(T), dependency.pair.Value.MakeGenericType(typeof(T).GenericTypeArguments)),
                                             dependency.Pattern));
                                   }
                                   catch
                                   {
                                        throw new InvalidCastException();
                                   }
                              }
                         }
                    }
               }

               return result;
          }

          public IEnumerable<T> ResolveAll<T>()
               where T : class
          {
               List<T> result = null;
               if (!typeof(T).IsGenericTypeDefinition)
               {
                    result = new List<T>();
                    foreach (Dependency dependency in dependecies)
                    {
                         if (dependency.pair.Key == typeof(T))
                         {
                              result.Add((T)Generate(Validator.GetUpperHeritor(this.dependecies, dependency.pair.Value)
                                   ?? dependency.pair.Value, dependency));
                         }
                    }

                    if (typeof(T).IsGenericType)
                    {
                         foreach(Dependency dependency in dependecies)
                         {
                              if (dependency.pair.Key == typeof(T).GetGenericTypeDefinition())
                              {
                                   try
                                   {
                                        Type generic = (Validator.GetUpperHeritor(this.dependecies, dependency.pair.Value) ?? dependency.pair.Value)
                                             .MakeGenericType(typeof(T).GenericTypeArguments);

                                        result.Add((T)Generate(generic, 
                                             new Dependency(
                                                  new KeyValuePair<Type, Type>(
                                                       typeof(T), dependency.pair.Value.MakeGenericType(
                                                            typeof(T).GenericTypeArguments)),
                                                       dependency.Pattern

                                             )));
                                   }
                                   catch
                                   {
                                        throw new InvalidCastException();
                                   }
                              }
                         }
                    }
               }

               return result;

          }

          private object Generate(Type type, Dependency dependency)
          {
               object result;
               if (dependency.Pattern != Patterns.SINGLTON)
               {
                    result = Create(type);
               }
               else 
               {    
                    if (singltonList.Keys.ToList().Exists(x => x.Key == dependency.pair.Key && x.Value == dependency.pair.Value))
                    {
                         singltonList.TryGetValue(dependency.pair, out result);
                    }
                    else
                    {
                         result = Create(type);
                         singltonList.Add(dependency.pair, result);
                    }
               }

               return result;
          }

          private object Create(Type type)
          {
               object result = null;

               List<Type> notAllowedTypes = new List<Type>();
               notAllowedTypes.Add(type);
               foreach (ConstructorInfo constructorInfo in type.GetConstructors())
               {
                    if ((result = InvokeConstructor(constructorInfo, notAllowedTypes)) != null)
                         break;
               }

               return result;
          }

          private object InvokeConstructor(ConstructorInfo constructor, List<Type> notAllowedTypes)
          {
               object result = null;

               ParameterInfo[] parametersInfo = constructor.GetParameters();
               object[] validParameters = new object[parametersInfo.Length];

               int item = 0;

               foreach (ParameterInfo parameterInfo in parametersInfo)
               {
                    List<Type> list;
                    Type type = Validator.GetUpperHeritor(dependecies, parameterInfo.ParameterType);
                    Type[] genericArgs = null;

                    if (type == null && parameterInfo.ParameterType.IsGenericType)
                    {
                         genericArgs = parameterInfo.ParameterType.GenericTypeArguments;
                         type = Validator.GetUpperHeritor(this.dependecies, parameterInfo.ParameterType.GetGenericTypeDefinition());
                    }

                    if (type == null || notAllowedTypes.Contains(type))
                    {
                         return null;
                    }

                    if (type.IsGenericTypeDefinition)
                    {
                         try
                         {
                              type = type.MakeGenericType(genericArgs);
                         }
                         catch
                         {
                              throw new InvalidCastException();
                         }
                    }

                    foreach (ConstructorInfo constructorInfo in type.GetConstructors())
                    {
                         list = new List<Type>(notAllowedTypes);
                         list.Add(type);

                         if ((result = InvokeConstructor(constructorInfo, list)) != null)
                         {
                              break;
                         }

                    }

                    if (result == null)
                         return null;

                    validParameters[item++] = result;
               }

               return constructor.Invoke(validParameters); 
          }
     }
}
