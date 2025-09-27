using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Windows;
using System.IO;
using AkutenWars.Cards;
using Newtonsoft.Json.Converters;

namespace AkutenWars
{


    [Serializable]
    public class Card : ObservableObject
    {
        public static Card GreatKingBearden=>new Card() { Name="Angel King Bearden", Rank=CardRank.GreatKing, ST=11, SP=3, RPS=RPS.Scissor};
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

        private CardRank _Rank = CardRank.Plebb;
        public virtual CardRank Rank
        {
            get => _Rank;
            set
            {
                _Rank = value;
               // Rarity = (int)value;
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void NotifyPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public override string ToString()
        {
            return $"{Name}: \t SP: {SP}\t ST: {ST}\t RPS: {RPS}\t Rank: {Rank}";
        }

        public static IEnumerable<Card> LoadCardsFromFile(string filePath = "Data/cards.json")
        {
            var json = File.ReadAllText(filePath);
            var settings = new JsonSerializerSettings  //adds a $type property to the serialized JSON with the full type assembly name
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new StringEnumConverter());
            
            IEnumerable<Card> cards = JsonConvert.DeserializeObject<IEnumerable<Card>>(json,settings);
            //foreach (var card in cards)
            //{
            //    MessageBox.Show($"Name: {card.Name}, SP: {card.SP}, ST: {card.ST}, RPS: {card.RPS}", card.Name);
            //}
            return cards;
        }

        public static void Export(IEnumerable<Card> cards,string filePath = "Data/cards.json")
        { 
            var settings = new JsonSerializerSettings  //adds a $type property to the serialized JSON with the full type assembly name
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            settings.Converters.Add(new StringEnumConverter());

            JsonConvert.SerializeObject(filePath, settings); 
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
        public static bool operator ==(Card left, Card right)
        {
            return EqualityComparer<Card>.Default.Equals(left, right);
        }
        public static bool operator !=(Card left, Card right)
        {
            return !(left == right);
        }
    }

    public class EmptyCard : Card
    {
        public EmptyCard() { this.Name = "Empty";}
    }
}

