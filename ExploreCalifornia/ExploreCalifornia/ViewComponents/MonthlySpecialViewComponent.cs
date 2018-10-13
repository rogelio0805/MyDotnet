using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.ViewComponents
{
    [ViewComponent]
    public class MonthlySpecialViewComponent: ViewComponent
    {
        private readonly SpecialDataContext specialDataContext;

        public MonthlySpecialViewComponent(SpecialDataContext specialDataContext)
        {
            this.specialDataContext = specialDataContext;
        }

        public IViewComponentResult Invoke()
        {
            return View(specialDataContext.GetMonthlySpecials());
        }

    }
}
