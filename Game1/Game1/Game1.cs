﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public class Game1 : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        //Texture objects

        Texture2D Floor; //Background used for each room
        Texture2D fullWall; //A wall that isn't open 
        Texture2D doorWall; //The wall with an opening for a door
        Texture2D sealedVDoor; // a vertical door that you cant walk through
        Texture2D sealedHDoor; // a horizontal door that you cant walk through
        Texture2D openDoor; //Open door
        Texture2D character; //The character's sprite
        Texture2D Enemy; //The enemy sprite
        Texture2D menuBG; //Background for menu screens
        Texture2D sword;
        Texture2D healthBar;
        Texture2D fullHealthBar;
        //Game Objects

        Texture2D ItemMenu;
        Texture2D Logo;
        Texture2D MenuBack;
		Texture2D ButtonBack;
        Texture2D RIP;
		Rectangle LogoLoc;
		Rectangle StatsLoc;
        Rectangle FullScreen;
        Rectangle RIPloc;
        Vector2 CenterScreen;
        string MainMenuText;
        string ItemMenuText;
        string PauseMenuText;
        string GameOverText;
		
        //Menu Objects

        Room testRoom;
        Character mainChar;
        KeyboardState kbState; //2 Keboard states for toggeling items
        KeyboardState previousKbState;
        Vector2 movement;
        Weapon blade;
        int movespeed;
        float rotate;
        float rotate2;
        MouseState ms;
		MouseState previousms;
        bool leftMousePress;
        bool rightMousePress;
        int enemyNo;
		Button SetStats;
		Button Play;
		Button Resume;
		Button Restart;
		Rectangle buttonPosSetStats;
		Rectangle buttonPosPlay;
		Rectangle buttonPosResume;
		Rectangle buttonPosRestart;

		//enum for Game State
		enum GameState
        {
            MainMenu, PauseMenu, ItemMenu, PlayGame, Gameover
        }
        GameState state;

        //enum for player movement
        enum PlayerMovement
        {
            North, South, East, West, NorthEast, NorthWest, SouthEast, SouthWest, Static
        }
        PlayerMovement direction;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //This changes the size and location of the window don't mess with it
            graphics.HardwareModeSwitch = false;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //State initialization
            state = GameState.MainMenu;
            direction = PlayerMovement.Static;
            this.IsMouseVisible = true;

            //Game Object initialzation
            mainChar = new Character(500, 500, 35);
            mainChar.attackDamage = 10;
            mainChar.healthPoints = 100;
            blade = new Weapon(mainChar);
            testRoom = new Room();



            movespeed = 10;

            //Sets testroom enemy number
            enemyNo = 3;

            //Setting walls

            // Menu Setup
            FullScreen = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            CenterScreen = new Vector2(280, 500);
            LogoLoc = new Rectangle(335, 250, 1000, 115);
            StatsLoc = new Rectangle(335, 85, 1000, 750);
            RIPloc = new Rectangle(800, 250, 500, 500);

			
			
			base.Initialize();
           
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Creating Game Object sprites
            character = Content.Load<Texture2D>("Character.png");
            Enemy = Content.Load<Texture2D>("Enemy.png");
            font = Content.Load<SpriteFont>("Font");
            sword = Content.Load<Texture2D>("sword.png");
            sealedVDoor = Content.Load<Texture2D>("vDoor.jpg");
            sealedHDoor = Content.Load<Texture2D>("hDoor.jpg");
            MenuBack = Content.Load<Texture2D>("oldpaper.jpg");
            Logo = Content.Load<Texture2D>("PlaceholderLogo.png");
            ItemMenu = Content.Load<Texture2D>("PlaceholderStats.png");
            RIP = Content.Load<Texture2D>("RIP.jpg");
			ButtonBack = Content.Load<Texture2D>("buttonTemplate.png");
            //Setting sprites
            blade.setWeaponSprite(sword);
            mainChar.SetSprite(character);
           
            //Setting room enemies and their textures   
            testRoom.SetEnemies(Enemy, enemyNo);

            //Spawinging enemies
            testRoom.SpawnEnemies();

            //Floor = Content.Load<Texture2D>(); //Background used for each room
            testRoom.SetWallTexure(Content.Load<Texture2D>("hWall.png"), Content.Load<Texture2D>("vWall.png")); //A wall that isnt open 
            testRoom.SetWalls();
            testRoom.leftWall.wallDoor.SetSprite(sealedVDoor);
            testRoom.topWall.wallDoor.SetSprite(sealedHDoor);
            testRoom.rightWall.wallDoor.SetSprite(sealedVDoor);
            testRoom.bottomWall.wallDoor.SetSprite(sealedHDoor);
            //doorWall = Content.Load<Texture2D>(); //The wall with an opening for a door
            //sealedDoor = Content.Load<Texture2D>(); // a door that you cant walk through
            //openDoor = Content.Load<Texture2D>(); //Open door
            //Character = Content.Load<Texture2D>(); //The character's sprite
            //Enemy = Content.Load<Texture2D>(); //The enemy sprite
            //logo = Content.Load<Texture2D>(); //Game's logo


            healthBar = Content.Load<Texture2D>("health.png");
            fullHealthBar = Content.Load<Texture2D>("health.png");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            //Keyboard states
            previousKbState = kbState;
            kbState = Keyboard.GetState();



            //Gameplay states
            switch (state)
            {
                case GameState.MainMenu:
                    ResetGame();
					// Has button location
					SetStats = new Button(buttonPosSetStats, 650, 720);
					// Has button size
					buttonPosSetStats = new Rectangle(SetStats.buttonX, SetStats.buttonY, 280, 80);
					previousms = ms;
					ms = Mouse.GetState();
					if (SetStats.enterButton() == true && previousms.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
                    { state = GameState.ItemMenu; }
                    break;
                case GameState.ItemMenu:
					
					// Has button size
					buttonPosPlay = new Rectangle(Play.buttonX, Play.buttonY, 150, 80);
					previousms = ms;
					ms = Mouse.GetState();
					if (Play.enterButton() == true && previousms.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
					{ state = GameState.PlayGame; }
                    break;
                case GameState.PlayGame:
                    if (mainChar.healthPoints <= 0)
                    { state = GameState.Gameover; }
                    else if (SingleKeyPress(Keys.P) == true)
                    { state = GameState.PauseMenu; }

                    //Function for player movement
                    CharacterMovement(mainChar);

                    //Player-Wall collision
                    mainChar.loc.Center.X = MathHelper.Clamp(mainChar.loc.Center.X, mainChar.loc.Radius + 50, 1550 - mainChar.loc.Radius);
                    mainChar.loc.Center.Y = MathHelper.Clamp(mainChar.loc.Center.Y, mainChar.loc.Radius + 50, 850 - mainChar.loc.Radius);

                    //Rotates the character to the mouse
                    ms = Mouse.GetState();
                    float xdist = ms.X - mainChar.loc.Center.X;
                    float ydist = ms.Y - mainChar.loc.Center.Y;
                    rotate = (float)(System.Math.Atan2(ydist, xdist) + 1.570);

                    blade.moveWeapon(mainChar, rotate);
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        if (blade.swingtime < 16)
                        {
                            leftMousePress = true;
                            foreach (Enemy e in testRoom.enemies)
                            {

                                if (e.playerIntersect(blade)&& e.checkAlive())
                                {
                                    Character.charHit(blade, e);
                                    e.healthPoints = 0;
                                }

                            }
                            blade.swingtime++;
                        }
                        else { leftMousePress = false; }
                    }
                    else { leftMousePress = false; blade.swingtime = 0; }

                    testRoom.RoomClear();

                    //Enemy AI function
                    for (int i = 0; i < testRoom.enemies.Count; i++)
                    {
                        //Rotates the enemy to the character
                        if (testRoom.enemies[i].checkAlive())
                        {
                            rotate2 = Character.getAngleBetween(mainChar, testRoom.enemies[i]);
                            testRoom.enemies[i].followChar(mainChar);
                        }
                    }





                    //Toggle between fullscreen and windowed with F11
                    if (SingleKeyPress(Keys.F11))
                    {
                        graphics.ToggleFullScreen();
                        graphics.ApplyChanges();
                    }
                    break;
                case GameState.PauseMenu:
					// Has button size
					buttonPosResume = new Rectangle(Resume.buttonX, Resume.buttonY, 240, 80);
					previousms = ms;
					ms = Mouse.GetState();
					if ((Resume.enterButton() == true && previousms.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
						|| (SingleKeyPress(Keys.P) == true))
                    { state = GameState.PlayGame; }
                    break;
                case GameState.Gameover:
					buttonPosRestart = new Rectangle(Restart.buttonX, Restart.buttonY, 240, 80);
					previousms = ms;
					ms = Mouse.GetState();
					if (Restart.enterButton() == true && previousms.LeftButton == ButtonState.Released && ms.LeftButton == ButtonState.Pressed)
					{ state = GameState.MainMenu; }
                    break;
            }



            //Exit on pressing escape
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) //Readded for ease of update
                Exit();



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            //States for animations and drawing
            switch (state)
            {
                case GameState.MainMenu:
					//menu filler art
					
                    spriteBatch.Draw(MenuBack, FullScreen, Color.White);
                    MainMenuText = "       Fight off enemies and try not to die! \nWASD to move, and click to swing your sword.\n       Press 'Set Stats' to build a character.";          
                    spriteBatch.Draw(Logo, LogoLoc, Color.White);
					
					spriteBatch.Draw(ButtonBack, buttonPosSetStats, Color.White);
					if (SetStats.enterButton() == true)
					{ spriteBatch.Draw(ButtonBack, buttonPosSetStats, Color.SandyBrown); }
					spriteBatch.DrawString(font, MainMenuText, CenterScreen, Color.SaddleBrown);
					spriteBatch.DrawString(font, "Set Stats", new Vector2(buttonPosSetStats.X + 32, buttonPosSetStats.Y+8), Color.Silver);

					break;
                case GameState.ItemMenu:
					
					spriteBatch.Draw(MenuBack, FullScreen, Color.White);
                    ItemMenuText = "                     Just kidding. \nLook at this pretty picture and press 'Play' to begin.";
                    spriteBatch.Draw(ItemMenu, StatsLoc, Color.White);
                    spriteBatch.DrawString(font, ItemMenuText, CenterScreen, Color.SaddleBrown);

					// Has button location
					Play = new Button(buttonPosPlay, 700, 720);
					spriteBatch.Draw(ButtonBack, buttonPosPlay, Color.White);
					if (Play.enterButton() == true)
					{ spriteBatch.Draw(ButtonBack, buttonPosPlay, Color.SandyBrown); }
					spriteBatch.DrawString(font, "Play", new Vector2(buttonPosPlay.X + 28, buttonPosPlay.Y + 8), Color.Silver);
					
					break;
                case GameState.PlayGame:
                    //walls, doors textures
                    //floor texture
                    //health bar, current weapon box
                    //spriteBatch.Draw(Character, characterPos, Color.White);
                    //enemies
                    //collision animations (create a method for this)
                    string health = (mainChar.healthPoints).ToString();
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    spriteBatch.DrawString(font, health, new Vector2(50, 40), Color.White);
                    


                    //Drawing Game Objects
                    spriteBatch.Draw(mainChar.getSprite(), mainChar.loc.Center, null, Color.White, rotate, mainChar.origin, 1.0f, SpriteEffects.None, 0f);

                    //Drawing enemies
                    for (int i = 0; i < testRoom.enemies.Count; i++)
                    {
                        Enemy enemyTemp = testRoom.enemies[i];
                        if (enemyTemp.checkAlive())
                            spriteBatch.Draw(testRoom.enemies[i].getSprite(), testRoom.enemies[i].loc.Center, null, Color.White, rotate2, testRoom.enemies[i].origin, 1.0f, SpriteEffects.None, 0f);
                    }


                    if (leftMousePress)
                    {

                        spriteBatch.Draw(blade.getSprite(), blade.loc.Center, null, Color.White, rotate + 3.926f - 0.1047f * blade.swingtime, blade.origin, 0.65f, SpriteEffects.None, 0f);


                    }

                    spriteBatch.Draw(fullHealthBar, new Rectangle(150, 50, 200, 40), Color.Black);
                    spriteBatch.Draw(healthBar, new Rectangle(150, 50, (int)mainChar.healthPoints * 2, 40), Color.White);

                    //Drawing walls
        testRoom.DrawWalls(spriteBatch);
                    


                    break;
                case GameState.PauseMenu:

                    PauseMenuText = "         The Game is Paused \n Press 'p' or 'Resume' to Resume.";
                    
                    health = (mainChar.healthPoints).ToString();
					spriteBatch.Draw(fullHealthBar, new Rectangle(150, 50, 200, 40), Color.Black);
					GraphicsDevice.Clear(Color.CornflowerBlue);

                    

                    //Drawing Game Objects
                    spriteBatch.Draw(mainChar.getSprite(), mainChar.loc.Center, null, Color.White, rotate, mainChar.origin, 1.0f, SpriteEffects.None, 0f);

                    //Drawing enemies
                    for (int i = 0; i < testRoom.enemies.Count; i++)
                    {
                        Enemy enemyTemp = testRoom.enemies[i];
                        
                        spriteBatch.Draw(testRoom.enemies[i].getSprite(), testRoom.enemies[i].loc.Center, null, Color.White, rotate2, testRoom.enemies[i].origin, 1.0f, SpriteEffects.None, 0f);
                    }


                    //Drawing walls
      testRoom.DrawWalls(spriteBatch);
                    spriteBatch.DrawString(font, PauseMenuText, CenterScreen, Color.SaddleBrown);

                    spriteBatch.DrawString(font, health, new Vector2(50, 40), Color.White);
                    spriteBatch.Draw(healthBar, new Rectangle(150, 50, (int)mainChar.healthPoints * 2, 40), Color.White);

					// Has button location
					Resume = new Button(buttonPosResume, 700, 720);
					spriteBatch.Draw(ButtonBack, buttonPosResume, Color.White);
					if (Play.enterButton() == true)
					{ spriteBatch.Draw(ButtonBack, buttonPosResume, Color.SandyBrown); }
					spriteBatch.DrawString(font, "Resume", new Vector2(buttonPosResume.X + 28, buttonPosResume.Y + 8), Color.Silver);

					break;
                case GameState.Gameover:
                    //game over background texture
                    //final stats
                    //buttons, back to main menu
                    spriteBatch.Draw(MenuBack, FullScreen, Color.White);
                    GameOverText = "     Ur ded Scrub";
                    spriteBatch.DrawString(font, GameOverText, CenterScreen, Color.SaddleBrown);
                    spriteBatch.Draw(RIP, RIPloc, Color.White);
					// Has button location
					Restart = new Button(buttonPosRestart, 700, 720);
					spriteBatch.Draw(ButtonBack, buttonPosRestart, Color.White);
					if (Restart.enterButton() == true)
					{ spriteBatch.Draw(ButtonBack, buttonPosRestart, Color.SandyBrown); }
					spriteBatch.DrawString(font, "Restart", new Vector2(buttonPosRestart.X + 28, buttonPosRestart.Y + 8), Color.Silver);
					break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Prevents a key from being pressed multiple times
        public bool SingleKeyPress(Keys k)
        {

            if (kbState.IsKeyDown(k) && previousKbState.IsKeyUp(k))
            { return true; }
            return false;
        }

		

		//Controls player wasd movement
		public void CharacterMovement(Character mc)
        {
            /*Code when we choose to use enums for player movement when we implement animations
            switch(direction)
            {
                case PlayerMovement.North:
                    break;
                case PlayerMovement.South:
                    break;
                case PlayerMovement.West:
                    break;
                case PlayerMovement.East:
                    break;
                case PlayerMovement.NorthWest:
                    break;
                case PlayerMovement.NorthEast:
                    break;
                case PlayerMovement.SouthEast:
                    break;
                case PlayerMovement.SouthWest:
                    break;
                case PlayerMovement.Static:
                    break;
              }
            */

            //Basic movement code for testing
            float speedmodifier = (float)(Math.Cos(0.785398) * movespeed);

            //move north
            if (kbState.IsKeyDown(Keys.W))
            {
                //move north west
                if (kbState.IsKeyDown(Keys.A))
                {
                    movement = new Vector2(-speedmodifier, -speedmodifier);

                }
                //move north east
                else if (kbState.IsKeyDown(Keys.D))
                    movement = new Vector2(speedmodifier, -speedmodifier);

                else
                    movement = new Vector2(0, -10);
            }

            //move south
            else if (kbState.IsKeyDown(Keys.S))
            {
                //move southwest
                if (kbState.IsKeyDown(Keys.A))
                    movement = new Vector2(-speedmodifier, speedmodifier);

                //move southeast
                else if (kbState.IsKeyDown(Keys.D))
                    movement = new Vector2(speedmodifier, speedmodifier);

                else
                    movement = new Vector2(0, 10);

            }

            //move west
            else if (kbState.IsKeyDown(Keys.A))
            {
                movement = new Vector2(-10, 0);
            }

            //move east
            else if (kbState.IsKeyDown(Keys.D))
            {
                movement = new Vector2(10, 0);
            }

            //when player is static    
            else
            {
                movement = new Vector2(0, 0);
            }

            mc.loc.Center += movement;
        }

        //Resets values that change during gameplay
        public void ResetGame()
        {

            mainChar.loc = new Circle(new Vector2(500, 500), mainChar.loc.Radius);
            mainChar.attackDamage = 10;
            mainChar.healthPoints = 100;
            enemyNo = 3;

        }


    }
}
