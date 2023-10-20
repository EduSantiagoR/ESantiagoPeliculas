using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace PL.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string? email, string password, string? emailNew)
        {
            HttpContext.Session.Clear();
            if(emailNew == null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5043/api/");
                    var requestTask = client.GetAsync($"Usuario?email={email}&password={password}");
                    requestTask.Wait();

                    var resultService = requestTask.Result;
                    if (resultService.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Mensaje = "Tu email o contraseña son incorrectas.";
                        return PartialView("Modal");
                    }
                }
            }
            else
            {
                ML.Result result = BL.Usuario.Add(emailNew, password);
                if (result.Correct)
                {
                    ViewBag.Mensaje = "Te has registrado de manera éxitosa. Inicia sesión.";
                }
                else
                {
                    ViewBag.Mensaje = "No hemos podido registrarte, intentalo de nuevo.";
                }
                return PartialView("Modal");
            }
        }
        
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string email)
        {
            ML.Result result = BL.Usuario.GetByEmail(email);
            if (result.Correct)
            {
                SendMail(email);
                HttpContext.Session.SetString("Email", email);

                ViewBag.Mensaje = $"Se ha enviado un correo a la dirección: {email}";
            }
            else
            {
                ViewBag.Mensaje = $"El correo {email} no se encuentra registrado, si lo deseas puedes hacer el registro.";
            }
            return PartialView("Modal");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string password)
        {
            string email = HttpContext.Session.GetString("Email");
            ML.Result result = BL.Usuario.UpdatePassword(email, password);
            if (result.Correct)
            {
                ViewBag.Mensaje = "Has actualizado tu contraseña. Inicia sesión con ella.";
            }
            else
            {
                ViewBag.Mensaje = "No se ha podido actualizar la contraseña, intentalo de nuevo";
            }
            HttpContext.Session.Clear();
            return PartialView("Modal");
        }
        public static void SendMail(string destino)
        {
            string from = "e.santiago.r05@gmail.com";
            string displayName = "Eduardo Santiago Ramírez";
            string body = System.IO.File.ReadAllText(Path.Combine("Views", "Login", "EmailBody.html"));

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from, displayName);
            mail.To.Add(destino);

            mail.Subject = "Reestablecimiento de contraseña";
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential(from, "mdpjchfcneawhwol");
            client.EnableSsl = true;

            client.Send(mail);
        }
    }
}
