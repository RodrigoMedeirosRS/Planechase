using Godot;
using System;

namespace CTRL
{
	public class MainCTRL : Control
	{
		private bool FirstCard { get; set; }
		private TextureButton Card { get; set; }
		private TextureButton Phenomenon { get; set; }
		private BLL.Interface.IDeck PlanarDeck { get; set; }
		private DTO.Card ActualPlane { get; set; }
		private DTO.Card ActualPhenomenon { get; set; }
		private Control OptionMenu { get; set; }
		public override void _Ready()
		{
			PopularNodes();
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
			AlterarVisibilidadeMenu(false);
		}
		private void PopularNodes()
		{
			Card = GetNode<TextureButton>("./Card");
			Phenomenon = GetNode<TextureButton>("./ConteinerHorizontal/Comandos/Fenomeno");
			OptionMenu = GetNode<Control>("./MenuDeOpcoes");
		}
		private void RealizarInjecaoDeDependencias()
		{
			FirstCard = true;
			ActualPlane = null;
			ActualPhenomenon = null;
			PlanarDeck = new BLL.Deck(Card.TextureNormal);
			PlanarDeck.LoadDeck();
			PlanarDeck.ShuffleDeck();
			Phenomenon.Visible = false;
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetProcess(false);
			SetPhysicsProcess(false);
		}
		private void AlterarVisibilidadeMenu(bool visivel)
		{
			OptionMenu.RectPosition = visivel ? new Vector2(0,0) : new Vector2(5000,0);
		}
		private void RevealCard()
		{
			var card = PlanarDeck.RevealTopCard();
			if (card.CardType == DTO.Type.Plane)
			{
				ActualPlane = card;
				ActualPhenomenon = null;
				Card.TextureNormal = ActualPlane.CardImage;
				Phenomenon.Visible = false;
			}
			else
			{
				ActualPhenomenon = card;
				Card.TextureNormal = ActualPhenomenon.CardImage;
				while(card.CardType != DTO.Type.Plane)
				{
					PlanarDeck.SendToCemitery(card);
					card = PlanarDeck.RevealTopCard();
				}
				ActualPlane = card;
				Phenomenon.Visible = true;
			}
		}
		private void _on_Card_button_up()
		{
			if (FirstCard)
			{
				RevealCard();
				FirstCard = false;
			}
			if (Phenomenon.Visible)
			{
				Card.TextureNormal = ActualPlane.CardImage;
			}
		}
		private void _on_Transplanar_button_up()
		{
			if (!FirstCard)
			{
				PlanarDeck.SendToCemitery(ActualPlane);
				RevealCard();
			}
		}
		private void _on_Ajustes_button_up()
		{
			AlterarVisibilidadeMenu(true);
		}
		private void _on_Embaralhar_button_up()
		{
			Card.TextureNormal = PlanarDeck.Verso;
			FirstCard = true;
			PlanarDeck.ShuffleDeck();
		}
		private void _on_Fenomeno_button_up()
		{
			if (ActualPhenomenon != null)
				Card.TextureNormal = ActualPhenomenon.CardImage;
		}
	}
}
