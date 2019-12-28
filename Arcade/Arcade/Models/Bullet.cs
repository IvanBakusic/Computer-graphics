using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arcade.Models
{
    public class Bullet
    {
        private Player Player;
        private Rectangle player;
        private Texture2D[] bulletImages;
        private int bulletDirection;

        private Rectangle bullet;

        public Bullet(Texture2D[] bulletImages, Player Player)
        {
            this.Player = Player;
            this.player = Player.GetPlayer();
            this.bulletImages = bulletImages;
            bulletDirection = 1; // Default right direction of bullet
            bullet = new Rectangle(player.X + player.Width + 5, player.Y + 15, bulletImages[0].Width, bulletImages[0].Height);
        }

        public Rectangle GetBullet()
        {
            return this.bullet;
        }
        public void setBullet(Rectangle bullet)
        {
            this.bullet = bullet;
        }

        public void ResetBullet(int playerDirection) // 0 - left, 1 - right
        {
            if (playerDirection == 0) bullet.X = player.X - bullet.Width - 5;
            else bullet.X = player.X + player.Width + 5;

            bullet.Y = player.Y + 15;
        }

        public void Update()
        {
            player = Player.GetPlayer(); // Always getting current position of player

            // Same direction as the player
            if (Player.shot)
            {
                if (bullet.X > player.X) bullet.X += 10; //right
                else bullet.X -= 10; //left
            }
            //initial position of the bullet (before firing)
            else if (Player.previousLeft)
            {
                bullet.X = player.X - bullet.Width - 5;
                bullet.Y = player.Y + 15;
                bulletDirection = 0;
            }
            else if (Player.previousRight)
            {
                bullet.X = player.X + player.Width + 5;
                bullet.Y = player.Y + 15;
                bulletDirection = 1;
            }
            else bullet.Y = player.Y + 15;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Update();
            if (Player.shot) spriteBatch.Draw(bulletImages[bulletDirection], bullet, Color.White);
        }
    }
}
