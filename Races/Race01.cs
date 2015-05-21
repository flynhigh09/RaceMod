using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;

namespace SPraceMod.Races
{
    class Race01 : Script
    {
        private UIRectangle _headsupRectangle;
        public static UIText _bigmessage;
            
        public Blip _Start;
        Blip _chks;

        Vehicle RaceCar01;
        Vehicle RaceCar02;
        Vehicle playerRaceCar;
        Ped playerRacer;
        Ped Racer01;
        Ped Racer02;
        
        TaskSequence Racer01Task;
        TaskSequence Racer02Task;

        private readonly List<Ped> _racers = new List<Ped>();

        float racercarhealthmax = 0;
        float racerhealthmax = 0;
        float racerhealth = 0;
        float racerSpeed = 100f;

        public Vector3 SpawnPointPlayer = new Vector3(648.682f, 3502.916f, 33.27346f);
        Vector3 Racer01Spawn = new Vector3(638.395f, 3503.451f, 35.62923f);

        Vector3 RaceCar01Spawn = new Vector3(638.435f, 3518.02f, 33.58217f);
        Vector3 RaceCar02Spawn = new Vector3(638.435f, 3522.02f, 33.58217f);
        Vector3 RaceCar03Spawn = new Vector3(638.435f, 3515.02f, 33.58217f);

        Vector3 Point01 = new Vector3(412.771f, 3502.336f, 34.02979f);
        Vector3 Point02 = new Vector3(330.752f, 3583.387f, 32.6484f);
        Vector3 Point03 = new Vector3(905.267f, 3633.396f, 32.01043f);
        private readonly Vector3[] _checkpoints = {
        new Vector3(412.771f, 3502.336f, 34.02979f),
        new Vector3(330.752f, 3583.387f, 32.6484f),
        new Vector3(905.267f, 3633.396f, 32.01043f)};

        int CurrentCheckpoint;

        Random rnd;

        public Race01()
        {
            Interval = 250;
            Tick += Race01_Tick;
            rnd = new Random();
        }
   
        Color Getrandomcolor()
        {
            return Color.FromArgb(rnd.Next(0,255), rnd.Next(0, 255), rnd.Next(0, 255));
        }
        int getrandomnumb(int nlow, int nhigh)
        {
            return rnd.Next(nlow, nhigh);
        }

