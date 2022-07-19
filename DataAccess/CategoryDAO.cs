using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CategoryDAO
    {
        FStoreDBContext context;
        private static CategoryDAO instance;
        private static readonly Object InstanceLock = new Object();
        private CategoryDAO()
        {
            context = new FStoreDBContext();
        }
        public static CategoryDAO Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                using (var context = new FStoreDBContext())
                {
                    return context.Categories;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public  IEnumerable<Category> GetCategories()
        {
            try
            {
                var list = from c in context.Categories
                           select c;
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(Category category)
        {
            try
            {
                var c = context.Categories.SingleOrDefault(x => x.CategoryName.Equals(category.CategoryName));
                if(c == null)
                {
                    context.Add(category);
                    context.SaveChanges();
                    return;
                } else
                {
                    throw new Exception("The name is already exist.");
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(Category category)
        {
            try
            {
                var c = context.Categories.SingleOrDefault(x => x.CategoryName.Equals(category.CategoryName) 
                                                            && x.CategoryId != category.CategoryId);
                if (c == null)
                {
                    context.Entry(c).CurrentValues.SetValues(category);
                    context.SaveChanges(true);
                    return;
                }
                else
                {
                    throw new Exception("The name is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var c = context.Categories.SingleOrDefault(x => x.CategoryId == id);
                if (c != null)
                {
                    context.Categories.Remove(c);
                    context.SaveChanges();
                    return;
                }
                else
                {
                    throw new Exception("Not found this category.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int GetMaxId()
        {
            try
            {
                int id = (from category in context.Categories
                          orderby category.CategoryId descending
                          select category.CategoryId).FirstOrDefault();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
