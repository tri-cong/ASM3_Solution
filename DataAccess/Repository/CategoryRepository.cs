using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        void ICategoryRepository.AddCategory(Category category) => CategoryDAO.Instance.Add(category);

        void ICategoryRepository.DeleteCategory(int id) => CategoryDAO.Instance.Delete(id);

        IEnumerable<Category> ICategoryRepository.GetAllCategoriesForList() => CategoryDAO.Instance.GetCategories();

        IEnumerable<Category> ICategoryRepository.GetCategories() => CategoryDAO.Instance.GetAll();

        int ICategoryRepository.GetMaxId() => CategoryDAO.Instance.GetMaxId();

        void ICategoryRepository.UpdateCategory(Category category) => CategoryDAO.Instance.Update(category);
    }
}
