using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    class Program
    {
        public static void Main(string[] args)
        {
            DataService dataService = new DataService();
            /*
            dataService.CreateUser("testName", "testEmail", "testPass");
            Console.WriteLine(dataService.GetUsers());
            Console.WriteLine(dataService.GetUser(15));
            dataService.UpdateUser(15, "Dragos", "dam@ruc.dk", "rucpass");
            dataService.DeleteUser(15);
            */
        }
    }
}
