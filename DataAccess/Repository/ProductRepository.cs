using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> SearchProduct(string? name, decimal? min, decimal? max)
            => ProductDAO.Instance.SearchProductByNameMinPriceMaxPrice(name, min, max);

        void IProductRepository.Add(Product product) => ProductDAO.Instance.Add(product);

        void IProductRepository.Delete(int id) => ProductDAO.Instance.Delete(id);

        int IProductRepository.GetMaxId() => ProductDAO.Instance.GetMaxID();

        Product IProductRepository.GetProductById(int id) => ProductDAO.Instance.GetByID(id);

        IEnumerable<Product> IProductRepository.GetProducts() => ProductDAO.Instance.GetAll();

        void IProductRepository.Update(Product product) => ProductDAO.Instance.Update(product);
    }
}
