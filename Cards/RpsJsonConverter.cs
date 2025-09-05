using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

}
