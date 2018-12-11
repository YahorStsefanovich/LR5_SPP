using System;
using System.Collections.Generic;
using System.Linq;
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
     }
}
