using EV3_Examen_Ejercicio_04.Managers;
using EV3_Examen_Ejercicio_04.Models;
using Microsoft.Extensions.Configuration;
using MyLib.SQL;

namespace EV3_Examen_Ejercicio_04
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            // Crea un esquema de base de datos que te permita trabajar con autenticación simple SIN autorización.
            // Escribe un programa con interfaz gráfica o de consola, que solicite los datos necesarios para autenticar
            // a un usuario en el esquema de base de datos que has creado.
            // Cualquier consulta debe realizarse mediante procedimientos almacenados.



            // Configuración
            string jsonFilepath = Path.Combine(Environment.CurrentDirectory, "Data", "DataLayerAccess.json");

            ConfigurationBuilder configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile(jsonFilepath);

            IConfiguration configtree = configBuilder.Build();
            ConfigModel config = configtree.Get<ConfigModel>();


            // Test de que la configuracion funciona
            Console.WriteLine(config.ConnectionString);
            
            
            SqlServer myDb = new SqlServer(config.ConnectionString);
            ClientManager clientManager = new ClientManager(myDb);



            Console.WriteLine("Introduce el identificador de usuario:");
            int idUser = int.Parse(Console.ReadLine());

            Console.WriteLine("Introduce la contraseña:");
            string inputPass= Console.ReadLine();


            // Me devuelve el usuario que vamos a comprobar en la propiedad CurrentUser de ClientManager
            await clientManager.GetUserHashFromDbAsync(idUser);


            SecurityManager.VerifyHash(inputPass, clientManager.currentUser.PasswordHash);


        }
    }
}