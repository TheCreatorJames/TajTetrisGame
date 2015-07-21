using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class TajLevelLinker : TetrisEventLinker, StackObject
    {
        private TajParser parser;
        private WorldRenderer world;
        private ClassicTetrisGame game;
        private LevelPack pack;
        private string text;
        private bool GameEnded;
        private bool GameLost;
        private string fileName;

        private bool flashLight;

        private bool fullSave; //little technique I thought of.

        private string rowCleared = "Nothing";
        private string redCleared = "Nothing";
        private string greenCleared = "Nothing";
        private string blueCleared = "Nothing";
        private string yellowCleared = "Nothing";
        private string tealCleared = "Nothing";
        private string purpleCleared = "Nothing";
        private string orangeCleared = "Nothing";
        private string anyCleared= "Nothing";

        private string Start = "Nothing";

        public TajLevelLinker()
        {
        }


        public void SetFlashlight(bool n)
        {
            this.flashLight = n;
        }

        

        public void CreateGame(String fileName)
        {
            parser = new TajParser();
            GameEnded = false;
            flashLight = false;

            GameLost = false;
            if(fileName.IndexOf("Levels" + Path.DirectorySeparatorChar) != 0)
            {
                fileName = "Levels" + Path.DirectorySeparatorChar + fileName;
            }
            //code to load in world
            world = new WorldRenderer(SaveFileSystem.LoadObjectFromFile<World>(fileName, new GameLoader()));
            this.parser.SetCustomHandler(this);
            game = new ClassicTetrisGame();

            game.SetEventLink(this);
            parser.Parse(Start);
        }

        public void ParseStart()
        {
            parser.Parse(Start);
        }


        public void SetParser(TajParser p)
        {
            this.parser = p;
            this.parser.SetCustomHandler(this);
        }

        public void SetWorld(World p)
        {
            this.world = new WorldRenderer(p);
        }

        public void SetGame(ClassicTetrisGame game)
        {
            this.game = game;
            this.game.SetEventLink(this);
        }


        public void OnStart(string n)
        {

            this.Start
                 = n;
        }
        public void SetRowCleared(String n)
        {
            this.rowCleared = n;
        }

        public void SetRedCleared(String n)
        {
            this.redCleared = n;
        }
        public void SetGreenCleared(String n)
        {
            this.greenCleared = n;
        }

        public void SetBlueCleared(String n)
        {
            this.blueCleared = n;
        }
        public void SetTealCleared(String n)
        {
            this.tealCleared = n;
        }

        public void SetYellowCleared(String n)
        {
            this.yellowCleared = n;
        }

        public void SetPurpleCleared(String n)
        {
            this.purpleCleared = n;
        }
        public void SetOrangeCleared(String n)
        {
            this.orangeCleared = n;
        }
        public void SetAnyCleared(String n)
        {
            this.anyCleared = n;
        }


        public override void RowCleared()
        {
            if(rowCleared != "Nothing")
            {
                parser.Parse(rowCleared);

            }
        }

        public override void RedCleared(int amount)
        {
            if (redCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(redCleared);

            }
        }

        public override void GreenCleared(int amount)
        {
            if (greenCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(greenCleared);

            }
        }

        public void SetFullSave(bool x)
        {
            this.fullSave = x;
        }
        public override void BlueCleared(int amount)
        {
            if (blueCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(blueCleared);

            }
        }

        public override void OrangeCleared(int amount)
        {
            if (orangeCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(orangeCleared);

            }
        }

        public override void TealCleared(int amount)
        {
            if (tealCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(tealCleared);

            }
        }

        public override void PurpleCleared(int amount)
        {
            if (purpleCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(purpleCleared);

            }
        }

        public void SetText(string text)
        {
            this.text = text;
        }

        public override void YellowCleared(int amount)
        {
            if (yellowCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(yellowCleared);

            }
        }

        public void SetLevelPack(LevelPack pack)
        {
            this.pack = pack;
        }

        public override void AnyColorCleared(int amount)
        {
            if (anyCleared != "Nothing")
            {
                parser.Parse(amount + " %Amount");
                parser.Parse(anyCleared);

            }
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch batch, PrimitiveDrawer drawer, FontHandler font)
        {
             world.Draw(graphicsDevice, batch, drawer, font);
            
            game.Draw(graphicsDevice, drawer, batch, font);

            

            if(GameEnded)
            {

                if(GameLost)
                batch.DrawString(font.GetVerdana(), "You Lost!", new Vector2(320,0) + TetrisGameRunner.GetOffsetVector(), Color.White);
                else
                batch.DrawString(font.GetVerdana(), "You Win!", new Vector2(320, 0) + TetrisGameRunner.GetOffsetVector(), Color.White);
            }
           batch.DrawString(font.GetVerdana(), text, new Vector2(320, 0) + TetrisGameRunner.GetOffsetVector(), Color.White);
        }

        public void Update(InputHandler handler)
        {

            if (!GameEnded)
            {
                game.Update(handler);
                world.Update(handler);
            }
        }
        
        public string GetFileName()
        {
            return fileName;
        }





        public void Save(Saver saver)
        {
            if(!fullSave)
            saver.Header("TajLevelLinker");
            if (fullSave) saver.Header("TajLevelSave");
            saver.Save(rowCleared, "RowCleared");
            saver.Save(redCleared, "RedCleared");
            saver.Save(blueCleared, "BlueCleared");
            saver.Save(greenCleared, "GreenCleared");
            saver.Save(tealCleared, "TealCleared");
            saver.Save(orangeCleared, "OrangeCleared");
            saver.Save(yellowCleared, "YellowCleared");
            saver.Save(anyCleared, "AnyCleared");
            saver.Save(purpleCleared, "PurpleCleared");
            saver.Save(Start, "Start");
            if(!fullSave)
            saver.Save(fileName, "FileName");
            if (fullSave)
            {
                saver.Save(parser, "Parser");
                saver.Save(text, "Text");

                saver.Save(flashLight, "Flashlight");
                saver.Save(game, "Game");
                saver.Save(new World(world.GetObjects()), "World");
            }

            saver.End();
        }

        public bool ExecuteCommand(string command, Stacker stack)
        {
            //"ClassicTetris", "RowMode", "SClusterMode", "BClusterMode", "MCluster Mode", "FlashlightMode", "WinGame", "EndGame", "ClearBoard"
            if(command == "ClassicTetris")
            {
                game.SetClassicMode();
            }
            else if (command == "RowMode")
            {
                game.SetRowMode();
            } else if(command == "UsePentaPieces")
            {
                game.AddPentonimoes();
            }
            else if (command == "SClusterMode")
            {
                game.SetClusterMode(9, 100);
            }
            else if (command == "MClusterMode")
            {
                game.SetClusterMode(16, 100);
            }
            else if (command == "BClusterMode")
            {
                game.SetClusterMode(20, 100);
            }
            else if (command == "FlashlightMode")
            {
                //flashlight
                flashLight = true;
                game.SetFlashLight(true);
            }
            else if (command == "WinGame")
            {
                GameEnded = true;
                GameLost = false;
                if(pack != null)
                pack.UnlockNext();
                //win
            }
            else if (command == "SetText")
            {

                if(stack.Peek() is StackObjectString)
                {
                    text = ((StackObjectString)stack.Pop()).GetValue();
                }
                //win
            }
            else if (command == "EndGame")
            {
                GameEnded = GameLost = true;
                //lose
            }
            else if (command == "ClearBoard")
            {
                game.SetBoard(new ByteBoard());
            }


            return false;
        }

        public bool GetFinished()
        {
            return GameEnded;
        }

        public void SetFileName(string p)
        {
            this.fileName = p;    
            this.world = new WorldRenderer(SaveFileSystem.LoadObjectFromFile<World>(fileName, new GameLoader()));
           
        }
    }
}
