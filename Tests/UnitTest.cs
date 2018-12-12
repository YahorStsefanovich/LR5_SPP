using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependecyInjectionLibrary;
using System.Collections.Generic;

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

     public interface I { }
     public class A : I { }
     public class B1 : A
     {
          public B1(C c)
          {

          }
     }
     public class B2 : A { }
     public class C { }
     public class D : C { }
     public class G : B2 { }

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
               config.RegistrateGeneric(typeof(IGeneric1<>), typeof(Generic1<>), Patterns.USUAL);
               config.RegistrateGeneric(typeof(IGeneric2<>), typeof(Generic2<>), Patterns.USUAL);
               generator = new DepecndencyGenerator(config);
               Assert.IsNotNull(generator.Resolve<IGeneric1<int>>());
          }

          [TestMethod]
          public void CheckAmountOfCretedObjects()
          {
               config.Registrate<A, B1>(Patterns.USUAL);
               config.Registrate<A, B2>(Patterns.USUAL);
               config.Registrate<C, D>(Patterns.USUAL);
               generator = new DepecndencyGenerator(config);
               Assert.AreEqual(2, ((List<A>)generator.ResolveAll<A>()).Count);
          }

          [TestMethod]
          public void CheckTypeOfCreatedInstance()
          {
               config.Registrate<I, A>(Patterns.USUAL);
               config.Registrate<A, B2>(Patterns.USUAL);
               config.Registrate<B2, G>(Patterns.USUAL);
               generator = new DepecndencyGenerator(config);
               Assert.IsTrue(generator.Resolve<I>() is G);
          }

          [TestMethod]
          public void CheckCaseParentEqualSon()
          {
               config.Registrate<A, A>(Patterns.USUAL);
               generator = new DepecndencyGenerator(config);
               Assert.IsNotNull(generator.Resolve<A>());
          }

          [TestMethod]
          public void CheckInvalidGenericCreation()
          {
               config.RegistrateGeneric(typeof(IGeneric1<>), typeof(Generic3<>), Patterns.USUAL);
               generator = new DepecndencyGenerator(config);
               Assert.IsNull(generator.Resolve<IGeneric2<string>>());
          }

          [TestMethod]
          public void CheckValidGenericCreation()
          {
               config.RegistrateGeneric(typeof(IGeneric2<>), typeof(Generic3<>), Patterns.USUAL);
               generator = new DepecndencyGenerator(config);
               Assert.IsNotNull(generator.Resolve<IGeneric2<int>>());
          }

          [TestMethod]
          public void CheckSinglton()
          {
               I value1, value2;
               config.Registrate<I, A>(Patterns.SINGLTON);
               config.Registrate<A, B2>(Patterns.USUAL);
               config.Registrate<B2, G>(Patterns.USUAL);

               generator = new DepecndencyGenerator(config);

               value1 = generator.Resolve<I>();
               value2 = generator.Resolve<I>();

               Assert.AreSame(value1, value2);
          }

          [TestMethod]
          public void CheckGenericSinglton()
          {
               IGeneric1<int> iGeneric1, iGeneric2;
               config.RegistrateGeneric(typeof(IGeneric1<>), typeof(Generic1<>), Patterns.SINGLTON);
               config.RegistrateGeneric(typeof(IGeneric2<>), typeof(Generic2<>), Patterns.SINGLTON);
               generator = new DepecndencyGenerator(config);

               iGeneric1 = generator.Resolve<IGeneric1<int>>();
               iGeneric2 = generator.Resolve<IGeneric1<int>>();

               Assert.AreSame(iGeneric1, iGeneric2);
          }
     }
}
