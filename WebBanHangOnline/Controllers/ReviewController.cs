﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult _Review(int productId)
        {
            ViewBag.ProductId = productId;
            var item = new ReviewProduct();
            if (User.Identity.IsAuthenticated)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var userManager = new UserManager<ApplicationUser>(userStore);
                var user = userManager.FindByName(User.Identity.Name);
                if(user != null)
                {
                    item.Email = user.Email;
                    item.FullName = user.FullName;
                    item.UserName = user.UserName;
                }
                return PartialView(item);
            }
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult _Load_Review(int productid)
        {
            var item = _db.ReviewProducts.Where(x => x.ProductId == productid).OrderByDescending(x => x.Id).ToList();
            ViewBag.Connt = item.Count;
            return PartialView(item);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostReview(ReviewProduct req)
        {
            if (ModelState.IsValid)
            {
                req.CreatedDate = DateTime.Now;
                _db.ReviewProducts.Add(req);
                _db.SaveChanges();
                return Json(new { Success = true });
            }
            return Json(new {Success = false });
        }
    }
}