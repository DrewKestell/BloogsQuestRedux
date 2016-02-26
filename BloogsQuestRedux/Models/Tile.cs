using Microsoft.Xna.Framework;

namespace BloogsQuest.Models
{
    public class Tile
    {
        public TilePrototype Prototype { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    50,
                    50);
            }
        }
    }
}