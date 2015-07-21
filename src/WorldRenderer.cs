using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TajTetrisGame
{
    class WorldRenderer
    {
        private List<LevelObject> objects;


        public WorldRenderer(World o)
        {
            this.objects = new List<LevelObject>();
            objects.AddRange(o.GetObjects());
        }

        public void Update(InputHandler handler)
        {
            foreach(LevelObject obj in objects)
            {
                obj.SetEditing(false);
                obj.Update(handler);
            }
        }

        public LevelObject[] GetObjects()
        {
            return objects.ToArray();
        }
        
        public void AddObject(LevelObject o)
        {
            objects.Add(o);
        }

        public void Draw(GraphicsDevice graphicsDevice, SpriteBatch batch, PrimitiveDrawer drawer, FontHandler font)
        {
            foreach (LevelObject o in objects)
            {
                if (o is LightLevelCircle) continue;
                o.Draw(drawer, graphicsDevice, batch, font);
            }


            drawer.DrawFilledRectangle(graphicsDevice, (Rectangle)(new GameRectangle(0, 0, 2000, 1000)), new Color(0, 0, 0, 220), false);


            foreach (LevelObject o in objects)
            {
                if (o is LightLevelCircle)
                    o.Draw(drawer, graphicsDevice, batch, font);
            }

        }



    }
}
