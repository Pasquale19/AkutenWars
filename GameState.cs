using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AkutenWars.Utilities;

namespace AkutenWars
{
    public class GameState : ObservableObject
    {
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

            move.Execute(Board);
            if (CurrentPlayer == EnumPlayer.Black) { CurrentPlayer = EnumPlayer.White; }
            else { CurrentPlayer = EnumPlayer.Black; }
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
