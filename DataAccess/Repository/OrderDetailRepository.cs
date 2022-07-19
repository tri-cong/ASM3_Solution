using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public decimal GetIncomeOfOrder(int orderId) => OrderDetailDAO.Instance.InComeOfAOrder(orderId);

        void IOrderDetailRepository.Add(OrderDetail orderDetail) => OrderDetailDAO.Instance.Add(orderDetail);

        void IOrderDetailRepository.Delete(OrderDetail detail) => OrderDetailDAO.Instance.Delete(detail);

        IEnumerable<OrderDetail> IOrderDetailRepository.GetOrderdetailByOrderId(int id)
                    => OrderDetailDAO.Instance.GetOrderdetailByOrderId(id);

        OrderDetail IOrderDetailRepository.GetProductInOrderDetail(int productId, int orderId)
            => OrderDetailDAO.Instance.GetOrderDetailByProductId(productId, orderId);

        void IOrderDetailRepository.Update(OrderDetail orderDetail) => OrderDetailDAO.Instance.Update(orderDetail);

        
    }
}
