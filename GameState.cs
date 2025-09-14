using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AkutenWars.Utilities;

namespace AkutenWars
{
    [Serializable]
    public class GameState : ObservableObject
    {
        public GameState() : this(Board.InitialBoard()) { }

        public GameState(Board board)
        {
            this.Board = board;
        }

        public GameState(EnumPlayer player) : this(player, Board.InitialBoard()) { }
        public GameState(EnumPlayer player, Board board)
        {
            this.CurrentPlayer = player;
            this.Board = board;
        }

        private Board _board;
        public Board Board
        {
            get => _board;
            set { _board = value; }
        }
        private EnumPlayer _currentPlayer = EnumPlayer.White;
        public EnumPlayer CurrentPlayer
        {
            get => _currentPlayer;
            private set
            {
                if (value != _currentPlayer)
                {
                    _currentPlayer = value;
                    NotifyPropertyChanged(nameof(CurrentPlayer));
                }
            }
        }

        public static GameState RandomStart()
        {
            GameState game = new GameState(EnumPlayer.White, Board.InitialBoard());
            game.AddRandomCards();
            return game;
        }

        ObservableCollection<Card> _blackDeck = Deck.DefaultDeck();

        public ObservableCollection<Card> blackDeck
        {
            get { return _blackDeck; }
            set
            {
                if (_blackDeck != value)
                {
                    _blackDeck = value;
                    NotifyPropertyChanged(nameof(blackDeck));
                }
            }
        }

        ObservableCollection<Card> _whiteDeck = Deck.DefaultDeck();

        public ObservableCollection<Card> whiteDeck
        {
            get { return _whiteDeck; }
            set
            {
                if (_whiteDeck != value)
                {
                    _whiteDeck = value;
                    NotifyPropertyChanged(nameof(whiteDeck));
                }
            }
        }

        public IEnumerable<Move> LegalMovesForPiece(Position pos)
        {
            bool isInside = Board.IsInside(pos);
            Piece piece = Board[pos];
            if (piece == null) { return Enumerable.Empty<Move>(); }
            EnumPlayer color = Board[pos].Color;
            if (!isInside || color != CurrentPlayer)
            {
                //MessageBox.Show($"No legal moves available for {Board[pos]}");
                return Enumerable.Empty<Move>();
            }

            IEnumerable<Move> legalMoves = piece.GetMoves(pos, Board);
            return legalMoves;
        }

        public void MakeMove(Move move)
        {
            GameResult result = GameResult.OnGoing;
            MakeMove(move, out result);
        }


        public void switchPlayer()
        {
            this.CurrentPlayer = this.CurrentPlayer.Opponent();
        }
       

        public void MakeMove(Move move, out GameResult result)
        {
            this.Board.MakeMove(move);
          //  move.Execute(Board);
            if (CurrentPlayer == EnumPlayer.Black) { CurrentPlayer = EnumPlayer.White; }
            else { CurrentPlayer = EnumPlayer.Black; }

            IEnumerable<Piece> blackPieces = Board.GetPiece(EnumPlayer.Black);
            IEnumerable<Piece> whitePieces = Board.GetPiece(EnumPlayer.White);

            bool whiteKing = whitePieces.Any(x => x is King);
            bool blackKing = blackPieces.Any(y => y is King);

            string title = "GameResult";
            if (!whiteKing && !blackKing)
            {
                MessageBox.Show("Stalemate", title);
                result = GameResult.StaleMate;
            }

            if (!whiteKing)
            {
                MessageBox.Show("black won", title);
                result = GameResult.blackWon;
            }

            if (!blackKing)
            {
                MessageBox.Show("white won", title);
                result = GameResult.whiteWon;
            }
            result = GameResult.OnGoing;
        }

        public void undoMove()
        {

        }

        public void AddRandomCards()
        {
            IEnumerable<Piece> pieces = Board.pieces.Cast<Piece>().Where(x => x != null);
            IEnumerable<Piece> whitePieces = pieces.Where(x => x.Color == EnumPlayer.White);

            IEnumerable<Piece> blackPieces = pieces.Where(x => x.Color == EnumPlayer.Black);

            Deck whiteDeck = Deck.DefaultDeck();
            Deck blackDeck = Deck.DefaultDeck();

            foreach (Piece whitePiece in whitePieces)
            {
                Card card = whiteDeck.GetRandomElement();
                whitePiece.Sleeve = new Sleeve(card);
            }

            foreach (Piece blackPiece in blackPieces)
            {
                Card card = blackDeck.GetRandomElement();
                blackPiece.Sleeve = new Sleeve(card);
            }
        }
    }
}
