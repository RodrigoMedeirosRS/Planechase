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
		private bool EnablePhenomenons { get; set; }
		private bool EnableCustomCards { get; set; }
		private int ChaosFacesCount { get; set; }
		private int TransplanarFacesCount { get; set; }
		private CheckButton Phenomenons { get; set; }
		private CheckButton CustomCards { get; set; }
		private SpinBox TransplanarFaces { get; set; }
		private SpinBox ChaosFaces { get; set; }
		private TextureRect TransplaneRoll1 { get; set; }
		private TextureRect TransplaneRoll2 { get; set; }
		private TextureRect TransplaneRoll3 { get; set; }
		private TextureRect ChaosRoll1 { get; set; }
		private TextureRect ChaosRoll2 { get; set; }
		private TextureRect ChaosRoll3 { get; set; }
		public override void _Ready()
		{
			CentralizarJanela();
			PopularNodes();
			AlterarVisibilidadeMenu(false);
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
			SetarValoresIniciais();
		}
		private void CentralizarJanela()
		{
			if (Godot.OS.GetName() == "Windows" || Godot.OS.GetName() == "X11")
			{
				var tamanhoDaJanela = Godot.OS.GetScreenSize(0);
				var tamanhoDaTela = Godot.OS.WindowSize;
				Godot.OS.WindowPosition = (tamanhoDaJanela * 0.5f - tamanhoDaTela * 0.5f);
			}
		}
		private void PopularNodes()
		{
			Card = GetNode<TextureButton>("./Card");
			Phenomenon = GetNode<TextureButton>("./ConteinerHorizontal/Comandos/Fenomeno");
			OptionMenu = GetNode<Control>("./MenuDeOpcoes");
			TransplanarFaces = GetNode<SpinBox>("./MenuDeOpcoes/Menu/ChanceDeTransplanar");
			ChaosFaces = GetNode<SpinBox>("./MenuDeOpcoes/Menu/ChanceDeTransplanar2");
			Phenomenons = GetNode<CheckButton>("./MenuDeOpcoes/Menu/AtivaFenomenos");
			CustomCards = GetNode<CheckButton>("./MenuDeOpcoes/Menu/AtivarCustomCards");
			TransplaneRoll1 = GetNode<TextureRect>("./ConteinerHorizontal/Transplanar/Rolagens/Rolagem1");
			TransplaneRoll2 = GetNode<TextureRect>("./ConteinerHorizontal/Transplanar/Rolagens/Rolagem2");
			TransplaneRoll3 = GetNode<TextureRect>("./ConteinerHorizontal/Transplanar/Rolagens/Rolagem3");
			ChaosRoll1 = GetNode<TextureRect>("./ConteinerHorizontal/Caos/Rolagens2/Rolagem1");
			ChaosRoll2 = GetNode<TextureRect>("./ConteinerHorizontal/Caos/Rolagens2/Rolagem2");
			ChaosRoll3 = GetNode<TextureRect>("./ConteinerHorizontal/Caos/Rolagens2/Rolagem3");
		}
		private void RealizarInjecaoDeDependencias()
		{
			FirstCard = true;
			ActualPlane = null;
			ActualPhenomenon = null;
			EnableCustomCards = BLL.Options.EnableCustomCards;
			EnablePhenomenons = BLL.Options.EnablePhenomenons;
			ChaosFacesCount = BLL.Options.ChaosFaces;
			TransplanarFacesCount = BLL.Options.TransplaneFaces;
			PlanarDeck = new BLL.Deck(Card.TextureNormal);
			PlanarDeck.LoadDeck();
			PlanarDeck.ShuffleDeck();
			Phenomenon.Visible = false;
			AlterarVisibilidadeRolagens();
		}
		private void AlterarVisibilidadeRolagens()
		{
			TransplaneRoll1.Visible = BLL.Options.TransplaneFaces >= 1;
			TransplaneRoll2.Visible = BLL.Options.TransplaneFaces >= 2;
			TransplaneRoll3.Visible = BLL.Options.TransplaneFaces >= 3;
			ChaosRoll1.Visible = BLL.Options.ChaosFaces >= 1;
			ChaosRoll2.Visible = BLL.Options.ChaosFaces >= 2;
			ChaosRoll3.Visible = BLL.Options.ChaosFaces >= 3;
		}
		private void SetarValoresIniciais()
		{
			ChaosFaces.Set("value", BLL.Options.ChaosFaces);
			Phenomenons.Set("pressed", BLL.Options.EnablePhenomenons);
			CustomCards.Set("pressed", BLL.Options.EnableCustomCards);
			TransplanarFaces.Set("value", BLL.Options.TransplaneFaces);
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
		private void _on_AtivaFenomenos_toggled(bool button_pressed)
		{
			EnablePhenomenons = button_pressed;
		}
		private void _on_AtivarCustomCards_toggled(bool button_pressed)
		{
			EnableCustomCards = button_pressed;
		}
		private void _on_ChanceDeTransplanar_value_changed(float value)
		{
			TransplanarFacesCount = Convert.ToInt32(value);
		}
		private void _on_ChanceDeTransplanar2_value_changed(float value)
		{
			ChaosFacesCount = Convert.ToInt32(value);
		}
		private void _on_Salvar_button_up()
		{
			BLL.Options.ChaosFaces = ChaosFacesCount;
			BLL.Options.TransplaneFaces = TransplanarFacesCount;
			BLL.Options.EnableCustomCards = EnableCustomCards;
			BLL.Options.EnablePhenomenons = EnablePhenomenons;
			Card.TextureNormal = PlanarDeck.Verso;
			FirstCard = true;
			PlanarDeck.ShuffleDeckAllDeck();
			AlterarVisibilidadeMenu(false);
			AlterarVisibilidadeRolagens();
		}
		private void _on_Cancelar_button_up()
		{
			ChaosFacesCount = BLL.Options.ChaosFaces;
			TransplanarFacesCount = BLL.Options.TransplaneFaces;
			EnableCustomCards = BLL.Options.EnableCustomCards;
			EnablePhenomenons = BLL.Options.EnablePhenomenons;
			SetarValoresIniciais();
			AlterarVisibilidadeMenu(false);
			AlterarVisibilidadeRolagens();
		}
	}
}
