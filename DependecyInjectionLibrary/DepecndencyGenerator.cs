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

               if (!typeof(T).IsGenericTypeDefinition)
               {
                    foreach  (Dependency dependency in dependecies)
                    {
                         if (dependency.pair.Key == typeof(T))
                         {
                              return null;
                         }
                    }
               }
               else
               {
                    return null;
               }
          }

          private object Generate(Type type, Dependency dependency)
          {
               object result;
               if (!dependency.isSingleton)
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

          private object InvokeConstructor(ConstructorInfo constructorInfo, List<Type> notAllowedTypes)
          {
               throw new NotImplementedException();
          }
     }
}