        internal void Initialize(VehicleHash VehicleModel)
        {
            UI.ShowSubtitle("~b~Race to Checkpoint 1 ~s~");
            playerRacer = Game.Player.Character;

            //Vehicles
            RaceCar01 = World.CreateVehicle(VehicleModel, RaceCar01Spawn, 98.702f);
            RaceCar02 = World.CreateVehicle(VehicleModel, RaceCar02Spawn, 98.702f);
            playerRaceCar = World.CreateVehicle(VehicleModel, RaceCar03Spawn, 98.702f);

            //PEDS
            Racer01 = Function.Call<Ped>(Hash.CREATE_RANDOM_PED, 637.415f, 3517.02f, 35.62923f);
            Racer02 = Function.Call<Ped>(Hash.CREATE_RANDOM_PED, 638.415f, 3519.02f, 35.62923f);

            //Mods
            RaceCar01.NeonLightsColor = Getrandomcolor();
            playerRaceCar.NeonLightsColor = Getrandomcolor();
            RaceCar02.NeonLightsColor = Getrandomcolor();

            RaceCar01.SetNeonLightsOn(VehicleNeonLight.Left,true);
            RaceCar01.SetNeonLightsOn(VehicleNeonLight.Right, true);

            RaceCar02.SetNeonLightsOn(VehicleNeonLight.Right, true);
            RaceCar02.SetNeonLightsOn(VehicleNeonLight.Left, true);
          
            playerRaceCar.SetNeonLightsOn(VehicleNeonLight.Left, true);
            playerRaceCar.SetNeonLightsOn(VehicleNeonLight.Right, true);
            playerRaceCar.NumberPlate = "FlynHi";
            Function.Call(Hash.SET_VEHICLE_NUMBER_PLATE_TEXT_INDEX, playerRaceCar, 5);

            playerRaceCar.CustomPrimaryColor = Getrandomcolor();
            playerRaceCar.CustomSecondaryColor = Getrandomcolor();
            RaceCar02.CustomPrimaryColor = Getrandomcolor();
            RaceCar02.CustomSecondaryColor = Getrandomcolor();
            RaceCar01.CustomPrimaryColor = Getrandomcolor();
            RaceCar01.CustomSecondaryColor = Getrandomcolor();

            playerRaceCar.SetMod(VehicleMod.Engine, (int)VehicleModel, true);
            playerRaceCar.SetMod(VehicleMod.Transmission, (int)VehicleModel, true);
                
            Function.Call(Hash._0x93A3996368C94158, playerRaceCar, 10000.0f);//SET_VEHICLE_ENGINE_POWER_MULTIPLIER
            //Function.Call(Hash.SET_VEHICLE_LIVERY, playerRaceCar, 5);
            Function.Call(Hash.SET_VEHICLE_MOD_COLOR_1, playerRaceCar, 5, 0, 0);
            Function.Call(Hash.SET_VEHICLE_WHEEL_TYPE, playerRaceCar, getrandomnumb(1,7));

            //Tasks
            playerRacer.Task.WarpIntoVehicle(playerRaceCar, VehicleSeat.Driver);

            RacerTask();

            RaceCar01.MarkAsNoLongerNeeded();
            RaceCar02.MarkAsNoLongerNeeded();
            Racer01.MarkAsNoLongerNeeded();
            Racer02.MarkAsNoLongerNeeded();

            Racer01.Task.PerformSequence(Racer01Task);
            Racer02.Task.PerformSequence(Racer02Task);
                    
            //Race?
            OnCheckpoint(0);            
        }

        void RacerTask()
        {
            Racer01Task = new TaskSequence();

            Racer01Task.AddTask.WarpIntoVehicle(RaceCar01, VehicleSeat.Driver);
            Racer01Task.AddTask.DriveTo(RaceCar01, Point01, 5f, racerSpeed, (int)DrivingStyle.AvoidTraffic);
            Racer01Task.AddTask.DriveTo(RaceCar01, Point02, 5f, racerSpeed, (int)DrivingStyle.AvoidTraffic);
            Racer01Task.AddTask.DriveTo(RaceCar01, Point03, 5f, racerSpeed, (int)DrivingStyle.AvoidTraffic);

            Racer01Task.Close();

            Racer02Task = new TaskSequence();

            Racer02Task.AddTask.WarpIntoVehicle(RaceCar02, VehicleSeat.Driver);
            Racer02Task.AddTask.DriveTo(RaceCar02, Point01, 5f, racerSpeed, (int)DrivingStyle.AvoidTraffic);
            Racer02Task.AddTask.DriveTo(RaceCar02, Point02, 5f, racerSpeed, (int)DrivingStyle.AvoidTraffic);
            Racer02Task.AddTask.DriveTo(RaceCar02, Point03, 5f, racerSpeed, (int)DrivingStyle.AvoidTraffic);

            Racer02Task.Close();
        }

        void Race01_Tick(object sender, EventArgs e)
        {
            racercarhealthmax = RaceCar01.MaxHealth;
            racerhealthmax = Racer01.MaxHealth;
            racerhealth = Racer01.Health;
        
            if (playerRacer.Position.DistanceTo(Point01) < 10f && CurrentCheckpoint == 0)
            {              
                CurrentCheckpoint += 1;
                OnCheckpoint(1);
                
                UI.ShowSubtitle("Reached Checkpoint 1/NAN");
            }
            if (playerRacer.Position.DistanceTo(Point02) < 10f && CurrentCheckpoint == 1)
            {
                CurrentCheckpoint += 1;
                OnCheckpoint(2);

                UI.Notify("Reached Checkpoint 2/NAN");
            }
            if (playerRacer.Position.DistanceTo(Point03) < 10f && CurrentCheckpoint == 1)
            {
                CurrentCheckpoint += 1;
                OnCheckpoint(3);

                UI.ShowSubtitle("Reached Checkpoint 3/NAN");
            }
        }

