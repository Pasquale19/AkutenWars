using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkutenWars
{
    public class Deck : ObservableCollection<Card>
    {
        public static Deck DefaultDeck()
        {
            Deck deck = new Deck();
            IEnumerable<Card> cards = Card.LoadCardsFromFile("Data/cards2.json");
            IEnumerable<Card> d2=deck.Concat(cards);
           return new Deck(d2);
            return deck;
        }

        public Deck() : base() { AddStartCards(); }

        public Deck(IEnumerable<Card> collection) : base(collection)
        {
            //AddStartCards();
        }

        void AddStartCards()
        {
            this.Add(new Landmine_D());
            this.Add(new Landmine_H());
        }
    }
}
