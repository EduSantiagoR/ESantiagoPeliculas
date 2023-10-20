using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace BL
{
    public class Dulceria
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw("DulceriaGetAll");
                    if(query != null)
                    {
                        result.Objects = new List<object>();
                        foreach(var obj in query)
                        {
                            ML.Dulceria dulce = new ML.Dulceria();
                            dulce.IdDulce = obj.IdDulce;
                            dulce.Nombre = obj.Nombre;
                            dulce.Descripcion = obj.Descripcion;
                            dulce.Precio = obj.Precio;
                            dulce.Imagen = obj.Imagen;
                            result.Objects.Add(dulce);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al consultar los registros de dulceria.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
        public static ML.Result GetById(int idDulce)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    var query = context.Dulceria.FromSqlRaw($"DulceriaGetById {idDulce}");
                    if( query != null )
                    {
                        foreach(var item in query)
                        {
                            result.Object = new object();
                            ML.Dulceria dulce = new ML.Dulceria();
                            dulce.IdDulce = item.IdDulce;
                            dulce.Nombre = item.Nombre;
                            dulce.Descripcion = item.Descripcion;
                            dulce.Precio = item.Precio;
                            dulce.Imagen = item.Imagen;
                            result.Object = dulce;
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al consultar el id.";
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}
