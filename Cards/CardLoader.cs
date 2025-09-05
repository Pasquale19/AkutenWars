using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace AkutenWars
{
    public static class CardLoader
    {
        //public static async Task<IEnumerable<Card>> LoadCardsFromJsonAsync(string filePath)
        //{
        //    //// JsonSerializer options - enum as strings
        //    //var options = new JsonSerializerOptions
        //    //{
        //    //    PropertyNameCaseInsensitive = true,
        //    //    Converters =
        //    //    {
        //    //        new System.Text.Json.Serialization.JsonStringEnumConverter()
        //    //    }
        //    //};

        //    //IEnumerable<Card> cards;
        //    //using (FileStream fs = File.OpenRead(filePath))

        //    //{
        //    //   string json=  JsonConvert.SerializeObject(card);
        //    //    JsonConvert.DeserializeObject
        //    //    cards = await JsonSerializer.<IEnumerable<Card>>(fs, options);
        //    //}
        //    //            return cards;
        //}
    }
}
