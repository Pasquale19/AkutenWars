using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    public class Rook : Piece
    {
        public Rook(EnumPlayer player) : base(player)
        {
        }
        protected override int scalar =>2;
        protected override Direction[] dirs
        {
            get
            {
                Direction[] dirs = new Direction[]
                {
                    Direction.North, Direction.South,Direction.East,Direction.West,
                };
                return dirs;    
            }
        }

        

        public override IEnumerable<NormalMove> GetMoves(Position from, Board board)
        {
            return base.GetMoves(from, board);
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