        void OnCheckpoint(int cpointID)
        {
            _chks = World.CreateBlip(Point01);
            Function.Call(Hash.SET_BLIP_COLOUR, _chks.Handle, 1);
            Function.Call(Hash.SET_BLIP_ROUTE, _chks.Handle, true);

            //ShowMessage("Racer health: ~b~" + racerhealthmax + "~w~", 100, Color.Crimson, 2.5f);
            // _bigmessage = new UIText("Racer health: ~b~" + racerhealthmax, new Point(75, 325), 0.7f, Color.Crimson, 1, true);
            // _headsupRectangle = new UIRectangle(new Point(0, 320), new Size(200, 110), Color.FromArgb(100, 0, 0, 0));

            if (cpointID == 0)
            {           
                if (racercarhealthmax <= 0)
                    racerhealth = 0;
             //  Checkpoint.Position = Point01;     
            }
            if (cpointID == 1 && CurrentCheckpoint == 1)
            {
                _chks.Remove();
                _chks = World.CreateBlip(Point02);
                Function.Call(Hash.SET_BLIP_COLOUR, _chks.Handle, 1);
                Function.Call(Hash.SET_BLIP_ROUTE, _chks.Handle, true);

                // Checkpoint.Position = Point02;
            }
            if (cpointID == 2 && CurrentCheckpoint == 2)
            {
                //Checkpoint.Remove();
            }
        }
        
        private Vector3 Getcheckpoints()
        {
            var lastDist = float.MaxValue;
            Vector3 outputDist = new Vector3(0.0f, 0.0f, 0.0f);
            foreach (var chcks in _checkpoints)
            {
                if ((chcks - Racer01.Position).Length() < lastDist)
                {
                    outputDist = chcks;
                    lastDist = (chcks - Racer01.Position).Length();
                }
            }      
            return outputDist;
        }

        Ped randomracers(int NumberOfRacers)
        {
               int[] peds = {NumberOfRacers};
               Racer01 = Function.Call<Ped>(Hash.CREATE_RANDOM_PED, 637.415f, 3517.02f, 35.62923f);
            return Racer01;
        }

        Ped GetRandompeds()
        {
            OutputArgument outArg = new OutputArgument();
            Racer01 = Function.Call<Ped>(Hash.CREATE_RANDOM_PED, outArg, 637.415f, 3517.02f, 35.62923f);
            Ped output = outArg.GetResult<Ped>();
            return output;
        }

        void AddCash(int amount)
        {
            string statNameFull = string.Format("SP{0}_TOTAL_CASH", 
            (Game.Player.Character.Model.Hash == new Model("player_zero").Hash) ? 0 :    //Michael
            (Game.Player.Character.Model.Hash == new Model("player_one").Hash) ? 1 :     //Franklin
            (Game.Player.Character.Model.Hash == new Model("player_two").Hash) ? 2 : 0); //Trevor

            int name = Function.Call<int>(Hash.GET_HASH_KEY, statNameFull);
            OutputArgument outArg = new OutputArgument();
            Function.Call<bool>(Hash.STAT_GET_INT, name, outArg, -1);
            var cash = outArg.GetResult<int>() + amount;
            Function.Call(Hash.STAT_SET_INT, name, cash, true);
        }

        public static void ShowMessage(string text, int time, Color color, float size = 2.5f)
        {          
            _bigmessage.Caption = text;
            _bigmessage.Color = color;
            _bigmessage.Scale = size;
        }

        private void LogToFile(string text)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.Write(text + "\n");
            }
        }
    }
}