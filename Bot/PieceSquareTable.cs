using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars.Bot
{
    /// <summary>
    /// other name reward Table
    /// references: https://www.chessprogramming.org/Piece-Square_Tables\n
    ///
    /// </summary>
    internal class PieceSquareTable
    {

        static readonly int[,] Lance =
     {
        {40, 40, 40, 40, 40, 40, 40, 40, 40},
        {30, 30, 30, 30, 30, 30, 30, 30, 30},
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {10, 10, 10, 10, 10, 10, 10, 10, 10},
        { 5,  5,  5,  5,  5,  5,  5,  5,  5},
        { 3,  3,  3,  3,  3,  3,  3,  3,  3},
        { 2,  2,  2,  2,  2,  2,  2,  2,  2},
        { 1,  1,  1,  1,  1,  1,  1,  1,  1},
        { 0,  0,  0,  0,  0,  0,  0,  0,  0}
    };

        static readonly int[,] Knight =
        {
        {20, 20, 20, 20, 20, 20, 20, 20, 20},
        {15, 15, 15, 15, 15, 15, 15, 15, 15},
        {10, 10, 10, 10, 10, 10, 10, 10, 10},
        { 5,  5,  5,  5,  5,  5,  5,  5,  5},
        { 3,  3,  3,  3,  3,  3,  3,  3,  3},
        { 2,  2,  2,  2,  2,  2,  2,  2,  2},
        { 1,  1,  1,  1,  1,  1,  1,  1,  1},
        { 0,  0,  0,  0,  0,  0,  0,  0,  0},
        { 0,  0,  0,  0,  0,  0,  0,  0,  0}
    };

        static readonly int[,] SilverGeneral =
        {
        {15, 15, 15, 15, 15, 15, 15, 15, 15},
        {12, 12, 12, 12, 12, 12, 12, 12, 12},
        {10, 10, 10, 10, 10, 10, 10, 10, 10},
        { 8,  8,  8,  8,  8,  8,  8,  8,  8},
        { 6,  6,  6,  6,  6,  6,  6,  6,  6},
        { 4,  4,  4,  4,  4,  4,  4,  4,  4},
        { 2,  2,  2,  2,  2,  2,  2,  2,  2},
        { 1,  1,  1,  1,  1,  1,  1,  1,  1},
        { 0,  0,  0,  0,  0,  0,  0,  0,  0}
    };

        static readonly int[,] GoldGeneral ={
            {18, 18, 18, 18, 18, 18, 18, 18, 18},
            {15, 15, 15, 15, 15, 15, 15, 15, 15},
            {12, 12, 12, 12, 12, 12, 12, 12, 12},
            {10, 10, 10, 10, 10, 10, 10, 10, 10},
            { 8,  8,  8,  8,  8,  8,  8,  8,  8},
            { 6,  6,  6,  6,  6,  6,  6,  6,  6},
            { 4,  4,  4,  4,  4,  4,  4,  4,  4},
            { 2,  2,  2,  2,  2,  2,  2,  2,  2},
            { 0,  0,  0,  0,  0,  0,  0,  0,  0}
        };

        static readonly int[,] Bishop ={
            {10, 15, 15, 15, 20, 15, 15, 15, 10},
            {15, 20, 20, 20, 25, 20, 20, 20, 15},
            {15, 20, 25, 25, 30, 25, 25, 20, 15},
            {15, 20, 25, 30, 35, 30, 25, 20, 15},
            {20, 25, 30, 35, 40, 35, 30, 25, 20},
            {15, 20, 25, 30, 35, 30, 25, 20, 15},
            {15, 20, 25, 25, 30, 25, 25, 20, 15},
            {15, 20, 20, 20, 25, 20, 20, 20, 15},
            {10, 15, 15, 15, 20, 15, 15, 15, 10}
        };

        static readonly int[,] Rook ={
            {20, 20, 25, 30, 40, 30, 25, 20, 20},
            {20, 20, 25, 30, 40, 30, 25, 20, 20},
            {20, 20, 25, 30, 35, 30, 25, 20, 20},
            {20, 20, 25, 30, 35, 30, 25, 20, 20},
            {20, 20, 25, 30, 35, 30, 25, 20, 20},
            {20, 20, 25, 30, 35, 30, 25, 20, 20},
            {20, 20, 25, 30, 35, 30, 25, 20, 20},
            {20, 20, 25, 30, 40, 30, 25, 20, 20},
            {20, 20, 25, 30, 40, 30, 25, 20, 20}
        };

        static readonly int[,] King =
            {
            { 0,  5, 10, 15, 20, 15, 10,  5,  0},
            { 5, 10, 15, 20, 25, 20, 15, 10,  5},
            {10, 15, 20, 25, 30, 25, 20, 15, 10},
            {15, 20, 25, 30, 35, 30, 25, 20, 15},
            {20, 25, 30, 35, 40, 35, 30, 25, 20},
            {15, 20, 25, 30, 35, 30, 25, 20, 15},
            {10, 15, 20, 25, 30, 25, 20, 15, 10},
            { 5, 10, 15, 20, 25, 20, 15, 10,  5},
            { 0,  5, 10, 15, 20, 15, 10,  5,  0}
        };

       protected static readonly int[,] PawnStart = new int[9, 9]
        {
            { 34, 34, 34, 34, 34, 34, 34, 34, 34 },
            { 33, 33, 33, 33, 33, 33, 33, 33, 33 },
            { 32, 32, 32, 32, 32, 32, 32, 32, 32 },
            { 29, 29, 29, 29, 29, 29, 29, 29, 29 },
            { 25, 25, 25, 25, 25, 25, 25, 25, 25 },
            { 20, 20, 20, 20, 20, 20, 20, 20, 20 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };





        protected static readonly int[,] PawnEnd = new int[9, 9] {
            {30, 30, 30, 30, 30, 30, 30, 30, 30}, // row 0, opponent’s promotion zone
            {25, 25, 25, 25, 25, 25, 25, 25, 25}, // row 1
            {20, 20, 20, 20, 20, 20, 20, 20, 20}, // row 2
            {15, 15, 15, 15, 15, 15, 15, 15, 15}, // row 3
            {10, 10, 10, 10, 10, 10, 10, 10, 10}, // row 4
            { 5,  5,  5,  5,  5,  5,  5,  5,  5}, // row 5
            { 3,  3,  3,  3,  3,  3,  3,  3,  3}, // row 6
            { 1,  1,  1,  1,  1,  1,  1,  1,  1}, // row 7
            { 0,  0,  0,  0,  0,  0,  0,  0,  0}  // row 8
        };

        protected static int[,] zeroRewardTable = new int[9, 9] {
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0}
        };
        static int[,] GetFlippedTable(int[,] table)
        {
            int cols = table.GetLength(1);
            int rows = table.GetLength(0);
            int[,] flippedTable = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                int r2 = rows - i - 1;
                for (int c = 0; c < cols; c++)
                {
                    flippedTable[r2, c] = table[i, c];
                }
            }
            return flippedTable;
        }
        protected static readonly Dictionary<Type, int[,]> pieceVal = new Dictionary<Type, int[,]>
        {
            { typeof(Pawn), PawnStart },
            { typeof(Lance), Lance },
            { typeof(Knight),  Knight },
            { typeof(SilverGeneral),  SilverGeneral },
            { typeof(GoldGeneral),  GoldGeneral },
            { typeof(Bishop),  Bishop },
            { typeof(Rook),  Rook },
            { typeof(King),  King },
        };

        /// <summary>
        /// static functions are more performant -> no overhead for this
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int[,] PositionBonusTable(Piece piece)
        {
            int[,] positionBonus = zeroRewardTable;
            switch (piece)
            {
                case Pawn _:
                    positionBonus = PawnStart;
                    break;
                case Lance _:
                    positionBonus = Lance;
                    break;
                case Knight _:
                    positionBonus = Knight;
                    break;
                case SilverGeneral _:
                    positionBonus = SilverGeneral;
                    break;
                case GoldGeneral _:
                    positionBonus = GoldGeneral;
                    break;
                case Bishop _:
                    positionBonus = Bishop;
                    break;
                case Rook _:
                    positionBonus = Rook;
                    break;
                case King _:
                    positionBonus = King;
                    break;
                case null:
                    throw new ArgumentNullException(nameof(piece));
                default:
                    throw new Exception($"forgot type {piece.GetType()}");
            }
            if (piece.Color == EnumPlayer.Black)
            { return GetFlippedTable(positionBonus); }
            return positionBonus;
        }
        public static int PositionBonus(Piece piece, Position position) => PositionBonus(piece, position.Row, position.Column);


        /// <summary>
        /// static functions are more performant -> no overhead for this
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static int PositionBonus(Piece piece, int row, int col, int phase = 0)
        {

            int[,] positionBonus = zeroRewardTable;
            int[,] positionBonusEnd = positionBonus;
            switch (piece)
            {
                case Pawn _:
                    positionBonus = PawnStart;
                    positionBonusEnd = PawnEnd;
                    break;
                case Lance _:
                    positionBonus = Lance;
                    positionBonusEnd = positionBonus;
                    break;
                case Knight _:
                    positionBonus = Knight;
                    positionBonusEnd = positionBonus;
                    break;
                case SilverGeneral _:
                    positionBonus = SilverGeneral;
                    positionBonusEnd = positionBonus;
                    break;
                case GoldGeneral _:
                    positionBonus = GoldGeneral;
                    positionBonusEnd = positionBonus;
                    break;
                case Bishop _:
                    positionBonus = Bishop;
                    positionBonusEnd = positionBonus;
                    break;
                case Rook _:
                    positionBonus = Rook;
                    positionBonusEnd = positionBonus;
                    break;
                case King _:
                    positionBonus = King;
                    positionBonusEnd = positionBonus;
                    break;
                case null:
                    throw new ArgumentNullException(nameof(piece));
                default:
                    throw new Exception($"forgot type {piece.GetType()}");
            }
            float interpolated;
            if (piece.Color == EnumPlayer.White)
            {
                int newRow = 9 - row - 1;
                interpolated = (1.0f - phase) * positionBonus[newRow, col] + phase * positionBonusEnd[newRow, col];
                return (int)(0.5 * interpolated);
            }
            interpolated = (1.0f - phase) * positionBonus[row, col] + phase * positionBonusEnd[row, col];
            return (int)(0.5 * interpolated);
            //return positionBonus[row, col];

        }




    }
}

