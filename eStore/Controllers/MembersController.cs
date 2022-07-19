using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using BussinessObject.Models;
using DataAccess.Repository;
using System.Net;

namespace eStore.Controllers
{
    public class MembersController : Controller
    {
        IMemberRepository memberRepository;

        public MembersController()
        {
            memberRepository = new MemberRepository();
        }
        // GET: MembersController
        public ActionResult Index()
        {
            string role = HttpContext.Session.GetString("Role");
            if( role != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                ViewBag.Role = role;
                var list = memberRepository.GetAll();
                return View(list);
            } catch(Exception ex)
            {
                return View(ex);
            }
        }

        // GET: MembersController/Details/5
        public ActionResult Details(int? id)
        {
            string email = HttpContext.Session.GetString("Email");
            Member member;
            if (id != null)
            {
                 member = memberRepository.Get(id.Value);
            } else
            {
                member = memberRepository.GetMemberByEmail(email);
            }
            if(member == null)
            {
                return NotFound();
            }
            string role = HttpContext.Session.GetString("Role");
            if (role != "admin")
            {
                ViewBag.Role = role;
                return View(member);
            }
            else if (role == "admin")
            {
                ViewBag.Role = role;
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
            ;
        }

        // GET: MembersController/Create
        public ActionResult Create()
        {
            
            Member member = new Member();
            member.MemberId = memberRepository.GetMaxId() + 1;
            return View(member);
        }

        // POST: MembersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    memberRepository.Add(member);
                }
                string role = HttpContext.Session.GetString("Role");
                if (role != "admin")
                {
                    return RedirectToAction("Details", "Members", new { id = member.MemberId });
                }
                else if (role == "admin")
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index", "Home");
               
            }
            catch
            {
                return View();
            }
        }

        // GET: MembersController/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            Member member = memberRepository.Get(id.Value);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: MembersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Member member)
        {
            try
            {
                if(id != member.MemberId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    memberRepository.Update(member);
                }

                string role = HttpContext.Session.GetString("Role");
                if (role != "admin")
                {
                    return RedirectToAction("Details", "Members", new {id = member.MemberId});
                } else if(role == "admin")
                {
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index", "Home");

            }
            catch(Exception ex)
            {
                ViewBag.message = ex.Message;
                return View(member);
            }
        }

        // GET: MembersController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Member member = memberRepository.Get(id.Value);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: MembersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {

            try
            {
                memberRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
