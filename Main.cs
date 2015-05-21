using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using SPraceMod.Races;
using System.ComponentModel;

namespace SPraceMod
{
    public class Main : Script 
    {
        Race01 Race01 = new Race01();
        Ped playerRacer;

        VehicleHash VehicleModel;
        String Vheicle;
        ScriptSettings _settings;

        enum Controls
        {
            DPAD_DOWN = 187,
            DPAD_UP = 188,
            DPAD_RIGHT = 190,
            DPAD_LEFT = 189,
            START = 199,
            SELETE = 217,
            X = 201,
            Square = 203,
            Circle = 202,
            Tringle = 204,
            L1 = 205,
            R1 = 206,
            L2 = 207,
            R2 = 208,
            L3 = 209,
            R3 = 210,
            R3Down = 198,
        };

        Boolean isPressed(int Button)
        {
            InputArgument button = new InputArgument(Button);
            return Function.Call<Boolean>(Hash.IS_CONTROL_JUST_PRESSED, 2, button);
        }
        Boolean isJustPress(int Button)
        {
            InputArgument button = new InputArgument(Button);
            return Function.Call<Boolean>(Hash.IS_CONTROL_PRESSED, 2, button);
        }

        Boolean isReleased(int Button)
        {
            InputArgument button = new InputArgument(Button);
            return Function.Call<Boolean>(Hash.IS_CONTROL_JUST_RELEASED, 2, button);
        }
        Boolean isJustReleased(int Button)
        {
            InputArgument button = new InputArgument(Button);
            return Function.Call<Boolean>(Hash.IS_DISABLED_CONTROL_JUST_RELEASED, 0, button);
        }

        bool isDown(int Button)
        {
            OutputArgument buttons = new OutputArgument();
            InputArgument button = new InputArgument(Button);
          return Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 0, button);
           // bool output = buttons.GetResult<bool>();
          //  return output;
        }
        Boolean isJustDown(int Button)
        {
            InputArgument button = new InputArgument(Button);
            return Function.Call<Boolean>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 0, button);
        }

        public Main()
        {
            LoadConfig();
            Interval = 200;
            Tick += Main_Tick;
            KeyDown += KeyDownEvent;
            KeyUp += KeyUpEvent;

         //  Race01._bigmessage = new UIText("Races", new Point(630, 100), 2.8f, Color.Firebrick, 2, true);
        }

        private void LoadConfig()
        {
              ScriptSettings _settings = ScriptSettings.Load("SPraceMod.net");
              VehicleModel = _settings.GetValue<VehicleHash>("[RACE_SETTINGS]", "VEHICLE", VehicleHash.Dukes2);
        }

        void Main_Tick(object sender, EventArgs e)
        {
           /* Race01._Start = World.CreateBlip(Race01.SpawnPointPlayer);
            Function.Call(Hash.SET_BLIP_COLOUR, Race01._Start.Handle, 2);
            Function.Call(Hash.SET_BLIP_ROUTE, Race01._Start.Handle, true);*/
        }

        void KeyDownEvent(object sender, EventArgs e)
        {
            if (Game.IsKeyPressed(Keys.NumPad1))
            {
                Race01.Initialize(VehicleModel);
            }
            if (Game.IsKeyPressed(Keys.NumPad3))
            {
                playerRacer = Game.Player.Character;
                var pos = playerRacer.Position;
                var pos2 = pos.Around(10f);
                Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, pos.X, pos.Y, pos.Z, pos2.X, pos2.Y, pos2.Z, 15, true, (int)playerRacer.Weapons[WeaponHash.AssaultRifle].Hash, playerRacer.Handle, 1, 1, 1f);
            }
        }
        void KeyUpEvent(object sender, EventArgs e)
        {
            if (Game.IsKeyPressed(Keys.K))
            {
                Ped player = Game.Player.Character;
                float pos = player.Heading;
                UI.ShowSubtitle("X:" + String.Format("{0:0,0.000}", player.Position.X) + " Y:" + player.Position.Y + " Z:" + player.Position.Z + "  Rotation =" + String.Format("{0:0,0.000}", pos));
            }
        }

        #region Commands
        void ActivateInvincibility()
        {
        }

        void DeactivateInvincibility()
        {
        }

        void HealPlayer()
        {
        }

        void ActivateNeverWanted()
        {
        }

        void DeactivateNeverWanted()
        {
        }

        void ActivateUnlimitedAmmo()
        {
        }

        void DeactivateUnlimitedAmmo()
        {
        }

        void DoUnlockAllWeapons()
        {
        }

        #endregion
    }
}