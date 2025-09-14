using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars.Bot
{
    public class Searcher
    {
        public static readonly Dictionary<string, int> PieceValues = new Dictionary<string, int>
    {
        { "K", 10000 },   // King
        { "R", 900 },     // Rook
        { "B", 800 },     // Bishop
        { "G", 600 },     // Gold General
        { "S", 500 },     // Silver General
        { "N", 300 },     // Knight
        { "L", 300 },     // Lance
        { "P", 100 },     // Pawn
        { "+R", 950 },    // Promoted Rook
        { "+B", 850 },    // Promoted Bishop
        { "+S", 600 },    // Promoted Silver (moves like Gold)
        { "+N", 600 },    // Promoted Knight (moves like Gold)
        { "+L", 600 },    // Promoted Lance (moves like Gold)
        { "+P", 600 }     // Promoted Pawn (moves like Gold)
    };

        protected static readonly Dictionary<Type, int> pieceVal = new Dictionary<Type, int>
        {
            {typeof( King), 10000 },   // King
        { typeof(Rook), 900 },     // Rook
        { typeof(Bishop), 800 },     // Bishop
        { typeof(GoldGeneral), 600 },     // Gold General
        { typeof(SilverGeneral), 500 },     // Silver General
        { typeof(Knight), 300 },     // Knight
        { typeof(Lance), 300 },     // Lance
        {typeof(Pawn), 100 },     // Pawn
        };

        

        public static int Evaluate(Board board, EnumPlayer color)
        {
            //  IEnumerable<Piece> pieces = board.GetPieces();

            Piece[,] pieces = board.pieces;
            int rows = pieces.GetLength(0);
            int cols = pieces.GetLength(1);
            int sum = 0;
            const int openSleeve = -20;
            const int maxCardVal = 14;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Piece piece = pieces[r, c];
                    if (piece == null) continue;
                   
                    int sumPiece = 0;
                    int val = pieceVal[piece.GetType()];
                    int Vz = piece.Color == color ? 1 : -1;
                    sumPiece += val;

                    if (piece.Sleeve.isOpen)
                    {
                        sumPiece += openSleeve; //is negative
                        Card card = piece.Card;
                        if (card is null) { throw new Exception(); }
                        if (card is Landmine)
                        {
                            sumPiece += maxCardVal;
                        }
                        else
                        {
                            int total = card.SP + card.ST;
                            if (card.RPS == RPS.Multi) total += 1;
                            sumPiece += total;
                        }

                    }
                    else
                    {
                        sumPiece += (int)(maxCardVal / 2.3);
                    }

                    sum += sumPiece * Vz;
                }
            }


            return sum;

        }
    }
}
