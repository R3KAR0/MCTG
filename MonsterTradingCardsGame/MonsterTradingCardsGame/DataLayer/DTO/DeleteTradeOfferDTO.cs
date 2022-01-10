using System.Text.Json.Serialization;

namespace MonsterTradingCardsGame.DataLayer.DTO
{
    internal class DeleteTradeOfferDTO
    {
        public DeleteTradeOfferDTO(Guid cardId)
        {
            CardId = cardId;
        }

        [JsonPropertyName("cardid")]
        public Guid CardId { get; private set; }
    }
}
