using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependecyInjectionLibrary
{

     public class Configuration
     {
          public List<Dependency> Dependencies { get; }

          public Configuration()
          {
               Dependencies = new List<Dependency>();
          }

          public void Registrate<T1, T2>(Patterns pattern)
               where T1 : class
               where T2 : class
          {
               Dependency dependency = new Dependency(new KeyValuePair<Type, Type>(typeof(T1), typeof(T2)), pattern);
               if (!Dependencies.Exists(x => x.pair.Key == dependency.pair.Key
               && x.pair.Value == dependency.pair.Value))
                    Dependencies.Add(dependency);
          }


          public void RegistrateGeneric(Type t1, Type t2, Patterns pattern)
          {
               if ((t1.IsClass || t1.IsInterface) && (t2.IsClass || t2.IsInterface))
               {
                    Dependency dependecy = new Dependency(new KeyValuePair<Type, Type>(t1, t2), pattern);
                    if (!Dependencies.Exists(x => x.pair.Key == dependecy.pair.Key 
                    && x.pair.Value == dependecy.pair.Value))
                         Dependencies.Add(dependecy);
               }
          }

    }
}
