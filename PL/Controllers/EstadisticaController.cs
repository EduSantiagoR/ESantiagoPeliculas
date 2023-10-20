using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EstadisticaController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5043/Cine");
                var responseTask = client.GetAsync("");
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var resultCine in readTask.Result.Objects)
                    {
                        ML.Cine resultItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(resultCine.ToString());
                        cine.Cines.Add(resultItem);
                    }
                }
            }
            return View(cine);
        }
    }
}
