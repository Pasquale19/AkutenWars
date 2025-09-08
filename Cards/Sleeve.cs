using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    [Serializable]
    public class Sleeve : ObservableObject
    {

        public Sleeve() { }
        public Sleeve(Card card)
        {
            Card = card;
        }
        bool _isOpen = false;

        public bool isOpen
        {
            get { return _isOpen; }
            set
            {
                if (_isOpen != value)
                {
                    _isOpen = value;
                    NotifyPropertyChanged(nameof(isOpen));
                }
            }
        }

        Card _card = new EmptyCard();


        public Card Card
        {
            get { return _card; }
            set
            {
                if (_card != value)
                {
                    _card = value;
                    NotifyPropertyChanged(nameof(Card));
                }
            }
        }


    }
}
