using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
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
                for (int j = 0; j < _pieces.GetLength(1); j++)
                {
                    copy[i, j] = _pieces[i, j].Copy();
                }
            }
            return copy;
        }

        public void MakeMove(Move move)
        {
            Position startSquare = move.startSquare;
            Position targetSquare = move.targetSquare;
            Board board = this;
            Piece piece = board[startSquare];
            board[startSquare] = null;
            Piece targetPiece = board[targetSquare];
            piece.hasMoved = true; //mighbt be not necassary
            if (targetPiece == null)
            {
                board[targetSquare] = piece;
                return;
            }
            Card currentCard = piece.Card;
            piece.Sleeve.isOpen = true;
            Card targetCard = targetPiece.Card;
            targetPiece.Sleeve.isOpen = true;

            if (targetCard is Landmine)
            {
                Landmine lm = (Landmine)targetCard;
                lm.Detonate(board, targetSquare);
                return; //sleves are not opened
            }
            if (currentCard is Landmine)
            {
                Landmine lm = (Landmine)currentCard;
                lm.Detonate(board, targetSquare);
                return; //sleves are not opened
            }

            IEnumerable<Piece> neighbours = board.GetNeighbourPieces(targetSquare).Where(x => x != null);
            Dictionary<Piece, Position> neighbours2 = board.GetNeighbourDict(targetSquare);
            IEnumerable<KeyValuePair<Piece, Position>> kvpNeigbours = neighbours2.Where(x => x.Key != null); //not necassary
            IEnumerable<Piece> neighbourPieces = kvpNeigbours.Select(x => x.Key);
            foreach (Piece p in neighbourPieces) { p.Sleeve.isOpen = true; }



            IEnumerable<Piece> lmsPieces = neighbourPieces.Where(x => x.Card is Landmine);

            foreach (Piece p in lmsPieces)
            {
                Position lmSquare = neighbours2[p];
                Landmine lm = p.Card as Landmine;
                lm.Detonate(board, lmSquare);
            }

            if (board[targetSquare] == null) { return; }

            neighbours = board.GetNeighbourPieces(targetSquare).Where(x => x != null);
            // EnumPlayer opponentColor = piece.Color == EnumPlayer.White ? EnumPlayer.Black : EnumPlayer.White;
            EnumPlayer opponentColor = piece.Color.Opponent();
            IEnumerable<Piece> currentNeighbours = neighbours.Where(x => x.Color == piece.Color);
            IEnumerable<Piece> opponentNeighbours = neighbours.Where(x => x.Color == opponentColor);

            int targetST = targetCard.ST;
            string info1 = $"{piece.FullName}\n{piece.Card}\nST={currentCard.ST}";
            string info2 = $"{targetPiece.FullName}\n{targetPiece.Card}\nST={targetCard.ST}";
            foreach (Piece opponentPiece in opponentNeighbours)
            {
                info2 += $"+{opponentPiece.Card.SP} /{opponentPiece.Card}\n";
                targetST += opponentPiece.Card.SP;
                opponentPiece.Sleeve.isOpen = true;
            }
            info2 += $"={targetST}";
            int currentST = currentCard.ST;
            foreach (Piece currentPiece in currentNeighbours)
            {
                info1 += $"+{currentPiece.Card.SP} /{currentPiece.Card}\n";
                currentST += currentPiece.Card.SP;
                currentPiece.Sleeve.isOpen = true;
            }
            info1 += $"={currentST}";
            if (Math.Abs(currentST - targetST) < 1)
            {
                info1 += $"{currentCard.RPS.ToString()}";
                info2 += $"{targetCard.RPS.ToString()}";
                //do Rock Paper Scizzor
                if (currentCard.RPS > targetCard.RPS)
                {
                    currentST += 1;
                    info1 += "+1";
                }
                else
                {
                    targetST += 1;
                    info2 += "+1";
                }
            }
            string info = info1 + "\n" + info2;
            MessageBox.Show(info, $"{piece.FullName} vs {targetPiece.FullName}");

            //if (currentST>targetST)
            //{
            //    piece.Sleeve.isOpen = true;
            //}
            //else
            //{
            //    targetPiece.Sleeve.isOpen = true;
            //}
            board[targetSquare] = currentST > targetST ? piece : targetPiece;

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


    }
}
