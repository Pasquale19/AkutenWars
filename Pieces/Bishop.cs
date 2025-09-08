using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    [Serializable]
    public class Bishop : Piece
    {
        public Bishop(EnumPlayer player) : base(player)
        {
        }

        protected override Direction[] dirs => new Direction[]
        {
            Direction.NorthEast,
            Direction.SouthEast,
            Direction.SouthWest,
            Direction.NorthWest
        };
        protected override int scalar => 2;
        protected override IEnumerable<Position> MovePosInDir(Position from, Board board, Direction dir)
        {
            return base.MovePosInDir(from, board, dir, scalar);
        }


        public override Piece Copy()
        {
            throw new NotImplementedException();
        }
    }
}
