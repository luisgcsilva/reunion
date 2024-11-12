using Microsoft.AspNetCore.Mvc;

namespace web.reunion.Controllers
{
    public class ManualController : Controller
    {
        /// <summary>
        /// Método para abrir um novo separador com o Manual de Administrador
        /// </summary>
        /// <returns></returns>
        public IActionResult ManualAdministrador()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ManualUtilizacaoAdmin.pdf");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Response.Headers.Append("Content-Disposition", "inline; filename=ManualUtilizacaoAdmin.pdf");
            return new FileStreamResult(fileStream, "application/pdf");
        }

        /// <summary>
        /// Método para abrir um novo separador com o Manual de Utilizador
        /// </summary>
        /// <returns></returns>
        public IActionResult ManualUtilizador()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ManualUtilizacao.pdf");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            Response.Headers.Append("Content-Disposition", "inline; filename=ManualUtilizacao.pdf");
            return new FileStreamResult(fileStream, "application/pdf");
        }
    }

}
