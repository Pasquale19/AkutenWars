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
        Move _lastMove;
        static EnumPlayer[] players => new EnumPlayer[] { EnumPlayer.White, EnumPlayer.Black };
        public GameState() : this(Board.InitialBoard()) { }

        public GameState(Board board) : this(players.GetRandomElement(), board)
        { }

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
        public GameMode Mode { get; set; } = GameMode.MultiPlayer;
        public static GameState RandomStart()
        {
            GameState game = new GameState(EnumPlayer.White, Board.InitialBoard());
            game.AddRandomCards();
            return game;
        }

        ObservableCollection<Card> _blackDeck = Deck.StartDeck();

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

        ObservableCollection<Card> _whiteDeck = Deck.StartDeck();

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


        /// <summary>
        /// applys the move and switches Player
        /// </summary>
        /// <param name="move"></param>
        /// <param name="result"></param>
        public void MakeMove(Move move, out GameResult result)
        {
            this.Board.MakeMove(move);
            _lastMove = move;
            //  move.Execute(Board);
            CurrentPlayer = CurrentPlayer.Opponent();

            result = AkutenWars.Board.Result(Board);
        }

        public void undoMove()
        {
            //if only a piece is moved it doubles the Piece
            if (_lastMove != null)
            {
                _lastMove.Unmake(Board);
            }
            _lastMove = null;
        }

        public void AddRandomCards()
        {
            AddRandomCards(EnumPlayer.White);
            AddRandomCards(EnumPlayer.Black);
            //IEnumerable<Piece> pieces = Board.pieces.Cast<Piece>().Where(x => x != null);
            //IEnumerable<Piece> whitePieces = pieces.Where(x => x.Color == EnumPlayer.White);

            //IEnumerable<Piece> blackPieces = pieces.Where(x => x.Color == EnumPlayer.Black);

            ////Deck deck = Deck.StartDeck();
            ////Deck blackDeck = Deck.StartDeck();


            //foreach (Piece whitePiece in whitePieces)
            //{
            //    Card card = whiteDeck.PopRandomElement();
            //    whitePiece.Sleeve = new Sleeve(card);
            //}

            //foreach (Piece blackPiece in blackPieces)
            //{
            //    Card card = blackDeck.PopRandomElement();
            //    blackPiece.Sleeve = new Sleeve(card);
            //}

        }

        public void AddRandomCards(EnumPlayer player)
        {
            if (player==EnumPlayer.White)
            {
                AddRandomCards(EnumPlayer.White,whiteDeck);
            }
            if (player == EnumPlayer.Black)
            {
                AddRandomCards(EnumPlayer.Black, blackDeck);
            }
        }

        public void AddRandomCards(EnumPlayer player,ObservableCollection<Card> deck)
        {
            IEnumerable<Piece> pieces = Board.pieces.Cast<Piece>().Where(x => x != null && x.Color==player);
            IEnumerable<Piece> whitePieces = pieces.Where(x => x.Color == player);

            foreach (Piece piece in whitePieces)
            {
                Card card = deck.PopRandomElement();
                piece.Sleeve = new Sleeve(card);
            }
        }
    }
}
