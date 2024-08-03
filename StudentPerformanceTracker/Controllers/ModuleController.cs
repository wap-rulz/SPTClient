using Microsoft.AspNetCore.Mvc;
using SPTClient.Services;
using SPTModels;

namespace StudentPerformanceTracker.Controllers
{
    public class ModuleController : Controller
    {
        private APIServiceI _apiServiceI;

        public ModuleController(APIServiceI apiServiceI)
        {
            _apiServiceI = apiServiceI;
        }

        // GET: ModuleController
        public async Task<ActionResult> Index()
        {
            var modules = await _apiServiceI.GetModulesAsync();
            return View(modules);
        }

        // GET: ModuleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ModuleController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Module module)
        {
            await _apiServiceI.AddOrUpdateModuleAsync(module);
            return RedirectToAction(nameof(Index));
        }

        // GET: ModuleController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var module = await _apiServiceI.GetModuleByIdAsync(id);
            return View(module);
        }

        // POST: ModuleController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Module module)
        {
            await _apiServiceI.AddOrUpdateModuleAsync(module);
            return RedirectToAction(nameof(Index));
        }

        // GET: ModuleController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await _apiServiceI.DeleteModuleByIdAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
