using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace TajTetrisGame
{

    class TempList : Saveable
    {
        public LooseDragDropLink[] elements;

        public void Save(Saver saver)
        {
            saver.Header("TempList");
            saver.SaveArray<LooseDragDropLink>(elements, "Elements");

                saver.End();
        }
    }
    class DragDropInterface
    {
        private DragDropElement[] modes;
        private DragDropElement selectedMode;

        private List<LooseDragDropLink> elements;
        private List<LooseDragDropLink> suggestions;

        private LooseDragDropLink selectedElement;
        private LooseDragDropLink draggedElement;

        private LooseDragDropLink suggestedElement;

        private ContinuousButton scrollDown, scrollUp;

        private int widthOfMenu;

        private Boolean dragging;
        private Point originalPoint;

        private Color background;
        private Color background2;

        private string fileName, safeFileName;

        private int width, height;

        private static byte scrollTextbox;

        private DragDropClassManager classManager;
        
        
        public DragDropInterface(int width, int height)
        {
            background = new Color(0x34, 0x49, 0x5e);
            background2 = new Color(0x2c, 0x3e, 0x50);

            this.width = width;
            this.height = height;

            scrollDown = new ContinuousButton(new Color(0x29, 0x80, 0xb9), new Color(0x34, 0x98, 0xdb));
            scrollUp = new ContinuousButton(new Color(0x29, 0x80, 0xb9), new Color(0x34, 0x98, 0xdb));


            

            

            elements = new List<LooseDragDropLink>();
            suggestions = new List<LooseDragDropLink>();
           

            //Stuff - Remove this
            modes = new DragDropElement[]
            {
                new ModeToggleButton(new Color(0x34, 0x98, 0xdb), new Color(0x29, 0x80, 0xb9), "f(x)"),
                new ModeToggleButton(new Color(0x34, 0x98, 0xdb), new Color(0x29, 0x80, 0xb9), "X="),
                new ModeToggleButton(new Color(0x34, 0x98, 0xdb), new Color(0x29, 0x80, 0xb9), ":)(:") 
            };
            //
            selectedMode = modes[0];
            selectedMode.TellSelected(true);

        }

    
        #region Update an Interaction between two elements.
        private void UpdateInteraction(InputHandler handler)
        {
            #region Check if released element is in the same area as another element
            foreach (LooseDragDropLink element in elements)
            {


                if (element == draggedElement) continue;
                
                
                if (handler.CheckMouseIn(element))
                {
                    #region If the Current Element Dropped in is a Variable Placeholder
                    if (element.GetElement() is DragDropVariablePlaceholder)
                    {
                        bool rep = ((DragDropVariablePlaceholder)element.GetElement()).AddVariable(draggedElement);
                        if (rep)
                        {
                            elements.Remove(draggedElement);
                            draggedElement = null;
                        }

                    }
                    #endregion
                    else
                    #region If the Current Element being Interacted with is a Variable Placeholder
                    if (element.GetElement().GetInteracting(handler) is DragDropVariablePlaceholder)
                    {
                        bool rep = ((DragDropVariablePlaceholder)element.GetElement().GetInteracting(handler)).AddVariable(draggedElement);
                        if (rep)
                        {
                            elements.Remove(draggedElement);
                            draggedElement = null;
                        }
                    }
                    #endregion
                    break;
                }
            }
            #endregion
        
        }
        #endregion

        #region Update Elements on Outer Layer of Interface
        private void UpdateElements(InputHandler handler)
        {
            if (draggedElement == null)
            {
                #region If Mouse Pressed
                if (handler.CheckLeftMouseJustPressed())
                {
                    #region Check if an Element was Clicked
                    for (int i = 0;i < elements.Count; i++)
                    {
                        var element = elements[elements.Count - i - 1];
                        if (handler.CheckMouseIn(element))
                        {
                            #region Set as Dragged Object
                            draggedElement = element;
                            originalPoint = element.GetPoint();
                            elements.Remove(element);
                            elements.Add(element);
                            #endregion
                            break;
                        }
                    }
                    #endregion

                }
                #endregion
            }
            else
            {
                if (dragging)
                {

                    if (handler.CheckLeftMouseJustReleased())
                    {
                        //Just in case stuff needs to be done.
                        #region Release Dragged Object
                        UpdateInteraction(handler);
                        if (draggedElement != null && draggedElement.GetX() < widthOfMenu)
                        {
                            RemoveElement(draggedElement);
                        }
                        draggedElement = null;
                        dragging = false;

                        

                        #endregion
                    }
                    else
                    {
                        #region Set the X and Y of the Dragged Object
                        draggedElement.SetX(originalPoint.X + handler.LeftMouseDraggedBy().X);
                        draggedElement.SetY(originalPoint.Y + handler.LeftMouseDraggedBy().Y);
                        #endregion
                    }
                }
                else
                {
                    #region Detect if Dragged Piece was actually moved.
                    if (!(handler.LeftMouseDraggedBy().X == 0 && handler.LeftMouseDraggedBy().Y == 0))
                    {
                        dragging = true;
                        #region Removes the element from a slot.
                        if (draggedElement.GetElement() is DragDropVariablePlaceholder)
                        {
                            if (((DragDropVariablePlaceholder)draggedElement.GetElement()).HasVariable())
                            {
                                elements.Add(((DragDropVariablePlaceholder)draggedElement.GetElement()).PopVariable());
                                draggedElement = elements[elements.Count - 1];
                                originalPoint = handler.GetMouseXY();
                            }
                        }
                        #endregion
                        else
                        #region Removes the element from a slot that is contained in another piece
                        if (draggedElement.GetElement().GetTopInteracting(handler) is DragDropVariablePlaceholder)
                        {
                            DragDropVariablePlaceholder slot = (DragDropVariablePlaceholder)((DragDropVariablePlaceholder)draggedElement.GetElement().GetTopInteracting(handler)).GetInnerMostDragDrop(handler);

                            if ((slot).HasVariable())
                            {
                                elements.Add((slot).PopVariable());
                                draggedElement = elements[elements.Count - 1];
                              
                                originalPoint = handler.GetMouseXY();
                            }
                        }
                        #endregion
                    }
                    #endregion
                    else
                    #region If the piece is released, select it.
                        if (handler.CheckLeftMouseJustReleased())
                        {
                            if (selectedElement != null && selectedElement != draggedElement) selectedElement.GetElement().TellSelected(false);
                            draggedElement.GetElement().GetInteracting(handler).TellSelected(true);
                            selectedElement = draggedElement;
                            draggedElement = null;
                        }
                    #endregion
                }

            }

        }
        #endregion

        #region Update the suggestions in the side pane.
        private void UpdateSuggestions(InputHandler handler)
        {
            if (draggedElement != null) return;

            if(handler.CheckLeftMouseJustPressed())
            foreach(LooseDragDropLink link in suggestions)
            {
                if(handler.CheckMouseIn(link))
                {
                    suggestedElement = link;
                    break;
                }
            }

            if (handler.CheckLeftMouseJustReleased())
            {
                suggestedElement = null;
            }

            if(suggestedElement != null)
            if (!(handler.LeftMouseDraggedBy().X == 0 && handler.LeftMouseDraggedBy().Y == 0))
            {
                elements.Add(new LooseDragDropLink(suggestedElement.GetElement().Clone()));
                draggedElement = elements[elements.Count - 1];
                Point XY = handler.GetMouseXY();
                originalPoint = XY;
                elements[elements.Count - 1].SetPoint(ref XY);
            }

            
        }
        #endregion

        #region Update the Selected Mode
        private void UpdateMode(InputHandler inputHandler)
        {
            if(inputHandler.CheckLeftMouseJustPressed())
            {
                foreach (ModeToggleButton button in modes)
                {
                    if (inputHandler.CheckMouseIn(button))
                    {
                        selectedMode.TellSelected(false);
                        button.TellSelected(true);
                        selectedMode = button;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Update the Scrolling Buttons
        private void UpdateScrolling(InputHandler handler)
        {
            #region Check if the Mouse was pressed in the button.
            if (handler.CheckLeftMouseJustPressed())
            {
                if(handler.CheckMouseIn(scrollUp))
                {
                    scrollUp.TellSelected(true);
                }
                else
                if(handler.CheckMouseIn(scrollDown))
                {
                    scrollDown.TellSelected(true);
                }
            }
            #endregion

            #region If mouse released, it knows both buttons are not pressed.
            if (handler.CheckLeftMouseJustReleased())
            {
                scrollDown.TellSelected(false);
                scrollUp.TellSelected(false);
            }
            #endregion
        }
        #endregion
       
        //Temporary Variable
        bool test = false;
        

        #region Textbox Handling
        public void UpdateTextBox(InputHandler inputHandler)
        {
            if (selectedElement != null)
            {
                DragDropTextbox textbox = null;
                #region If This is a Textbox
                if (typeof(DragDropTextbox).IsAssignableFrom( selectedElement.GetElement().GetType()))
                {
                    textbox = ((DragDropTextbox)selectedElement.GetElement());
                }
                #endregion
                #region If this is a Placeholder Containing a Textbox
                if (typeof(DragDropVariablePlaceholder).IsAssignableFrom(selectedElement.GetElement().GetType()))
                {
                    DragDropVariablePlaceholder pl = ((DragDropVariablePlaceholder)selectedElement.GetElement());
                    if (typeof(DragDropTextbox).IsAssignableFrom(pl.GetInteracting(inputHandler).GetType()))
                    {
                        textbox = ((DragDropTextbox)selectedElement.GetElement().GetInteracting(inputHandler));
                    }
                }
                #endregion
                #region If there is a textbox
                if (textbox != null)
                {
                    UpdateTextBox(textbox, inputHandler);
                }
            

                #endregion

                }
            
        }

        public static void UpdateTextBox(DragDropTextbox textbox, InputHandler inputHandler)
        {
                #region If there is a textbox
                if (textbox != null)
                {
                    var keys = inputHandler.KeysJustPressed();
                    #region Detect Normal Keys
                    foreach (var key in keys)
                    {

                        char keyLetter = (char)key;
                        if ((keyLetter >= 'A' && keyLetter <= 'Z') || (keyLetter >= '0' && keyLetter <= '9'))
                        {
                            if (!inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            {
                                keyLetter = Char.ToLower(keyLetter);
                            }
                            else
                            {
                                switch (keyLetter)
                                {
                                    case '0':
                                        keyLetter = ')';
                                        break;
                                    case '1':
                                        keyLetter = '!';
                                        break;
                                    case '2':
                                        keyLetter = '@';
                                        break;
                                    case '3':
                                        keyLetter = '#';
                                        break;
                                    case '4':
                                        keyLetter = '$';
                                        break;
                                    case '5':
                                        keyLetter = '%';
                                        break;
                                    case '6':
                                        keyLetter = '^';
                                        break;
                                    case '7':
                                        keyLetter = '&';
                                        break;
                                    case '8':
                                        keyLetter = '*';
                                        break;
                                    case '9':
                                        keyLetter = '(';
                                        break;
                                }
                            }

                            (textbox).AddLetter(keyLetter);

                        }
                    }
                    #endregion
                    #region Other Keys
                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.Back))
                    {
                        (textbox).RemoveLetter();
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.OemMinus))
                    {
                        if (inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            (textbox).AddLetter('_');
                        else
                            (textbox).AddLetter('-');
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.OemPlus))
                    {
                        if (inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            (textbox).AddLetter('+');
                        else
                            (textbox).AddLetter('=');
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.Left))
                    {
                        (textbox).PositionDecrease();
                        scrollTextbox = 1;
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.Enter))
                    {
                        (textbox).Enter();
                    }



                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.Space))
                    {
                        (textbox).AddLetter(' ');
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.OemComma))
                    {
                        if (!inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            (textbox).AddLetter(',');
                        else
                            (textbox).AddLetter('<');

                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.OemPipe))
                    {
                        if (!inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            (textbox).AddLetter('\\');
                        else
                            (textbox).AddLetter('|');

                    }

                    if(inputHandler.CheckJustPressedKey(Keys.OemTilde))
                    {
                         if (!inputHandler.CheckPressedKey(Keys.LeftShift))
                            (textbox).AddLetter('`');
                        else
                            (textbox).AddLetter('~');
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.OemPeriod))
                    {

                        if (!inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            (textbox).AddLetter('.');
                        else
                            (textbox).AddLetter('>');
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.OemQuestion))
                    {
                        if (inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            (textbox).AddLetter('?');
                        else
                            (textbox).AddLetter('/');
                    }

                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.OemQuotes))
                    {
                        if (inputHandler.CheckPressedKey(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                            (textbox).AddLetter('\"');
                        else
                            (textbox).AddLetter('\'');


                    }



                    if (inputHandler.CheckJustPressedKey(Microsoft.Xna.Framework.Input.Keys.Right))
                    {
                        (textbox).PositionIncrease();
                        scrollTextbox = 1;
                    }

                    #endregion
                    #region Arrow Keys
                    if (inputHandler.CheckPressedKey(Keys.Left))
                    {
                        scrollTextbox = (byte)((scrollTextbox < 5) ? (scrollTextbox + 1) : 0);


                        if (scrollTextbox == 0)
                        {
                            (textbox).PositionDecrease();
                        }
                    }
                    else
                        if (inputHandler.CheckPressedKey(Keys.Right))
                        {
                            scrollTextbox = (byte)(scrollTextbox < 5 ? (scrollTextbox + 1) : 0);
                            if (scrollTextbox == 0)
                            {
                                (textbox).PositionIncrease();
                            }
                        }
                        else if (inputHandler.CheckPressedKey(Keys.Back))
                        {
                            scrollTextbox = (byte)(scrollTextbox < 12 ? (scrollTextbox + 1) : 0);
                            if (scrollTextbox == 0)
                            {
                                (textbox).RemoveLetter();
                            }
                        }
                        else
                        {
                            scrollTextbox = 1;
                        }

                    #endregion

                
                #endregion
            }
        }
        #endregion



        public void Update(InputHandler inputHandler)
        {
            #region Update Parts
            UpdateMode(inputHandler);
            UpdateElements(inputHandler);
            UpdateScrolling(inputHandler);
            UpdateTextBox(inputHandler);
            UpdateSuggestions(inputHandler);
            #endregion

            if(inputHandler.CheckJustPressedKey(Keys.E) && inputHandler.CheckPressedKey(Keys.D9))
            {
               TajLevelLinker linker = new TajLevelLinker();
               foreach(LooseDragDropLink link in elements)
                {
                    if(link  != null)
                    {
                        if(link.GetElement() is CodeDragDropHolder)
                        {
                            CodeDragDropHolder holder = ((CodeDragDropHolder)link.GetElement());

                            // "Code", "If", "Else", "EndIf", "Start", "Cleared Row", "Cleared Red", "Cleared Blue", "Cleared Teal", "Cleared Purple", "Cleared Orange", "Cleared Yellow", "Cleared Green", "Cleared Any Color"
                            switch(holder.GetMode())
                            {
                            case 4:
                                linker.OnStart(holder.GetCode());
                                break;
                            case 5 :
                                linker.SetRowCleared(holder.GetCode());
                                break;
                            case 6:
                                
                                linker.SetRedCleared(holder.GetCode());
                                break;
                            case 7:

                                linker.SetBlueCleared(holder.GetCode());
                                break;
                            case 8:

                                linker.SetTealCleared(holder.GetCode());
                                break;
                            case 9:
                                linker.SetPurpleCleared(holder.GetCode());
                                break;
                            case 10:

                                linker.SetOrangeCleared(holder.GetCode());
                                break;
                            case 11:

                                linker.SetYellowCleared(holder.GetCode());
                                break;
                            case 12:
                                linker.SetGreenCleared(holder.GetCode());
                                break;
                            case 13: 
                                linker.SetAnyCleared(holder.GetCode());
                                break;
                            }

                        }
                    }
                }
               linker.CreateGame(fileName);
               linker.SetFullSave(true);
               SaveFileSystem.SaveObjectToFile(linker, "LevelPacks" + Path.DirectorySeparatorChar + safeFileName);
            }

            /*
            if(inputHandler.CheckJustPressedKey(Keys.Y))
            {

                TempList list = new TempList();
                list.elements = this.elements.ToArray();

                SaveFileSystem.SaveObjectToFile(list, "Haha.taj");
            }
            */
            //Temporary Code
            if(classManager == null && !test)
            {

               
                suggestions.Add(new LooseDragDropLink(new StringTextbox()));
                suggestions.Add(new LooseDragDropLink(new NumberTextbox()));
                suggestions.Add(new LooseDragDropLink(new BooleanDragDrop()));
                suggestions.Add(new LooseDragDropLink(new NotBooleanMethod()));
                suggestions.Add(new LooseDragDropLink(new BooleanMethod()));
                suggestions.Add(new LooseDragDropLink(new NumberMethodDragDrop()));
                suggestions.Add(new LooseDragDropLink(new NumberMethod()));
                suggestions.Add(new LooseDragDropLink(new StringMethod()));
                suggestions.Add(new LooseDragDropLink(new SetVariableDragDrop()));
                suggestions.Add(new LooseDragDropLink(new GameCommandDragDrop()));
                suggestions.Add(new LooseDragDropLink(new GetVariableDragDrop()));
                suggestions.Add(new LooseDragDropLink(new CodeTextbox()));
                suggestions.Add(new LooseDragDropLink(new CodeDragDropHolder()));
                
                test = true;
                //Show New Class Button
            }

        }

        private void GetSuggestions()
        {
            int modeType;
            for (modeType = 0; modeType < modes.Length; modeType++)
                if (modes[modeType] == selectedMode) break;

            if(modeType == 2)
            {
                suggestions.Add(new LooseDragDropLink(new NotBooleanMethod()));
            }

        }

        private void DrawModes(SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, GraphicsDevice graphics)
        {
            foreach (DragDropElement element in modes)
            {
                element.Draw(spriteBatch, primitiveDrawer, fontHandler, graphics);
            }
        }

        private void DrawSuggestions(SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, GraphicsDevice graphics)
        {
            int helperY = 50;

            foreach (LooseDragDropLink element in suggestions)
            {
                element.SetY(helperY);
                element.SetX(20);
                element.Draw(spriteBatch, primitiveDrawer, fontHandler, graphics);

                helperY += 31;
            }
        }

        


        private void DrawElements(SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, GraphicsDevice graphics)
        {
            foreach (LooseDragDropLink element in elements)
            {
                element.Draw(spriteBatch, primitiveDrawer, fontHandler, graphics);
            }

        }
       


        /// <summary>
        /// Removes the link from the interface
        /// </summary>
        /// <param name="link"></param>
        public void RemoveElement(LooseDragDropLink link)
        {
            elements.Remove(link);
        }


        /// <summary>
        /// Needed if lazy, or do not have the ability to remove it otherwise.
        /// </summary>
        /// <param name="element"></param>
        public void RemoveElement(DragDropElement element)
        {
            LooseDragDropLink linkA = null;
            foreach(LooseDragDropLink link in elements)
            {
                if(link.GetElement() == element)
                {
                    linkA = link;
                }
            }
            RemoveElement(linkA);
        }

        
       
        public void Draw(SpriteBatch spriteBatch, PrimitiveDrawer primitiveDrawer, FontHandler fontHandler, GraphicsDevice graphics)
        {
            #region Draw the Background of the Interface
            widthOfMenu = modes[0].GetWidth() + modes[1].GetWidth() + modes[2].GetWidth();
            primitiveDrawer.DrawFilledRectangle(graphics, new Rectangle(0, 0, widthOfMenu, height), background);

            DrawSuggestions(spriteBatch, primitiveDrawer, fontHandler, graphics);
           
            primitiveDrawer.DrawFilledRectangle(graphics, new Rectangle(widthOfMenu, 0, width - widthOfMenu, height), background2);
            primitiveDrawer.DrawFilledRectangle(graphics, new Rectangle(widthOfMenu, 0, width - widthOfMenu, modes[0].GetHeight()), background);
            #endregion

            
            #region Draw the Text in the Top Right Corner
            String str = safeFileName;

            if(classManager != null)
            {
                str = classManager.GetName();
            }

            int wdt = (int)fontHandler.GetVerdana().MeasureString(str).X + 3;
            primitiveDrawer.DrawFilledRectangle(graphics, new Rectangle(width - wdt - 6, 0, wdt + 6, modes[0].GetHeight()), new Color(0x29, 0x80, 0xb9));
            spriteBatch.DrawString(fontHandler.GetVerdana(), str, new Vector2(width - wdt, 6) + TetrisGameRunner.GetOffsetVector(), Color.White);
            #endregion

            #region Set the Position of the Scroll Buttons
            scrollDown.SetY(height - scrollDown.GetHeight());
            scrollUp.SetY(height - scrollUp.GetHeight());
            
            scrollDown.SetWidth(widthOfMenu / 2 - 2);
            scrollDown.SetX(1);
            
            scrollUp.SetWidth(widthOfMenu / 2 - 2);
            scrollUp.SetX(widthOfMenu / 2 + 1);
            #endregion

            #region Set the Position of the Mode Buttons
            modes[1].SetX(modes[0].GetWidth());
            modes[2].SetX(modes[0].GetWidth() + modes[1].GetWidth());
            #endregion

            #region Draw the Elements and Modes and Such
            DrawModes(spriteBatch, primitiveDrawer, fontHandler, graphics);
            DrawElements(spriteBatch, primitiveDrawer, fontHandler, graphics);
            #endregion

            #region Draw the Scroll Buttons
            scrollDown.Draw(spriteBatch, primitiveDrawer, fontHandler, graphics);
            scrollUp.Draw(spriteBatch, primitiveDrawer, fontHandler, graphics);
            #endregion
        }





        internal void SetClassManager(DragDropClassManager dragDropClassManager)
        {
            this.classManager = dragDropClassManager;
        }

        internal void SetClassName(string name)
        {
            this.classManager.SetName(name);
        }

        internal void SetFile(string fileName, string safeFileName)
        {
            this.fileName = fileName;
            this.safeFileName = safeFileName;
        }
    }
}
