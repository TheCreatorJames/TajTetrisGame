using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    abstract class LevelObject : GameObject, Saveable
    {
        virtual public void Save(Saver saver)
        {
            throw new NotImplementedException();
        }

        abstract public void SetColor(Color col);
        abstract public bool CheckInside(InputHandler handler);
        abstract public void SetEditing(bool x);
        abstract public bool Modifying();
        abstract public void Update(InputHandler handler);
        abstract public void Draw(PrimitiveDrawer drawer, GraphicsDevice graphics, SpriteBatch batch, FontHandler font);
    }
}
