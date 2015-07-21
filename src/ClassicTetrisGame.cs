using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    enum TetrisGameMode : byte
    {
        Classic,
        Cluster,
        Rows
    }


    class ClassicTetrisGame : Saveable
    {

        //Used for an option.
        private static bool pieceMode;
        private TetrisEventLinker eventLinker;

        private ByteBoard board;
        private int[] cluster;

        private TetrisGameMode mode;
        private byte low, high;      //Only used in some game modes.
        private bool powerUp, flashLight;

        private GameObject workPiece;
        private int x, y;
        private string piece;
        private byte col = 1;
        private int mouseX, mouseY;

        private int size = 32;
        
        private PseudoRandomGenerator generator;
        
        private sbyte sliding;
        private byte timeUntilNextFall, speedFall, timeUntilDrop;
            
        private List<string> pieces;
        private Color[] colors;
        
        /// <summary>
        /// Constructs a classic tetris game.
        /// </summary>
        public ClassicTetrisGame()
        {
            this.workPiece = new GameRectangle();
            this.board = new ByteBoard();
            this.pieces = new List<string>();
            this.generator = new PseudoRandomGenerator();
            this.colors = new Color[7];
            this.powerUp = false;
            SetClassicMode();
            this.x = 3; 
            this.y = 1;

            AddTetronimoes();

            col = (byte)(generator.GetRandomByte() % pieces.Count + 1);
            piece = pieces[col - 1];
            col %= 7;
            col++;

            colors[0] = Color.Red; //Red
            colors[1] = Color.Lime; //Green
            colors[2] = Color.Blue; //Blue
            colors[3] = Color.Yellow; //Yellow
            colors[4] = Color.Orange; //Orange
            colors[5] = Color.Purple; //Purple
            colors[6] = Color.Turquoise; //Teal

            board.SetBoardCommand(x, y, piece, 1);
        }

        #region Mode Setting.
        /// <summary>
        /// Set the mode to classic mode.
        /// </summary>
        public void SetClassicMode()
        {
            this.low = this.high = 0;
            this.mode = TetrisGameMode.Classic;
        }

        /// <summary>
        /// Sets whether there is a power up or not.
        /// </summary>
        /// <param name="x"></param>
        public void SetPowerUp(bool x)
        {
            this.powerUp = x;
        }

        /// <summary>
        /// Gets whether there is a power up or not.
        /// </summary>
        /// <returns></returns>
        public bool GetPowerUp()
        {
            return this.powerUp;
        }

        /// <summary>
        /// Set the mode to crossing the rows mode.
        /// </summary>
        public void SetRowMode()
        {
            this.low = this.high = 0;
            this.mode = TetrisGameMode.Rows;
        }

        /// <summary>
        /// Set the mode to getting clusters mode.
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public void SetClusterMode(byte low, byte high)
        {
            this.high = high;
            this.low = low;
            this.mode = TetrisGameMode.Cluster;
        }
        #endregion
       
        #region Piece assortment methods.
        /// <summary>
        /// Clears all of the pieces.
        /// </summary>
        public void ClearPieces()
        {
            pieces.Clear();
        }

        /// <summary>
        /// Adds the given pieces to the board.
        /// </summary>
        /// <param name="pieces"></param>
        public void AddPieces(string[] pieces)
        {
            this.pieces.AddRange(pieces);
        }

        /// <summary>
        /// Adds tetronimoes to the game.
        /// </summary>
        public void AddTetronimoes()
        {
            String[] piecesB = new String[] { "d1r2", "d1l2", "l1r2l1d1", "r1d1l1", "r1d1r1", "l1d1l1", "d3" };
            pieces.AddRange(piecesB);
        }


        /// <summary>
        /// Adds pentonimoes to the game.
        /// </summary>
        public void AddPentonimoes()
        {
            String[] piecesB = new String[] { "d1r2u1", "l1d3", "r1d3", "l1d2l1", "l1d1l1r1d1", "r2l1d2", "d3r1", "d3l1", "u1d2u1l1r2", "d4", "d2r2", "r1d2r1", "r1d1r1d1" };
            pieces.AddRange(piecesB);
        }

        /// <summary>
        /// Adds a new piece to the board.
        /// </summary>
        /// <param name="piece"></param>
        public void AddPiece(String piece)
        {
            pieces.Add(piece);
        }
        #endregion

        #region Checking Methods.
        /// <summary>
        /// Checks if the rows are filled. 
        /// </summary>
        private void CheckRows()
        {
            for(int y = 0; y < board.GetHeight(); y++)
            {
                if(board.CheckFullRow(y))
                {
                    board.ShiftDown(board.GetHeight() - 1 - y);
                   if(eventLinker != null)
                    eventLinker.RowCleared();
                }
                
            }
        }


        /// <summary>
        /// Returns true or false if the cluster is the same size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool CheckClusterSize(byte size)
        {
            return cluster.Length == size;
        }

        /// <summary>
        /// Returns true or false if the cluster is within a certain range.
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public bool CheckClusterSizeRange(byte lower, byte upper)
        {
            return cluster.Length >= lower && cluster.Length <= upper;
        }



        /// <summary>
        /// Checks if the piece is going from one side to the other.
        /// </summary>
        /// <returns></returns>
        private bool CheckAcrossBoard()
        {
            bool left, right;
            left = right = false;
            for(int n = 0; n < cluster.Length; n++)
            {
                if(cluster[n] % board.GetWidth() == 0)
                {
                    left = true;
                }

                if(cluster[n] % board.GetWidth() == board.GetWidth() - 1)
                {
                    right = true;
                }

            }

            return (left && right);
        }
        #endregion

        #region Pulse the timer.
        private bool pulseRight()
        {
            if (sliding > 0)
            sliding++;

            
            if(sliding == 2)
            {
                return true;
            }

            if (sliding == 8)
            {
                sliding = 1;
            }

            return false;
        }

        private bool pulseLeft()
        {
            if (sliding < 0)
            sliding--;

            if (sliding == -2)
            {
                return true;
            }

            if(sliding == -8)
            {
                sliding = -1;
            }

            return false;
        }
        #endregion


        /// <summary>
        /// Checks clickability for different modes. Simplifies Code.
        /// </summary>
        /// <returns></returns>
        public bool ModeCheckMethod()
        {
            if(mode == TetrisGameMode.Classic)
            {
                return false;
            } else if(mode == TetrisGameMode.Cluster)
            {
                return CheckClusterSizeRange(low, high);
            }
            else
            {
                return CheckAcrossBoard();
            }
        }

        /// <summary>
        /// Sets how the piece is rendered.
        /// </summary>
        /// <param name="pieceMode"></param>
        public static void SetPieceMode(bool pieceMode)
        {
            ClassicTetrisGame.pieceMode = pieceMode;
        }

        /// <summary>
        /// Updates the Tetris Game. Makes pieces fall. Etc.
        /// </summary>
        /// <param name="handler"></param>
        public void Update(InputHandler handler)
        {
            board.ClearBoardCommand(x, y, piece);
            int oX = x, oY = y;
            mouseX = handler.GetMouseX();
            mouseY = handler.GetMouseY();


            /*
            if(handler.CheckJustPressedKey(Keys.X))
            {
                size++;
            }

            if (handler.CheckJustPressedKey(Keys.Z))
            {
                size--;
            }
            */

            #region Start Sliding pieces.
            if (handler.CheckJustPressedKey(Keys.Right) || handler.CheckJustPressedKey(Keys.D))
            {
                sliding = 1;
            }
            
            if (handler.CheckJustPressedKey(Keys.Left) || handler.CheckJustPressedKey(Keys.A))
            { 
                sliding = -1;
            }
            #endregion

            #region Speed up the fall.
            if (handler.CheckPressedKey(Keys.Down) || handler.CheckPressedKey(Keys.S))
            {
                if(speedFall == 0)
                speedFall = 1;

                if (speedFall == 1)
                {
                    y++;
                }

                speedFall++;
                
                if(y == board.GetHeight() -1)
                {
                    speedFall = 2;
                }

                if(speedFall == 8)
                {
                    speedFall = 1;
                }

                
                
            }
            else
            {
                speedFall = 0;
            }
            #endregion

            #region Correct Y Errors
            if (board.CheckBoardCommand(x, y, piece))
            {
                y = oY;
            }
            else oY = y;
            #endregion

            #region End Sliding Pieces
            if ( handler.CheckJustReleasedKey(Keys.Left) || handler.CheckJustReleasedKey(Keys.A))
           {
                if(sliding < 0)
               sliding = 0;
           }

            if (handler.CheckJustReleasedKey(Keys.Right) || handler.CheckJustReleasedKey(Keys.D))
            {
                if (sliding > 0) 
                sliding = 0;
            }
            #endregion

            #region Slide the piece left and right
            if (pulseLeft())
            {
                if(timeUntilNextFall != 18)
                x--;
                timeUntilDrop = 0;
            }
            else
            if(pulseRight())
            {
                timeUntilDrop = 0;

                if (timeUntilNextFall != 18)
                x++;
            }
           #endregion

            #region Correct X errors.
            if (board.CheckBoardCommand(x, y, piece))
            {
                x = oX;

            }
            else
            oX = x;
            #endregion

            #region Flip the Piece.
            if (handler.CheckJustPressedKey(Keys.Up) || handler.CheckJustPressedKey(Keys.W))
            {
                string o = piece; //just in case
                #region Cipher to Rotate Piece
                piece = piece.Replace('r', 'x');
                piece = piece.Replace('u', 'r');
                piece = piece.Replace('l', 'u');
                piece = piece.Replace('d', 'l');
                piece = piece.Replace('x', 'd');
                #endregion

                timeUntilDrop = 0;
                if(board.CheckBoardCommand(x, y, piece))
                {
                    y--;
                    if(board.CheckBoardCommand(x, y, piece))
                    {
                        y = oY;
                        x++;
                        if(board.CheckBoardCommand(x,y,piece))
                        {
                            x = oX;
                            piece = o;
                    
                        }
                    }

                }

            }
            #endregion
            else 
            #region Delay the fall
            timeUntilNextFall++;
            if(timeUntilNextFall == 18)
            {
                y++;
                timeUntilNextFall = 0;
            }
            #endregion

            #region Delay fall for next piece.
            if (board.CheckBoardCommand(x, y, piece) || timeUntilDrop != 0)
            {
                y = oY;
                x = oX;

                board.SetBoardCommand(x, y, piece, col);
                timeUntilDrop++;
                if (timeUntilDrop == 17)
                {
                    CheckRows();
                    #region Setting Color and Piece
                    if (mode == TetrisGameMode.Classic)
                    {
                        #region Set Color Normally.
                        col = (byte)(generator.GetRandomByte() % pieces.Count);
                        piece = pieces[col];
                        col %= (byte)colors.Length;
                        col++;
                        
                        #endregion
                    }
                    else
                    {
                        #region Set Color Randomly
                        col = (byte)(generator.GetRandomByte() % pieces.Count);
                        piece = pieces[col];
                        col = (byte)(generator.GetRandomByte() % pieces.Count);
                        col %= (byte)colors.Length;
                        col++;
                        

                        if(col == 5 && powerUp)
                        {
                            col = (byte)(generator.GetRandomByte() % pieces.Count + 1);
                            if (col >= 7)
                            {
                                col %= 7;
                                col++;
                            }
                        }
                        #endregion
                    }
                    #endregion
                   
                    x = 3;
                    y = 1;
                    timeUntilDrop = 0;
                }
            }
            #endregion
            else
            #region Check Piece Selection and Clear.
            if(mode != TetrisGameMode.Classic)
            for (int pieceY = 0; pieceY < board.GetHeight(); pieceY++)
            {
                workPiece.SetY(pieceY * size);
                for (int pieceX = 0;pieceX < board.GetWidth();pieceX++)
                {
                    workPiece.SetX(pieceX * size);
                    if (board.GetPos(pieceX, pieceY) != 0) //If the piece is not empty.
                    {
                        if(handler.CheckMouseIn(workPiece)) 
                        {
                            sbyte[] acluster = board.FindCluster(pieceX, pieceY);
                            cluster = board.ModifyCluster(pieceX, pieceY, acluster);
                            Array.Sort(cluster);

                            if (ModeCheckMethod() || (board.GetPos(pieceX, pieceY) == 5 && powerUp))
                            {
                                if (handler.CheckLeftMouseJustPressed())
                                {
                                    //EXECUTE THE LINKED EVENTS.


                                    if (eventLinker != null)
                                    {
                                        byte lCol = board.GetPos(pieceX, pieceY);

                                        eventLinker.AnyColorCleared(cluster.Length);

                                        if (lCol == 1)
                                        {
                                            //Red
                                            eventLinker.RedCleared(cluster.Length);

                                        }
                                        else if (lCol == 2)
                                        {
                                            //Green
                                            eventLinker.GreenCleared(cluster.Length);
                                        }
                                        else if (lCol == 3)
                                        {
                                            //Blue

                                            eventLinker.BlueCleared(cluster.Length);
                                        }
                                        else if (lCol == 4)
                                        {
                                            //Yellow

                                            eventLinker.YellowCleared(cluster.Length);

                                        }
                                        else if (lCol == 5)
                                        {
                                            //Orange

                                            eventLinker.OrangeCleared(cluster.Length);
                                        }
                                        else if (lCol == 6)
                                        {
                                            //Purple

                                            eventLinker.PurpleCleared(cluster.Length);
                                        }
                                        else if (lCol == 7)
                                        {
                                            //Teal

                                            eventLinker.TealCleared(cluster.Length);
                                        }
                                    }

                                    board.RemoveClusterClearRow(cluster);
                                    //Console.Beep();
                                }
                            }
                            else
                            {
                                cluster = null;
                            }

                            board.SetBoardCommand(x, y, piece, col);
                            return;
                        }
                    }
                }
            }
            #endregion

            board.SetBoardCommand(x, y, piece, col);
            cluster = null;
        }

        
        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="drawer"></param>
        /// <param name="batch"></param>
        /// <param name="handler"></param>
        public void Draw(GraphicsDevice graphics, PrimitiveDrawer drawer, SpriteBatch batch, FontHandler handler)
        {
             #region Draw the board.
            workPiece.SetSize(size, size);
            for(int y = 0; y < board.GetHeight(); y++)
            {
                workPiece.SetY(y*size);
                for(int x = 0; x < board.GetWidth(); x++)
                {
                    workPiece.SetX(x*size);
                    if (board.GetPos(x, y) != 0)
                    {

                       
                        
                        if(mode != TetrisGameMode.Classic && cluster != null && cluster.Contains(board.UnsafeConversionToPosition(x, y)))
                        {
                            drawer.DrawRoundedRectangle(graphics, workPiece, Color.Fuchsia, !pieceMode);
                        } else
                        drawer.DrawRoundedRectangle(graphics, workPiece, colors[(board.GetPos(x, y) - 1) % colors.Length], !pieceMode);
                        drawer.DrawRoundedRectangle(graphics, workPiece, new Color(0,0,0, .33f),!pieceMode);
                    }
                    else
                    {
                        drawer.DrawRoundedRectangle(graphics, workPiece, Color.Black, !pieceMode);
                    }
                }
            }


            if(flashLight)
            {
                drawer.DrawFilledRectangle(graphics, (Rectangle)(new GameRectangle(0, 0, 320, 768)), new Color(0, 0, 0, 240), false);
                graphics.BlendState = TetrisGameRunner.LightState();

                for (int i = 0;i < 4; i++)
                    drawer.DrawCulledCircle(graphics, new Vector2(mouseX, mouseY), 350, Color.White, Color.Black, 60, new GameRectangle(0, 0, 320, 768));


                graphics.BlendState = TetrisGameRunner.OriginalState();

            }



            #endregion

        }


        /// <summary>
        /// Saves the Tetris game.
        /// </summary>
        /// <param name="saver"></param>
        public void Save(Saver saver)
        {
            saver.Header("ClassicTetris");
            saver.Save(board, "TheBoard");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(piece, "Piece");
            saver.Save(col, "Color");
            saver.Save(low, "Low");
            saver.Save(high, "High");
            saver.SaveArray<string>(pieces.ToArray(), "Pieces");
            saver.Save((byte)mode, "Mode");
            saver.Save(flashLight, "Flashlight");
            saver.Save(powerUp, "PowerUp");
            saver.End();
        }


        /// <summary>
        /// Sets the x position of the current piece
        /// </summary>
        /// <param name="x"></param>
        public void SetX(int x)
        {
            this.x = x;
        }

        /// <summary>
        /// Sets the Y position of the current piece.
        /// </summary>
        /// <param name="y"></param>
        public void SetY(int y)
        {
            this.y = y;
        }

        /// <summary>
        /// Sets the board.
        /// </summary>
        /// <param name="byteBoard"></param>
        public void SetBoard(ByteBoard byteBoard)
        {
            this.board = byteBoard;
        }

        /// <summary>
        /// Sets the current piece.
        /// </summary>
        /// <param name="piece"></param>
        public void SetPiece(string piece)
        {
            this.piece = piece;
        }

        /// <summary>
        /// Sets color of current piece.
        /// </summary>
        /// <param name="col"></param>
        public void SetColor(byte col)
        {
            this.col = col;
        }

        /// <summary>
        /// Allows interfacing with the Tetris game.
        /// </summary>
        /// <param name="tajLevelLinker"></param>
        internal void SetEventLink(TetrisEventLinker tajLevelLinker)
        {
            this.eventLinker = tajLevelLinker;
        }

        /// <summary>
        /// Determines if the light on the board is on or not.
        /// </summary>
        /// <param name="p"></param>
        public void SetFlashLight(bool p)
        {
            this.flashLight = p;
        }
    }
}
