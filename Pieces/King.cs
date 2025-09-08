using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    [Serializable]
    public class King : Piece
    {
        public King(EnumPlayer player) : base(player)
        {
        }

        protected override Direction[] dirs => new Direction[]
        {
            Direction.North,Direction.NorthEast,
            Direction.East,Direction.SouthEast,
            Direction.South,Direction.SouthWest,
            Direction.West,Direction.NorthWest
        };
        protected override int scalar => 1;

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
