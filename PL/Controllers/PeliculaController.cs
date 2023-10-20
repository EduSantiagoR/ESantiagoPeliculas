using Microsoft.AspNetCore.Mvc;
using PL.Models;

namespace PL.Controllers
{
    public class PeliculaController : Controller
    {
        public IActionResult GetAll(int? page)
        {
            Pelicula pelicula = new Pelicula();
            pelicula.Peliculas = new List<object>();
            if(page <= 0)
            {
                page = 1;
            }
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie/");
                string urlApi = "";
                if(page == null)
                {
                    urlApi = "popular?api_key=dfda34907a5287cce7e96ce7d7104318&language=es-MX&page=1";
                    ViewBag.Page = 1;
                }
                else
                {
                    urlApi = $"popular?api_key=dfda34907a5287cce7e96ce7d7104318&language=es-MX&page={page}";
                    ViewBag.Page = page;
                }
                var responseTask = client.GetAsync(urlApi);
                responseTask.Wait();
                var resultService = responseTask.Result;

                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadFromJsonAsync<Root>();
                    readTask.Wait();
                    foreach (var resultPelicula in readTask.Result.results)
                    {
                        pelicula.Peliculas.Add(resultPelicula);
                    }
                }
            }
            return View(pelicula);
        }
        public IActionResult FavoritoAddDelete(int idPelicula, bool isFavorite)
        {
            Favorite favorite = new Favorite();
            favorite.media_type = "movie";
            favorite.media_id = idPelicula;
            favorite.favorite = isFavorite;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/20522136/");
                var responseTask = client.PostAsJsonAsync("favorite?api_key=dfda34907a5287cce7e96ce7d7104318&session_id=8768d2af9dfc36afe93b4709fe844723ac1a29d9", favorite);
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    if (favorite.favorite)
                    {
                        ViewBag.Mensaje = "Agregado a favoritos.";
                    }
                    else
                    {
                        ViewBag.Mensaje = "Eliminado de favoritos.";
                    }
                }
            }
            return PartialView("Modal");
        }
        public IActionResult Favoritos()
        {
            Pelicula pelicula = new Pelicula();
            pelicula.Peliculas = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/20522136/favorite/");
                var responseTask = client.GetAsync("movies?api_key=dfda34907a5287cce7e96ce7d7104318&session_id=8768d2af9dfc36afe93b4709fe844723ac1a29d9&language=es-MX");
                responseTask.Wait();

                var resultService = responseTask.Result;
                if (resultService.IsSuccessStatusCode)
                {
                    var readTask = resultService.Content.ReadFromJsonAsync<Root>();
                    readTask.Wait();
                    foreach (var resultPelicula in readTask.Result.results)
                    {
                        pelicula.Peliculas.Add(resultPelicula);
                    }
                }
            }
            return View(pelicula);
        }
    }
}
