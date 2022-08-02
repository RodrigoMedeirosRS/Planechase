using Godot;

namespace BLL.Interface
{
    public interface IDeck
    {

        void LoadDeck();
        void ShuffleDeck();
        Texture Verso { get; }
        DTO.Card RevealTopCard();
        void SendToCemitery(DTO.Card card);
        void SendToBotton(DTO.Card card);
    }
}