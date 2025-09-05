using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    public  class Sleeve
    {

        public Sleeve() {  }
        public Sleeve(Card card)
        {
            Card = card;
        }
        bool _isOpen = false;

        public bool isOpen { get { return _isOpen; } set { _isOpen = value; } }

        Card _card=new Card();


        public Card Card { get { return _card; } set { _card = value; } }


    }
}
