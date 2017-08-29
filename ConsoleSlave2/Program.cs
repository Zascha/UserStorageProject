namespace ConsoleSlave2
{
    using System;
    using UserStorage.Services.UserServices;
    using UserStorage.Entities;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Slave2 Console was launched.");

            SlaveUserService slave = new SlaveUserService();

            Console.WriteLine("\r\nData from Slave after the Master Add operation:");
            Console.WriteLine("\r\nData from Slave:");

            foreach (var user in slave.SelectAll())
            {
                Console.WriteLine(user.ToString());
            }
            
            Console.ReadKey();
        }
    }
}
