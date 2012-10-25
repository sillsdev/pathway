using System;
using System.Collections.Generic;
using Microsoft.Win32;
using SIL.Tool;

namespace SIL.PublishingSolution
{
    static class Program
    {
        public static void Main(String[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Argument is empty \r\n"); // Check for null array
                Console.WriteLine("Argument 1 - Internet IP address \r\n"); // Internet Availability
                Console.WriteLine("Argument 2 - Registry Key Location \r\n"); // Registry Key Location
                Console.WriteLine("Argument 3 - Registry Key Name \r\n"); // Registry Key Name
            }
            else
            {
                Console.Write("args length is ");
                Console.WriteLine(args.Length); // Write array length
                for (int i = 0; i < args.Length; i++) // Loop through array
                {
                    string argument = args[i];
                    Console.Write("args index ");
                    Console.Write(i); // Write index
                    Console.Write(" is [");
                    Console.Write(argument); // Write string
                    Console.WriteLine("]");
                }

                Console.Write("\r\n args index completed \r\n");
                string sendUsageData = Common.GetValueFromRegistryFromCurrentUser(args[1], args[2]);
                Console.Write("\r\n args index registry check" + " -- " + args[1] + " -- " + args[2] + " -- " + sendUsageData);
                UserInformation user = new UserInformation();
                if (sendUsageData != null && sendUsageData.ToLower() == "yes") 
                {
                    Console.Write("\r\n args index send usage data \r\n");
                    if (user.CheckInternetAvailability(args[0]))
                    {
                        Console.Write("args index checked availablity \r\n");
                        user.GetUserInformation(true);
                        Console.Write("args index User information completed \r\n");
                    }
                }
            }
            Console.Write("Exit \r\n");

            System.Environment.Exit(1);

            //string sendUsageData = Common.GetValueFromRegistryFromCurrentUser("Software\\Wow6432Node\\SIL\\Pathway", "HelpImprove");
            //UserInformation user = new UserInformation();
            //if (sendUsageData != null && sendUsageData.ToLower() == "yes")
            //{
            //    if (user.CheckInternetAvailability("204.93.172.30"))
            //    {
            //        user.GetUserInformation(true);
            //    }
            //}
        }
    }
}
