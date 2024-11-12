using api.reunion.Data;
using Microsoft.EntityFrameworkCore;

namespace api.reunion.Services
{
    public class AdminGroupService(AgendaformacaoContext context, ILdapService ldapService) : IAdminGroupService
    {
        private readonly AgendaformacaoContext _context = context;
        private readonly ILdapService _ldapService = ldapService;

        /// <summary>
        /// Vai verificar qual é o grupo a que um utilizador pertence
        /// </summary>
        /// <param name="username">Email do utilizador</param>
        /// <returns>Role do utilizador </returns>
        public string GetUserRole(string username)
        {
            var user = _ldapService.GetLdapUser(username);
            var userGroups = user.Groups;

            var adminGroups = _context.AdminGroups.ToList();
            var adminGroupLookup = adminGroups.ToDictionary(ag => ag.SecurityGroup, ag => ag.Grupo);

            foreach (var group in userGroups!)
            {
                if (adminGroupLookup.TryGetValue(group, out var role))
                {
                    return role;
                }
            }

            return "Client";
        }

        /// <summary>
        /// Devolve o grupo de segurança respetivo da role
        /// </summary>
        /// <param name="role">Role do utilizador</param>
        /// <returns>Grupo de Segurança</returns>
        public string GetGroup(string role)
        {
            var adminGroups = _context.AdminGroups.ToList();

            var group = adminGroups.Where(ag => ag.Grupo == role).First().SecurityGroup;

            return group;
        }

        /// <summary>
        /// Verifica se o utilizador que iniciou sessão é administrador
        /// no local que selecionou
        /// </summary>
        /// <param name="username">Utilizador</param>
        /// <param name="selectedLocal">Id do local</param>
        /// <returns>Role do utilizador</returns>
        public string VerifyUserRole(string username, int selectedLocal)
        {
            var user = _ldapService.GetLdapUser(username);
            var userGroups = user.Groups;
            var role = "Client";

            var adminGroups = _context.AdminGroups.ToList();
            var adminGroupLookup = adminGroups.ToDictionary(ag => ag.SecurityGroup, ag => ag.Grupo);

            foreach (var group in userGroups!)
            {
                if (adminGroupLookup.TryGetValue(group, out var foundRole))
                {
                    role = foundRole;
                    break;
                }
            }

            if (role != "AdminSecCA" && role != "SuperAdmin" && role != "AdminCA")
            {
                var local = _context.Locais
                    .Include(l => l.AdminGroup)
                    .FirstOrDefault(l => l.LocalId == selectedLocal);

                if (local == null || local.AdminGroup.Grupo != role)
                {
                    role = "Client";
                }
            }

            return role;
        }
    }
}
