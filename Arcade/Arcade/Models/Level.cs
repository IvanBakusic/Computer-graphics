using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade.Models
{
    public class Level
    {
        // 1720 x 880
        private int screenWidth;
        private int screenHeight;

        private Texture2D backgroundTexture;
        private Texture2D platformTexture;
        private Rectangle[] levelPlatforms;
        private Rectangle backgroundImage;

        public bool isCollision = false; //nije potrebno

        public Level(Texture2D background, Texture2D platformTexture, int screenWidth, int screenHeight)
        {
            this.backgroundTexture = background;
            this.platformTexture = platformTexture;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.backgroundImage = new Rectangle(0, 0, screenWidth, screenHeight);

            LoadPlatforms();
        }


        private void LoadPlatforms()
        {
            levelPlatforms = new Rectangle[9];

            levelPlatforms[0] = new Rectangle(800, 670, 920, 30);
            levelPlatforms[1] = new Rectangle(0, 500, 300, 30);
            levelPlatforms[2] = new Rectangle(1400, 550, 320, 30);
            levelPlatforms[3] = new Rectangle(500, 550, 200, 30);
            levelPlatforms[4] = new Rectangle(800, 400, 920, 30);
            levelPlatforms[5] = new Rectangle(430, 370, 200, 30);
            levelPlatforms[6] = new Rectangle(0, 230, 700, 30);
            levelPlatforms[7] = new Rectangle(870, 190, 400, 30);
            levelPlatforms[8] = new Rectangle(1500, 280, 20, 20);
        }

        public Rectangle[] GetPlatforms()
        {
            return levelPlatforms;
        }

        public void LevelCollision(Bullet Bullet, Player Player) //overloading method
        {
            Rectangle bullet = LevelBordersCollision(Bullet.GetBullet());

            if (bullet.X <= 0 || bullet.X >= screenWidth - bullet.Width)
            {
                Bullet.ResetBullet(Player.currentFrame);
                Player.shot = false;
            }

            foreach (Rectangle platform in levelPlatforms)
            {
                if (bullet.Intersects(platform))
                {
                    Bullet.ResetBullet(Player.currentFrame);
                    Player.shot = false;
                }
            }
        }

        public void LevelCollision(Player Player)
        {
            Rectangle player = Player.GetPlayer();

            player = LevelBordersCollision(player);

            foreach (Rectangle platform in levelPlatforms)
            {
                if (player.Intersects(platform))
                {
                    isCollision = true;
                    String firstCollision = FindFirstCollision(player, platform);

                    switch (firstCollision)
                    {
                        case "Left":
                            player.X = platform.X - player.Width - 1;
                            break;
                        case "Up":
                            player.Y = platform.Y - player.Height - 1;
                            break;
                        case "Right":
                            player.X = platform.X + platform.Width + 1;
                            break;
                        case "Below":
                            player.Y = platform.Y + platform.Height + 1;
                            break;
                        case "None":
                            break;
                        default:
                            break;
                    }
                }
            }
            Player.SetPlayer(player);
        }

        private string FindFirstCollision(Rectangle player, Rectangle platform)
        {
            float leftBorder = player.X + player.Width + 1 - platform.X;
            float upperBorder = player.Y + player.Height + 1 - platform.Y;
            float rightBorder = platform.X + platform.Width + 1 - player.X;
            float bottomBorder = platform.Y + platform.Height + 1 - player.Y;


            if ((leftBorder < upperBorder && leftBorder < rightBorder && leftBorder < bottomBorder)
                || (leftBorder == upperBorder && player.Y < platform.Y)) return "Left";
            else if ((rightBorder < upperBorder && rightBorder < leftBorder && rightBorder < bottomBorder)
                || (rightBorder == upperBorder && player.Y < platform.Y)) return "Right";
            else if ((upperBorder < leftBorder && upperBorder < rightBorder && upperBorder < bottomBorder)) return "Up";
            else if ((bottomBorder < upperBorder && bottomBorder < rightBorder && bottomBorder < leftBorder)
                || leftBorder == bottomBorder || rightBorder == bottomBorder) return "Below";

            return "None";
        }

        private Rectangle LevelBordersCollision(Rectangle Object)
        {
            // Multiple if-statements for corner detection
            if (Object.X < 0) Object.X = 0;
            if ((Object.X + Object.Width) > screenWidth) Object.X = screenWidth - Object.Width;
            if (Object.Y < 0) Object.Y = 0;
            if ((Object.Y + Object.Height) > (screenHeight - 80)) Object.Y = screenHeight - Object.Height - 80;

            return Object;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundImage, Color.White);
            foreach (Rectangle platform in levelPlatforms)
            {
                spriteBatch.Draw(platformTexture, platform, Color.White);
            }
        }
    }
}
