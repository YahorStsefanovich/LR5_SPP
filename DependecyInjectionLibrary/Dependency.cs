using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependecyInjectionLibrary
{
     public enum Patterns
     {
          USUAL = 0,
          SINGLTON
     };

     public class Dependency
    {
          public KeyValuePair<Type, Type> pair;
          private Patterns pattern;

          public Patterns Pattern
          {
               get { return pattern; }
               set { pattern = value; }
          }

          public Dependency(KeyValuePair<Type, Type> pair, Patterns pattern)
          {
               this.pair = pair;
               Pattern = pattern;
          }
    }
}
