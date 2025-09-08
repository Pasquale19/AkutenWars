using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AkutenWars
{


    public class RpsJsonConverter : JsonConverter<RPS>
    {
        public override void WriteJson(JsonWriter writer, RPS value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
        public override RPS ReadJson(JsonReader reader, Type objectType, RPS existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            string input = s;
            RPSEnum rpsValue;
            if (!Enum.TryParse(input, ignoreCase: true, out rpsValue))
            {
                rpsValue = RPSEnum.Scissor;
            }
            return new RPS(rpsValue);
        }
    }


    public class Piece2DArrayConverter : JsonConverter<Piece[,]>
    {
        public override void WriteJson(JsonWriter writer, Piece[,] value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            int rows = value.GetLength(0);
            int cols = value.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                writer.WriteStartArray();
                for (int j = 0; j < cols; j++)
                {
                    serializer.Serialize(writer, value[i, j]);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }

        public override Piece[,] ReadJson(JsonReader reader, Type objectType, Piece[,] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray outerArray = JArray.Load(reader);
            int rows = outerArray.Count;
            if (rows == 0)
                return new Piece[0, 0];

            int cols = outerArray[0].Count();

            Piece[,] result = new Piece[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                JArray innerArray = (JArray)outerArray[i];
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = innerArray[j].ToObject<Piece>(serializer);
                }
            }
            return result;
        }
    }
}
