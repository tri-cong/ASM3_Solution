using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        void IOrderRepository.Add(Order order) => OrderDAO.Instance.Add(order);

        void IOrderRepository.Delete(int orderId) => OrderDAO.Instance.Delete(orderId);

        IEnumerable<Order> IOrderRepository.GetAll() => OrderDAO.Instance.GetOrders();

        int IOrderRepository.GetMaxId() => OrderDAO.Instance.GetMaxId();

        IEnumerable<Order> IOrderRepository.GetOrderByMemberId(int id) => OrderDAO.Instance.GetOrderByMemberId(id);
        Order IOrderRepository.GetOrderById(int id) => OrderDAO.Instance.GetOrderById(id);

        void IOrderRepository.Update(Order order) => OrderDAO.Instance.Update(order);

        public IEnumerable<Order> GetOrderInRange(DateTime start, DateTime end) => OrderDAO.Instance.GetOrderInRange(start, end);
    }
}
