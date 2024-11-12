using web.reunion.Models;

namespace web.reunion.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetRolesAsync();
    }
    public class RoleService(HttpClient httpClient) : IRoleService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IEnumerable<string>> GetRolesAsync()
        {
            var response = await _httpClient.GetAsync("AdminGroups");
            response.EnsureSuccessStatusCode();
            var roles = await response.Content.ReadFromJsonAsync<IEnumerable<AdminGroup>>();
            var grupos = roles.Where(r => r.Grupo != "SuperAdmin").Where(r => r.Grupo != "AdminCA").Where(r => r.Grupo != "AdminSecCA").ToList();
            var adminGrups = grupos.Select(r => r.Grupo);
            return adminGrups;
        }
    }
}
