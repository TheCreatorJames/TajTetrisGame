using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class GameLoader : Loader
    {
        private void BuildPerson()
        {
            TestPerson person = new TestPerson("", 0);

            if(variablesToBuild.Peek().ContainsKey("Name") && variablesToBuild.Peek().ContainsKey("Age"))
            {
                person.SetName((String)variablesToBuild.Peek()["Name"]);
                person.SetAge((Int32)variablesToBuild.Peek()["Age"]);
                
                buildStack.Push(person);
            } else
            {
            }


            currentClass.Pop();
            variablesToBuild.Pop();

        }


        
        protected override void BuildClass()
        {
            switch(currentClass.Peek())
            {
            case "BodyAnimation":
                BuildAnimation();
                break;
            case "ArmTool":
                BuildArm();
                break;
            case "Person":
                BuildPerson();
                break;
            case "Button":
                BuildButton();
                break;
            case "ClassicTetris":
                BuildTetris();
                break;
            case "ByteBoard":
                BuildBoard();
                break;
            case "Stacker":
                BuildStacker();
                break;
            case "TajParser":
                BuildParser();
                break;
            case "StackString":
                BuildStackString();
                break;
            case "StackBoolean":
                BuildStackBoolean();
                break;
            case "StackNumber":
                BuildStackNumber();
                break;
            case "Color":
                BuildColor();
                break;
            case "LRect":
                BuildLevelRectangle();
                break;
            case "LCircle":
                BuildLevelCircle();
                break;
            case "World":
                BuildLevel();
                break;

            case "LevelPerson":
                BuildLevelPerson();
                break;
            case "LightLevelCircle":
                BuildLightLevelCircle();
                break;

            case "TajLevelSave":
                BuildTajLevelSave();
                break;

            case "TajLevelLinker":
                BuildTajLevelLinker();
                break;
            }
        }

        private void BuildTajLevelSave()
        {
            TajLevelLinker tjl = new TajLevelLinker();
            tjl.SetRedCleared((string)variablesToBuild.Peek()["RedCleared"]);
            tjl.SetBlueCleared((string)variablesToBuild.Peek()["BlueCleared"]);
            tjl.SetTealCleared((string)variablesToBuild.Peek()["TealCleared"]);
            tjl.SetYellowCleared((string)variablesToBuild.Peek()["YellowCleared"]);
            tjl.SetPurpleCleared((string)variablesToBuild.Peek()["PurpleCleared"]);
            tjl.SetGreenCleared((string)variablesToBuild.Peek()["GreenCleared"]);
            tjl.SetOrangeCleared((string)variablesToBuild.Peek()["OrangeCleared"]);
            tjl.SetAnyCleared((string)variablesToBuild.Peek()["AnyCleared"]);
            tjl.SetRowCleared((string)variablesToBuild.Peek()["RowCleared"]);


            tjl.SetGame((ClassicTetrisGame)variablesToBuild.Peek()["Game"]);
            tjl.SetFlashlight((bool)variablesToBuild.Peek()["Flashlight"]);
            if (variablesToBuild.Peek().ContainsKey("Text"))
                tjl.SetText((String)variablesToBuild.Peek()["Text"]);
            else tjl.SetText("");
            tjl.SetParser((TajParser)variablesToBuild.Peek()["Parser"]);
            tjl.OnStart((string)variablesToBuild.Peek()["Start"]);
            
            tjl.SetWorld((World)variablesToBuild.Peek()["World"]);
            buildStack.Push(tjl);
            currentClass.Pop();
            variablesToBuild.Pop();
        }


        private void BuildTajLevelLinker()
        {
            TajLevelLinker tjl = new TajLevelLinker();
            tjl.SetRedCleared((string)variablesToBuild.Peek()["RedCleared"]);
            tjl.SetBlueCleared((string)variablesToBuild.Peek()["BlueCleared"]);
            tjl.SetTealCleared((string)variablesToBuild.Peek()["TealCleared"]);
            tjl.SetYellowCleared((string)variablesToBuild.Peek()["YellowCleared"]);
            tjl.SetPurpleCleared((string)variablesToBuild.Peek()["PurpleCleared"]);
            tjl.SetGreenCleared((string)variablesToBuild.Peek()["GreenCleared"]);
            tjl.SetOrangeCleared((string)variablesToBuild.Peek()["OrangeCleared"]);
            tjl.SetAnyCleared((string)variablesToBuild.Peek()["AnyCleared"]);
            tjl.SetRowCleared((string)variablesToBuild.Peek()["RowCleared"]);

            tjl.OnStart((string)variablesToBuild.Peek()["Start"]);
            tjl.SetFileName((string)variablesToBuild.Peek()["FileName"]);

            buildStack.Push(tjl);
            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildLightLevelCircle()
        {
           

            LightLevelCircle lc = new LightLevelCircle((int)variablesToBuild.Peek()["X"], (int)variablesToBuild.Peek()["Y"], (int)variablesToBuild.Peek()["Radius"]);
            lc.SetColor((Color)variablesToBuild.Peek()["Color"]);

            if (variablesToBuild.Peek().ContainsKey("Culled"))
                lc.SetCulled((bool)variablesToBuild.Peek()["Culled"]);
            else lc.SetCulled(true);
            buildStack.Push(lc);
            currentClass.Pop();
            variablesToBuild.Pop();
            
        }

        private void BuildLevelPerson()
        {

            LevelPerson lp = new LevelPerson((int)variablesToBuild.Peek()["X"], (int)variablesToBuild.Peek()["Y"], (int)variablesToBuild.Peek()["Width"], (int)variablesToBuild.Peek()["Height"], (String)variablesToBuild.Peek()["BodyAnimation"]);
            lp.SetFrame((int)variablesToBuild.Peek()["Frame"]);
            lp.SetLoop((bool)variablesToBuild.Peek()["Loop"]);
            if(variablesToBuild.Peek().ContainsKey("Name"))
            {
                lp.SetName((String)variablesToBuild.Peek()["Name"]);
            }
            lp.SetPlaying((bool)variablesToBuild.Peek()["Playing"]);
            lp.SetBorderColor((Color)variablesToBuild.Peek()["Border"]);
            lp.SetColor((Color)variablesToBuild.Peek()["Color"]);
            buildStack.Push(lp);
            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildLevelCircle()
        {
            LevelCircle lc = new LevelCircle((int)variablesToBuild.Peek()["X"], (int)variablesToBuild.Peek()["Y"], (int)variablesToBuild.Peek()["Radius"]);
            lc.SetColor((Color)variablesToBuild.Peek()["Color"]);
            lc.SetBorderColor(Color.Black);
            buildStack.Push(lc);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildColor()
        {
            Color n = new Color((byte)variablesToBuild.Peek()["R"], (byte)variablesToBuild.Peek()["G"], (byte)variablesToBuild.Peek()["B"], (byte)variablesToBuild.Peek()["A"]);
            buildStack.Push(n);
            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildLevel()
        {
            buildStack.Push(new World(convertArray<LevelObject>((Object[])variablesToBuild.Peek()["Objects"])));
            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildLevelRectangle()
        {
            LevelRectangle lr = new LevelRectangle((int)variablesToBuild.Peek()["X"], (int)variablesToBuild.Peek()["Y"], (int)variablesToBuild.Peek()["Width"], (int)variablesToBuild.Peek()["Height"]);
            lr.SetColor((Color)variablesToBuild.Peek()["Color"]);
            lr.SetBorderColor(Color.Black);
            buildStack.Push(lr);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        #region These are the build methods.
        #region These are not documented because they are quite obvious in function. They all serve the same purpose
        private void BuildStackBoolean()
        {
            StackObjectBoolean b = new StackObjectBoolean((bool)variablesToBuild.Peek()["Value"]);
            buildStack.Push(b);

            currentClass.Pop();
            variablesToBuild.Pop();

        }

        private void BuildStackNumber()
        {
            StackObjectNumber b = new StackObjectNumber((float)variablesToBuild.Peek()["Value"]);
            buildStack.Push(b);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildStackString()
        {
            StackObjectString b = new StackObjectString((string)variablesToBuild.Peek()["Value"]);
            buildStack.Push(b);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildParser()
        {
            TajParser p = new TajParser();

            p.SetStack((Stacker)variablesToBuild.Peek()["Stack"]);
            String[] s = convertArray<String>((Object[])(variablesToBuild.Peek()["Keys"]));
            StackObject[] b = convertArray<StackObject>((Object[])(variablesToBuild.Peek()["Values"]));
            p.SetVariables(s, b);

            buildStack.Push(p);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildStacker()
        {
            Stacker stack = new Stacker();
            stack.SetStack(convertArray<StackObject>((Object[])variablesToBuild.Peek()["Stack"]));
            buildStack.Push(stack);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildAnimation()
        {
            BodyAnimation a = new BodyAnimation((ArmTool)(variablesToBuild.Peek()["Left"]),(ArmTool)(variablesToBuild.Peek()["Right"]), (ArmTool)(variablesToBuild.Peek()["LegLeft"]),(ArmTool)(variablesToBuild.Peek()["LegRight"]), (int)(variablesToBuild.Peek()["Frames"]));
            buildStack.Push(a);

            currentClass.Pop();
            variablesToBuild.Pop();

        }

        private void BuildArm()
        {
            ArmTool arm = new ArmTool();

            arm.SetPoints(convertArray<short>((Object[])variablesToBuild.Peek()["Points"]));
            buildStack.Push(arm);
            currentClass.Pop();
            variablesToBuild.Pop();

        }

        private void BuildBoard()
        {
            ByteBoard board = new ByteBoard();
            board.SetBoard(convertArray<byte>((Object[])variablesToBuild.Peek()["Board"]));
            buildStack.Push(board);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildTetris()
        {
            ClassicTetrisGame game = new ClassicTetrisGame();
            game.SetX((int)variablesToBuild.Peek()["X"]);
            game.SetY((int)variablesToBuild.Peek()["Y"]);
            game.SetPiece((string)variablesToBuild.Peek()["Piece"]);
            game.SetColor((byte)variablesToBuild.Peek()["Color"]);
            game.SetBoard((ByteBoard)variablesToBuild.Peek()["TheBoard"]);

            game.SetFlashLight((bool)variablesToBuild.Peek()["Flashlight"]);
         
            if((byte)variablesToBuild.Peek()["Mode"] == 1)
            {
                game.SetClusterMode((byte)variablesToBuild.Peek()["Low"], (byte)variablesToBuild.Peek()["High"]);
            }
            else if ((byte)variablesToBuild.Peek()["Mode"] == 2)
            {
                game.SetRowMode();
            }
            else
            {
                game.SetClassicMode();
            }

            game.SetPowerUp((bool)variablesToBuild.Peek()["PowerUp"]);

            game.ClearPieces();
            game.AddPieces(convertArray<String>((object[])variablesToBuild.Peek()["Pieces"]));
            buildStack.Push(game);

            currentClass.Pop();
            variablesToBuild.Pop();
        }

        private void BuildButton()
        {
            TestButton butt = new TestButton(0, 0);

            butt.SetX((int)variablesToBuild.Peek()["X"]);
            butt.SetY((int)variablesToBuild.Peek()["Y"]);

            buildStack.Push(butt);

            currentClass.Pop();
            variablesToBuild.Pop();
        }
        #endregion
        #endregion

    }
}
