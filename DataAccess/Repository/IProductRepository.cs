using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);

        int GetMaxId();
        IEnumerable<Product> SearchProduct(string? name, decimal? min, decimal? max);

    }
}
