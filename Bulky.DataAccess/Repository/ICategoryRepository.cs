using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public interface ICategoryRepository : IRepository<CategoryModel>
    {
        void Update(CategoryModel objCategory);
        void Save();

    }
}
