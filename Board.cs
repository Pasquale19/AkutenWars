using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using AkutenWars.Cards;
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

        //public  bool IsInside(Position pos)
        //{
        //    bool inside = pos.Row >= 0 && pos.Column >= 0 && pos.Row < _pieces.GetLength(0) && pos.Column < _pieces.GetLength(1);
        //    return inside;
        //}
        public static bool IsInside(Position pos)
        {
            bool inside = pos.Row >= 0 && pos.Column >= 0 && pos.Row < 9 && pos.Column <9;
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
                for (int j = 0; j < _pieces.GetLength(1); j++)
                {
                    Piece p = _pieces[i, j];
                    if (p == null) continue;
                    copy[i, j] = p.Copy();
                }
            }
            return copy;
        }

        bool isGameOver()
        {
            throw new NotImplementedException();
        }
        public void MakeMove(Move move)
        {
            move.Execute(this); return;
        }

        public IEnumerable<Move> GenerateMoves(EnumPlayer player)
        {
            int rows = this.pieces.GetLength(0);
            int cols = this.pieces.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Piece piece = this.pieces[r, c];
                    if (piece == null || piece.Color != player) continue;
                    Position position = new Position(r, c);
                    IEnumerable<Move> moves = piece.GetMoves(position, this);
                    foreach (Move move in moves) { yield return move; }
                }
            }
        }


        /// <summary>
        /// returns a dictionairy of Piece and Position -> Pieces==null are excluded
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
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
        /// <summary>
        /// returns all pieces that are not null
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Piece> GetPieces()
        {

            foreach (Piece piece in pieces)
            {
                if (piece == null) continue;
                yield return piece;
            }
            yield break;
        }
        public IEnumerable<Piece> GetPiece(EnumPlayer Color)
        {

            foreach (Piece piece in pieces)
            {
                if (piece == null) continue;
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
        }

        public static bool checkDeck(IEnumerable<Card> cards, out string errorMsg)
        {
            errorMsg = "";
            int anzEmpty = cards.OfType<EmptyCard>().Count();
            if (anzEmpty > 0)
            {
                errorMsg += $"{anzEmpty} empty Cards found\n";
            }
            var groupedCards = cards.GroupBy(card => card.Rank);



            Dictionary<CardRank, int> limits = new Dictionary<CardRank, int>()
            {
                { CardRank.GreatKing, 1 },
                { CardRank.King, 2 },
                 { CardRank.Gen, 4 },
                {CardRank.Plebb,100 },
                { CardRank.Landmine,1 }
            };

            foreach (var group in groupedCards)
            {
                int anz = group.Count();
                int limit = limits[group.Key];
                if (anz > limit)
                {
                    errorMsg += $"{group.Key} exceeds card limit of {limit}\t {anz}/{limit}\n";
                }
            }
            return errorMsg == "";
        }

        public bool checkCards(out string errorMsg)
        {
            string fullerorLog = $"";
            errorMsg = String.Empty;
            string errorWhite = "";
            bool whiteDeckisValid = checkCards(EnumPlayer.White, out errorWhite);
            string errorBlack = "";
            bool blackDeckisValid = checkCards(EnumPlayer.White, out errorBlack);
            if (!whiteDeckisValid)
            {
                fullerorLog += $"white Cards are invalid\n" + errorWhite + "\n\n";
            }

            if (!blackDeckisValid)
            {
                fullerorLog += $"black Cards are invalid\n" + errorBlack;
            }
            errorMsg = fullerorLog;
            return whiteDeckisValid && blackDeckisValid;
        }
        public bool checkCards(EnumPlayer player, out string errorMsg)
        {
            IEnumerable<Piece> pieces = GetPiece(player);
            IEnumerable<Card> cards = pieces.Select(x => x.Card);
            return checkDeck(cards, out errorMsg);
        }

        public GameResult Result() => Result(this);

        public static GameResult Result(Board board)
        {
            GameResult result = GameResult.OnGoing;
            IEnumerable<Piece> blackPieces = board.GetPiece(EnumPlayer.Black);
            IEnumerable<Piece> whitePieces = board.GetPiece(EnumPlayer.White);

            bool whiteKing = whitePieces.Any(x => x is King);
            bool blackKing = blackPieces.Any(y => y is King);

            string title = "GameResult";
            if (!whiteKing && !blackKing)
            {
                // MessageBox.Show("Stalemate", title);
                result = GameResult.StaleMate;
            }

            if (!whiteKing)
            {
                //  MessageBox.Show("black won", title);
                result = GameResult.blackWon;
            }

            if (!blackKing)
            {
                //   MessageBox.Show("white won", title);
                result = GameResult.whiteWon;
            }
            return result;
        }
        public void importBinary(string filePath)
        {
            Piece[,] pieces = new Piece[9, 9];
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                pieces = (Piece[,])formatter.Deserialize(stream);
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    this.pieces[i, j] = pieces[i, j];
                }
            }
        }
        public void saveBinary(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this.pieces);
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != this.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;


            var other = (Board)obj;
            int rows = 9;// this.pieces.GetLength(0)
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Piece p1 = this[i, j];
                    Piece p2 = other[i, j];
                    if (p1 != p2)
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        public static bool operator ==(Board left, Board right)
        {
            return EqualityComparer<Board>.Default.Equals(left, right);
        }
        public static bool operator !=(Board left, Board right)
        {
            return !(left == right);
        }

    }
}
