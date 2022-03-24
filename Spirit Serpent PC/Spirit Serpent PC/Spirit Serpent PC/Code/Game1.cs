using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Spirit_Serpent_PC
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch UIBatch;
        SpriteBatch backgroundBatch;

        int smallCoinStreak = 0;
        int bigCoinStreak = 0;
        int invinciTimer = 0;
        int battleTimer = 0;

        Level workingLevel;

        cursor theCursor;

        KeyboardState oldState;


        //SoundEffect soundEffect;
        SoundEffect music;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;

            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            screenDimensions.dimensions = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
    
            base.Initialize();
        }

        protected override void LoadContent()
        {          
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            UIBatch = new SpriteBatch(GraphicsDevice);
            backgroundBatch = new SpriteBatch(GraphicsDevice);

            music = Content.Load<SoundEffect>("Output12");

            SoundEffectInstance loopingMusic = music.CreateInstance();
            loopingMusic.IsLooped = true;
            //loopingMusic.Play();

            texLib.addTextures(Content);
            mainSnake.init();

            workingLevel = new BlocksLevel();
         
            theCursor = new cursor();

            //soundEffect = Content.Load<SoundEffect>("Windows Ding");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {// Allow the game to exit.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed)
                this.Exit();

            mainSnake.updateSnake();
            workingLevel.update();

            mainSnake.character.setGreen();

            theCursor.update();

            //addEnemies();

            //UpdateInput();

            for (int i = 0; i < objectManager.enemies.Count; i++)
            {
                if (objectManager.enemies[i].willDelete == false)
                {
                    objectManager.enemies[i].update();
                }
                else
                {
                    objectManager.enemies.RemoveAt(i);
                    i--;
                }

            }


            for (int i = 0; i < objectManager.pickups.Count; i++)
            {
                if (objectManager.pickups[i].willDelete == false)
                {
                    objectManager.pickups[i].update();
                }
                else
                {
                    objectManager.pickups.RemoveAt(i);
                    i--;
                }

            }

            for (int i = 0; i < objectManager.bullets.Count; i++)
            {
                if (objectManager.bullets[i].willDelete == false)
                {
                    objectManager.bullets[i].update();
                }
                else
                {
                    objectManager.bullets.RemoveAt(i);
                    i--;
                }

            }

            battleTimer--;
            if (battleTimer >= 0)
            {
                mainSnake.character.setFire();
                objectManager.bullets.Add(new bullet(mainSnake.character, 20, 5));
            }

            invinciTimer--;
            if (invinciTimer <= 0)
            {
                for (int i = 0; i < objectManager.enemies.Count; i++)
                {
                    CheckForCollision(mainSnake.character, objectManager.enemies[i]);
                }
            }
            else
            {
                mainSnake.character.changeColor();

                for (int i = 0; i < objectManager.enemies.Count; i++)
                {
                    CheckInvulnCollision(mainSnake.character, objectManager.enemies[i]);
                }
            }

            for (int i = 0; i < objectManager.pickups.Count; i++)
            {
                if (CollectPowerUp(mainSnake.character, objectManager.pickups[i]))
                {
                    objectManager.pickups.RemoveAt(i);
                    i--;
                }

            }

            for (int i = 0; i < objectManager.bullets.Count; i++)
            {
                for (int j = 0; j < objectManager.enemies.Count; j++)
                {
                    BulletHit(objectManager.bullets[i], objectManager.enemies[j]);
                }

            }




            score.update();

            base.Update(gameTime);
        }

        void CheckForCollision(gameSprite sprite1, gameSprite sprite2)
        {

            if (sprite2.isSquare)
            {
                squareSprite sqSprite2 = (squareSprite)sprite2;
                if ((sprite1.center.X + sprite1.collideSize > sqSprite2.collideLeft && sprite1.center.X - sprite1.collideSize < sqSprite2.collideRight) &&
                    (sprite1.center.Y - sprite1.collideSize > sqSprite2.collideTop && sprite1.center.Y - sprite1.collideSize < sqSprite2.collideBottom))
                {
                    score.yourScore = 0;
                    smallCoinStreak = 0;
                    bigCoinStreak = 0;
                    score.deaths++;
                    objectManager.clear();

                    workingLevel = new BlocksLevel();
                }
            }
            else
            {


                int distance = (int)Math.Sqrt(Math.Abs(sprite1.center.X - sprite2.center.X) * Math.Abs(sprite1.center.X - sprite2.center.X) + Math.Abs(sprite1.center.Y - sprite2.center.Y) * Math.Abs(sprite1.center.Y - sprite2.center.Y));

                if (distance <= (sprite1.collideSize + sprite2.collideSize))
                {
                    score.yourScore = 0;
                    smallCoinStreak = 0;
                    bigCoinStreak = 0;
                    score.deaths++;
                    objectManager.clear();

                    workingLevel = new BlocksLevel();

                }

            }
            //soundEffect.Play();

        }

        void CheckInvulnCollision(snakeHead sprite1, gameSprite sprite2)
        {
            int distance = (int)Math.Sqrt(Math.Abs(sprite1.center.X - sprite2.center.X) * Math.Abs(sprite1.center.X - sprite2.center.X) + Math.Abs(sprite1.center.Y - sprite2.center.Y) * Math.Abs(sprite1.center.Y - sprite2.center.Y));

            if (distance <= (sprite1.collideSize2 + sprite2.collideSize))
            {
                sprite2.willDelete = true;
                score.yourScore += 500;

            }

            //soundEffect.Play();

        }

        void BulletHit(gameSprite sprite1, gameSprite sprite2)
        {
            int distance = (int)Math.Sqrt(Math.Abs(sprite1.center.X - sprite2.center.X) * Math.Abs(sprite1.center.X - sprite2.center.X) + Math.Abs(sprite1.center.Y - sprite2.center.Y) * Math.Abs(sprite1.center.Y - sprite2.center.Y));

            if (distance <= (sprite1.collideSize + sprite2.collideSize))
            {
                sprite1.willDelete = true;
                sprite2.willDelete = true;
                score.yourScore += 100;

            }

            //soundEffect.Play();

        }

        Boolean CollectPowerUp(snakeHead sprite1, powerUp sprite2)
        {
            int distance = (int)Math.Sqrt(Math.Abs(sprite1.center.X - sprite2.center.X) * Math.Abs(sprite1.center.X - sprite2.center.X) + Math.Abs(sprite1.center.Y - sprite2.center.Y) * Math.Abs(sprite1.center.Y - sprite2.center.Y));

            if (distance <= (sprite1.collideSize2 + sprite2.collideSize))
            {
                switch (sprite2.powerUpNumber)
                {
                    case 1:
                        smallCoinStreak++;
                        score.yourScore += 10 * smallCoinStreak;
                        break;
                    case 2:
                        bigCoinStreak++;
                        score.yourScore += 100 * bigCoinStreak;
                        break;
                    case 3:
                        if (invinciTimer < 0)
                        {
                            invinciTimer = 0;
                        }
                        invinciTimer += 150;
                        break;
                    case 4:
                        if (battleTimer < 0)
                        {
                            battleTimer = 0;
                        }
                        battleTimer += 300;
                        break;

                }
                return true;
            }

            return false;
        }
        /*
        private void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();


            if (newState.IsKeyDown(Keys.Q))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.Q))
                {
                    objectManager.enemies.Add(new superBird(character, 15));
                }
            }
            else if (oldState.IsKeyDown(Keys.Q))
            {
                // Key was down last update, but not down now, so
                // it has just been released.
            }

            if (newState.IsKeyDown(Keys.W))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.W))
                {
                    objectManager.enemies.Add(new rainMan(5, 3, 3));
                }
            }
            else if (oldState.IsKeyDown(Keys.W))
            {
                // Key was down last update, but not down now, so
                // it has just been released.
            }


            if (newState.IsKeyDown(Keys.E))
            {
                // If not down last update, key has just been pressed.
                if (!oldState.IsKeyDown(Keys.E))
                {
                    objectManager.enemies.Add(new deathEnemy(character, 3, 15, 10, 3));
                }
            }
            else if (oldState.IsKeyDown(Keys.E))
            {
                // Key was down last update, but not down now, so
                // it has just been released.
            }


            // Update saved state.
            oldState = newState;
        }*/

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            backgroundBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque);
            backgroundBatch.Draw(workingLevel.backGround1.texture, workingLevel.backGround1.position, null, Color.White);
            backgroundBatch.Draw(workingLevel.backGround2.texture, workingLevel.backGround2.position, null, Color.White);
            backgroundBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(mainSnake.character.texture, mainSnake.character.position, null, mainSnake.character.color, mainSnake.character.rotation, mainSnake.character.origin, 1.0f, SpriteEffects.None, 0f);

            for (int i = 0; i < mainSnake.tailSegments.Count; i++)
            {
                spriteBatch.Draw(mainSnake.tailSegments[i].texture, mainSnake.tailSegments[i].position, null, mainSnake.character.color, mainSnake.tailSegments[i].rotation, mainSnake.tailSegments[i].origin, 1.0f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < objectManager.enemies.Count; i++)
            {
                spriteBatch.Draw(objectManager.enemies[i].texture, objectManager.enemies[i].position, null, Color.White, objectManager.enemies[i].rotation, objectManager.enemies[i].origin, 1.0f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < objectManager.pickups.Count; i++)
            {
                spriteBatch.Draw(objectManager.pickups[i].texture, objectManager.pickups[i].position, null, Color.White, objectManager.pickups[i].rotation, objectManager.pickups[i].origin, 1.0f, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < objectManager.bullets.Count; i++)
            {
                spriteBatch.Draw(objectManager.bullets[i].texture, objectManager.bullets[i].position, null, Color.White, objectManager.bullets[i].rotation, objectManager.bullets[i].origin, 1.0f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            UIBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            UIBatch.DrawString(score.scoreFont, score.scoreString, score.scorePos, Color.White);
            UIBatch.Draw(theCursor.texture, theCursor.position, null, Color.White, theCursor.rotation, theCursor.origin, 1.0f, SpriteEffects.None, 0f);
            UIBatch.End();



            /*spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.Opaque);
            spriteBatch.Draw(texture2, spritePosition2, Color.Gray);
            spriteBatch.End();*/

            base.Draw(gameTime);
        }
    }
}
