using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    [Serializable]
    public class Lance : Piece
    {
        public Lance(EnumPlayer player) : base(player)
        {
        }
        protected override int scalar => 2;
        protected override Direction[] dirs
        {
            get
            {
                Direction[] dirs = new Direction[]
                {
                    Direction.North*VZ_direcion
                };
                return dirs;    
            }
        }

        

        

        public override Piece Copy()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Position> MovePosInDir(Position from, Board board, Direction dir)
        {
            return base.MovePosInDir(from, board, dir,scalar);
        }
    }
}
