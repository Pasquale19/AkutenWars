using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AkutenWars
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            testImportJson();
          
        }

        void testImportJson()
        {
            string filePath = "Data/cards.json";
            var json = File.ReadAllText(filePath);
            // Deserialize to List<WarriorCard> and cast to IEnumerable<Card>
            var cards = JsonConvert.DeserializeObject<List<Card>>(json);
           foreach (var card in cards)
            {
                MessageBox.Show($"Name: {card.Name}, SP: {card.SP}, ST: {card.ST}, RPS: {card.RPS}", card.Name);
            }
            string json2 = JsonConvert.SerializeObject(cards, Formatting.Indented);

            // Write JSON string to the file
            File.WriteAllText("Data/cards2.json", json2);
            //var cards = CardLoader.LoadCardsFromJsonAsync(jsonFilePath);

            //foreach (var card in cards)
            //{
            //    MessageBox.Show($"Name: {card.Name}, SP: {card.SP}, ST: {card.ST}, RPS: {card.RPS}", card.name);
            //}
        }


       
    }
}
