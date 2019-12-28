using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Arcade.Models
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private Song song;
        private SpriteBatch spriteBatch;
        private SpriteFont fontForLivesAndScore;
        private SpriteFont fontGameOverWin;
        private Vector2 fontVector;

        private const int screenWidth = 1720;
        private const int screenHeight = 880;

        private Level Level;
        private Player Player;
        private Bullet Bullet;
        private Enemies Enemies;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = screenWidth,
                PreferredBackBufferHeight = screenHeight,
                IsFullScreen = false
            };
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadMultimedia();
            LoadLevel();
            LoadPlayer();
            LoadBullet();
            LoadEnemies();
        }

        private void LoadMultimedia()
        {
            song = Content.Load<Song>("Audio/GuileTheme");
            fontForLivesAndScore = Content.Load<SpriteFont>("Fonts/Score");
            fontGameOverWin = Content.Load<SpriteFont>("Fonts/GameDone");
            fontVector = new Vector2(0, 0);

            MediaPlayer.Volume = (float)0.2;
            MediaPlayer.Play(song);
        }

        private void LoadLevel()
        {
            Level = new Level(Content.Load<Texture2D>("Textures/LevelTextures/BackgroundLevel1"), Content.Load<Texture2D>("Textures/LevelTextures/levelPlatforms"), screenWidth, screenHeight);
        }

        private void LoadPlayer()
        {
            Texture2D[] playerMovements = new Texture2D[14] { Content.Load<Texture2D>("Textures/PlayerTextures/shootingLeft"), Content.Load<Texture2D>("Textures/PlayerTextures/runningLeft1"),
            Content.Load<Texture2D>("Textures/PlayerTextures/runningLeft2"), Content.Load<Texture2D>("Textures/PlayerTextures/runningLeft3"), Content.Load<Texture2D>("Textures/PlayerTextures/runningLeft4"),
            Content.Load<Texture2D>("Textures/PlayerTextures/runningLeft5"), Content.Load<Texture2D>("Textures/PlayerTextures/runningLeft6"), Content.Load<Texture2D>("Textures/PlayerTextures/shootingRight"),
            Content.Load<Texture2D>("Textures/PlayerTextures/runningRight1"), Content.Load<Texture2D>("Textures/PlayerTextures/runningRight2"), Content.Load<Texture2D>("Textures/PlayerTextures/runningRight3"),
            Content.Load<Texture2D>("Textures/PlayerTextures/runningRight4"), Content.Load<Texture2D>("Textures/PlayerTextures/runningRight5"), Content.Load<Texture2D>("Textures/PlayerTextures/runningRight6")};
            Player = new Player(playerMovements, screenHeight);
        }

        private void LoadBullet()
        {
            Texture2D[] bulletImages = new Texture2D[2] { Content.Load<Texture2D>("Textures/BulletTextures/bulletLeft"), Content.Load<Texture2D>("Textures/BulletTextures/bulletRight") };
            Bullet = new Bullet(bulletImages, Player);
        }

        private void LoadEnemies()
        {
            Texture2D[] enemyMovements = new Texture2D[14] { Content.Load<Texture2D>("Textures/EnemyTextures/walkingLeft1"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingLeft2"),
            Content.Load<Texture2D>("Textures/EnemyTextures/walkingLeft3"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingLeft4"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingLeft5"),
            Content.Load<Texture2D>("Textures/EnemyTextures/walkingLeft6"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingLeft7"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingRight1"),
            Content.Load<Texture2D>("Textures/EnemyTextures/walkingRight2"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingRight3"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingRight4"),
            Content.Load<Texture2D>("Textures/EnemyTextures/walkingRight5"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingRight6"), Content.Load<Texture2D>("Textures/EnemyTextures/walkingRight7")};

            Enemies = new Enemies(enemyMovements, Bullet, Player, Level);
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (Player.exitTheGame) Exit();

            Player.playerKeys();
            Level.LevelCollision(Player);
            Level.LevelCollision(Bullet, Player);
            Enemies.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (Enemies.lives >= 0 && Enemies.score <= 8)
            {
                Level.Draw(spriteBatch);
                Player.Draw(spriteBatch);
                Bullet.Draw(spriteBatch);
                Enemies.Draw(spriteBatch);

                fontVector.X = 1500;
                fontVector.Y = 0;
                spriteBatch.DrawString(fontForLivesAndScore, $"Lives: {Enemies.lives}", fontVector, Color.Snow);
                fontVector.Y = 50;
                spriteBatch.DrawString(fontForLivesAndScore, $"Score: {Enemies.score}", fontVector, Color.Snow);
            }
            else
            {
                if (Enemies.score >= 8)
                {
                    Enemies.score = 8; // Since it can be 9 (for previous if-statement, max is 8)
                    fontVector.X = 670;
                    fontVector.Y = 300;
                    spriteBatch.DrawString(fontGameOverWin, "YOU WIN", fontVector, Color.DarkOliveGreen);
                }
                else
                {
                    Enemies.lives = 0;
                    fontVector.X = 610;
                    fontVector.Y = 300;
                    spriteBatch.DrawString(fontGameOverWin, "GAME OVER", fontVector, Color.Crimson);  
                }
                
                fontVector.X = 740;
                fontVector.Y = 460;
                spriteBatch.DrawString(fontForLivesAndScore, $"SCORE: {Enemies.score}", fontVector, Color.DarkViolet);

            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
