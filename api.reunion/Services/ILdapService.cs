using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Options;
using api.reunion.Models;
using System.DirectoryServices.AccountManagement;

namespace api.reunion.Services
{
    public interface ILdapService
    {
        bool ValidateUser(string username, string password);
        LdapUser GetLdapUser(string username);
        List<string> GetLdapUsersFromGroup(string groupName);
    }
    public class LdapService(IOptions<LdapSettings> ldapSettings) : ILdapService
    {
        private readonly LdapSettings _ldapSettings = ldapSettings.Value;

        /// <summary>
        /// Vai à AD buscar o utilizador que iniciou sessão
        /// </summary>
        /// <param name="username">Email do utilizador</param>
        /// <returns></returns>
        public LdapUser GetLdapUser(string username)
        {
            using var context = new PrincipalContext(
                ContextType.Domain,
                _ldapSettings!.Domain,
                _ldapSettings!.MachineAccountName,
                _ldapSettings!.MachineAccountPassword);
            
            using var searcher = new PrincipalSearcher(new UserPrincipal(context) { SamAccountName = username });

            var name = "";

            if (searcher.FindOne() is UserPrincipal result)
            {
                if (result.GivenName != null)
                {
                    name = result.GivenName + " " + result.Surname;
                }
                else
                {
                    name = result.DisplayName;
                }
                var ldapUser = new LdapUser
                {
                    Username = username,
                    FullName = name,
                    Email = result.EmailAddress,
                    Groups = result.GetGroups().Select(g => g.Name).ToList()
                };

                return ldapUser;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Vai à AD validar o utilizador
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">Palavra-passe</param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password)
        {
            using var context = new PrincipalContext(
                ContextType.Domain,
                _ldapSettings.Domain,
                _ldapSettings.MachineAccountName,
                _ldapSettings.MachineAccountPassword);
            return context.ValidateCredentials(username, password);
        }

        /// <summary>
        /// Vai buscar à AD os utilizadores de um grupo
        /// </summary>
        /// <param name="groupName">Nome do grupo</param>
        /// <returns>Lista de Utilizadores</returns>
        public List<string> GetLdapUsersFromGroup(string groupName)
        {
            using var context = new PrincipalContext(
                ContextType.Domain,
                _ldapSettings.Domain,
                _ldapSettings.MachineAccountName,
                _ldapSettings.MachineAccountPassword);

            using var group = GroupPrincipal.FindByIdentity(context, groupName);

            if (group == null)
            {
                throw new Exception($"Group {groupName} not found.");
            }

            var users = new List<string>();

            foreach (var principal in group.GetMembers())
            {
                if (principal is UserPrincipal userPrincipal)
                {
                    users.Add(userPrincipal.EmailAddress);
                }
            }

            return users;
        }
    }
}
