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
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            GetCarrito(carrito);
            
            Document document = new Document();
            
            PdfPTable tblProductos = new PdfPTable(4);
            tblProductos.WidthPercentage = 100;

            PdfPCell clNombre = new PdfPCell(new Phrase("Nombre", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;
            PdfPCell clDescripcion = new PdfPCell(new Phrase("Descripción", _standardFont));
            clDescripcion.BorderWidth = 0;
            clDescripcion.BorderWidthBottom = 0.75f;
            PdfPCell clCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
            clCantidad.BorderWidth = 0;
            clCantidad.BorderWidthBottom = 0.75f;
            PdfPCell clPrecio = new PdfPCell(new Phrase("Precio", _standardFont));
            clPrecio.BorderWidth = 0;
            clPrecio.BorderWidthBottom = 0.75f;

            tblProductos.AddCell(clNombre);
            tblProductos.AddCell(clDescripcion);
            tblProductos.AddCell(clCantidad);
            tblProductos.AddCell(clPrecio);
            int total = 0;
            foreach(ML.Dulceria dulce in carrito.Carrito)
            {
                clNombre = new PdfPCell(new Phrase(dulce.Nombre, _standardFont));
                clNombre.BorderWidth = 0;
                clDescripcion = new PdfPCell(new Phrase(dulce.Descripcion, _standardFont));
                clDescripcion.BorderWidth = 0;
                clCantidad = new PdfPCell(new Phrase(dulce.Cantidad.ToString(), _standardFont));
                clCantidad.BorderWidth = 0;
                clPrecio = new PdfPCell(new Phrase(dulce.Precio.ToString(), _standardFont));
                total += (dulce.Cantidad * int.Parse(dulce.Precio.ToString("0")));
                clPrecio.BorderWidth = 0;
                tblProductos.AddCell(clNombre);
                tblProductos.AddCell(clDescripcion);
                tblProductos.AddCell(clCantidad);
                tblProductos.AddCell(clPrecio);
            }

            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            document.Add(new Paragraph("Adquisición de productos"));
            document.Add(Chunk.NEWLINE);
            document.Add(tblProductos);
            document.Add(Chunk.NEWLINE);
            document.Add(new Paragraph("Total de compra ($): "+total));
            document.Close();

            HttpContext.Session.Clear();

            return File(memoryStream.ToArray(), "application/pdf", "Compra-" + DateTime.Now + ".pdf");

        }
    }
}
