using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BussinessObject.Models;
using DataAccess.Repository;
using System;
using System.Dynamic;
using System.Collections.Generic;

namespace eStore.Controllers
{
    public class OrdersController : Controller
    {
        IOrderRepository orderRepository;
        IMemberRepository memberRepository;
        IOrderDetailRepository detailRepository;
        public OrdersController()
        {
            orderRepository = new OrderRepository();
            memberRepository = new MemberRepository();
            detailRepository = new OrderDetailRepository(); 
        }
        // GET: OrdersController
       
        public ActionResult Index()
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                string email = HttpContext.Session.GetString("Email");
                Member member = memberRepository.GetMemberByEmail(email);
                var list = orderRepository.GetOrderByMemberId(member.MemberId);
                ViewBag.Role = role;
                return View(list);
            } else
            {
                try
                {
                    var list = orderRepository.GetAll();
                    ViewBag.Role = role;
                    return View(list);

                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View();
                }
            }
            
        }

        // GET: OrdersController/Details/5
        public ActionResult Details(int? id)
        {
            string role;
            try
            {
                 role = HttpContext.Session.GetString("Role");
                if(role != "admin" && role != "user" )
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
            if(order == null)
            {
                return NotFound();

            }else
            {
                var myModel = new Tuple<Order, IEnumerable<OrderDetail>>(order, list);
                ViewBag.Role = role;
                return View(myModel);
            }
        }

        // GET: OrdersController/Create
        public ActionResult Create(int? id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            Member member = memberRepository.Get(id.Value);
            if(member == null)
            {
                ViewBag.ErrorMessage = "Not found this member.";
                return View();
            }
            Order order = new Order();
            order.OrderId = orderRepository.GetMaxId() + 1;
            order.Member = member;
            order.MemberId = id.Value;
            order.OrderDate = DateTime.Now;
            return View(order); 
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                orderRepository.Add(order);
                
                return RedirectToAction( nameof(OrderDetailsController.Create), "OrderDetails",  new { orderId = order.OrderId });
            } catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(order);
            }
        }

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int? id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            } 
            Order order = orderRepository.GetOrderById(id.Value);
            if( order == null)
            {
                return NotFound();
            } else
            {
                return View(order);
            } 
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order order)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                orderRepository.Update(order);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(order);
                return View();
            }
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int? id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }
            Order order = orderRepository.GetOrderById(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return View(order);
            }
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                orderRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
