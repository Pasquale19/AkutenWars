using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using Newtonsoft.Json;

namespace AkutenWars
{
    [Serializable]
    public class Board
    {

        [JsonConverter(typeof(Piece2DArrayConverter))]
        private readonly Piece[,] _pieces = new Piece[9, 9];

        public Piece[,] pieces
        {
            get
            {
                return _pieces;
            }

        }

        public Piece this[int row, int col]
        {
            get { return _pieces[row, col]; }
            set { _pieces[row, col] = value; }
        }

        public Piece this[Position p]
        {
            get { return _pieces[p.Row, p.Column]; }
            set { _pieces[p.Row, p.Column] = value; }
        }

        public static Board InitialBoard()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        public void AddStartPieces()
        {
            int row1 = 2; //remember row starts at 0
            int rows = _pieces.GetLength(0);
            int cols = _pieces.GetLength(1);

            for (int i = 0; i < cols; i++)
            {
                this[row1, i] = new Pawn(EnumPlayer.White);
                this[rows - row1 - 1, i] = new Pawn(EnumPlayer.Black);
            }
            int index = 2;
            this[1, 1] = new Bishop(EnumPlayer.White);
            this[rows - 2, cols - 2] = new Bishop(EnumPlayer.Black);
            this[1, cols - 2] = new Rook(EnumPlayer.White);
            this[rows - 2, 1] = new Rook(EnumPlayer.Black);


            this[0, 0] = new Lance(EnumPlayer.White);
            this[0, cols - 1] = new Lance(EnumPlayer.White);
            this[rows - 1, 0] = new Lance(EnumPlayer.Black);
            this[rows - 1, cols - 1] = new Lance(EnumPlayer.Black);

            this[0, 1] = new Knight(EnumPlayer.White);
            this[0, cols - 2] = new Knight(EnumPlayer.White);
            this[rows - 1, 1] = new Knight(EnumPlayer.Black);
            this[rows - 1, cols - 2] = new Knight(EnumPlayer.Black);


            this[0, index] = new SilverGeneral(EnumPlayer.White);
            this[0, cols - index - 1] = new SilverGeneral(EnumPlayer.White);
            this[rows - 1, index] = new SilverGeneral(EnumPlayer.Black);
            this[rows - 1, cols - index - 1] = new SilverGeneral(EnumPlayer.Black);

            index = 3;
            this[0, index] = new GoldGeneral(EnumPlayer.White);
            this[0, cols - index - 1] = new GoldGeneral(EnumPlayer.White);
            this[rows - 1, index] = new GoldGeneral(EnumPlayer.Black);
            this[rows - 1, cols - index - 1] = new GoldGeneral(EnumPlayer.Black);

            this[0, 4] = new King(EnumPlayer.White);
            this[rows - 1, 4] = new King(EnumPlayer.Black);
        }

        public bool IsInside(Position pos)
        {
            bool inside = pos.Row >= 0 && pos.Column >= 0 && pos.Row < _pieces.GetLength(0) && pos.Column < _pieces.GetLength(1);
            return inside;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public Board Copy()
        {
            Board copy = new Board();
            for (int i = 0; i < _pieces.GetLength(0); i++)
            {
                for (int j = 0; j < _pieces.GetLength(0); j++)
                {
                    copy[i, j] = _pieces[i, j].Copy();
                }
            }
            return copy;
        }
        public Dictionary<Piece, Position> GetNeighbourDict(Position pos)
        {
            Dictionary<Piece, Position> dic = new Dictionary<Piece, Position>();
            int boardSize = this.pieces.GetLength(0);
            //int[] dRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            //int[] dCols = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dRows = { 0, 1, 0, -1 };
            int[] dCols = { 1, 0, -1, 0 };

            for (int i = 0; i < dRows.GetLength(0); i++)
            {
                int newRow = pos.Row + dRows[i];
                int newCol = pos.Column + dCols[i];
                // Check board bounds
                if (newRow >= 0 && newRow < boardSize && newCol >= 0 && newCol < boardSize)
                {
                    Position newPos = new Position(newRow, newCol);
                    Piece piece = pieces[newRow, newCol];
                    if (piece != null) { dic[piece] = newPos; }

                }
            }
            return dic;
        }

        public IEnumerable<Piece> GetNeighbourPieces(Position pos)
        {
            int boardSize = this.pieces.GetLength(0);
            //int[] dRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            //int[] dCols = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] dRows = { 0, 1, 0, -1 };
            int[] dCols = { 1, 0, -1, 0 };

            for (int i = 0; i < dRows.GetLength(0); i++)
            {
                int newRow = pos.Row + dRows[i];
                int newCol = pos.Column + dCols[i];
                // Check board bounds
                if (newRow >= 0 && newRow < boardSize && newCol >= 0 && newCol < boardSize)
                {
                    Position newPos = new Position(newRow, newCol);
                    yield return pieces[newRow, newCol];
                }
            }
        }

        public IEnumerable<Piece> GetNeighbourPieces(Position pos, EnumPlayer Color)
        {
            return GetNeighbourPieces(pos).Where(x => x.Color == Color);
        }
        public IEnumerable<Piece> GetPiece(EnumPlayer Color)
        {
            
            foreach (Piece piece in pieces)
            {
                if (piece==null) continue;
                if (piece.Color == Color) yield return piece;
            }
            yield break;
        }
        //check if all selectedCardAreValid
        public bool isValid(EnumPlayer player)
        {
            IEnumerable<Piece> pieces = GetPiece(player);
            IEnumerable<Card> Cards = pieces.Select(x =>
            {
                return x.Sleeve.Card;
            }).OrderBy(card => card.Name);
            string error = "";
            return checkDeck(Cards, out error);
            //IEnumerable<Piece> whitePieces = GetPiece(EnumPlayer.White);
            //IEnumerable<Piece> blackPieces = GetPiece(EnumPlayer.Black);

            //IEnumerable<Card> whiteCards = whitePieces.Select(x =>
            //{
            //    return x.Sleeve.Card;
            //}).OrderBy(card=>card.Name);
            //IEnumerable<Card> blackCards = blackPieces.Select(x =>
            //{
            //    return x.Sleeve.Card;
            //}).OrderBy(card=>card.Name);


            //if (player == EnumPlayer.None)
            //{
            //    string errorW = "";
            //    string errorB = "";
            //    bool W = checkDeck(whiteCards,out errorW);
            //    bool B = checkDeck(blackCards, out errorB);
            //    return W && B;
            //}

            //if (player == EnumPlayer.Black)
            //{

            //}

        }

        bool checkDeck(IEnumerable<Card> cards, out string errorMsg)
        {
            errorMsg = "";
           // IEnumerable<Landmine> lms = cards.Select(x => { if (x is Landmine) return x as Landmine; } );
            foreach (Card card in cards)
            {
                int Limit = card.Rarity;
                int ct = cards.Select(x => card.Name == x.Name).Count();
                if (ct > Limit)
                {
                    errorMsg += $"{card.Name} exceeds Limit  {ct}/{card.Rarity} (Rank:{card.Rank})\n";
                    return false;
                }
            }
            return true;
        }
    }
}