using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO instance;
        private static readonly object InstanceLock = new object();
        FStoreDBContext context;
        private OrderDetailDAO()
        {
            context = new FStoreDBContext();
        }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (InstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<OrderDetail> GetOrderdetailByOrderId(int id)
        {
            try
            {
                return context.OrderDetails.Where(x => x.OrderId == id).Include(x => x.Product).Include(x => x.Order);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public OrderDetail GetOrderDetailByProductId(int id, int orderId)
        {
            try
            {
                return context.OrderDetails.Where(x => x.ProductId == id && x.OrderId == orderId).Include(x => x.Product).Include(x => x.Order).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Add(OrderDetail detail)
        {
            try
            {
                var o = context.OrderDetails.SingleOrDefault(x => x.OrderId == detail.OrderId
                                                            && x.ProductId == detail.ProductId);
                if (o == null)
                {
                    Product product = context.Products.SingleOrDefault(x => x.ProductId == detail.ProductId);
                    product.UnitsInStock -= detail.Quantity;
                    if(product.UnitsInStock < 0)
                    {
                        throw new Exception("Don't enough quantity.");
                    }
                    ProductDAO.Instance.Update(product);
                    context.Add(detail);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Product is already exist.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(OrderDetail detail)
        {
            try
            {
                var o = context.OrderDetails.SingleOrDefault(x => x.OrderId == detail.OrderId
                                                            && x.ProductId == detail.ProductId);
                if (o != null)
                {
                    Product product = context.Products.SingleOrDefault(x => x.ProductId == detail.ProductId);
                    product.UnitsInStock += detail.Quantity;
                    ProductDAO.Instance.Update(product);
                    context.OrderDetails.Remove(o);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Product is not found.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(OrderDetail detail)
        {
            try
            {
                var o = context.OrderDetails.SingleOrDefault(x => x.OrderId == detail.OrderId
                                                            && x.ProductId == detail.ProductId);
                if (o != null)
                {
                    if(o.Quantity != detail.Quantity)
                    {
                        //Lấy product trong kho 
                        Product product = context.Products.SingleOrDefault(x => x.ProductId == detail.ProductId);
                        int x = o.Quantity - detail.Quantity;
                        product.UnitsInStock += x;
                        if(product.UnitsInStock < 0)
                        {
                            throw new Exception("Don't enough quantity.");
                        }
                        ProductDAO.Instance.Update(product);
                    }
                    context.Entry(o).CurrentValues.SetValues(detail);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Product is not found.");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal InComeOfAOrder(int orderId)
        {
            decimal total = 0;
            try
            {
                using (FStoreDBContext context = new FStoreDBContext())
                {
                    var list = context.OrderDetails.Where(x => x.OrderId == orderId).ToList();
                    foreach (var item in list)
                    {
                        total += item.UnitPrice * item.Quantity * (decimal)(100 - item.Discount) / 100;
                    }
                    return total;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
