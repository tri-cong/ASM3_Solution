using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetOrderdetailByOrderId(int id);
        void Add(OrderDetail orderDetail);
        void Update(OrderDetail orderDetail);
        void Delete(OrderDetail detail);

        OrderDetail GetProductInOrderDetail(int productId, int orderId);
        decimal GetIncomeOfOrder(int orderId);

    }
}
