using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();

        IEnumerable<Category> GetAllCategoriesForList();

        void AddCategory(Category category);    
        void UpdateCategory(Category category);
        void DeleteCategory(int id);

        int GetMaxId();
    }
}
