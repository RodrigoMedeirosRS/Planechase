using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

using BLL.Utils;

namespace BLL
{
    public class Deck
    {
        public List<DTO.Card> PlanarDeck { get; set; }
        public List<DTO.Card> Cemitery { get; set; }

        public Deck()
        {
            PlanarDeck = new List<DTO.Card>();
            Cemitery = new List<DTO.Card>();
        }
        private void LoadDeck()
        {
            PlanarDeck.Clear();
            Cemitery.Clear();
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
                    CardImage = ImportadorDeImagensUtil.BuscarImagem(string.Empty, card) 
                });
            }

            return deck;
        }
        public DTO.Card RevealTopCard()
        {
            try
            {
                return PlanarDeck.FirstOrDefault();
            }
            catch
            {
                LoadDeck();
                ShuffleDeck();
                return PlanarDeck.FirstOrDefault();
            }
        }
        public void SendToCemitery(DTO.Card card)
        {
            PlanarDeck.Remove(card);
            Cemitery.Add(card);
        }
        public void ShuffleDeck()
        {
            var random = new Random();
            PlanarDeck = PlanarDeck.OrderBy(item => random.Next()).ToList();
        }
    }
}
