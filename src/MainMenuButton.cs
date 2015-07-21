using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class MainMenuButton : GameRectangle
    {
        private Color col;
        private String text;


        public MainMenuButton() : this("")
        {

        }


        public MainMenuButton(String text)
        {
            this.text = text;
            SetWidth(200);
            SetHeight(50);
            col = Color.Yellow;
            ModifyX((1024-200)/2 - 10);
            ModifyY(300);
        }

        public void Update(InputHandler handler)
        {
            if(handler.CheckMouseIn(this))
            {
                col = Color.Gold;

            } else
            {
                col = Color.Yellow;
            }

        }




        public void Draw(SpriteBatch batch, FontHandler handler, GraphicsDevice graphics, PrimitiveDrawer drawer)
        {
            drawer.DrawRoundedRectangle(graphics, this, col);
            batch.DrawString(handler.GetVerdana(), text, GetVector() + new Vector2(20, 10), Color.Black);
        }


        internal void SetText(string p)
        {
            this.text = p;
        }

        internal string GetText()
        {
            return this.text;
        }
    }
}
