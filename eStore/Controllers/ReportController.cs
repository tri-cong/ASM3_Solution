using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BussinessObject.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;

namespace eStore.Controllers
{
    public class ReportController : Controller
    {
        IOrderRepository orderRepository;
        IOrderDetailRepository detailRepository;

        public ReportController()
        {
            orderRepository = new OrderRepository();
            detailRepository = new OrderDetailRepository();
        }
        public ActionResult View(DateTime? end, DateTime? start)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                try
                {
                    ViewData["end"] = end;
                    ViewData["start"] = start;

                    if (end.HasValue && start.HasValue)
                    {
                        var list = orderRepository.GetOrderInRange(start.Value, end.Value);
                        ViewBag.Role = role;
                        decimal total = 0;
                        foreach (var item in list)
                        {
                            total += detailRepository.GetIncomeOfOrder(item.OrderId);
                        }
                        ViewBag.Total = total;
                        ViewBag.Role = role;
                        return View(list);
                        
                    }
                    else
                    {
                        ViewBag.Role = role;
                        return View(null);
                    }


                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View();
                }
            }

        }

        public ActionResult Details(int? id, DateTime? end, DateTime? start)
        {
            string role;
            ViewData["end"] = end;
            ViewData["start"] = start;
            try
            {
                role = HttpContext.Session.GetString("Role");
                if (role != "admin")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return NotFound();
            }
            Order order = orderRepository.GetOrderById(id.Value);
            var list = detailRepository.GetOrderdetailByOrderId(order.OrderId);

            ViewBag.Total = detailRepository.GetIncomeOfOrder(order.OrderId);
            if (order == null)
            {
                return NotFound();

            }
            else
            {
                var myModel = new Tuple<Order, IEnumerable<OrderDetail>>(order, list);
                ViewBag.Role = role;
                return View(myModel);
            }
        }



    }
}
