#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace TajTetrisGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TetrisGameRunner : Game
    {
        GraphicsDeviceManager graphics;
        
        //Game Requirements
        private InputHandler handler;
        private SpriteBatch spriteBatch;
        private FontHandler fontHandler;

        private Process Myself;

        private static BlendState original;
        private static BlendState light;
        private DragDropInterface dragDropInterface;
        private PrimitiveDrawer primitiveDrawer;
        private WorldEditor editor;
     
        private double totalFPS;
        private long count;
        private bool fixer;

        private const int VirtualWidth = 1024;
        private const int VirtualHeight = 768;

        private byte state; //this allows me to know what to do


        

        private int currentMouseX, currentMouseY;

        private MainMenuButton classic, story, options, maker, exit, anyLevel;
        private MainMenuButton Save, Load;
         private MainMenuButton cache, pieceMode, back;
        private MainMenuButton[] levels;
        private bool pieceModeA;
       
        private TajLevelLinker classicLink;
        private TajLevelLinker storyLink;
        private TajLevelLinker freePlayLink;
        private LevelPack package;
        private WorldRenderer mainMenu, optionsMenu;

        /// <summary>
        /// Constructor for the Game.
        /// </summary>
        public TetrisGameRunner()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            handler = new InputHandler();
            original = null;
            Myself = Process.GetCurrentProcess();
            light = new BlendState();
            light.ColorBlendFunction = BlendFunction.Add;
            light.ColorSourceBlend = Blend.DestinationColor;
            light.ColorDestinationBlend = Blend.One;
            editor = new WorldEditor();
            classic = new MainMenuButton("Play Classic");
            story = new MainMenuButton("Beta Story Mode");
            story.ModifyY(51 * 4);
            options = new MainMenuButton("Options");
            options.ModifyY(51*2);
            maker = new MainMenuButton("Make Levels");
            maker.ModifyY(51*3);
            anyLevel = new MainMenuButton("Play Any Level");
            anyLevel.ModifyY(51);
            exit = new MainMenuButton("Exit");
            exit.ModifyY(51 * 5);

            
            package = new LevelPack();
            package.AddLevel("Volcano");
            package.AddLevel("Chef");

            package.AddLevel("RaiseDaRoof");

            package.AddLevel("School");

            package.AddLevel("Vault");

            package.AddLevel("TheMugging");

            package.AddLevel("CabinetDiver");

            package.AddLevel("Programmer");
            package.UnlockUpTo(1);
           
            cache = new MainMenuButton("Cache Off");
            
            pieceMode = new MainMenuButton("Piece Mode A");
            pieceMode.ModifyY(51);
            back = new MainMenuButton("Back To Menu");
            back.ModifyY(51 * 2);

            Save = new MainMenuButton("Save");
            Load = new MainMenuButton("Load");
            Load.ModifyY(51);

            graphics.PreferredBackBufferHeight = VirtualHeight;
            graphics.PreferredBackBufferWidth = VirtualWidth;
            
            classicLink = new TajLevelLinker();
            classicLink.OnStart("ClassicTetris 0 %Score \"Score:`0\" SetText");
            classicLink.SetRowCleared("\"Score:`\" $Score 1 + string + SetText $Score 1 + %Score del");
            
           
            /*
            TajParser p = new TajParser();
            p.Parse("\"Hello`World`\" 10 string +");
            p.Parse("10 20 + 10 * %Hello");
            p.Parse("$Hello $Hello + %Corn");
            p.Parse("$Corn $Corn * $Corn + %g");
            p.Parse("\"Hello`world`I`am`a`computer.`\" %Computer");
            SaveFileSystem.SaveObjectToFile(p, "Heh.taj");
            p = SaveFileSystem.LoadObjectFromFile<TajParser>("Heh.taj", new GameLoader());

            Console.Beep();
            */
        }

        /// <summary>
        /// Gets the light state of rendering.
        /// </summary>
        /// <returns></returns>
        public static BlendState LightState()
        {

            return light;
        }


        /// <summary>
        /// Gets the Original State of Rendering.
        /// </summary>
        /// <returns></returns>
        public static BlendState OriginalState()
        {

            return original;
        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.IsFullScreen = true;

            
            base.Initialize();
        }


        /// <summary>
        /// Gives the Offset of the X. Deprecated.
        /// Not bothering to remove because a compiler will do this itself.
        /// </summary>
        /// <returns></returns>
        public static int GetOffsetX()
        {
            return 0;
        }

        /// <summary>
        /// Gives the Offset of the Y. Deprecated
        /// Not bothering to remove because a compiler will do this itself.
        /// </summary>
        /// <returns></returns>
        public static int GetOffsetY()
        {
            return 0;
        }
       

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fontHandler = new FontHandler();
            Logger.Load();

            mainMenu = new WorldRenderer(SaveFileSystem.LoadObjectFromFile<World>("Levels/MainMenuLevel.taj", new GameLoader()));
            optionsMenu = new WorldRenderer(SaveFileSystem.LoadObjectFromFile<World>("Levels/OptionMenuLevel.taj", new GameLoader()));
            
            fontHandler.LoadContent(Content);
            SoundEffectInstance song =Content.Load<SoundEffect>("NewTetris").CreateInstance();
            song.IsLooped = true;
            song.Play();
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        Loader load = new GameLoader();
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if(primitiveDrawer == null)
            {
                primitiveDrawer = new PrimitiveDrawer(GraphicsDevice.Viewport.TitleSafeArea.Width, GraphicsDevice.Viewport.TitleSafeArea.Height);
                primitiveDrawer.Setup(GraphicsDevice, 0, 0);
                dragDropInterface = new DragDropInterface(VirtualWidth, VirtualHeight);
            
            }

            handler.Update();

           
            

            if(handler.CheckPressedKey(Keys.LeftShift) && handler.CheckPressedKey(Keys.Escape))
            {
                state = 0;
            }

            currentMouseX = handler.GetMouseX();
            currentMouseY = handler.GetMouseY();
           
            mainMenu.Update(handler);


            if(state == 2)
            {

                if (handler.CheckJustPressedKey(Keys.Escape))
                {
                    state = 7;
                }

                editor.Update(handler);
                if(editor.Finished())
                {
                    string fileName = editor.GetFileName();
                    string safeFileName = editor.GetSafeFileName();
                    dragDropInterface.SetFile(fileName, safeFileName);
                    state = 3;
                }
            }
            else
            if(state ==4)
            {
                cache.Update(handler);
                pieceMode.Update(handler);
                back.Update(handler);
                optionsMenu.Update(handler);
                if (handler.CheckMouseIn(back) && handler.CheckLeftMouseJustPressed())
                {
                    state = 0;
                }

                if (handler.CheckMouseIn(pieceMode) && handler.CheckLeftMouseJustPressed())
                {
                    pieceModeA = !pieceModeA;

                    pieceMode.SetText((pieceModeA) ? "Piece Mode B" : "Piece Mode A");
                    ClassicTetrisGame.SetPieceMode(pieceModeA);
                }
            }
            else
            if (state == 0)
            {
                classic.Update(handler);
                maker.Update(handler);
                story.Update(handler);

                anyLevel.Update(handler);
                options.Update(handler);
                exit.Update(handler);

                if (handler.CheckMouseIn(options) && handler.CheckLeftMouseJustPressed())
                {
                    state = 4;
                    //classicLink.CreateGame("TajLevel.taj");
                }

                if (handler.CheckMouseIn(story) && handler.CheckLeftMouseJustPressed())
                {
                    string[] levels = package.GetProtectedLevels();

                    this.levels = new MainMenuButton[levels.Length];
                    
                    for (int i = 0;i < levels.Length; i++)
                    {
                        this.levels[i] = new MainMenuButton(levels[i]);
                        this.levels[i].ModifyY(51 * i);
                    }

                        state = 9;
                }


                if (handler.CheckMouseIn(anyLevel) && handler.CheckLeftMouseJustPressed())
                {
                    string[] levelsN = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/TajTetris/LevelPacks");
                    
                    List<string> levels = new List<string>();
                    for (int i = 0;i < levelsN.Length; i++)
                    {
                        if(levelsN[i].EndsWith(".taj"))
                        levels.Add(levelsN[i]);
                    }

                    this.levels = new MainMenuButton[levels.Count];

                    for (int i = 0;i < levels.Count;i++)
                    {
                        this.levels[i] = new MainMenuButton(Path.GetFileNameWithoutExtension(levels[i]));
                        this.levels[i].ModifyY(51 * i);
                    }

                    state = 8;
                }


                if (handler.CheckMouseIn(classic) && handler.CheckLeftMouseJustPressed())
                {
                    classicLink.CreateGame("TajLevel.taj");
                    Logger.WriteLine("Started new Classic Game.");
                    state = 1;   
                }

                if (handler.CheckMouseIn(maker) && handler.CheckLeftMouseJustPressed())
                {
                    state = 2;
                }

                if (handler.CheckMouseIn(exit) && handler.CheckLeftMouseJustPressed())
                {
                    this.Exit();
                }
            }
            else
            if(state == 1)
            {

                classicLink.Update(handler);

                if(handler.CheckJustPressedKey(Keys.Escape))
                {
                    state = 5;
                }
            }
            else
            if(state == 3)
            {
                dragDropInterface.Update(handler);
            }
            else
           //dragDropInterface.Update(handler);
            
            if (state == 5 || state == 12 || state == 13)
            {
                Save.Update(handler);
                Load.Update(handler);
                back.Update(handler);

                if (handler.CheckMouseIn(back) && handler.CheckLeftMouseJustPressed())
                {
                    state = 0;
                }

                if (state == 5)
                {
                    if (handler.CheckMouseIn(Save) && handler.CheckLeftMouseJustPressed())
                    {

                        state = 1;
                        classicLink.SetFullSave(true);
                            
                        Logger.WriteLine("Saved Classic Game.");
                        SaveFileSystem.SaveObjectToFile(classicLink, "ClassicGame.tajSave");
                    }

                    if (handler.CheckMouseIn(Load) && handler.CheckLeftMouseJustPressed())
                    {
                        classicLink = SaveFileSystem.LoadObjectFromFile<TajLevelLinker>("ClassicGame.tajSave", new GameLoader());

                        Logger.WriteLine("Loaded Classic Game.");
                        state = 1;
                    }

                    if (handler.CheckJustPressedKey(Keys.Escape))
                    {
                        state = 1;
                    }

                }

                if (state == 12)
                {
                    if (handler.CheckMouseIn(Save) && handler.CheckLeftMouseJustPressed())
                    {

                        state = 11;
                        freePlayLink.SetFullSave(true);
                        Logger.WriteLine("Saved Free Play Game.");
                        SaveFileSystem.SaveObjectToFile(freePlayLink, "FreePlayGame.tajSave");
                    }

                    if (handler.CheckMouseIn(Load) && handler.CheckLeftMouseJustPressed())
                    {
                        freePlayLink = SaveFileSystem.LoadObjectFromFile<TajLevelLinker>("FreePlayGame.tajSave", new GameLoader());

                        Logger.WriteLine("Loaded Freeplay Game.");
                        state = 11;
                    }

                    if (handler.CheckJustPressedKey(Keys.Escape))
                    {
                        state = 11;
                    }

                }

                if (state == 13)
                {
                    if (handler.CheckMouseIn(Save) && handler.CheckLeftMouseJustPressed())
                    {

                        state = 10;
                        classicLink.SetFullSave(true);

                        Logger.WriteLine("Saved Story Game.");
                        SaveFileSystem.SaveObjectToFile(classicLink, "StoryGame.tajSave");
                    }

                    if (handler.CheckMouseIn(Load) && handler.CheckLeftMouseJustPressed())
                    {
                        classicLink = SaveFileSystem.LoadObjectFromFile<TajLevelLinker>("StoryGame.tajSave", new GameLoader());

                        Logger.WriteLine("Loaded Story Game.");
                        state = 10;
                    }

                    if (handler.CheckJustPressedKey(Keys.Escape))
                    {
                        state = 10;
                    }

                }
            }

            else if (state ==7)
            {
                back.Update(handler);
                if (handler.CheckJustPressedKey(Keys.Escape))
                {
                    state = 2;
                }
                if (handler.CheckMouseIn(back) && handler.CheckLeftMouseJustPressed())
                {
                    state = 0;
                }

            }
            else if (state == 8)
            {
                optionsMenu.Update(handler);


                if (handler.CheckJustPressedKey(Keys.Escape))
                {
                    state = 0;
                }

                foreach (MainMenuButton b in levels)
                {
                    b.Update(handler);
                    b.ModifyY((int)(handler.LeftMouseDraggedBy().Y / 40.0f));


                    if (handler.CheckMouseIn(b) && handler.CheckLeftMouseJustPressed())
                    {
                        
                            freePlayLink = SaveFileSystem.LoadObjectFromFile<TajLevelLinker>("LevelPacks/" + b.GetText() + ".taj", new GameLoader());
                            state = 11;
                            break;
                    }
                }
            }
            else if (state == 9)
            {
                optionsMenu.Update(handler);
                

                if(handler.CheckJustPressedKey(Keys.Escape))
                {
                    state = 0;
                }

               foreach(MainMenuButton b in levels)
               {
                   b.Update(handler);
                   b.ModifyY((int)(handler.LeftMouseDraggedBy().Y / 40.0f));


                   if (handler.CheckMouseIn(b) && handler.CheckLeftMouseJustPressed())
                   {
                       if(b.GetText() != "Locked")
                       {
                           storyLink = SaveFileSystem.LoadObjectFromFile<TajLevelLinker>("LevelPacks/" + b.GetText() + ".taj", new GameLoader());
                           storyLink.SetLevelPack(package);
                           state = 10;
                           break;
                       }
                   }
               }
                
            } else if (state == 10)
            {
                if (handler.CheckJustPressedKey(Keys.Escape))
                {
                    state = 13;
                }
                storyLink.Update(handler);

            }
            else if (state == 11)
            {

                if (handler.CheckJustPressedKey(Keys.Escape))
                {
                    state = 12;
                }
                freePlayLink.Update(handler);
            }
            


            //lnk.Update(handler);
            //tGame.Update(handler);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if(original == null)
            {
                original = BlendState.NonPremultiplied;
            }

            //pd.DrawLine(GraphicsDevice, new Vector2(0, 0), new Vector2(100, 100), Color.Red);
            // TODO: Add your drawing code here
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            totalFPS += (int)frameRate;
            count++;

          
            spriteBatch.Begin();
            primitiveDrawer.DrawFilledRectangle(GraphicsDevice, new Rectangle(0, 0, VirtualWidth, VirtualHeight), Color.DarkGray);
            
            if(false)
             if (fixer)
            {
                spriteBatch.DrawString(fontHandler.GetVerdana(), "" + frameRate + " " + (totalFPS / count) + " " + Myself.PrivateMemorySize64/1024/1024 + "MB", Vector2.Zero, Color.Red);
            }
            else
            if (count == 200)
            {
                count = 0;
                totalFPS = 0;
                fixer = true;
            }

            if(state ==9 || state == 8)
            {
                spriteBatch.DrawString(fontHandler.GetVerdana(), "Click and Drag Mouse to scroll\nEscape to return to Main Menu", Vector2.Zero, Color.White);
                optionsMenu.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);
                
               foreach(MainMenuButton b in levels)
               {
                   b.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);

               }
                
            }

            if (state == 10 || state == 13)
            {
                storyLink.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);

            }

            if(state == 11 || state == 12)
            {
                freePlayLink.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);
            }

            if(state == 0)
            {
                mainMenu.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);
                classic.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                exit.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                story.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                options.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                anyLevel.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                maker.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
            }

            if(state == 4)
            {
                optionsMenu.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);
                
                back.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                cache.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                pieceMode.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
            }

            if(state == 2 || state == 7)
            {
                editor.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);

                spriteBatch.DrawString(fontHandler.GetVerdana(), "Read the Manual to Use this Tool.\nWarning:Not very user friendly", Vector2.Zero, Color.White);
            }


            if(state == 3)
            {
                dragDropInterface.Draw(spriteBatch, primitiveDrawer, fontHandler, GraphicsDevice);
            }

            if(state == 1 || state == 5)
            {
                classicLink.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);
            }

            

            if (state == 5 || state == 12 || state == 13)
            {
                Save.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                Load.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
                back.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);
            }




            if(state == 7)
            {
                back.Draw(spriteBatch, fontHandler, GraphicsDevice, primitiveDrawer);

            }


             //dragDropInterface.Draw(spriteBatch, primitiveDrawer, fontHandler, GraphicsDevice);
            //lnk.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);
           //tGame.Draw(GraphicsDevice, spriteBatch, primitiveDrawer, fontHandler);
          //cGame.Draw(GraphicsDevice, primitiveDrawer, spriteBatch, fontHandler);


          
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Returns what to shift the game by. Deprecated.
        /// A Compiler will likely remove this.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetOffsetVector()
        {
            return Vector2.Zero;
        }
    }
}
