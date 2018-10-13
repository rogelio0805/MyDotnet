using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExploreCalifornia.Contollers
{
    [Route("blog")]
    public class BlogController : Controller
    {

        private readonly BlogDataContext db;

        public BlogController(BlogDataContext db)
        {
            this.db = db;
        }
        [Route("")]
        public IActionResult Index()
        {
            var posts = db.Posts.OrderByDescending(x => x.Posted).Take(5).ToArray();

            return View(posts);
        }

        [Route("{year:int:min(2016)}/{month:int:range(1,12)}/{key}")]
        public IActionResult Post(int year, int month, string key)
        {
            #region ViewBag
            //ViewBag Object pass data to a view

            //ViewBag.Title = "My blog post";
            //ViewBag.Posted = DateTime.Now.ToShortDateString();
            //ViewBag.Author = "Rogelio Fernandez Pupo";
            //ViewBag.Body = "This is a great blog post, dont you think?";
            #endregion

            var post = db.Posts.FirstOrDefault(x => x.Key == key);
            return View(post);            
        }

        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("create")]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            post.Author = User.Identity.Name;
            post.Posted = DateTime.Now;

            db.Posts.Add(post);
            db.SaveChanges();

            return RedirectToAction("Post", "Blog", new
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });
        }
    }
}