using System;
using System.Collections.Generic;
using System.Text;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Initialization;

namespace SpyStore.Service.Tests.TestClasses.Base
{
    public abstract class BaseTestClass : IDisposable
    {
        protected string ServiceAddress_Docker = "http://localhost:55882/";
        protected string ServiceAddress = "http://localhost:23741/";
        protected string RootAddress = String.Empty;

        protected void ResetTheDatabase()
        {
            SampleDataInitializer.InitializeData(new StoreContextFactory().CreateDbContext(null));
        }

        public virtual void Dispose()
        {
            
        }
    }
}
