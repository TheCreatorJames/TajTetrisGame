using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TajTetrisGame
{
    class ByteBoard : ICloneable, Saveable
    {
        //Each color gets 4 bits - 16 colors. 15 colors actually.
        protected const int BOARDHEIGHT = 24;
        protected const int BOARDWIDTH = 10;
        protected const int BOARDSIZE = (BOARDHEIGHT * BOARDWIDTH)/2;
        
        //In theory, it should be about 100 bytes of RAM. Other implementations will likely
        //use many more. 
        protected byte[] board;
        protected List<sbyte> cluster;
        

        public ByteBoard()
        {
            board = new byte[BOARDSIZE];
            this.cluster = new List<sbyte>();
        }

        public ByteBoard(ByteBoard board)
        {
            this.board = (byte[])board.board.Clone();
            this.cluster = new List<sbyte>();
        }

        /// <summary>
        /// Gets the Color of the Position
        /// </summary>
        /// <param name="x">Gets the X Coordinate</param>
        /// <param name="y">Gets the Y Coordinate</param>
        /// <returns>The Color of the Position</returns>
        virtual public byte GetPos(int x, int y)
        {
            int pos = y * BOARDWIDTH + x;

            if (pos >= BOARDSIZE * 2)
            {
                return 255;
            }


            if (pos < 0)
            {
                return 255;
            }

            return GetPos(pos);
        }

        /// <summary>
        /// Sets the color at the position on the board.
        /// </summary>
        /// <param name="x">The X Coordinate</param>
        /// <param name="y">The Y Coordinate</param>
        /// <param name="col">Color to set to.</param>
        virtual public void SetPos(int x, int y, byte col)
        {
            int pos = y * BOARDWIDTH + x;

            col &= 15; //only 0-15 is allowed.

            if (pos >= BOARDSIZE * 2)
            {
                return;
            }

            if (pos <= 0)
            {
                return;
            }

             SetPos(pos, col);
        }

        /// <summary>
        /// Sets the position under the hood.
        /// </summary>
        /// <param name="pos">Position coordinate</param>
        /// <param name="col">Color</param>
        virtual protected void SetPos(int pos, byte col)
        {
            int bPos = pos / 2;
            int pPos = pos % 2;

            board[bPos] &= (byte)(255 ^ (15 << (4*pPos))); //clear
            board[bPos] |= (byte)(col << (4 *pPos)); //set
        }

        
        #region Clustering Methods.
        /// <summary>
        /// Checks if there is a path between two points by getting a cluster, if 
        /// the two points are in the cluster, then they are connected.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns>A boolean saying whether there is a path between two points</returns>
        public bool IsPathBetween(int x1, int y1, int x2, int y2)
        {
            sbyte[] clust = FindCluster(x1, y1);

            int pos = y1 * GetWidth() + x1;
            int pos2 = y2 * GetWidth() + x2;

            sbyte test = (sbyte)(pos - pos2);

            return clust.Contains(test);
        }

        /// <summary>
        /// Fixes the cluster to be exact position and not 
        /// just relative.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cluster">The relative cluster.</param>
        public int[] ModifyCluster(int x, int y, sbyte[] cluster)
        {
            int pos = y * GetWidth() + x;
            int[] nCluster = new int[cluster.Length];
            for(int i = 0; i < cluster.Length; i++)
            {
                nCluster[i] = (pos - cluster[i]);
            }

            return nCluster;
        }


        /// <summary>
        /// Checks if there is a connection between two points.
        /// If you take your cluster, you may use this.
        /// </summary>
        /// <param name="clust"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public bool IsPathBetween(sbyte[] clust, int x1, int y1, int x2, int y2)
        {
            int pos = y1 * GetWidth() + x1;
            int pos2 = y2 * GetWidth() + x2;

            sbyte test = (sbyte)(pos - pos2);

            return clust.Contains(test);
        }

        /// <summary>
        /// Removes a cluster, by removing its entire row.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void RemoveClusterClearRow(int x, int y)
        {
            RemoveClusterClearRow(FindCluster(x, y), x, y);
        }

        /// <summary>
        /// Removes a cluster, by removing its entire row.
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void RemoveClusterClearRow(sbyte[] cluster, int x, int y)
        {
            RemoveClusterClearRow(ModifyCluster(x, y, cluster));
        }


        /// <summary>
        /// Removes a cluster, by removing its entire row.
        /// </summary>
        /// <param name="clusterB"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void RemoveClusterClearRow(int[] clusterB)
        {
            cluster.Clear();
            for (int i = 0;i < clusterB.Length; i++)
            {
                int row = clusterB[i] / GetWidth();
                if (!cluster.Contains(((sbyte)row)))
                {
                    cluster.Add((sbyte)row);
                    ShiftDown(GetHeight() - row - 1);

                }
            }

        }


        /// <summary>
        /// Remove the Cluster from the Board.
        /// This implementation lowers the rest of the board.
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void RemoveClusterGravity(sbyte[] cluster, int x, int y)
        {
            int pos = y * GetWidth() + x;

            Array.Sort(cluster); //The Highest Pieces Go First
            Array.Reverse(cluster);
            foreach(sbyte offset in cluster)
            {
                int nY = (pos - offset) / GetWidth(); 
                int nX = (pos - offset) % GetWidth();

                #region Copy all the parts above down.
                while (nY >= 1)
                {
                    SetPos(nX, nY, GetPos(nX, nY - 1));
                    nY--;
                }
                #endregion
            }
        }

        /// <summary>
        /// Remove the Cluster from the Board.
        /// This implementation removes it from the board.
        /// </summary>
        /// <param name="cluster"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void RemoveCluster(sbyte[] cluster, int x, int y)
        {
            int pos = y * GetWidth() + x;
            foreach (sbyte offset in cluster)
            {
                SetPos(pos - offset, 0);
            }
        }


        /// <summary>
        /// Finds a cluster of the color at the given coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>A calculated cluster.</returns>
        public sbyte[] FindCluster(int x, int y)
        {
            cluster.Clear();
            int pos = y * GetWidth() + x;
            byte col = GetPos(pos);

            if (col != 0)
            {
                cluster.Add(0);
                if (col != 255)
                    SearchCluster(pos, pos, col);
            }
            return cluster.ToArray();
        }


        /// <summary>
        /// A method that is recursively called to path find a cluster.
        /// </summary>
        /// <param name="oPos">Original Position</param>
        /// <param name="pos">Modified Position</param>
        /// <param name="col">Original Color</param>
        private void SearchCluster(int oPos, int pos, byte col)
        {
            #region Check if there is a cell left.
            if (pos % GetWidth() != (GetWidth() - 1)) //Check if it is on the edge.
                if (GetPos(pos + 1) == col)
                {
                    if (cluster.Contains((sbyte)(oPos - (pos + 1))))
                    {

                    }
                    else
                    {
                        cluster.Add((sbyte)(oPos - (pos + 1)));
                        SearchCluster(oPos, pos + 1, col);
                    }
                }
            #endregion
            #region Check if there is a cell right.
            if (pos % GetWidth() != 0) //Check if it is on the edge.
                if (GetPos(pos - 1) == col)
                {
                    if (cluster.Contains((sbyte)(oPos - (pos - 1))))
                    {

                    }
                    else
                    {
                        cluster.Add((sbyte)(oPos - (pos - 1)));
                        SearchCluster(oPos, pos - 1, col);
                    }
                }
            #endregion
            #region Check if there is a cell above.
            if (pos >= GetWidth()) //Check if there is stuff above.
            if (GetPos(pos - GetWidth()) == col)
            {
                if (cluster.Contains((sbyte)(oPos - (pos-GetWidth()))))
                {

                }
                else
                {
                    cluster.Add((sbyte)(oPos - (pos-GetWidth())));
                    SearchCluster(oPos, pos - GetWidth(), col);
                }

            }
            #endregion
            #region Check if there is a Cell below.
            if (pos < GetHeight() * GetWidth() - GetWidth()) //Check if there is even stuff below.
            if (GetPos(pos + GetWidth()) == col)
            {
                if (cluster.Contains((sbyte)(oPos - (pos+GetWidth()))))
                {

                }
                else
                {
                    cluster.Add((sbyte)(oPos - (pos+GetWidth())));
                    SearchCluster(oPos, pos + GetWidth(), col);
                }
            }
            #endregion

        }




        /// <summary>
        /// Get the size of a cluster of a cell.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The size of the cluster.</returns>
        public int FindClusterSize(int x, int y)
        {
            return FindCluster(x, y).Length;
        }
        #endregion


        /// <summary>
        /// Checks whether there is a piece in the location or not.
        /// </summary>
        /// <param name="x">Starting Position X</param>
        /// <param name="y">Starting Position Y</param>
        /// <returns>A boolean saying whether the position is filled or not.</returns>
        virtual public bool CheckPos(int x, int y)
        {
            if (x >= GetWidth()) return true;
            if (x < 0) return true;

            return GetPos(x, y) != 0;
        }


        #region Conversion Methods

        /// <summary>
        /// This is used to convert a position to an
        /// XY coordinate. Due to encapsulation purposes, we ONLY
        /// want you to use XY coordinates when calling our ByteBoard.
        /// 
        /// However, this method might be necessary to make certain tasks easy.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        virtual public void UnsafeConversionToXY(int pos, out int x, out int y)
        {
            x = pos % GetWidth();
            y = pos / GetWidth();
        }


        /// <summary>
        /// This is a method that converts the coordinates to a position.
        /// We'd prefer this to only be used by people who know
        /// what they're doing with it.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>A coordinate position</returns>
        virtual public int UnsafeConversionToPosition(int x, int y)
        {
            return y * GetWidth() + x;
        }

        #endregion

        /*
        private byte[] old;
        virtual public void Store()
        {
            old = (byte[])board.Clone();
        }

        virtual public void Restore()
        {
            if (old != null)
            {
                board = (byte[])old.Clone();
            }
        }
        */


        /// <summary>
        /// Replaces every given color with another given color.
        /// </summary>
        /// <param name="col">The color to be replaced.</param>
        /// <param name="newCol">The new color.</param>
        virtual public void ReplaceColor(byte col, byte newCol)
        {
            for (int i = 0; i < BOARDSIZE * 2; i++)
            {
                if (GetPos(i) == col)
                {
                    SetPos(i, newCol);
                }
            }
        }

        #region Check Row Functions

        /// <summary>
        /// Checks if the row is partially filled.
        /// </summary>
        /// <param name="row">Row position from the top.</param>
        /// <returns>A boolean determining if the row is partially filled or not.</returns>
        virtual public bool CheckPartialRow(int row)
        {
            return !CheckFullRow(row) && !CheckEmptyRow(row);
        }

        /// <summary>
        /// Checks if the row is empty.
        /// </summary>
        /// <param name="row">Row position from the top</param>
        /// <returns>A boolean determining if the row is empty or not.</returns>
        virtual public bool CheckEmptyRow(int row)
        {
            int pos = row * GetWidth();
            for (int x = 0; x < GetWidth(); x += 2)
            {
                if (board[(pos + x)/2] != 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a row is full.
        /// </summary>
        /// <param name="row">Row Position from Top</param>
        /// <returns>A boolean determining whether the row is full or not.</returns>
        virtual public bool CheckFullRow(int row)
        {
            int pos = row * GetWidth();
            for (int x = 0; x < GetWidth(); x += 2)
            {
                if ((board[(pos + x) / 2] & 15) == 0 || (board[(pos + x) / 2] & (15<<4)) == 0)
                    return false;
            }

            return true;
        }
        #endregion

        /// <summary>
        /// Shifts the Rows Down from the specified position from the bottom.
        /// </summary>
        /// <param name="fromBottom">The row from the bottom. (very bottom = 0)</param>
        virtual public void ShiftDown(int fromBottom)
        {
            for (int y = GetHeight() - fromBottom - 1; y >= 1; y--)
            {
                int pos = y * GetWidth();
                for (int x = 0; x < GetWidth(); x +=2)
                {
                    board[(pos + x)/2] = board[(pos + x - GetWidth())/2];
                }

                if (CheckEmptyRow(y)) //This may not always apply. But I think it is safe.
                {
                    break;
                }
            }
        }



        /// <summary>
        /// Gets the Color at the Coordinate under the hood.
        /// </summary>
        /// <param name="pos">Position</param>
        /// <returns>The Color Byte.</returns>
        virtual protected byte GetPos(int pos)
        {
            int bPos = pos / 2;
            int pPos = pos % 2;

            return (byte)((board[bPos] >> (4 * pPos)) & 15);
        }

        /// <summary>
        /// Gets the size of the board.
        /// </summary>
        /// <returns></returns>
        virtual public int GetSize()
        {
            return BOARDSIZE;
        }

        /// <summary>
        /// Gets the width of the board.
        /// </summary>
        /// <returns></returns>
        virtual public int GetWidth()
        {
            return BOARDWIDTH;
        }

        /// <summary>
        /// Gets the Height of the Board
        /// </summary>
        /// <returns>Height of the Board</returns>
        virtual public int GetHeight()
        {
            return BOARDHEIGHT;
        }

        /// <summary>
        /// Clears the board in the given instruction area.
        /// </summary>
        /// <param name="x">Starting Position X</param>
        /// <param name="y">Starting Position Y</param>
        /// <param name="instructions">Instruction Code</param>
        public void ClearBoardCommand(int x, int y, string instructions)
        {
            SetBoardCommand(x, y, instructions, 0);
        }

        /// <summary>
        /// Checks the board for other pieces in the given instruction area
        /// </summary>
        /// <param name="x">Starting Position X</param>
        /// <param name="y">Starting Position Y</param>
        /// <param name="instructions">Instruction Code</param>
        /// <returns>Whether the move is allowed or not.</returns>
        public bool CheckBoardCommand(int x, int y, string instructions)
        {
            char[] cInstructions = instructions.ToCharArray();

            byte direction = 0;
            //0 - down
            //1 - right
            //2 - left
            //3 - up

            if(CheckPos(x, y))
            {
                return true;
            }

            foreach (char instruction in cInstructions)
            {
                int distance = 0;
                #region Set the direction or distance of the command.
                switch (instruction)
                {
                    case 'd':
                        direction = 0;
                        break;
                    case 'r':
                        direction = 1;
                        break;
                    case 'l':
                        direction = 2;
                        break;
                    case 'u':
                        direction = 3;
                        break;
                    default:
                        distance = instruction - '0';
                        break;
                }
                #endregion
                #region Check the piece according to instructions.
                for (int i = 0; i < distance; i++)
                {
                    if (direction == 0)
                    {
                        y++;
                    }
                    else if (direction == 1)
                    {
                        x++;
                    }
                    else if (direction == 2)
                    {
                        x--;
                    }
                    else if (direction == 3)
                    {
                        y--;
                    }

                    if (CheckPos(x, y))
                    {
                        return true;
                    }
                }
                #endregion
               
            }
            return false;
        }

        /// <summary>
        /// Sets everything in the instruction code to be the given color.
        /// </summary>
        /// <param name="x">Starting Position X</param>
        /// <param name="y">Starting Position Y</param>
        /// <param name="instructions">Instruction Code</param>
        /// <param name="col">Color</param>
        public void SetBoardCommand(int x, int y, string instructions, byte col)
        {
            char[] cInstructions = instructions.ToCharArray();
            
            byte direction = 0;
            //0 - down
            //1 - right
            //2 - left
            //3 - up

            SetPos(x, y, col);
            foreach (char instruction in cInstructions)
            {
                int distance = 0;
                #region Set the direction or distance of the command.
                switch (instruction)
                {
                    case 'd':
                        direction = 0;
                        break;
                    case 'r':
                        direction = 1;
                        break;
                    case 'l':
                        direction = 2;
                        break;
                    case 'u':
                        direction = 3;
                        break;
                    default:
                        distance = instruction - '0';
                        break;
                }
                #endregion
                #region Move the piece according to instructions.
                for (int i = 0; i < distance; i++)
                {
                    if (direction == 0)
                    {
                        y++;
                    }
                    else if (direction == 1)
                    {
                        x++;
                    }
                    else if (direction == 2)
                    {
                        x--;
                    }
                    else if (direction == 3)
                    {
                        y--;
                    }
                    SetPos(x, y, col);
                }
                #endregion

            }

        }

        public object Clone()
        {
            return new ByteBoard(this);
        }

        public void Save(Saver saver)
        {
            saver.Header("ByteBoard");
            saver.SaveArray<byte>(board, "Board");
            saver.End();
        }

        internal void SetBoard(byte[] p)
        {
            this.board = p;
        }
    }
}
