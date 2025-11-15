using lap4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lap4.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lap4.Controllers
{
    public class LearnerController : Controller
    {
        private SchoolContext db;
        private int pageSize = 3;
        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        public IActionResult Index(int? mid)
        {
            var learners = (IQueryable<Learner>)db.Learner.Include(m => m.Major);
            if(mid != null)
            {
                learners = (IQueryable<Learner>)db.Learner.Where(l => l.MajorID == mid).Include(m => m.Major);
            }
            int pageNum = (int)Math.Ceiling(learners.Count() / (float)pageSize);
            ViewBag.PageNum = pageNum;
            var result = learners.Take(pageSize).ToList();
            return View(result);
        }
        public IActionResult ResultLearnerFilter(int? mid, string keyword, int? pageIndex)
        {
            IQueryable<Learner> learners = (IQueryable<Learner>)db.Learner;
            int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);
            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
                ViewBag.mid = mid;
            }
            if (keyword != null)
            {
                learners = learners.Where(l => l.FirstMidName.ToLower()
                    .Contains(keyword.ToLower()));
                ViewBag.keyword = keyword;
            }
            int pageNum = (int)Math.Ceiling((float)learners.Count() / (float)pageSize);
            ViewBag.pageNum = pageNum;
            var result = learners.Skip(pageSize * (page - 1))
                .Take(pageSize).Include(m => m.Major).ToList();

            return PartialView("LearnerTable", result);
        }
        public IActionResult LearnerByMajorID(int mid)
        {
            var learners = db.Learner.SkipWhile(l=>l.MajorID==mid).Include(m=> m.Major).ToList();
            return PartialView("LearnerTable",learners);
        }

        public IActionResult Create()
        {
            ViewBag.MajorID = new SelectList(db.Major.ToList(), "MajorID", "MajorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learner.Add(learner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MajorID = new SelectList(db.Major.ToList(), "MajorID", "MajorName", learner.MajorID);

            return View(learner);
        }
        public IActionResult Edit(int id)
        {
            if (id == null || db.Learner == null)
            {
                return NotFound();
            }

            var learner = db.Learner.Find(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorID = new SelectList(db.Major, "MajorID", "MajorName", learner.MajorID);

            return View(learner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner); 
                    db.SaveChanges();  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Major, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }
        private bool LearnerExists(int id)
        {
            return (db.Learner?.Any(e => e.LearnerID == id)).GetValueOrDefault();
        }
        public IActionResult Delete(int id)
        {
            if (id == null || db.Learner == null)
            {
                return NotFound();
            }
            var learner = db.Learner
                .Include(l => l.Major)
                .Include(e => e.Enrollments)
                .FirstOrDefault(m => m.LearnerID == id);

            if (learner == null)
            {
                return NotFound();
            }
            if (learner.Enrollments.Count > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }

            return View(learner);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (db.Learner == null)
            {
                return Problem("Entity set 'Learners' is null.");
            }

            var learner = db.Learner.Find(id);

            if (learner != null)
            {
                db.Learner.Remove(learner);
            }
            db.SaveChanges(); 
            return RedirectToAction(nameof(Index));
        }
    }
}
