using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml.html.head;
using iTextSharp.tool.xml;

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
        public ActionResult GenerarPDF()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            GetCarrito(carrito);
            string htmlContent = "<head><link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN\" crossorigin=\"anonymous\"></head>";

            htmlContent += "<body><table class=\"table table-dark text-center\"><thead><tr><th>Nombre</th><th>Descripción</th><th>Cantidad</th><th>Precio</th></tr></thead><tbody>";
            foreach (ML.Dulceria dulce in carrito.Carrito){
                htmlContent += $"<tr><td>{dulce.Nombre}</td><td>{dulce.Descripcion}</td><td>{dulce.Cantidad}</td><td>{dulce.Precio}</td></tr>";
            }
            htmlContent += "</tbody></table><script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL\" crossorigin=\"anonymous\"></script></body>";

            Document document = new Document();
            document.AddTitle("Productos adquiridos");
            
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            HTMLWorker htmlWorker = new HTMLWorker(document);
            htmlWorker.Parse(new StringReader(htmlContent));

            //using(TextReader reader = new StringReader(htmlContent))
            //{
            //    XMLWorkerHelper.GetInstance().ParseXHtml(writer,document,reader);
            //}

            document.Close();

            HttpContext.Session.Clear();

            return File(memoryStream.ToArray(), "application/pdf", "Compra-" + DateTime.Now + ".pdf");

        }
    }
}
