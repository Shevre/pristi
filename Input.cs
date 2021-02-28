using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace pristi
{ 
    public class InputManager
    {
        public Action MoveUp = new Action("MoveUp",Keys.Up, Keys.W, Buttons.DPadUp, Buttons.LeftThumbstickUp);
        public Action MoveDown = new Action("MoveDown", Keys.Down, Keys.A, Buttons.DPadDown, Buttons.LeftThumbstickDown);
        public Action MoveLeft = new Action("MoveLeft", Keys.Left, Keys.S, Buttons.DPadLeft, Buttons.LeftThumbstickLeft);
        public Action MoveRight = new Action("MoveRight", Keys.Right, Keys.D, Buttons.DPadRight, Buttons.LeftThumbstickRight);

        public Action Jump = new Action("Jump", Keys.Space, Keys.None, Buttons.B, null);

        public Action Start = new Action("Start",Keys.Enter, Keys.Enter, Buttons.Start, null);

        public Action Debug = new Action("Debug",Keys.OemTilde, Keys.OemTilde, null, null);
        public Action Control = new Action("Control", Keys.LeftControl, Keys.RightControl, null, null);

        public List<Action> allActions = new List<Action>();

        KeyboardState PrevKbState;
        GamePadState PrevConState;



        public InputManager(){

            PrevKbState = Keyboard.GetState();
            PrevConState = GamePad.GetState(PlayerIndex.One);

            allActions.Add(MoveUp);
            allActions.Add(MoveDown);
            allActions.Add(MoveRight);
            allActions.Add(MoveLeft);
            allActions.Add(Jump);
            allActions.Add(Start);
            allActions.Add(Debug);
            allActions.Add(Control);
        }
        


        public void Update(GameTime gameTime,KeyboardState kbState, GamePadState gamePadState){

            foreach (Action a in allActions){
                a.Update(kbState,gamePadState);
                
            }

            PrevKbState = kbState;
            PrevConState = gamePadState;
        }

        public void DrawDebug(SpriteBatch spriteBatch){
            string debugString = " Name      | IsDown    | IsUp      | IsPressed | IsToggled\n";

            foreach (Action a in allActions){
                debugString += a.ToString() + "\n";
                
                    
            }


            spriteBatch.DrawString(Game1.DebugFont, debugString, Vector2.Zero, Color.Magenta);
        }

        
    }

    public static class GlobalInput
    {
        public static InputManager Manager { get; private set; }
    
        public static void SetManager(InputManager manager){
            Manager = manager;
        }
    }

    public class Action{
        public Keys PrimaryKey;
        public Keys SecondaryKey;
        public Buttons? PrimaryButton;
        public Buttons? SecondaryButton;

        public string Name { get; private set; }

        public bool IsDown { get; private set; }
        public bool IsPressed { get; private set; }
        public bool IsUp { get; private set; }
        public bool IsToggled { get; private set; }

        public Action() { 
        }

        public Action(string name,Keys primaryKey, Keys secondaryKey, Buttons? primaryButton, Buttons? secondaryButton)
        {
            Name = name;

            PrimaryKey = primaryKey;
            SecondaryKey = secondaryKey;
            PrimaryButton = primaryButton;
            SecondaryButton = secondaryButton;
            
            IsDown = false;
            IsPressed = false;
            IsUp = true;
            IsToggled = false;
        }

        public void Update(KeyboardState kbState, GamePadState conState){
            bool buttonDown = ((PrimaryButton != null)?conState.IsButtonDown((Buttons)PrimaryButton):false) || ((SecondaryButton != null)?conState.IsButtonDown((Buttons)SecondaryButton):false);
            


            IsDown = (kbState.IsKeyDown(PrimaryKey) || kbState.IsKeyDown(SecondaryKey) || buttonDown);
            IsPressed = IsUp && IsDown;
            if (IsPressed)
                IsToggled = !IsToggled;
            IsUp = !IsDown;
        }

        public override string ToString()
        {
            string outString = " " + Name;
            for (; outString.Length < 11;)
                outString += " ";

            string boolsString = "| " + IsDown.ToString();
            for (; boolsString.Length < 12;)
                boolsString += " ";
            outString += boolsString;

            boolsString = "| " + IsUp.ToString();
            for (; boolsString.Length < 12;)
                boolsString += " ";
            outString += boolsString;

            boolsString = "| " + IsPressed.ToString();
            for (; boolsString.Length < 12;)
                boolsString += " ";
            outString += boolsString;


            outString += $"| {IsToggled}";
            return outString;
        }


    }
}
