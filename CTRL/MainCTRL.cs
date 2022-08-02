using Godot;
using System;

public class MainCTRL : Control
{
	private bool FirstCard { get; set; }
	private TextureButton Card { get; set; }
	private TextureButton Phenomenon { get; set; }
	private BLL.Interface.IDeck PlanarDeck { get; set; }
	private DTO.Card ActualPlane { get; set; }


	public override void _Ready()
	{
		PopularNodes();
		RealizarInjecaoDeDependencias();
		DesativarFuncoesDeAltoProcessamento();
	}
	private void PopularNodes()
	{
		Card = GetNode<TextureButton>("./Card");
		Phenomenon = GetNode<TextureButton>("./ConteinerHorizontal/Comandos/Fenomeno");
	}
	private void RealizarInjecaoDeDependencias()
	{
		FirstCard = true;
		Phenomenon.Visible = false;
		PlanarDeck = new BLL.Deck(Card.TextureNormal);
		PlanarDeck.LoadDeck();
		PlanarDeck.ShuffleDeck();
	}
	private void DesativarFuncoesDeAltoProcessamento()
	{
		SetProcess(false);
		SetPhysicsProcess(false);
	}
	private void RevealCard()
	{
		ActualPlane = PlanarDeck.RevealTopCard();
		Card.TextureNormal = ActualPlane.CardImage;
	}
	private void _on_Card_button_up()
	{
		if (FirstCard)
		{
			RevealCard();
			FirstCard = false;
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
		// Replace with function body.
	}
	private void _on_Embaralhar_button_up()
	{
		Card.TextureNormal = PlanarDeck.Verso;
		FirstCard = true;
		PlanarDeck.ShuffleDeck();
	}
	private void _on_Fenomeno_button_up()
	{
		// Replace with function body.
	}
}
