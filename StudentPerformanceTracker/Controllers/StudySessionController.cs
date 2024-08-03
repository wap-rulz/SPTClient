using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SPTClient.Services;
using SPTModels;

namespace StudentPerformanceTracker.Controllers
{
    public class StudySessionController : Controller
    {
        private APIServiceI _apiServiceI;

        public StudySessionController(APIServiceI apiServiceI)
        {
            _apiServiceI = apiServiceI;
        }

        // GET: StudySessionController
        public async Task<ActionResult> Index()
        {
            var modules = await _apiServiceI.GetModulesAsync();
            var studySessions = await _apiServiceI.GetStudySessionsAsync();
            var sessions = studySessions.Select(s => new
            {
                s.Id,
                s.Date,
                s.Duration,
                s.StudyLevel,
                ModuleName = modules.SingleOrDefault(m => m.Id == s.ModuleId)?.Name
            })
                .OrderBy(s => s.Date)
                .ToList();
            return View(sessions);
        }

        // GET: StudySessionController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Modules = await _apiServiceI.GetModulesAsync();
            PopulateStudyLevelSelector(null);
            PopulateDurationSelectors(0);
            return View();
        }

        // POST: StudySessionController/Create
        [HttpPost]
        public async Task<ActionResult> Create(StudySession session)
        {
            await _apiServiceI.AddOrUpdateStudySessionAsync(session);
            return RedirectToAction(nameof(Index));
        }

        // GET: StudySessionController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var session = await _apiServiceI.GetStudySessionByIdAsync(id);
            ViewBag.Modules = await _apiServiceI.GetModulesAsync();
            PopulateStudyLevelSelector(session.StudyLevel);
            PopulateDurationSelectors(session.Duration);
            return View(session);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StudySession session)
        {
            await _apiServiceI.AddOrUpdateStudySessionAsync(session);
            //ViewBag.Modules = await _apiServiceI.GetModulesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: StudySessionController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await _apiServiceI.DeleteStudySessionByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDurationSelectors(int duration)
        {
            var hours = Enumerable.Range(0, 13).Select(i => new { Key = i, Value = $"{i} h" });
            var minutes = Enumerable.Range(0, 60).Where(i => i % 5 == 0).Select(i => new { Key = i, Value = $"{i} min" });

            ViewBag.Hours = new SelectList(hours, "Key", "Value", duration / 60);
            ViewBag.Minutes = new SelectList(minutes, "Key", "Value", duration % 60);
        }

        private void PopulateStudyLevelSelector(StudyLevel? studyLevel)
        {
            SelectList StudyLevels;
            if (studyLevel == null)
            {
                StudyLevels = new SelectList(Enum.GetValues(typeof(StudyLevel)));
            }
            else
            {
                StudyLevels = new SelectList(Enum.GetValues(typeof(StudyLevel)), studyLevel);
            }
            ViewBag.StudyLevels = StudyLevels;
        }
    }
}
