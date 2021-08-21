using Sharing_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharing_API.Interface
{
    public interface ICategoryRepository
    {
        void Update(Category category);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
    }
}
