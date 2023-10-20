using Microsoft.Extensions.Configuration;

namespace DL
{
    public class Conexion
    {
        //private readonly IConfiguration _configuration;

        public static string GetConnection()
        {
            //string connectionString = _configuration.GetConnectionString("ESantiagoCine");
            //string providerName = _configuration["ConnectionStrings:ProviderName"];
            return "Data Source=.;Initial Catalog=ESantiagoCine;User ID=sa;Password=pass@word1";
        }
    }
}