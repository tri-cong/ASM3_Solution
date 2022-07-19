using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OrderDAO
    {
        FStoreDBContext context;
        private static OrderDAO instance;
        private static readonly object InstanceLock = new object();
        private OrderDAO()
        {
            context = new FStoreDBContext();
        }
        public static OrderDAO Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Order> GetOrders()
        {
            try
            {
                return context.Orders.Include(x => x.Member);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(Order order)
        {
            try
            {
                context.Add(order);
                context.SaveChanges();
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
                var o = context.Orders.SingleOrDefault(x => x.OrderId == id);
                if (o != null)
                {
                    context.Orders.Remove(o);
                }
                else
                {
                    throw new Exception("Not found this id");
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(Order order)
        {
            try
            {
                var o = context.Orders.SingleOrDefault(x => x.OrderId == order.OrderId);
                if (o != null)
                {
                    context.Entry(o).CurrentValues.SetValues(order);
                    context.SaveChanges(true);
                }
                else
                {
                    throw new Exception("Not found");
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
                int id = 0;
                id = (from order in context.Orders
                      orderby order.OrderId descending
                      select order.OrderId).FirstOrDefault();
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public IEnumerable<Order> GetOrderByMemberId(int id)
        {
            try
            {
                var o = context.Orders.Where(x => x.MemberId == id).Include(x => x.Member).Include(x => x.OrderDetails);
                return o;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
                var o = context.Orders.Where(x => x.OrderId == id).Include(x => x.Member).Include(x => x.OrderDetails).FirstOrDefault();
                return o;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public IEnumerable<Order> GetOrderInRange(DateTime start, DateTime end)
        {

            try
            {
                var list = context.Orders.Where(order => order.OrderDate >= start && order.OrderDate <= end).Include(x => x.Member);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
