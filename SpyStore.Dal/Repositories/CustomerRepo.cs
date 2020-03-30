using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SpyStore.Dal.EfStructures;
using SpyStore.Dal.Repositories.Base;
using SpyStore.Dal.Repositories.Interfaces;
using SpyStore.Models.Entities;

namespace SpyStore.Dal.Repositories
{
    public class CustomerRepo : RepoBase<Customer>, ICustomerRepo
    {
        public CustomerRepo(StoreContext context) : base(context)
        {
        }
        internal CustomerRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public override IEnumerable<Customer> GetAll() => base.GetAll(c => c.FullName);

    }
}
