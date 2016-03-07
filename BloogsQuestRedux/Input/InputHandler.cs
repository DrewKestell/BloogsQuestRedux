using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using BloogsQuestRedux.Models;

namespace BloogsQuestRedux.Input
{
    public class InputHandler
    {
        private bool IsMovingHorizontally(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.Left);
        }

        private bool IsMovingVertically(KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Down);
        }

        public void HandleInput(Scene currentScene, KeyboardState keyboardState)
        {
            MovePlayer(currentScene, keyboardState);
        }

        private void MovePlayer(Scene currentScene, KeyboardState keyboardState)
        {
            Vector2 newPlayerPosition;

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                newPlayerPosition = new Vector2(currentScene.Player.Position.X + Global.PlayerSpeed, currentScene.Player.Position.Y);
            }

            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                newPlayerPosition = new Vector2(currentScene.Player.Position.X, currentScene.Player.Position.Y - Global.PlayerSpeed);
            }

            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                newPlayerPosition = new Vector2(currentScene.Player.Position.X - Global.PlayerSpeed, currentScene.Player.Position.Y);
            }

            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                newPlayerPosition = new Vector2(currentScene.Player.Position.X, currentScene.Player.Position.Y + Global.PlayerSpeed);
            }

            else
                newPlayerPosition = new Vector2(currentScene.Player.Position.X, currentScene.Player.Position.Y);

            if (!CheckCollision(currentScene, newPlayerPosition))
            {
                currentScene.Player.Position = newPlayerPosition;
                currentScene.Camera.TryMove(keyboardState, currentScene.Player.Position, newPlayerPosition - currentScene.Player.Position, IsMovingHorizontally(keyboardState), IsMovingVertically(keyboardState));
            }                
        }

        private bool CheckCollision(Scene currentScene, Vector2 newPlayerPosition)
        {
            return currentScene.Map.GetBlockedTiles().Any(x => x.BoundingBox.Intersects(new Rectangle((int)newPlayerPosition.X, (int)newPlayerPosition.Y, 30, 30)));
        }
    }
}