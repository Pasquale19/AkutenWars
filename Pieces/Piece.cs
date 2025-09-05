using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace AkutenWars
{

    public enum PieceType
    {
        None,
        Pawn,
        Bishop,
        Rook,
        Lance,
        Knight,
        Silver,
        Gold
    }
    public abstract class Piece : ObservableObject
    {
        public Piece(EnumPlayer player)
        {
            this.Color = player;
        }
        #region properties
        private Sleeve _sleeve=new Sleeve();
        public Sleeve Sleeve
        {
            get => _sleeve;
            set
            {
                if (value != _sleeve)
                {
                    _sleeve = value;
                    NotifyPropertyChanged(nameof(Sleeve));
                    NotifyPropertyChanged(nameof(Card));
                }
            }
        }

        public Card Card => Sleeve.Card;
        public bool SleeveOpen => Sleeve.isOpen;

        public virtual PieceType Type { get; set; }
        public virtual string Name => GetType().Name;
        public virtual string FullName => $"{Color.ToString()} {GetType().Name}";

        public bool hasMoved { get; set; } = false;
        public EnumPlayer Color { get; }

        protected virtual int scalar => 1; //scalar for len

        protected virtual Direction[] dirs => new Direction[]
        {
            Direction.North,Direction.South,Direction.East
        };

        protected int VZ_direcion => (int)Math.Pow(-1, (int)Color);

        #endregion
        public abstract Piece Copy();



        public virtual IEnumerable<NormalMove> GetMoves(Position from, Board board)
        {
            IEnumerable<Position> moves = MovePosInDir(from, board, this.dirs, scalar);
            //Console.WriteLine($"legalMoves {this.Name} {moves.Count()}");
            // MessageBox.Show($"legalMoves {this.Name} {moves.Count()}", this.Name);
            IEnumerable<NormalMove> moves2 = moves.Select(to => new NormalMove(from, to));
            foreach (NormalMove move in moves2)
            {
                yield return move;
            }

            yield break;
            //foreach (Direction d in Direction)
            //{
            //    IEnumerable<Position> ps = MovePosInDir(from, board, d, this.scalar);
            //    foreach (Position p in ps)
            //    {
            //        yield return p;
            //    }
            //    //posis.Append(ps.ToList());
            //}
        }
        protected abstract IEnumerable<Position> MovePosInDir(Position from, Board board, Direction dir);

        protected virtual IEnumerable<Position> MovePosInDir(Position from, Board board, Direction dir, int scalar)
        {
            int scal = 1;
            for (Position pos = from + dir; board.IsInside(pos) && scal <= scalar; pos += dir)
            {

                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    scal++;
                    continue;
                }

                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;
                    yield break;
                }
                scal++;
                if (scal >= scalar)
                {
                    yield break;
                }

            }

        }
        protected IEnumerable<Position> MovePosInDir(Position from, Board board, Direction[] dirs, int scalar)
        {
            return dirs.SelectMany(dir => MovePosInDir(from, board, dir, scalar));
        }
        public override string ToString()
        {
            string info = $"{Color.ToString()} {this.Name}";

            if (Sleeve.isOpen)
            {
                info += $"\nCard: {Sleeve.Card.ToString()}";
            }
            return info;
        }

    }


}
