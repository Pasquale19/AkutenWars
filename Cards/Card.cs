using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Windows;
using System.IO;
using AkutenWars.Cards;

namespace AkutenWars
{


    [Serializable]
    public class Card : ObservableObject
    {
        private int _SP = 0;
        private int _ST = 0;
        private RPS _RPS = RPS.Scissor;
        private string _Name = "";
        private int _Rarity = 4;

        #region Konstruktor
        public Card() { }
        public Card(string Name)
        {
            _Name = Name;
        }
        #endregion

        public int SP
        {
            get => _SP;
            set
            {
                if (_SP != value)
                {
                    _SP = value;
                    NotifyPropertyChanged(nameof(SP));
                }
            }
        }

        public int ST
        {
            get => _ST;
            set
            {
                if (_ST != value)
                {
                    _ST = value;
                    NotifyPropertyChanged(nameof(ST));
                }
            }
        }

        [JsonConverter(typeof(RpsJsonConverter))]
        public RPS RPS
        {
            get => _RPS;
            set
            {
                if (_RPS != value)
                {
                    _RPS = value;
                    NotifyPropertyChanged(nameof(RPS));
                }
            }
        }

        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public int Rarity
        {
            get => _Rarity;
            set { if (_Rarity != value) { _Rarity = value; NotifyPropertyChanged(nameof(Rarity)); } }
        }

        public CardRank Rank
        {
            get => (CardRank)Rarity;
            set
            {
                Rarity = (int)value;
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void NotifyPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public override string ToString()
        {
            return $"Card{Name}: \t SP:{SP}\t ST:{ST}\t RPS:{RPS}";
        }

        public static IEnumerable<Card> LoadCardsFromFile(string filePath = "Data/cards.json")
        {
            var json = File.ReadAllText(filePath);
            List<Card> cards = JsonConvert.DeserializeObject<List<Card>>(json);
            //foreach (var card in cards)
            //{
            //    MessageBox.Show($"Name: {card.Name}, SP: {card.SP}, ST: {card.ST}, RPS: {card.RPS}", card.Name);
            //}
            return cards;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || obj.GetType() != this.GetType())
                return false;
            if (ReferenceEquals(this, obj))
                return true;


            var other = (Card)obj;
            return string.Equals(Name, other.Name) &&
                   SP == other.SP &&
                   ST == other.ST &&
                   RPS == other.RPS &&
                   Rarity == other.Rarity;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, SP, ST, RPS, Rarity);
        }
    }

    public class EmptyCard : Card
    {
        public EmptyCard() { this.Name = "Empty";}
    }
}

