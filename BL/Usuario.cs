using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Login(string email, string password)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] bytesSha256 = Encriptar(bytes);

                    var query = context.Usuarios.FromSqlRaw($"UsuarioLogin '{email}',@Password", new SqlParameter("@Password", bytesSha256)).ToList();
                    if (query.Count() > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existe el usuario.";
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
        
        public static ML.Result GetByEmail(string email)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByEmail '{email}'").ToList();
                    if (query.Count() > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "El email no existe.";
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
        public static ML.Result UpdatePassword(string email, string password)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] passwordSha = Encriptar(bytes);

                    int rowsAffected = context.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{email}',@Password", new SqlParameter("@Password",passwordSha));
                    if( rowsAffected > 0 )
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se ha podido actualizar la contraseña";
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
        public static ML.Result Add(string email, string password)
        {
            ML.Result result = new ML.Result();
            try
            {
                
                using (DL.EsantiagoCineContext context = new DL.EsantiagoCineContext())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] passwordSha = Encriptar(bytes);

                    int rowsAffected = context.Database.ExecuteSqlRaw($"UsuarioAdd '{email}',@Password", new SqlParameter("@Password", passwordSha));
                    if (rowsAffected > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se ha podido agregar el usuario";
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
        public static byte[] Encriptar(byte[] data)
        {
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] datosEncriptados = sha256.ComputeHash(data);
                return datosEncriptados;
            }
        }
    }
}
