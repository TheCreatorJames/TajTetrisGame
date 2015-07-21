using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TajTetrisGame
{
    /// <summary>
    /// This class is designed
    /// to draw primitive shapes like
    /// triangles, rectangles,
    /// lines, etc.
    /// </summary>
    class PrimitiveDrawer
    {
        private List<VertexPositionColor> vertexPositionColor;
        private int width, height;
        private const int CIRCLE_POINTS = 60;
        private bool cacheMode;

        #region Mathematical Calculation Caches to increase performance.
        //I devised the cache system. I want most of my graphics to be done
        //in software, and when it came to circles, I was generating a Texture
        //and using it. But it didn't scale. 
        //Therefore, I analyzed another algorithm, and to optimize the CPU Cycles (and make it more power efficient)
        //I realized most of the calculations were the same input. Why not cache? It'd take up a few KB at most.
        //But save a ton of cycles.
        //Although, some hardware might have slower RAM with stronger processors.
        //and some might want to use less CPU intensive processes to save energy.
        private Dictionary<Double, Double> sinCache;
        private Dictionary<Double, Double> cosCache;
        private Dictionary<Double, Double> radCache;
        #endregion
        #region Variables used to Draw the Primitives
        private VertexBuffer vertexBuffer, vertexBufferB;
        private RasterizerState rasterizerState;

        private Matrix world = Matrix.CreateTranslation(0, 0, 0);
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        private Matrix projection;

        private BasicEffect basicEffect;
        #endregion

        public PrimitiveDrawer(int width, int height)
        {
            projection = Matrix.CreateOrthographic(width, height, 0, 100);
            rasterizerState = new RasterizerState();
            vertexPositionColor = new List<VertexPositionColor>();
            this.width = width;
            this.height = height;
            this.radCache = new Dictionary<double, double>();
            this.sinCache = new Dictionary<double, double>();
            this.cosCache = new Dictionary<double, double>();
            rasterizerState.CullMode = CullMode.None;
            
        }

        public void setCacheMode(bool x)
        {
            this.cacheMode = x;
        }

        public bool getCacheMode()
        {
            return this.cacheMode;
        }

        public void invertCacheMode()
        {
            this.cacheMode = !this.cacheMode;
        }

        #region Mathematical Operations that Cache the Answers. Good for things like Circle Calculations.
        /// <summary>
        /// Gets the cos of an Angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double Cos(double angle)
        {
            if(!cosCache.ContainsKey(angle))
            {
                double ans = Math.Cos(angle);
                cosCache[angle] = ans;
                return ans;
            }

            return cosCache[angle];
        }

        /// <summary>
        /// Gets the Sin of an angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double Sin(double angle)
        {
            if (!sinCache.ContainsKey(angle))
            {
                double ans = Math.Sin(angle);
                sinCache[angle] = ans;
                return ans;
            }

            return sinCache[angle];
        }

        /// <summary>
        /// Converts Degrees to Radians.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double ToRad(double angle)
        {
            if (!radCache.ContainsKey(angle))
            {
                double ans = (Math.PI / 180) * angle;
                radCache[angle] = ans;
                return ans;
            }

            return radCache[angle];
        }
        #endregion

        /// <summary>
        /// Sets the Primitive Drawer up. Allows it to allocate RAM on the GPU and etc.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public void Setup(GraphicsDevice graphicsDevice, int offsetX, int offsetY)
        {
            basicEffect = new BasicEffect(graphicsDevice);
            world.Translation = new Vector3(offsetX, offsetY, 0);
            basicEffect.World = world;
            
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = true;

            
            
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 6, BufferUsage.WriteOnly);
            vertexBufferB = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), CIRCLE_POINTS * 2 + 8, BufferUsage.WriteOnly);
        }

        
        /// <summary>
        /// Draws a rounded rectangle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        public void DrawRoundedRectangle(GraphicsDevice graphicsDevice, Rectangle rectangle, Color color)
        {
            DrawRoundedRectangle(graphicsDevice, rectangle, color, true);
        }
   
        /// <summary>
        /// Draws a rounded rectangle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        /// <param name="glitched"></param>
        public void DrawRoundedRectangle(GraphicsDevice graphicsDevice, Rectangle rectangle, Color color, bool glitched)
        {
            #region Draw the Rounded Rectangle by drawing 3 rectangles (6 Triangles)
            Rectangle rex = rectangle;

            rex.X++;
            rex.Width -= 2;

            DrawFilledRectangle(graphicsDevice, rex, color, glitched);


            rex.X--;
            rex.Width += 2;
            rex.Y++;
            rex.Height -= 2;

            DrawFilledRectangle(graphicsDevice, rex, color, glitched);


            //Now to draw the rounded pixels.. a third rectangle.
            rex.X++;
            //rex.Y++; /*Leftover*/
            rex.Height += 2 /*Leftover*/ - 2;
            rex.Width -= 2;

            DrawFilledRectangle(graphicsDevice, rex, color, glitched);
            #endregion
        }

        /// <summary>
        /// Draws a rounded rectangle.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="workPiece"></param>
        /// <param name="color"></param>
        /// <param name="pieceMode"></param>
        public void DrawRoundedRectangle(GraphicsDevice graphics, GameObject workPiece, Color color, bool pieceMode)
        {
            DrawRoundedRectangle(graphics, (Rectangle)workPiece, color, pieceMode);
        }

        /// <summary>
        /// Draws a rounded rectangle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameObject"></param>
        /// <param name="color"></param>
        public void DrawRoundedRectangle(GraphicsDevice graphicsDevice, GameObject gameObject, Color color)
        {

            DrawRoundedRectangle(graphicsDevice, (Rectangle)gameObject, color);
        }


        /// <summary>
        /// Draws an empty rectangle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameObject"></param>
        /// <param name="color"></param>
        public void DrawLineRectangle(GraphicsDevice graphicsDevice, GameObject gameObject, Color color)
        {
            DrawLineRectangle(graphicsDevice, new Rectangle(gameObject.GetX(), gameObject.GetY(), gameObject.GetWidth(), gameObject.GetHeight()), color);
        }


        /// <summary>
        /// Gets the points to be drawn on the Bezier Curve.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        private Vector2 GetPoint(float t, ref Vector2 p0, ref Vector2 p1, ref  Vector2 p2, ref Vector2 p3)
        {
            float cx = 3 * (p1.X - p0.X);
            float cy = 3 * (p1.Y - p0.Y);

            float bx = 3 * (p2.X - p1.X) - cx;
            float by = 3 * (p2.Y - p1.Y) - cy;

            float ax = p3.X - p0.X - cx - bx;
            float ay = p3.Y - p0.Y - cy - by;

            float Cube = t * t * t;
            float Square = t * t;

            float resX = (ax * Cube) + (bx * Square) + (cx * t) + p0.X;
            float resY = (ay * Cube) + (by * Square) + (cy * t) + p0.Y;

            return new Vector2((Int16)resX, (Int16)resY);
        }

        /// <summary>
        /// Draws a Bezier Curve.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="start"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="end"></param>
        /// <param name="color"></param>
        public void DrawCurve(GraphicsDevice graphicsDevice, Vector2 start, Vector2 a, Vector2 b, Vector2 end, Color color)
        {
            bool first = true;
            Vector2 last = a;
            for(float i = 0; i <= 1.01f; i+= .01f)
            {
               if(first)
               {
                   last = GetPoint(i, ref start, ref a, ref b, ref end);
                   first = false;
                   continue;
               }

               Vector2 current = GetPoint(i, ref start, ref a, ref b, ref end);
               DrawLine(graphicsDevice, last, current, color);


               last = current;

            }


        }

        /// <summary>
        /// Draws a Bezier Curve.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="start"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="end"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public void DrawCurve(GraphicsDevice graphicsDevice, Vector2 start, Vector2 a, Vector2 b, Vector2 end, int x, int y, Color color)
        {
            bool first = true;
            Vector2 last = a;
            for (float i = 0;i <= 1.01f;i += .01f)
            {
                if (first)
                {
                    last = GetPoint(i, ref start, ref a, ref b, ref end);
                    first = false;
                    continue;
                }

                Vector2 current = GetPoint(i, ref start, ref a, ref b, ref end);


                DrawThickerLine(graphicsDevice, last, current, x, y, color);


                last = current;

            }


        }



        /// <summary>
        /// Draws a filled rectangle of the given color.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="gameObject"></param>
        /// <param name="color"></param>
        public void DrawFilledRectangle(GraphicsDevice graphicsDevice, GameObject gameObject, Color color)
        {
            DrawFilledRectangle(graphicsDevice, (Rectangle)gameObject, color);
        }

        /// <summary>
        /// Draws an empty rectangle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        public void DrawLineRectangle(GraphicsDevice graphicsDevice, Rectangle rect, Color color)
        {
            vertexPositionColor.Clear();
            graphicsDevice.RasterizerState = rasterizerState;
            #region Add Five Coordinates to Draw the Line Strip Between.
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X, rect.Y, 0)), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X + rect.Width, rect.Y, 0)), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X + rect.Width, rect.Y + rect.Height, 0)), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X, rect.Y + rect.Height, 0)), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X, rect.Y, 0)), color));
            #endregion
            DrawPrimitives(graphicsDevice);
        }

        public void DrawFilledRectangle(GraphicsDevice graphicsDevice, Rectangle rect, Color color)
        {
            DrawFilledRectangle(graphicsDevice, rect, color, true);
        }

        /// <summary>
        /// Draws a Filled Rectangle of the given color.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="rect"></param>
        /// <param name="color"></param>
        /// <param name="glitched"></param>
        public void DrawFilledRectangle(GraphicsDevice graphicsDevice, Rectangle rect, Color color, bool glitched)
        {
            vertexPositionColor.Clear();
            graphicsDevice.RasterizerState = rasterizerState;

            #region Add Five Coordinates to draw the triangles between.
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X, rect.Y, 0)), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X + rect.Width, rect.Y, 0)), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X + rect.Width, rect.Y + rect.Height, 0)), color));

            if(!glitched)
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X + rect.Width, rect.Y + rect.Height, 0)), color));

            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X, rect.Y + rect.Height, 0)), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector3(rect.X, rect.Y, 0)), color));
            #endregion
            DrawFilledPrimitives(graphicsDevice);
        }

        /// <summary>
        /// Draws a thicker line via drawing rectangles.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos"></param>
        /// <param name="pos2"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="col"></param>
        public void DrawThickerLine(GraphicsDevice graphicsDevice, Vector2 pos, Vector2 pos2, int x, int y, Color col)
        {
            DrawLineRectangle(graphicsDevice, pos, pos2, new Vector2(pos.X + x, pos.Y + y), new Vector2(pos2.X + x, pos2.Y + y), col);
        }


        /// <summary>
        /// Draws an empty rectangle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos"></param>
        /// <param name="pos2"></param>
        /// <param name="pos3"></param>
        /// <param name="pos4"></param>
        /// <param name="color"></param>
        public void DrawLineRectangle(GraphicsDevice graphicsDevice, Vector2 pos, Vector2 pos2, Vector2 pos3, Vector2 pos4, Color color)
        {
            vertexPositionColor.Clear();
            graphicsDevice.RasterizerState = rasterizerState;
            #region Add five Coordinates to draw line strip.
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos2), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos3), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos4), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos), color));
            #endregion


            DrawPrimitives(graphicsDevice);
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos"></param>
        /// <param name="pos2"></param>
        /// <param name="pos3"></param>
        /// <param name="pos4"></param>
        /// <param name="color"></param>
        public void DrawFilledRectangle(GraphicsDevice graphicsDevice, Vector2 pos, Vector2 pos2, Vector2 pos3, Vector2 pos4, Color color)
        {
            vertexPositionColor.Clear();
            graphicsDevice.RasterizerState = rasterizerState;

            #region Add five coordinates to draw triangles in between.
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos2), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos3), color));

            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos4), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos), color));
            #endregion


            DrawFilledPrimitives(graphicsDevice);
        }

        /// <summary>
        /// Draws the Circle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        public void DrawCircle(GraphicsDevice graphicsDevice, Vector2 pos, int radius, Color color)
        {
            DrawCircle(graphicsDevice, pos, radius, color, CIRCLE_POINTS);
        }

        /// <summary>
        /// Draws a Circle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="sides"></param>
        public void DrawCircle(GraphicsDevice graphicsDevice, Vector2 pos, int radius, Color color, int sides)
        {
            DrawCircle(graphicsDevice, pos, radius, color, color, sides);
        }

        /// <summary>
        /// Draws a Circle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="colorB"></param>
        /// <param name="sides"></param>
        public void DrawCircle(GraphicsDevice graphicsDevice, Vector2 pos, int radius, Color color, Color colorB, int sides)
        {
            vertexPositionColor.Clear();
            graphicsDevice.RasterizerState = rasterizerState;

            float change = 360f / sides;

            //vertexPositionColor.Add();

            float x1, y1;
            if(cacheMode)
            {
                x1 = pos.X + (float)Cos(ToRad(change)) * radius; 
                y1 = pos.Y + (float)Sin(ToRad(change)) * radius;

            }
            else
            {
                x1 = pos.X + (float)Math.Cos(ToRad(change)) * radius;
                y1 = pos.Y + (float)Math.Sin(ToRad(change)) * radius;
            }
            
            Vector3 work = modifyVector(new Vector2(x1, y1));
            VertexPositionColor ExtraPoint = new VertexPositionColor(modifyVector(pos), color);
            vertexPositionColor.Add(ExtraPoint);
            vertexPositionColor.Add(new VertexPositionColor(work, colorB));
            for(float i = change*2; i <= 360+change*2; i+= change)
            {
                float x2, y2;
                if (cacheMode)
                {
                    x2 = pos.X + (float)Cos(ToRad(i - change)) * radius;
                    y2 = pos.Y + (float)Sin(ToRad(i - change)) * radius;
                } else
                {
                    x2 = pos.X + (float)Math.Cos(ToRad(i - change)) * radius;
                    y2 = pos.Y + (float)Math.Sin(ToRad(i - change)) * radius;
                }
                vertexPositionColor.Add(ExtraPoint);
                vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector2(x2, y2)), colorB));
            }

            DrawCirclePrimitives(graphicsDevice);

        }

        /// <summary>
        /// Draws a Culled Circle.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="colorB"></param>
        /// <param name="sides"></param>
        public void DrawCulledCircle(GraphicsDevice graphicsDevice, Vector2 pos, int radius, Color color, Color colorB, int sides, GameObject o)
        {
            vertexPositionColor.Clear();
            graphicsDevice.RasterizerState = rasterizerState;

            bool insideOf = pos.X >= o.GetX() && pos.X <= (o.GetX() + o.GetWidth());
            insideOf = insideOf && pos.Y >= o.GetY() && pos.Y <= o.GetY() + o.GetHeight();

            if (!insideOf) return;

            float change = 360f / sides;

            //vertexPositionColor.Add();

            float x1, y1;
            if (cacheMode)
            {
                x1 = pos.X + (float)Cos(ToRad(change)) * radius;
                y1 = pos.Y + (float)Sin(ToRad(change)) * radius;

            }
            else
            {
                x1 = pos.X + (float)Math.Cos(ToRad(change)) * radius;
                y1 = pos.Y + (float)Math.Sin(ToRad(change)) * radius;
            }

            if (x1 <= o.GetX()) x1 = o.GetX();
            if (x1 >= o.GetX() + o.GetWidth()) x1 = o.GetX() + o.GetWidth();
            if (y1 <= o.GetY()) y1 = o.GetY();
            if (y1 >= o.GetY() + o.GetHeight()) y1 = o.GetX() + o.GetHeight();


            Vector3 work = modifyVector(new Vector2(x1, y1));
            VertexPositionColor ExtraPoint = new VertexPositionColor(modifyVector(pos), color);
            vertexPositionColor.Add(ExtraPoint);
            vertexPositionColor.Add(new VertexPositionColor(work, colorB));
            for (float i = change * 2; i <= 360 + change * 2; i += change)
            {
                float x2, y2;
                if (cacheMode)
                {
                    x2 = pos.X + (float)Cos(ToRad(i - change)) * radius;
                    y2 = pos.Y + (float)Sin(ToRad(i - change)) * radius;
                }
                else
                {
                    x2 = pos.X + (float)Math.Cos(ToRad(i - change)) * radius;
                    y2 = pos.Y + (float)Math.Sin(ToRad(i - change)) * radius;
                }

                if (x2 <= o.GetX()) x2 = o.GetX();
                if (x2 >= o.GetX() + o.GetWidth()) x2 = o.GetX() + o.GetWidth();
                if (y2 <= o.GetY()) y2 = o.GetY();
                if (y2 >= o.GetY() + o.GetHeight()) y2 = o.GetX() + o.GetHeight();



                vertexPositionColor.Add(ExtraPoint);
                vertexPositionColor.Add(new VertexPositionColor(modifyVector(new Vector2(x2, y2)), colorB));
            }

            DrawCirclePrimitives(graphicsDevice);

        }

        #region Adjust the Vector's coordinates to have (0, 0) in the top left, rather than in the center.
        private Vector3 modifyVector(Vector2 vec)
        {
            Vector3 modifiedVector = new Vector3(vec, 0);

            modifiedVector.Y -= height / 2;
            modifiedVector.Y *= -1;

            modifiedVector.X -= width / 2;

            return modifiedVector;
        }

        private Vector3 modifyVector(Vector3 vec)
        {
            Vector3 modifiedVector = vec;

            modifiedVector.Y -= height / 2;
            modifiedVector.Y *= -1;

            modifiedVector.X -= width / 2;

            return modifiedVector;
        }
        #endregion

        /// <summary>
        /// Draws a line. 
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="color"></param>
        public void DrawLine(GraphicsDevice graphicsDevice, Vector2 pos1, Vector2 pos2, Color color)
        {
            vertexPositionColor.Clear();
            graphicsDevice.RasterizerState = rasterizerState;

            #region Add two coordinates to draw a line between.
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos1), color));
            vertexPositionColor.Add(new VertexPositionColor(modifyVector(pos2), color));
            #endregion
            DrawPrimitives(graphicsDevice);
        }

        /// <summary>
        /// Draws the primitives. (Lines).
        /// </summary>
        /// <param name="graphicsDevice"></param>
        private void DrawPrimitives(GraphicsDevice graphicsDevice)
        {
            vertexBuffer.SetData<VertexPositionColor>(vertexPositionColor.ToArray());
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, vertexPositionColor.Count - 1);
            }
        }

        /// <summary>
        /// Draws primitives (rectangles, triangles).
        /// </summary>
        /// <param name="graphicsDevice"></param>
        private void DrawFilledPrimitives(GraphicsDevice graphicsDevice)
        {
            vertexBuffer.SetData<VertexPositionColor>(vertexPositionColor.ToArray());
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, vertexPositionColor.Count - 2);
            }
        }

        /// <summary>
        /// Draws primitive triangles for circles.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        private void DrawCirclePrimitives(GraphicsDevice graphicsDevice)
        {
            vertexBufferB.SetData<VertexPositionColor>(vertexPositionColor.ToArray());
            graphicsDevice.SetVertexBuffer(vertexBufferB);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, vertexPositionColor.Count-2);
            }
        }




        
    }
}
