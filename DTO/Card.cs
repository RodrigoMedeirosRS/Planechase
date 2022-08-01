using Godot;

namespace DTO
{
    public class Card
    {
        public Type CardType { get; set; }
        public string FilePath { get; set; }
        public Texture CardImage { get; set; }
    }
}
