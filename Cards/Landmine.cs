using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AkutenWars
{
    public class Landmine : Card
    {
        public Landmine() { Name = "Landmine"; }
        protected virtual Direction[] Direction => new Direction[]
        {
            AkutenWars.Direction.North, AkutenWars.Direction.South, AkutenWars.Direction.East
        };
        public override string ToString()
        {
            return $"Card: {GetType().Name}";
        }

        public virtual void Detonate(Board board, Position pos)
        {
            Direction[] dirs = Direction;
            foreach (Direction dir in dirs)
            {
                Position p2 = pos + dir;
                if (board.IsInside(p2))
                {
                    board[p2] = null;
                }
            }
            board[pos] = null;
        }

        public virtual string imagePath => "pack://application:,,,/Assets/Landmine.png";

        public virtual BitmapImage Image => new BitmapImage(new Uri(imagePath, UriKind.Absolute));
    }

    /// <summary>
    /// horizontal Landmine Card
    /// </summary>
    public class Landmine_H : Landmine
    {
        public Landmine_H() { Name = "Landmine-H"; }
        public override string imagePath => "pack://application:,,,/Assets/Landmine_H.png";

    }

    /// <summary>
    /// diagonal Landmine
    /// </summary>
    public class Landmine_D : Landmine
    {
        public Landmine_D() { Name = "Landmine-D"; }
        protected override Direction[] Direction => new Direction[]
        {
            AkutenWars.Direction.NorthEast,AkutenWars.Direction.NorthWest ,AkutenWars.Direction.SouthEast, AkutenWars.Direction.SouthWest
        };
        public override string imagePath => "pack://application:,,,/Assets/Landmine_D.png";
    }
}
