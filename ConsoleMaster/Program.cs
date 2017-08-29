namespace ConsoleMaster
{
    using System;
    using UserStorage.Services.UserServices;
    using UserStorage.Entities;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Launching Master Console...");

            MasterUserService master = new MasterUserService();

            Console.WriteLine("Master was created...");
            Console.WriteLine("\r\nData from Master:");

            foreach (var user in master.SelectAll())
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("\r\nEnter to Add a User...");
            Console.ReadKey();

            master.Add(new User("Alexandra","Bulka", 21));

            Console.WriteLine("New User was added");

            Console.WriteLine("\r\nData from Master:");

            foreach (var user in master.SelectAll())
            {
                Console.WriteLine(user.ToString());
            }

            Console.ReadKey();
        }
    }
}
