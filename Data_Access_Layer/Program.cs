using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    class Program
    {
        public static void Main(string[] args)
        {
            DataService dataService = new DataService();
            dataService.CreateUser("name", "email", "pass");
            Console.WriteLine(dataService.GetUsers());
            Console.WriteLine(dataService.GetUser(7));
            dataService.UpdateUser(7,"user123", "email@email.com", "pass123");
            dataService.DeleteUser(7);


        }
    }
}
