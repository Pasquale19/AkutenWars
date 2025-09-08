using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    [Serializable]
    public class GoldGeneral : Piece
    {
        public GoldGeneral(EnumPlayer player) : base(player)
        {
        }

        protected override Direction[] dirs => new Direction[]
        {
            Direction.North*VZ_direcion,
            Direction.NorthEast*VZ_direcion,
            Direction.East,
            Direction.South * VZ_direcion,
            Direction.West,
            Direction.NorthWest * VZ_direcion
        };
        protected override int scalar =>1;
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
