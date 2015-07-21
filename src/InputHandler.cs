using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TajTetrisGame
{
    class InputHandler
    {
        private Keys[] keysLastUpdate;
        private Keys[] pressedKeys;

        private List<Keys> buffer;

        private bool leftMousePressed, leftMouseJustPressed, leftMouseJustReleased;
        private Point leftMouseDownPoint;
        //I need to add vectors to these.
        //This will add dragging detection.
        private bool rightMousePressed, rightMouseJustPressed, rightMouseJustReleased;
        private Point rightMouseDownPoint;
        private Point FixPoint;


        public InputHandler()
        {
            buffer = new List<Keys>();
        }

        public void SetFixPoint(int offsetX, int offsetY)
        {
            FixPoint = new Point(offsetX, offsetY);
        }

        public void Update()
        {
            keysLastUpdate = pressedKeys;
            pressedKeys = Keyboard.GetState().GetPressedKeys();

            #region Right Mouse Detection
            {
                if (rightMouseJustReleased)
                {
                    rightMouseJustReleased = false;
                }

                if (rightMousePressed)
                {
                    rightMouseJustPressed = false;
                }

                if (!rightMousePressed && Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    rightMouseJustPressed = true;
                    rightMousePressed = true;
                    rightMouseDownPoint = Mouse.GetState().Position - FixPoint;
                }

                if (rightMousePressed && Mouse.GetState().RightButton == ButtonState.Released)
                {
                    rightMousePressed = false;
                    rightMouseJustReleased = true;
                }
            }
            #endregion
            #region Left Mouse Detection
            {
                if (leftMouseJustReleased)
                {
                    leftMouseJustReleased = false;
                }

                if (leftMousePressed)
                {
                    leftMouseJustPressed = false;
                }

                if (!leftMousePressed && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    leftMouseJustPressed = true;
                    leftMousePressed = true;
                    leftMouseDownPoint = Mouse.GetState().Position - FixPoint;
                }

                if (leftMousePressed && Mouse.GetState().LeftButton == ButtonState.Released)
                {
                    leftMousePressed = false;
                    leftMouseJustReleased = true;
                }
            }
            #endregion
        }

        public bool CheckPressedKey(Keys key)
        {
            if (pressedKeys == null) return false;
            return pressedKeys.Contains(key);
        }

        public Keys[] KeysJustPressed()
        {
            buffer.Clear();

            #region Finds the Keys that are NOT in the last update.
            if(keysLastUpdate != null)
            foreach (Keys key in pressedKeys)
            {
                if (!keysLastUpdate.Contains(key))
                {
                    buffer.Add(key);
                }
            }
            #endregion

            return buffer.ToArray();
        }

        public bool CheckJustPressedKey(Keys key)
        {
            return KeysJustPressed().Contains(key);
        }

        
        public Keys[] KeysJustReleased()
        {
            buffer.Clear();
            #region Finds the Keys that are in the last update but not in the current 
            if (keysLastUpdate != null)
            foreach(Keys key in keysLastUpdate)
            {
                if(!pressedKeys.Contains(key))
                {
                    buffer.Add(key);
                }
            }
            #endregion
            return buffer.ToArray();
        }

        public bool CheckJustReleasedKey(Keys key)
        {
            return KeysJustReleased().Contains(key);
        }

        public bool CheckLeftMousePressed()
        {
            return leftMousePressed;
        }

        public bool CheckLeftMouseJustPressed()
        {
            return leftMouseJustPressed;
        }

        public bool CheckLeftMouseJustReleased()
        {
            return leftMouseJustReleased;
        }

        public bool CheckRightMousePressed()
        {
            return rightMousePressed;
        }

        public bool CheckRightMouseJustPressed()
        {
            return rightMouseJustPressed;
        }

        public bool CheckRightMouseJustReleased()
        {
            return rightMouseJustReleased;
        }

        public Point RightMouseDraggedBy()
        {
            if(rightMousePressed)
            return (Mouse.GetState().Position-FixPoint) - rightMouseDownPoint;
            return Point.Zero;
        }

        public Point LeftMouseDraggedBy()
        {
            if(leftMousePressed)
            return (Mouse.GetState().Position-FixPoint) - leftMouseDownPoint;
            return Point.Zero;
        }

        public Point GetMouseXY()
        {
            return (Mouse.GetState().Position - FixPoint);
        }

        public int GetMouseX()
        {
            return (Mouse.GetState().Position - FixPoint).X;
        }

        public int GetMouseY()
        {
            return (Mouse.GetState().Position - FixPoint).Y;
        }
        

        public bool CheckMouseIn(GameObject GO)
        {
            #region Checks if the mouse is within the bounds of the game object.
            if (GetMouseX() > GO.GetX() && GetMouseX() < GO.GetX() + GO.GetWidth())
            {
                if(GetMouseY() > GO.GetY() && GetMouseY() < GO.GetY() + GO.GetHeight())
                {
                    return true;
                }
            }
            #endregion

            return false;
        }

    }
}
