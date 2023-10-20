using Microsoft.AspNetCore.Mvc;
using ML;
using NuGet.Protocol;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5043/Cine");
                var responseTask = client.GetAsync("");
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach(var resultCine in readTask.Result.Objects)
                    {
                        ML.Cine resultItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultCine.ToString());
                        cine.Cines.Add(resultItem);
                    }
                }
            }
            return View(cine);
        }
        public IActionResult Form(int? idCine)
        {
            ML.Result resultZonas = BL.Zona.GetAll();
            ML.Cine cine = new ML.Cine();
            cine.Zona = new ML.Zona();
            if (idCine == null)
            {
                cine.Zona.Zonas = resultZonas.Objects.ToList();
            }
            else
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5043");
                    var responseTask = client.GetAsync($"Cine/{idCine}");
                    responseTask.Wait();

                    var resultService = responseTask.Result;
                    if(resultService.IsSuccessStatusCode)
                    {
                        var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Cine resultItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(readTask.Result.Object.ToString());
                        cine = resultItem;
                    }
                }
                cine.Zona.Zonas = resultZonas.Objects.ToList();
            }
            return View(cine);
        }
        public IActionResult Delete(int idCine)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5043/Cine");
                var responseTask = client.DeleteAsync($"?idCine={idCine}");
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Eliminado correctamente.";
                }
                else
                {
                    ViewBag.Mensaje = "Error al eliminar.";
                }
            }
            return PartialView("Modal");
        }
        [HttpPost]
        public IActionResult Form(ML.Cine cine)
        {
            if(cine.IdCine == null)
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5043");
                    var responseTask = client.PostAsJsonAsync("Cine",cine);
                    responseTask.Wait();

                    var resultService = responseTask.Result;
                    if (resultService.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Agregado correctamente.";
                    }
                    else
                    {
                        ViewBag.Mensaje = "Error al agregar.";
                    }
                }
            }
            else
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5043");
                    var responseTask = client.PutAsJsonAsync($"Cine/{cine.IdCine}", cine);
                    responseTask.Wait();

                    var resultService = responseTask.Result;
                    if (resultService.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Actualizado correctamente.";
                    }
                    else
                    {
                        ViewBag.Mensaje = "Error al actualizar.";
                    }
                }
            }
            return PartialView("Modal");
        }
    }
}
