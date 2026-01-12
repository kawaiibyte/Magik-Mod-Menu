using Swed32;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.Title = "Magik Mod Menu v1";

        Console.WriteLine("PvZ isnt detected...");

        while (Process.GetProcessesByName("popcapgame1").Length == 0) //checks to see if  pvz is running
        {
            Thread.Sleep(1000);
        }

        Swed swed = new Swed("popcapgame1");
        IntPtr moduleBase = swed.GetModuleBase("popcapgame1.exe");

        Console.Clear(); //clears the undetected message

        bool infHealth = false;
        bool noCooldown = false;
        bool noSunLoss = false;
        bool sunIncrease = false;
        bool zombieInf = false;
        bool godPlayer = false;


        // offsets
        IntPtr plantHealthAddress = moduleBase + 0x1447A0;
        IntPtr cooldownTimeAddress = moduleBase + 0x958BF;
        IntPtr sunValueAddress = moduleBase + 0x1F636;
        IntPtr zombieHurtAddress = moduleBase + 0x145E04;

        while (true)
        {
            string magik = @"
  __  __          _____ _____ _  __
 |  \/  |   /\   / ____|_   _| |/ /
 | \  / |  /  \ | |  __  | | | ' / 
 | |\/| | / /\ \| | |_ | | | |  <  
 | |  | |/ ____ \ |__| |_| |_| . \ 
 |_|  |_/_/    \_\_____|_____|_|\_\
";



            string options = @"Mod options:

1. No sun loss
2. Inf health
3. No cooldown
4. Planting increases sun bank
5. No chomper bite cooldown (TODO)
6. Godmode (activates 2,3,4, and 5 all at once)
7. Zombies take no damages (only recommended for I. Zombie)
Enter option:";

            while (true)
            {
                Console.WriteLine(magik);
                Console.WriteLine(options);

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (!noSunLoss)
                        {
                            // popcapgame1.exe+1F636 - 89 B7 78 55 00 00
                            Status("No sun loss enabled", true);
                            swed.WriteBytes(sunValueAddress, "90 90 90 90 90 90"); // changed bytes
                            noSunLoss = true;
                        }
                        else
                        {
                            Status("No sun loss disabled", false);
                            swed.WriteBytes(sunValueAddress, "89 B7 78 55 00 00"); //returns them back to normal
                            noSunLoss = false;
                        }
                        break;

                    case "2":
                        if (!infHealth)
                        {
                            // popcapgame1.exe+1447A0 - 83 46 40 FC
                            Status("Inf health enabled", true);
                            swed.WriteBytes(plantHealthAddress, "90 90 90 90");
                            infHealth = true;
                        }
                        else
                        {
                            Status("Inf health disabled", false);
                            swed.WriteBytes(sunValueAddress, "83 46 40 FC");
                            infHealth = false;
                        }
                        break;

                    case "3":
                        if (!noCooldown)
                        {
                            // popcapgame1.exe+958BF - 8B 47 24
                            Status("No cooldown enabled", true);
                            swed.WriteBytes(cooldownTimeAddress, "90 90 90");
                            noCooldown = true;
                        }
                        else
                        {
                            Status("No cooldown disabled", false);
                            swed.WriteBytes(cooldownTimeAddress, "8B 47 24");
                            noCooldown = false;
                        }
                        break;

                    case "4":
                        if (!sunIncrease)
                        {
                            // popcapgame1.exe+1F636 - 89 B7 78 55 00 00
                            // popcapgame1.exe+1F636 - 01 B7 78 55 00 00
                            Status("Increasing sun enabled", true);
                            swed.WriteBytes(sunValueAddress, "01 B7 78 55 00 00");
                            sunIncrease = true;
                        }
                        else
                        {
                            Status("Increasing sun disabled", false);
                            swed.WriteBytes(sunValueAddress, "89 B7 78 55 00 00");
                            sunIncrease = false;
                        }
                        break;

                    case "6":
                        if (!godPlayer)
                        {
                            Status("Godmode enabled", true);
                            swed.WriteBytes(plantHealthAddress, "90 90 90 90");
                            swed.WriteBytes(cooldownTimeAddress, "90 90 90");
                            swed.WriteBytes(sunValueAddress, "01 B7 78 55 00 00");
                            godPlayer = true;
                        }
                        else
                        {
                            Status("Godmode disabled", false);
                            swed.WriteBytes(plantHealthAddress, "83 46 40 FC");
                            swed.WriteBytes(cooldownTimeAddress, "8B 47 24");
                            swed.WriteBytes(sunValueAddress, "89 B7 78 55 00 00");
                            godPlayer = false;
                        }
                        break;

                    case "7":
                        if (!zombieInf)
                        {
                            // popcapgame1.exe+145E04 - 89 AF C8 00 00 00
                            Status("Inf health zombies enabled", true);
                            swed.WriteBytes(zombieHurtAddress, "90 90 90 90 90 90");
                            zombieInf = true;
                        }
                        else
                        {
                            Status("Inf health zombies disabled", false);
                            swed.WriteBytes(zombieHurtAddress, "89 AF C8 00 00 00");
                            zombieInf = false;
                        }
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Invalid choice");
                        Console.ResetColor();
                        break;
                }
            }
            // colored text
            static void Status(string text, bool enabled)
            {
                Console.ForegroundColor = enabled ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ResetColor();
            }

        }
    }
}
