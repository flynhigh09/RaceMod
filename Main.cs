using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using SPraceMod.Races;

namespace SPraceMod
{
    public class Main : Script 
    {
        Race01 Race01 = new Race01();

        string VehicleModel = "BTYPE";


        public Main()
        {
            LoadConfig();
            Interval = 200;
            Tick += Main_Tick;
            KeyDown += KeyDownEvent;

            


        }

        private void LoadConfig()
        {
            Settings.SetValue("RACE SETTINGS", "VEHICLE MODEL", "BODHI2");
        }

        void Main_Tick(object sender, EventArgs e)
        {
            GTA.UI.ShowSubtitle("X:" + String.Format("{0:0,0.000}", Game.Player.Character.Position.X) + " Y:" + Game.Player.Character.Position.Y + " Z:" + Game.Player.Character.Position.Z + "  Rotation =" + String.Format("{0:0,0.000}", Game.Player.Character.Heading));
        }




        void KeyDownEvent(object sender, EventArgs e)
        {
            if (Game.IsKeyPressed(Keys.NumPad1))
            {
                Race01.Initialize(1, VehicleModel);
                
            }
        }
    }
}
