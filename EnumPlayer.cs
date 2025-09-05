using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AkutenWars
{
    public enum EnumPlayer
    {
        White=0,
        Black=1,
        None=-1
    }

    public static class PlayerExtension
    {
        public static EnumPlayer Opponent(this EnumPlayer player)
        {
            if (player == EnumPlayer.None)
            {
               // return EnumPlayer.None;
                throw new Exception("player must != 0");
            }
            if (player == EnumPlayer.White)
            {

                return EnumPlayer.Black;
            }
            else
            {
                return EnumPlayer.White;
            }
        }
    }
}
