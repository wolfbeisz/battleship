using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleSDK;

namespace MonsterKi
{
    public class AttackMapBuilder
    {

        public static Board Build(Board board)
        {
            int[][] state = (int[][])board.State.Clone();

            int size = board.State.Count();
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                for (int colIndex = 0; colIndex < size; colIndex++)
                {
                    int currentValue = state[rowIndex][colIndex];

                    if (currentValue != (int)Field.WATER) 
                    {
                        //var template = BoardUtil.BuildShipTemplate(1, Direction.HORIZONTAL, 0, 0, 0);
                        //BoardUtil.SetFields(state, template, rowIndex - 1, colIndex - 1, ApplyField);
                        
                    }
                }
            }
            return new Board(new List<Ship>(), state);
        }

        public static void ApplyField(int[][] state, int row, int column) 
        { 
        
        }

        public static IList<SimpleShip> DetectShips(int[][] state)
        {
            List<SimpleShip> detected = new List<SimpleShip>();
            

            for (int rowIndex = 0; rowIndex < state.Count(); rowIndex++)
            {
                for (int colIndex = 0; colIndex < state.Count(); colIndex++)
                {
                    // if the current value is a hit => check the surrounding cells 
                    // if the cell exists, then check whether the cells belong to a ship (in detected)
                }
            }

            return detected;
        }
    }
}
