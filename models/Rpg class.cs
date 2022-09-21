using System.Text.Json.Serialization;

namespace dotnet_rpg.models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Rpgclass
    {
        knight = 1,
        mage =2,
        cleric  =3,
    }
}