using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    public class Knight : Piece
    {
        public Knight(EnumPlayer player) : base(player)
        {
        }

        protected override Direction[] dirs => new Direction[]
        {
            new Direction(-1,2)*VZ_direcion,
           new Direction(1,2)*VZ_direcion
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
