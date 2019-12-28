using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arcade.Models
{
    public class Player
    {
        int screenHeight;

        private Rectangle player;
        private Texture2D[] playerMovements;

        public int currentFrame;
        private int lowerMovementFrame;
        private int jumpIteration;
        private bool spacePressed;

        public bool previousLeft;
        public bool previousRight;
        public bool shot;

        public bool exitTheGame;

        public Player(Texture2D[] playerMovements, int screenHeight)
        {
            player = new Rectangle(0, screenHeight - player.Height - 80, playerMovements[0].Width * 2, playerMovements[0].Height * 2);
            currentFrame = 7;
            lowerMovementFrame = 0;
            jumpIteration = 0;
            spacePressed = false;
            previousLeft = false;
            previousRight = false;
            shot = false;
            exitTheGame = false;

            this.screenHeight = screenHeight;
            this.playerMovements = playerMovements;
        }

        public Rectangle GetPlayer()
        {
            return this.player;
        }

        public void SetPlayer(Rectangle player)
        {
            this.player = player;
        }

        public void ResetPlayer()
        {
            player.X = 0;
            player.Y = screenHeight - player.Height - 80;
        }

        public void playerKeys()
        {
            bool up = Keyboard.GetState().IsKeyDown(Keys.Up);
            bool down = Keyboard.GetState().IsKeyDown(Keys.Down);
            bool left = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool right = Keyboard.GetState().IsKeyDown(Keys.Right);
            bool shotBullet = Keyboard.GetState().IsKeyDown(Keys.F);

            if (left)
            {
                player.X -= 5;
                if (currentFrame >= 6 && lowerMovementFrame % 4 == 0) currentFrame = 0;
                if (lowerMovementFrame % 4 == 0) // %4 jer zelim pokrete 4 puta sporije
                {
                    currentFrame++;
                    lowerMovementFrame = 0;
                }
            }
            if (right)
            {
                player.X += 5;
                if (currentFrame < 7 || (currentFrame == 13 && lowerMovementFrame % 4 == 0)) currentFrame = 7;
                if (lowerMovementFrame % 4 == 0)
                {
                    currentFrame++;
                    lowerMovementFrame = 0;
                }
            }          
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (jumpIteration == 0) spacePressed = true; //ogranicenje da se ne moze duplo skociti ili pak vise puta
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) exitTheGame = true;
            if (shotBullet) shot = true;
            Update(left, right);
            playerJump();
        }

        private void playerJump()
        {
            if (spacePressed)
            {
                player.Y -= 10; //10 jer je 5 pad stoga je skok 5 piksela gore po pozivu
                jumpIteration++;
                if (jumpIteration == 29)
                {
                    spacePressed = false;
                }
            }

            else if (!spacePressed && jumpIteration != 0)
            {
                //nema zbrajanja visine(Y) jer je 5 pixela po pozivu vec dovoljno za pad
                jumpIteration--;
            }
        }

        private void Update(bool left, bool right)
        {
            player.Y += 5;
            lowerMovementFrame++;
            if (lowerMovementFrame > 4) lowerMovementFrame = 1; // Limit to numbers from (0)1 to 4 to not exceed max integer value

            if (left == false && previousLeft == true) currentFrame = 0;
            if (right == false && previousRight == true) currentFrame = 7;

            previousLeft = left;
            previousRight = right;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playerMovements[currentFrame], player, Color.White);
        }
    }
}
