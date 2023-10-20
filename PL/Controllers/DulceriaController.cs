using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DulceriaController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Result result = BL.Dulceria.GetAll();
            ML.Dulceria dulce = new ML.Dulceria();
            if (result.Correct)
            {
                dulce.Dulces = result.Objects;
            }
            return View(dulce);
        }
        public ActionResult Carrito()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            if(HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }
        }
        public ActionResult AddCarrito(int idDulce, string? accion)
        {
            bool existe = false;
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            ML.Result result = BL.Dulceria.GetById(idDulce);
            if(HttpContext.Session.GetString("Carrito") == null)
            {
                if (result.Correct)
                {
                    ML.Dulceria dulce = (ML.Dulceria)result.Object;
                    dulce.Cantidad = 1;
                    carrito.Carrito.Add(dulce);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            else
            {
                ML.Dulceria dulce = (ML.Dulceria)result.Object;
                GetCarrito(carrito);
                foreach(ML.Dulceria dulce1 in carrito.Carrito)
                {
                    if(dulce.IdDulce == dulce1.IdDulce)
                    {
                        if (accion == "Delete")
                        {
                            int index = carrito.Carrito.IndexOf(dulce1);
                            carrito.Carrito.RemoveAt(index);
                        }
                        dulce1.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe)
                {

                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
                else
                {
                    dulce.Cantidad += 1;
                    carrito.Carrito.Add(dulce);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }
            return RedirectToAction("GetAll");
        }
        public ActionResult ClearCarrito()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Carrito");
        }
        public ML.Venta GetCarrito(ML.Venta carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));
            foreach(var obj in ventaSession)
            {
                ML.Dulceria objDulce = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Dulceria>(obj.ToString());
                carrito.Carrito.Add(objDulce);
            }
            return carrito;
        }
    }
}
