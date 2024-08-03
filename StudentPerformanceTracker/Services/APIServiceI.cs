using SPTModels;

namespace SPTClient.Services
{
    public interface APIServiceI
    {
        Task<bool> AuthenticateAsync(string username, string password);
        Task RegisterAsync(string username, string password);
        Task<List<Module>> GetModulesAsync();
        Task<Module> GetModuleByIdAsync(int id);
        Task AddOrUpdateModuleAsync(Module module);
        Task DeleteModuleByIdAsync(int id);
        Task<List<StudySession>> GetStudySessionsAsync();
        Task<StudySession> GetStudySessionByIdAsync(int id);
        Task AddOrUpdateStudySessionAsync(StudySession studySession);
        Task DeleteStudySessionByIdAsync(int id);
    }
}
