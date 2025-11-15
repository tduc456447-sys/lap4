using Microsoft.AspNetCore.Mvc;
using lap4.Data;
using lap4.Models;
namespace lap4.ViewComponents
{
    public class MajorViewComponent : ViewComponent

    {
        SchoolContext db;
        List<Major> majors;
        public MajorViewComponent(SchoolContext _context)
        {
            db= _context;
            majors = db.Major.ToList();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
