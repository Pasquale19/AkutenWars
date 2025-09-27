using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public Sleeve Copy()
        {
            return new Sleeve(Card) { isOpen = this.isOpen };
        }

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != this.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;


            var other = (Sleeve)obj;
            return other.isOpen==this.isOpen && other.Card==this.Card;
        }
        public static bool operator ==(Sleeve left, Sleeve right)
        {
            return EqualityComparer<Sleeve>.Default.Equals(left, right);
        }
        public static bool operator !=(Sleeve left, Sleeve right)
        {
            return !(left == right);
        }


    }
}
