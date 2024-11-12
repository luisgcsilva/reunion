using web.reunion.Models;

namespace web.reunion.Services
{
    /// <summary>
    /// Serviço para fazer chamdas à API
    /// Relacionadas com Admins
    /// </summary>
    public class AdminRoleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _adminRolesApiUrl;
        private List<string> _adminRoles = [];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        public AdminRoleService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _adminRolesApiUrl = configuration["AdminRolesApiUrl"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task InitializeRolesAsync()
        {
            using var httpClient = _httpClientFactory.CreateClient("apiClient");
            var response = await httpClient.GetAsync("AdminGroups");
            response.EnsureSuccessStatusCode();
            var r = await response.Content.ReadFromJsonAsync<AdminGroup[]>();
            foreach(var adm in r)
            {
                if (adm.Grupo != "AdminCA" && adm.Grupo != "SuperAdmin" && adm.Grupo != "AdminSecCA")
                {
                    _adminRoles.Add(adm.Grupo);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetAdminRoles()
        {
            return _adminRoles;
        }
    }

}
