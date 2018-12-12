using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependecyInjectionLibrary;

namespace Tests
{

     public interface IGeneric1<T> { }
     public interface IGeneric2<T> { }
     public class Generic1<T> : IGeneric1<T>
     {
          public Generic1(IGeneric2<T> generic2)
          {

          }
     }
     public class Generic2<T> : IGeneric2<T> { }
     public class Generic3<T> : IGeneric2<int> { }


     [TestClass]
     public class UnitTest1
     {

          private Configuration config;
          private DepecndencyGenerator generator;

          [TestInitialize]
          public void SetUp()
          {
               config = new Configuration();
          }

          [TestMethod]
          public void CheckValidation()
          {
               config.RegistrateGeneric(typeof(IGeneric1<>), typeof(Generic1<>), false);
               config.RegistrateGeneric(typeof(IGeneric2<>), typeof(Generic2<>), false);
               generator = new DepecndencyGenerator(config);
               Assert.IsNotNull(generator.Resolve<IGeneric1<int>>());
          }
     }
}
