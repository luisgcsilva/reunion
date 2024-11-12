using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using web.reunion.Models;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using web.reunion.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace web.reunion.Controllers
{
    [AllowAnonymous]
    public class AccountController(
        IHttpClientFactory clientFactory,
        ILocalService localService) : Controller
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly ILocalService _localService = localService;

        /*
         * Método para abir a view para o login
         */
         /// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                var locaisList = await _localService.GetLocaisAsync();

                locaisList = [.. locaisList.Where(l => l.IsActive == true)];

                var model = new LoginViewModel
                {
                    Locais = locaisList,
                };

                ViewData["LocalId"] = new SelectList(locaisList, "LocalId", "Descricao");
                return View(model);
            } 
            else
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                return role switch
                {
                    "Client" => RedirectToAction("Client", "Home"),
                    "AdminCA" => RedirectToAction("Admin", "Home"),
                    "AdminSecCA" => RedirectToAction("AdminSecretariadoCA", "Home"),
                    "SuperAdmin" => Redirect("/Home/SuperAdmin"),
                    _ => RedirectToAction("AdminDashboard", "Home"),
                };
            }
        }

        /*
         * Método para fazer o login
         * Envia os dados para a API
         * Depois de receber a resposta bem sucedida guarda as informações do utilizador
         */
         /// <summary>
         /// Método para iniciar sessão
         /// Envia os dados para a API fazer a validação
         /// Caso a resposta seja bem sucedida guarda as informações do utilizador que recebe da API
         /// </summary>
         /// <param name="model">Utilizador e palavra-passe</param>
         /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("apiClient");
                var response = await client.PostAsJsonAsync("Authentication/login", new LoginModel { Username = model.Username, Password = model.Password, LocalId = model.LocalId });

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<AuthResponse>(responseContent, MyJsonOptions.Default);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(result!.Token);
                    var claims = jwtToken.Claims.ToList();

                    var locaisList = await _localService.GetLocaisAsync();

                    var localCA = locaisList.Where(l => l.Descricao == "CA").First();

                    locaisList = locaisList.Where(l => l.IsActive == true).ToList();

                    claims.Add(new Claim("access_token", result.Token!));

                    if (claims.First(c => c.Type == ClaimTypes.Role)?.Value == "AdminSecCA")
                    {
                        claims.Add(new Claim(ClaimTypes.StateOrProvince, localCA.Descricao));

                        claims.Add(new Claim(ClaimTypes.Locality, localCA.LocalId.ToString()));
                    } 
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.StateOrProvince, locaisList.Where(l => l.LocalId == model.LocalId).First().Descricao));

                        claims.Add(new Claim(ClaimTypes.Locality, model.LocalId.ToString()));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var locais = await _localService.GetLocaisAsync();
                    ViewData["LocalId"] = new SelectList(locais, "LocalId", "Descricao");

                    ModelState.AddModelError(string.Empty, "Utilizador ou Palavra-Passe errados!");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Método para terminar a sessão do utilizador
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Abre a view de Acesso Negado
        /// </summary>
        /// <returns></returns>
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}
