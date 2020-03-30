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
    public class CategoryRepo : RepoBase<Category>, ICategoryRepo
    {
        public CategoryRepo(StoreContext context) : base(context)
        {
        }
        internal CategoryRepo(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public override IEnumerable<Category> GetAll() => base.GetAll(c => c.CategoryName);
        

    }
}
