using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    [Serializable]
    public class Pawn : Piece
    {
        public Pawn(EnumPlayer player=EnumPlayer.None) : base(player)
        {

        }
        public Pawn(EnumPlayer player, Sleeve sleeve) : base(player, sleeve)
        {
        }

        protected override Direction[] dirs
        {
            get
            {
                return new Direction[]{
            Direction.North*VZ_direcion };
            }

        }

        protected override int scalar => 1;

        public Direction forward => Direction.North * VZ_direcion;

        private static bool CanMoveTo(Position p, Board board)
        {
            return (board.IsEmpty(p) && board.IsInside(p));
        }

        public override PieceType Type => PieceType.Pawn;

        public override string Name => "Pawn";

        public override Piece Copy()
        {
            Pawn copy = new Pawn(this.Color,this.Sleeve);
            copy.hasMoved = true;
            return copy;
        }



        private IEnumerable<Move> ForwardMoves(Position from, Board board)
        {
            Position oneMovePos = from + forward;
            if (CanMoveTo(oneMovePos, board))
            {
                yield return new Move(from, oneMovePos);
                Position twoMovesPos = oneMovePos + forward;
                if (!this.hasMoved && CanMoveTo(twoMovesPos, board))
                {
                    yield return new Move(from, twoMovesPos);
                }
            }

        }

        protected override IEnumerable<Position> MovePosInDir(Position from, Board board, Direction dir)
        {
            return base.MovePosInDir(from, board, dir, scalar);
        }
    }
}
