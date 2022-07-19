using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance;
        private FStoreDBContext context;

        private static readonly object InstanceLock = new object(); 
        private ProductDAO() {
            context = new FStoreDBContext();
        }
        public static ProductDAO Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    } 
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetAll()
        {
            try
            {
                return context.Products.Include(x => x.Category) ;
                
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Product GetByID(int id)
        {
            try
            {
                    return context.Products.SingleOrDefault(x => x.ProductId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(Product product)
        {
            try
            {
                    context.Products.Add(product);
                    context.SaveChanges();
                    return;
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
                    var product = context.Products.SingleOrDefault(x => x.ProductId == id); 
                    context.Products.Remove(product);
                    context.SaveChanges();
                    return;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(Product product)
        {
            try
            {
                var old = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                context.Entry(old).CurrentValues.SetValues(product);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int GetMaxID()
        {
            try
            {
                int id = (from product in context.Products
                          orderby product.ProductId descending
                          select product).FirstOrDefault().ProductId;
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Product> SearchProductByNameMinPriceMaxPrice(string? name, decimal? min, decimal? max)
        {
            try
            {
                    return context.Products.Where(p => (p.UnitPrice >= min || min == 0 || min == null)
                                                && (p.UnitPrice <= max || max == 0 || max == null)
                                                && (p.ProductName.Contains(name) || name == null)).Include(p => p.Category);
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
