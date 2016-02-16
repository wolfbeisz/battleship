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
            Type[] bots = new Type[KIs.AvaliableKIs.Count()];

            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (Type ki in KIs.AvaliableKIs)
            {
                Console.WriteLine(KIs.NewKi(ki, -1).GetName());
                bots.
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

            for (int i = 0; i < bots.Length; i++)
            {
                for (int j = i + 1; j < bots.Length; j++)
                {
                    match(bots[i], bots[j]);
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
                var winnerKI = playGame(bN0, bN1);
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

        static Type playGame(Type bN0, Type bN1)
        {
            return new Random().Next(100) < 50 ? bN0 : bN1;
        }
    }
}
