using Microsoft.EntityFrameworkCore;
using ML;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    var query = context.Cines.FromSqlRaw("CineGetAll");
                    if(query != null)
                    {
                        result.Objects = new List<object>();
                        foreach(var registro in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.Zona = new ML.Zona();
                            cine.IdCine = registro.IdCine;
                            cine.Nombre = registro.Nombre;
                            cine.Direccion = registro.Direccion;
                            cine.Zona.IdZona = registro.IdZona;
                            cine.Zona.Nombre = registro.NombreZona;
                            cine.Ventas = double.Parse(registro.Ventas.ToString());
                            result.Objects.Add(cine);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Sin registros.";
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
        public static ML.Result GetById(int idCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    var query = context.Cines.FromSqlRaw($"CineGetById {idCine}");
                    if (query != null)
                    {
                        result.Object = new object();
                        foreach (var registro in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.Zona = new ML.Zona();
                            cine.IdCine = registro.IdCine;
                            cine.Nombre = registro.Nombre;
                            cine.Direccion = registro.Direccion;
                            cine.Zona.IdZona = registro.IdZona;
                            cine.Zona.Nombre = registro.NombreZona;
                            cine.Ventas = double.Parse(registro.Ventas.ToString());
                            result.Object = cine;
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Registro no encontrado";
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
        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    int rowsAffected = context.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}','{cine.Direccion}',{cine.Zona.IdZona},{cine.Ventas}");
                    if(rowsAffected > 0 )
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al agregar cine";
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
        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    int rowsAffected = context.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine},'{cine.Nombre}','{cine.Direccion}',{cine.Zona.IdZona},{cine.Ventas}");
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al actualizar cine";
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
        public static ML.Result Delete(int idCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    int rowsAffected = context.Database.ExecuteSqlRaw($"CineDelete {idCine}");
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al eliminar cine";
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
    }
}