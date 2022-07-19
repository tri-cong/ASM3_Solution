using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BussinessObject.Models;
using DataAccess.Repository;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Dynamic;
using System.Collections.Generic;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;

namespace eStore.Controllers
{
    public class OrderDetailsController : Controller
    {
        IOrderDetailRepository detailRepository;
        IOrderRepository orderRepository;
        IProductRepository productRepository;
        public OrderDetailsController()
        {
            detailRepository = new OrderDetailRepository();
            orderRepository = new OrderRepository();
            productRepository = new ProductRepository();
        }
        // GET: OrderDetailsController
        public ActionResult Index(int? orderId)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            if (orderId == null)
            {
                return NotFound();
            }
            var list = detailRepository.GetOrderdetailByOrderId(orderId.Value);
            return View(list);
        }

        // GET: OrderDetailsController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var list = detailRepository.GetOrderdetailByOrderId(id);
                return View(list);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        // GET: OrderDetailsController/Create
        public ActionResult Create(int? orderId, string? SearchValue, decimal? Max, decimal? Min)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                ViewData["Search"] = SearchValue;
                ViewData["min"] = Min;
                ViewData["max"] = Max;

                IEnumerable<Product> productList;
                if (SearchValue == null)
                {
                    productList = null;
                }
                else
                {
                    productList = productRepository.SearchProduct(SearchValue, Min, Max);
                }
                if (orderId == null)
                {
                    return NotFound();
                }
                Order order = orderRepository.GetOrderById(orderId.Value);
                if (order == null)
                {
                    return NotFound();
                }
                var list = productRepository.GetProducts();


                ViewBag.ProductId = new SelectList(list, nameof(Product.ProductId), nameof(Product.ProductName));

                OrderDetail detail = new OrderDetail();
                detail.OrderId = orderId.Value;
                IEnumerable<OrderDetail> orderDetails;
                //try
                //{
                //    var str = HttpContext.Session.GetString("LIST");
                //    orderDetails  = JsonConvert.DeserializeObject<List<OrderDetail>>(str);
                //}
                //catch
                //{

                //}
                orderDetails = detailRepository.GetOrderdetailByOrderId(detail.OrderId);

                var mymodel = new Tuple<OrderDetail, IEnumerable<Product>, IEnumerable<OrderDetail>>(detail, productList, orderDetails);
                return View(mymodel);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
            return View();
        }

        // POST: OrderDetailsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderDetail detail)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                Product product = productRepository.GetProductById(detail.ProductId);
                detail.UnitPrice = product.UnitPrice;
                detailRepository.Add(detail);
                //var str = HttpContext.Session.GetString("LIST");
                //List<OrderDetail> orderDetails = null;
                //try
                //{
                //    orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(str);
                //}
                //catch
                //{
                //    orderDetails =new List<OrderDetail>();  
                //}
                //Product product = productRepository.GetProductById(detail.ProductId);
                //detail.UnitPrice = product.UnitPrice;
                //detail.Product = product;
                //orderDetails.Add(detail);

                //var newString = JsonConvert.SerializeObject(orderDetails);
                //HttpContext.Session.SetString("LIST", newString);
                return RedirectToAction(nameof(Create), new { orderId = detail.OrderId }); ;
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderDetailsController/Edit/5
        public ActionResult Edit(int? productId, int? orderId)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            if (productId == null || orderId == null)
            {
                return NotFound();
            }
            OrderDetail detail = detailRepository.GetProductInOrderDetail(productId.Value, orderId.Value);
            //return RedirectToAction("Edit", new { id = productId.Value, orderId = orderId.Value});
            return View(detail);
        }

        // POST: OrderDetailsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int productId, int orderId, OrderDetail detail)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                if (orderId != detail.OrderId && productId != detail.ProductId)
                {
                    return NotFound();
                }
                detailRepository.Update(detail);
                return RedirectToAction("Create", new { orderId = detail.OrderId });
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderDetailsController/Delete/5
        public ActionResult Delete(int? productId, int? orderId)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            if (productId == null || orderId == null)
            {
                return NotFound();
            }
            OrderDetail detail = detailRepository.GetProductInOrderDetail(productId.Value, orderId.Value);
            //return RedirectToAction("Edit", new { id = productId.Value, orderId = orderId.Value});
            return View(detail);
        }

        // POST: OrderDetailsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int productId, int orderId, OrderDetail detail)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                if (productId != detail.ProductId && orderId != detail.OrderId)
                {
                    return NotFound();
                }
                detailRepository.Delete(detail);
                return RedirectToAction("Create", new { orderId = detail.OrderId });
            }
            catch
            {
                return View(detail);
            }
        }

        [HttpPost]
        public ActionResult AddOrder(OrderDetail detail)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            var str = HttpContext.Session.GetString("LIST");
            List<OrderDetail> orderDetails = JsonConvert.DeserializeObject<List<OrderDetail>>(str);
            orderDetails.Add(detail);

            var newString = JsonConvert.SerializeObject(orderDetails);
            HttpContext.Session.SetString("LIST", newString);

            return RedirectToAction(nameof(Create));
        }


    }
}
