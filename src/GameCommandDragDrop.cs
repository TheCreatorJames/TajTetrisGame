using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class GameCommandDragDrop : SingleReturnMethodElement
    {
        private int mode;
        private static string[] modes = { "ClassicTetris", "RowMode", "SClusterMode", "BClusterMode", "MCluster Mode", "FlashlightMode", "WinGame", "EndGame", "ClearBoard", "AddPentaPieces" };

        public GameCommandDragDrop()
        {
            this.mode = 0;
        }

        public override string GetCode()
        {
            return modes[mode];   
        }

        public override string GetReturnType()
        {
            return "Code";
        }

        public override void TellSelected(bool selected)
        {
            if(selected && this.selected)
            {
                mode = (mode + 1) % modes.Length;
            }

            base.TellSelected(selected);
        }

        public override DragDropElement GetInteracting(InputHandler handler)
        {
            return this;
        }

        public override DragDropElement GetTopInteracting(InputHandler handler)
        {
            return this;
        }

        public override DragDropElement Clone()
        {
            return new GameCommandDragDrop();
        }

        public override DragDropElement[] GetOperations()
        {
            throw new NotImplementedException();
        }

        public override DragDropElement[] GetAssignments()
        {
            throw new NotImplementedException();
        }

        public override DragDropElement[] GetOptions()
        {
            throw new NotImplementedException();
        }

        public override void Save(Saver saver)
        {
          
            saver.Header("GameCommand");
            saver.Save(x, "X");
            saver.Save(y, "Y");
            saver.Save(mode, "Mode");
            saver.End();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            this.SetHeight(20);
            this.SetWidth((int)fontHandler.GetVerdana().MeasureString(modes[mode]).X + 6);



            primitiveDrawer.DrawRoundedRectangle(graphicsDevice, this, Color.White);


            this.ModifyY(3);
            this.ModifyX(2);
            spriteBatch.DrawString(fontHandler.GetVerdana(), modes[mode], GetVector() + TetrisGameRunner.GetOffsetVector(), Color.Teal);
            this.ModifyX(-2);
            this.ModifyY(-3);
        }
    }
}
