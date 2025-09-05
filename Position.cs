using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public IEnumerable<Position> GetNeighborPositions(int boardSize = 9)
        {
            Position pos = this;
          
            int[] dRows = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dCols = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newRow = pos.Row + dRows[i];
                int newCol = pos.Column + dCols[i];
                // Check board bounds
                if (newRow >= 0 && newRow < boardSize && newCol >= 0 && newCol < boardSize)
                {
                    yield return new Position(newRow, newCol);
                }
            }
        }

        public override string ToString()
        {
            return $"Pos {Row} - {Column}";
        }

        #region operator overloading
        public override bool Equals(object obj)
        {
            if (obj is Position)

            {
                Position other = (Position)obj;
                if (other.Row == this.Row && other.Column == this.Column) return true;
                return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = HashCode.Combine(Row, Column);
            return hashCode;
        }



        public static bool operator ==(Position left, Position right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }

        public static Position operator +(Position left, Position right)
        {
            return new Position(left.Row+right.Row, left.Column+right.Column);
        }
        public static Position operator +(Position p, Direction d)
        {
            return new Position(p.Row + d.RowDelta, p.Column + d.ColumnDelta);
        }

        public static Position operator +(Direction d,Position p )
        {
            return new Position(p.Row + d.RowDelta, p.Column + d.ColumnDelta);
        }

        #endregion
    }
}
