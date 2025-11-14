using Microsoft.AspNetCore.Mvc;
using lap4.Models;
using th1.Models;

namespace lap4.ViewComponents
{
    public class RenderViewComponent : ViewComponent
    {
        private List<MenuItem> MenuItems { get; set; }

        public RenderViewComponent()
        {
            MenuItems = new List<MenuItem>()
            {
                new MenuItem(){ Id=1, Name="Dashboard", link="~/Home/Index"},
                new MenuItem(){ Id=2, Name="Student", link="~/Student/Index"},
                new MenuItem(){ Id=3, Name="Subjects", link="~/Subjects/Index"},
                new MenuItem(){ Id=4, Name="Courses", link="~/Courses/Index"}
            };
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderLeftMenu", MenuItems);
        }
    }
}
