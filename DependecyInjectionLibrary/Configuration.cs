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



    }
}
