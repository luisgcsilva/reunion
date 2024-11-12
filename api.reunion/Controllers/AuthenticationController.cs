using Microsoft.AspNetCore.Mvc;
using api.reunion.Services;
using System.Security.Claims;
using api.reunion.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json.Linq;

namespace api.reunion.Controllers
{
    [Route("api.reunion/[controller]")]
    [ApiController]
    public class AuthenticationController(ILdapService ldapService,
        IAdminGroupService adminGroupService,
        IConfiguration configuration) : ControllerBase
    {
        private readonly ILdapService _ldapService = ldapService;
        private readonly IAdminGroupService _adminGroupService = adminGroupService;
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// - Faz a validação do utilizador no serviço de Ldap
        /// - Vai buscar os dados do LdapUser
        /// - Verifica se está em algum grupo de administrador
        /// - Guarda as informações necessárias nas Claims do Utilizador
        /// - Cria um Token de acesso
        /// </summary>
        /// <param name="model">Username: nome de utilizador, Password: palavra-passe</param>
        /// <returns>Token </returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (_ldapService.ValidateUser(model.Username!, model.Password!))
            {
                var user = _ldapService.GetLdapUser(model.Username!);
                /*
                    Ir à bd buscar lista de admin groups
                    comparar se está em algum deles
                    guardar o que nome e id do que encontrar senao guarda client
                 */
                var role = _adminGroupService.GetUserRole(model.Username!);

                var upRole = _adminGroupService.VerifyUserRole(model.Username!, model.LocalId);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, model.Username!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("FullName", user.FullName!),
                    new Claim(ClaimTypes.Role, upRole)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(12),
                    signingCredentials: creds
                    );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Vai buscar todos os utilizadores de um determinado grupo/role   
        /// </summary>
        /// <param name="role">Role do utilizador</param>
        /// <returns>Lista de Utilizadores desse grupo/role</returns>
        [HttpGet("users/{role}")]
        public IActionResult GetUsersFromGroup(string role)
        {
            try
            {
                var groupName = _adminGroupService.GetGroup(role);
                var users = _ldapService.GetLdapUsersFromGroup(groupName);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
