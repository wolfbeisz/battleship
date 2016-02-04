using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship
{
    class Program
    {
        private static int numGames = 11;
        static void Main(string[] args)
        {
            Console.WriteLine("");

            string[] botNames = {"bot_a", "b", "c", "ds"};

            for (int i = 0; i < botNames.Length; i++) {
                for (int j = i + 1; j < botNames.Length; j++)
                {
                    match(botNames[i], botNames[j]);
                }
            }

            Console.ReadKey();
        }

        static void match(string bN0, string bN1)
        {
            Console.ResetColor();
            Console.Write(bN0);
            Console.Write(" vs ");
            Console.Write(bN1);
            Console.Write("\n");
            int wins0 = 0;
            int wins1 = 0;
            for (int i = 0; i < numGames; i++)
            {
                var winner = playGame();
                if (winner == true)
                    wins0++;
                else
                    wins1++;

                Console.WriteLine("Game " + ((i + 1) + "/" + numGames).PadRight(9) + " " + Math.Floor((decimal) (i + 1) / (decimal) numGames * 100) + "%");
            }
        }

        static bool playGame()
        {
            return new Random().Next(100) < 50 ? true : false;
        }
    }
}
