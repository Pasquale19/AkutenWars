using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace AkutenWars.Cards
{

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum CardRank
    {
        Plebb=5,
        Gen=4, //General
        King=2,
        GreatKing=1,
        Landmine=-1
    }
}
