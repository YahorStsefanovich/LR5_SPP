using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependecyInjectionLibrary
{
     public class Validator
     {
          private IEnumerable<Dependency> dependecies;

          public bool Validate(Configuration config)
          {
               dependecies = config.Dependencies;
               foreach (Dependency dependency in dependecies)
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
     }
}
