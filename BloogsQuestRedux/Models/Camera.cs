using Microsoft.Xna.Framework;

namespace BloogsQuest.Models
{
    public class Camera
    {
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }

        public Camera()
        {
            Origin = new Vector2(0, 0);
            Position = Origin;
        }

        public Matrix GetTransform()
        {
            return Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}