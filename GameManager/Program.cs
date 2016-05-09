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
            string stats = "";
            for (int i = 0; i < KIs.AvaliableKIs.Count(); i++)
            {
                for (int j = i + 1; j < KIs.AvaliableKIs.Count(); j++)
                {
                    stats += match(KIs.AvaliableKIs[i], KIs.AvaliableKIs[j]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n\n\nGesamtstatistik:\n\n" + stats);
            Console.ReadKey();
        }

        static string match(Type bN0, Type bN1)
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
                if (winnerKI != null) {
                    if (winnerKI.Equals(bN0))
                        wins0++;
                    else
                        wins1++;
                }

                Console.WriteLine("Game " + ((i + 1) + "/" + numGames).PadRight(9) + " " + Math.Floor((decimal) (i + 1) / (decimal) numGames * 100) + "% gespielt");
                Console.WriteLine(bN0N + " " + wins0  + "      " + bN1N + " " + wins1);
            }

            string winnerName;
            if (wins0 == wins1)
                winnerName = "unentschieden";
            else if (wins0 > wins1)
                winnerName = bN0N;
            else
                winnerName = bN1N;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n" + bN0N + " " + wins0 + "      " + bN1N + " " + wins1 + " Winner: " + winnerName + "\n\n");
            Console.ResetColor();
            return "\n" + bN0N.PadRight(15) + " vs " + bN1N.PadRight(15) + "  " + wins0.ToString().PadRight(4) + ":" + wins1.ToString().PadRight(4) + " Winner: " + winnerName + "\n";
        }
    }
}
