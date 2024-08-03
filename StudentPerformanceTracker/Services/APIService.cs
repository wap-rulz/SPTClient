using SPTModels;
using System.Net;

namespace SPTClient.Services
{
    public class APIService : APIServiceI
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _apiEndpoints;
        private readonly string _authServiceUrl;
        private readonly string _managementServiceUrl;

        public APIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authServiceUrl = configuration.GetSection("APIEndpoints")["AuthServiceUrl"];
            _managementServiceUrl = configuration.GetSection("APIEndpoints")["ManagementServiceUrl"];
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var userDTO = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync(_authServiceUrl + "/login", userDTO);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task RegisterAsync(string username, string password)
        {
            var userDTO = new { Username = username, Password = password };
            await _httpClient.PostAsJsonAsync(_authServiceUrl + "/register", userDTO);
        }

        public async Task<List<Module>> GetModulesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Module>>(_managementServiceUrl + "/Module/");
            return response ?? new List<Module>();
        }

        public async Task<Module> GetModuleByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Module>(_managementServiceUrl + $"/Module/{id}");
        }

        public async Task AddOrUpdateModuleAsync(Module module)
        {
            await _httpClient.PostAsJsonAsync(_managementServiceUrl + "/Module/", module);
        }

        public async Task DeleteModuleByIdAsync(int id)
        {
            await _httpClient.DeleteAsync(_managementServiceUrl + $"/Module/{id}");
        }

        public async Task<List<StudySession>> GetStudySessionsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<StudySession>>(_managementServiceUrl + "/StudySession/");
            return response ?? new List<StudySession>();
        }

        public async Task<StudySession> GetStudySessionByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<StudySession>(_managementServiceUrl + $"/StudySession/{id}");
        }

        public async Task AddOrUpdateStudySessionAsync(StudySession studySession)
        {
            await _httpClient.PostAsJsonAsync(_managementServiceUrl + "/StudySession/", studySession);
        }

        public async Task DeleteStudySessionByIdAsync(int id)
        {
            await _httpClient.DeleteAsync(_managementServiceUrl + $"/StudySession/{id}");
        }
    }
}
