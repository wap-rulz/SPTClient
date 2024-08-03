using Microsoft.AspNetCore.Mvc;
using SPTClient.Services;
using SPTModels;

namespace StudentPerformanceTracker.Controllers
{
    public class ReportController : Controller
    {
        private APIServiceI _apiServiceI;

        public ReportController(APIServiceI apiServiceI)
        {
            _apiServiceI = apiServiceI;
        }


        // GET: ReportController
        public async Task<ActionResult> Index()
        {
            List<StudySessionSummary> studySessionSummaries = await getStudySessionSummaries();
            foreach (var summary in studySessionSummaries)
            {
                summary.PredictedGrade = await predictGrade(summary.ModuleId, summary.KnowledgeLevel);
            }
            return View(studySessionSummaries);
        }

        public async Task<ActionResult> Graph()
        {
            var studySessionSummaries = await getStudySessionSummaries();
            var labels = studySessionSummaries.Select(s => s.ModuleName).ToArray();
            var data = studySessionSummaries.Select(s => s.TotalDuration).ToArray();
            ViewBag.ModuleNames = labels;
            ViewBag.TotalDuration = data;
            return View();
        }

        private async Task<List<StudySessionSummary>> getStudySessionSummaries()
        {
            var studySessions = await _apiServiceI.GetStudySessionsAsync();
            var modules = await _apiServiceI.GetModulesAsync();
            var studySessionSummaries = modules.Select(m => new StudySessionSummary
                {
                    ModuleId = m.Id,
                    ModuleName = m.Name
                }).ToList();
            foreach (var session in studySessions)
            {
                var summary = studySessionSummaries.SingleOrDefault(s => s.ModuleId == session.ModuleId);
                summary.TotalSessions++;
                summary.TotalDuration += session.Duration;
                double durationInDecimal = (session.Duration / 60) + ((double)(session.Duration % 60) / 60);
                summary.KnowledgeLevel += durationInDecimal * GetStudyLevel(session.StudyLevel);
            }
            return studySessionSummaries;
        }

        private async Task<string> predictGrade(int moduleId, double knowledgeLevel)
        {
            var module = await _apiServiceI.GetModuleByIdAsync(moduleId);
            knowledgeLevel = ((double)knowledgeLevel / module.Credits) * 100;
            if (knowledgeLevel >= 85)
            {
                return "A+";
            }
            else if (knowledgeLevel >= 75)
            {
                return "A";
            }
            else if (knowledgeLevel >= 70)
            {
                return "A-";
            }
            else if (knowledgeLevel >= 65)
            {
                return "B+";
            }
            else if (knowledgeLevel >= 60)
            {
                return "B";
            }
            else if (knowledgeLevel >= 55)
            {
                return "B-";
            }
            else if (knowledgeLevel >= 50)
            {
                return "C+";
            }
            else if (knowledgeLevel >= 45)
            {
                return "C";
            }
            else if (knowledgeLevel >= 40)
            {
                return "C-";
            }
            else if (knowledgeLevel >= 35)
            {
                return "D";
            }
            else
            {
                return "F";
            }
        }

        private double GetStudyLevel(StudyLevel studyLevel)
        {
            switch (studyLevel)
            {
                case StudyLevel.Beginner:
                    return 0.25;
                case StudyLevel.Intermediate:
                    return 0.5;
                case StudyLevel.Advanced:
                    return 0.75;
                case StudyLevel.Expert:
                    return 1.0;
                default:
                    return 0.0;
            }
        }
    }
}
