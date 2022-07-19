using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetOrderByMemberId(int id);
        Order GetOrderById(int id);
        int GetMaxId();
        void Add(Order order);
        void Update(Order order);
        void Delete(int orderId);

        IEnumerable<Order> GetOrderInRange(DateTime start, DateTime end);
    }
}
