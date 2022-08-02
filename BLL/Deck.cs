using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

using BLL.Utils;
using BLL.Interface;
using DTO;

namespace BLL
{
    public class Deck : IDeck
    {
        private List<DTO.Card> PlanarDeck { get; set; }
        private List<DTO.Card> Cemitery { get; set; }
        public Texture Verso { get; private set; }

        public Deck(Texture verso)
        {
            Verso = verso;
            PlanarDeck = new List<DTO.Card>();
            Cemitery = new List<DTO.Card>();
        }
        public void LoadDeck()
        {
            PlanarDeck.Clear();
            Cemitery.Clear();
            PlanarDeck = LoadCards(CardList.Plane, DTO.Type.Plane);
            if (Options.EnableCustomCards)
                PlanarDeck = LoadCards(CardList.CustomPlane, DTO.Type.Plane);
            if (Options.EnablePhenomenons)
            {
                PlanarDeck.AddRange(LoadCards(CardList.Phenomenon, DTO.Type.Phenomenon));
                if (Options.EnableCustomCards)
                    PlanarDeck = LoadCards(CardList.CustomPhenomenon, DTO.Type.Phenomenon);
            }
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
            var card = PlanarDeck.FirstOrDefault();
            if (card == null)
            {
                ShuffleDeckAllDeck();
                card = PlanarDeck.FirstOrDefault();
                return card;
            }
            return card;
        }

        public void ShuffleDeckAllDeck()
        {
            LoadDeck();
            ShuffleDeck();
        }

        public void SendToCemitery(DTO.Card card)
        {
            PlanarDeck.Remove(card);
            Cemitery.Add(card);
        }
        public void SendToBotton(DTO.Card card)
        {
            PlanarDeck.Remove(card);
            PlanarDeck.Add(card);
        }
        public void ShuffleDeck()
        {
            var random = new Random();
            PlanarDeck = PlanarDeck.OrderBy(item => random.Next()).ToList();
        }
    }
}
