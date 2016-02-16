using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManager;

namespace GameManager
{
    class Program
    {
        private static int numGames = 1000;

        static void Main(string[] args)
        {
			Console.WriteLine("Loading KIs...");
			KIs.LoadKIs();

			Console.WriteLine ("Avaliable KIs:");

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Type ki in KIs.AvaliableKIs)
            {
                Console.WriteLine(KIs.NewKi(ki, -1).GetName());
			}
            Console.ForegroundColor = ConsoleColor.Red;
            if (KIs.AvaliableKIs.Count() == 0)
            {
                Console.WriteLine("Keine KIs!");
                Console.ReadKey();
                return;
            }
            else if (KIs.AvaliableKIs.Count() == 1)
            {
                Console.WriteLine("Nur eine KI!");
                Console.ReadKey();
                return;
            }
            Console.ResetColor();

            for (int i = 0; i < KIs.AvaliableKIs.Count(); i++)
            {
                for (int j = i + 1; j < KIs.AvaliableKIs.Count(); j++)
                {
                    match(KIs.AvaliableKIs[i], KIs.AvaliableKIs[j]);
                }
            }
            Console.WriteLine("\n\n\n\n");
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Gesamtstatistik");

            Console.ReadKey();
        }

        static void match(Type bN0, Type bN1)
        {
            string bN0N = KIs.NewKi(bN0, -1).GetName();
            string bN1N = KIs.NewKi(bN1, -1).GetName();
            Console.WriteLine(bN0N + " vs " + bN1N);
            int wins0 = 0;
            int wins1 = 0;
            for (int i = 0; i < numGames; i++)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                var winnerKI = new GameManager().PlayGame(bN0, bN1);
                Console.ResetColor();
                if (winnerKI.Equals(bN0))
                    wins0++;
                else
                    wins1++;

                Console.WriteLine("Game " + ((i + 1) + "/" + numGames).PadRight(9) + " " + Math.Floor((decimal) (i + 1) / (decimal) numGames * 100) + "% gespielt");
                Console.WriteLine(bN0N + " " + wins0  + "      " + bN1N + " " + wins1);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n" + bN0N + " " + wins0 + "      " + bN1N + " " + wins1 + "\n\n");
            Console.ResetColor();
        }
    }
}
