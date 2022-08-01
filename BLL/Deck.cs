using Godot;
using System.Collections.Generic;

namespace BLL
{
    public class Deck
    {
        public List<DTO.Card> PlanarDeck { get; set; }

        public Deck()
        {
            PlanarDeck = new List<DTO.Card>();
        }
        private void LoadDeck()
        {
            PlanarDeck = LoadCards(CardList.Plane, DTO.Type.Plane);
            if (Options.EnablePhenomenons)
                PlanarDeck.AddRange(LoadCards(CardList.Phenomenon, DTO.Type.Plane));
        }
        private List<DTO.Card> LoadCards(List<string> cards, DTO.Type cardType)
        {
            var deck = new List<DTO.Card>();

            foreach(var card in cards)
            {
                deck.Add(new DTO.Card()
                {
                    CardType = cardType,
                    FilePath = card,
                    //CardImage = 
                });
            }

            return deck;
        }
    }
}
