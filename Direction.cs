using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    [Serializable]
    public class Direction
    {

        public readonly static Direction North = new Direction(1, 0);
        public readonly static Direction NorthEast = new Direction(1, 1);
        public readonly static Direction East = new Direction(0, 1);
        public readonly static Direction SouthEast = new Direction(-1, 1);
        public readonly static Direction South = new Direction(-1, 0);
        public readonly static Direction SouthWest = new Direction(-1, -1);
        public readonly static Direction West = new Direction(0, -1);
        public readonly static Direction NorthWest = new Direction(1, -1);


        public int RowDelta{ get; }
        public int ColumnDelta { get; }


        public Direction(int rowDelta = 0, int columnDelta = 0)
        {
            this.RowDelta = rowDelta;
            this.ColumnDelta = columnDelta;
        }


        //public static operator +(Direction left, Direction right)
        //{
        //    Direction leftDirection = new Direction(left.RowDelta + right.RowDelta, left.ColumnDelta + right.ColumnDelta);
        //    return leftDirection;
        //}

        public override bool Equals(object obj)
        {
            if (obj is Direction)

            {
                Direction other = (Direction)obj;
                if (other.RowDelta == this.RowDelta && other.ColumnDelta == this.ColumnDelta) return true;
                return false;
            }
            return false;
        }
        public static Direction operator +(Direction a,Direction b)
        {
            return new Direction(a.RowDelta+b.RowDelta,a.ColumnDelta+b.ColumnDelta);
        }

        public static Direction operator *(int scalar, Direction b)
        {
            return new Direction(scalar* b.RowDelta,scalar* b.ColumnDelta);
        }

        public static Direction operator *(Direction b,int scalar )
        {
            return new Direction(scalar * b.RowDelta, scalar * b.ColumnDelta);
        }
        public static bool operator ==(Direction a, Direction b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Direction a, Direction b)
        {
            return a.Equals(b);
        }

        public override string ToString()
        {
            return $"Dir=[{RowDelta} {ColumnDelta}]";
        }

    }
}
