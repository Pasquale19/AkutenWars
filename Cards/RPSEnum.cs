using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace AkutenWars
{
    public enum RPSEnum
    {
        Rock,
        Paper,
        Scissor,
        Multi
    }
    [JsonConverter(typeof(RpsJsonConverter))]
    public struct RPS
    {
        public static readonly RPS Scissor = new RPS(RPSEnum.Scissor);
        public static readonly RPS Rock = new RPS(RPSEnum.Rock);
        public static readonly RPS Paper = new RPS(RPSEnum.Paper);
        public static readonly RPS Multi = new RPS(RPSEnum.Multi);
        public RPSEnum Value { get; }

        public RPS(RPSEnum value)
        {
            Value = value;
        }

        public static bool operator >(RPS left, RPS right)
        {
            return Beats(left.Value, right.Value);
        }

        public static bool operator <(RPS left, RPS right)
        {
            return Beats(right.Value, left.Value);
        }

        private static bool Beats(RPSEnum a, RPSEnum b)
        {
            if (a == RPSEnum.Multi) return b != RPSEnum.Multi; // Multi beats all except Multi
            if (b == RPSEnum.Multi) return false;

            return (a == RPSEnum.Rock && b == RPSEnum.Scissor) ||
                   (a == RPSEnum.Scissor && b == RPSEnum.Paper) ||
                   (a == RPSEnum.Paper && b == RPSEnum.Rock);
        }
        public static bool operator ==(RPS left, RPS right)
        {
            return left.Value == right.Value;
        }
        public static bool operator !=(RPS left, RPS right)
        {
            return left.Value != right.Value;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is RPS)) return false;
            RPS rPS = (RPS)obj;
            return rPS.Value!=this.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Value);
        }

        public override string ToString() => Value.ToString();
    }
}
