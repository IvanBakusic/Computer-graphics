using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arcade.Models
{
    public class Enemies
    {
        private ArrayList enemy;
        private ArrayList direction; // Direction for each enemy
        private Texture2D[] enemyMovements;
        private List<Rectangle> platforms;
        private Bullet Bullet;
        private Player Player;
        private Level Level;

        private int leftFrame; // Frame iterator for right direciton
        private int rightFrame; // Frame iterator for left direction
        private int lowerFrameRate;
        public int score;
        public int lives;

        public Enemies(Texture2D[] enemyMovements, Bullet Bullet, Player Player, Level Level)
        {
            enemy = new ArrayList();
            this.enemyMovements = enemyMovements;
            this.Bullet = Bullet;
            this.Player = Player;
            this.Level = Level;

            direction = new ArrayList();
            leftFrame = 7;
            rightFrame = 0;
            lowerFrameRate = 0;
            score = 0;
            lives = 3;

            InitializeEnemies(enemyMovements[1].Width, enemyMovements[1].Height); // Biggest frame(by 1 px)
        }

        private void InitializeEnemies(int width, int height)
        {
            platforms = Level.GetPlatforms().ToList(); // Converting to list --> sync local delete with enemy list

            // Setting up enemies on generated level platforms
            enemy.Add(new Rectangle(platforms[0].X, platforms[0].Y - height, width, height));
            enemy.Add(new Rectangle(platforms[1].X, platforms[1].Y - height, width, height));
            enemy.Add(new Rectangle(platforms[2].X, platforms[2].Y - height, width, height));
            enemy.Add(new Rectangle(platforms[3].X, platforms[3].Y - height, width, height));
            enemy.Add(new Rectangle(platforms[4].X, platforms[4].Y - height, width, height));
            enemy.Add(new Rectangle(platforms[5].X, platforms[5].Y - height, width, height));
            enemy.Add(new Rectangle(platforms[6].X, platforms[6].Y - height, width, height));
            enemy.Add(new Rectangle(platforms[7].X, platforms[7].Y - height, width, height));

            for (int it = 0; it < 8; it++) direction.Add(1); // 1 - Right, -1 - Left
        }

        public void Update()
        {
            if (lowerFrameRate > 3)
            {
                if (++leftFrame == 14) leftFrame = 7;
                if (++rightFrame == 7) rightFrame = 0;
                lowerFrameRate = 0;
            }
            lowerFrameRate++;

            UpdateEnemyMovements();
            CheckPlayerCollision();
            CheckBulletCollision();
        }

        private void UpdateEnemyMovements()
        {
            Rectangle individualEnemy;
            int enemyDirection;
            for (int it = 0; it < enemy.Count; it++)
            {
                individualEnemy = (Rectangle)enemy[it];
                enemyDirection = (int)direction[it];

                if (enemyDirection == 1) individualEnemy.X += 2; // Right
                else individualEnemy.X -= 2; // Left

                if (individualEnemy.X + individualEnemy.Width >= platforms[it].X + platforms[it].Width ||
                    individualEnemy.X < platforms[it].X) // Enemy went to the end of the platform
                {
                    direction[it] = enemyDirection * -1;
                }
                enemy[it] = individualEnemy;
            }
        }

        private void CheckPlayerCollision()
        {
            if (lives == 0) lives = -1; // Does if-statement in Game.Draw() one more time (to draw lives 0)

            Rectangle individualEnemy;
            for (int it = 0; it < enemy.Count; it++)
            {
                individualEnemy = (Rectangle)enemy[it];
                if (individualEnemy.Intersects(Player.GetPlayer()))
                {
                    lives--;
                    Player.ResetPlayer();
                }
            }
        }

        private void CheckBulletCollision()
        {
            if (score == 8) score = 9; // Does if-statement in Game.Draw() one more time (to draw score 8)

            Rectangle individualEnemy;
            int deleteEnemy = -1;
            for (int it = 0; it < enemy.Count; it++)
            {
                individualEnemy = (Rectangle)enemy[it];
                if (individualEnemy.Intersects(Bullet.GetBullet())) deleteEnemy = it;
            }

            if (deleteEnemy > -1)
            {
                enemy.RemoveAt(deleteEnemy); // Remove enemy
                platforms.RemoveAt(deleteEnemy); // Remove corresponding local platform
                direction.RemoveAt(deleteEnemy); // Remove corresponding direction
                Bullet.ResetBullet(Player.currentFrame); // Reset bullet
                Player.shot = false; // Disable pressed shooting key
                deleteEnemy = -1;
                score++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int enemyDirection;
            for (int it = 0; it < enemy.Count; it++)
            {
                enemyDirection = (int)direction[it];
                if (enemyDirection == -1) spriteBatch.Draw(enemyMovements[rightFrame], (Rectangle)enemy[it], Color.White);
                else spriteBatch.Draw(enemyMovements[leftFrame], (Rectangle)enemy[it], Color.White);
            }
        }
    }
}
