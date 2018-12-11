using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependecyInjectionLibrary
{
    public class Dependency
    {
          public KeyValuePair<Type, Type> pair;
          public bool isSingleton;

          public Dependency(KeyValuePair<Type, Type> pair, bool isSinglton)
          {
               this.pair = pair;
               this.isSingleton = isSingleton;
          }
    }
}
