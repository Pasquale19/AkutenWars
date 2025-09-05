using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Windows;
using System.IO;

namespace AkutenWars
{


    public class Card : INotifyPropertyChanged
    {
        private int _SP = 0;
        private int _ST = 0;
        private RPS _RPS = RPS.Scissor;
        private string _Name = "";

        public int SP
        {
            get => _SP;
            set
            {
                if (_SP != value)
                {
                    _SP = value;
                    OnPropertyChanged(nameof(SP));
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
                    OnPropertyChanged(nameof(ST));
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
                    OnPropertyChanged(nameof(RPS));
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
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"Card{Name}\tSP:{SP}\tST:{ST}\tRPS:{RPS}";
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
    }
}

