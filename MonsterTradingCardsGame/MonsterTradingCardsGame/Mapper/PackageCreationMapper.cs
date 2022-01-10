using MonsterTradingCardsGame.Models;
using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.Mapper
{
    public class PackageCreationMapper
    {
        [JsonPropertyName("kindChances")]
        public Dictionary<EKind, int> KindChances { get; private set; } = new Dictionary<EKind, int>();

        [JsonPropertyName("elementChances")]
        public Dictionary<EElement, int> ElementChances { get; private set; } = new Dictionary<EElement, int>();

        [JsonPropertyName("standardPackage_prize")]
        public int PackagePrize { get; private set; }
        [JsonPropertyName("standardPackage_size")]
        public int PackageSize { get; private set; }
        [JsonPropertyName("standardPackage_description")]
        public string PackageDescription { get; private set; }

        public PackageCreationMapper(Dictionary<EKind, int> kindChances, Dictionary<EElement, int> elementChances, int packagePrize, int packageSize, string packageDescription)
        {
            KindChances = kindChances ?? throw new ArgumentNullException(nameof(kindChances));
            ElementChances = elementChances ?? throw new ArgumentNullException(nameof(elementChances));
            PackagePrize = packagePrize;
            PackageSize = packageSize;
            PackageDescription = packageDescription ?? throw new ArgumentNullException(nameof(packageDescription));
        }
    }
}