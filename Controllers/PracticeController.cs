using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DataModel;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PracticeController : Controller
    {
        PracticeEntities content=new PracticeEntities();

        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult DashBoard()
        {
            ViewBag.CompanyCount=content.tblCompanies.Count();
            ViewBag.BranchCount = content.tblBranches.Count();
            ViewBag.DepartmentCount = content.tblDepartments.Count();
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tblCompany c)
        {
            var company = content.tblCompanies.Where(x => x.Password.Equals(c.Password) && x.UserName.Equals(c.UserName)).ToList();
            if (company != null) 
            {
                Session["UserName"]=c.UserName;
                return RedirectToAction("DashBoard");
            }
            else
            {
                return RedirectToAction("Login");

            }
        }

        [HttpGet]
        public ActionResult RegisterCompany()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterCompany(tblCompany c)
        {
            if (ModelState.IsValid)
            {
                content.tblCompanies.Add(c);
                content.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("RegisterCompany");

            }
        }

        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult RegisterBranch()
        {
            var companies = content.tblCompanies.ToList();
            ViewBag.Companies = new SelectList(companies, "CompanyId", "CompanyName");
            return View();
        }

        [CustomAuthenticationFilter]
        [HttpPost]
        public ActionResult RegisterBranch(tblBranch b)
        {
            if (ModelState.IsValid)
            {
                content.tblBranches.Add(b);
                content.SaveChanges();
                return RedirectToAction("DashBoard");
            }
            else
            {
                return RedirectToAction("RegisterBranch");

            }
        }

        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult RegisterDepartment()
        {
            var companies = content.tblCompanies.ToList();
            ViewBag.Companies = new SelectList(companies, "CompanyId", "CompanyName");
            return View();
        }

        [CustomAuthenticationFilter]
        [HttpPost]
        public ActionResult RegisterDepartment(tblDepartment d)
        {
            if (ModelState.IsValid)
            {
                content.tblDepartments.Add(d);
                content.SaveChanges();
                return RedirectToAction("DashBoard");
            }
            else
            {
                return RedirectToAction("RegisterDepartment");

            }
        }

        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult FetchBranches(int CompanyId)
        {
            var branches = content.tblBranches.Where(x =>x.CompanyId==CompanyId).ToList();
            List<SelectListItem> branchlist = new List<SelectListItem>();
            foreach(var item in branches)
            {
                branchlist.Add(new SelectListItem { Value=item.BranchId.ToString(),Text=item.BranchName });
            }
            return Json(branchlist,JsonRequestBehavior.AllowGet);
        }

        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult Branches()
        {
            var branches = content.tblBranches.Include("tblCompany").ToList();
            return View(branches);
        }

        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult Companies()
        {
            var companies = content.tblCompanies.ToList();
            return View(companies);
        }

        [CustomAuthenticationFilter]
        [HttpGet]
        public ActionResult Departments()
        {
            var departments = content.tblDepartments.Include("tblCompany").Include("tblBranch").ToList();
            return View(departments);
        }
    }
}