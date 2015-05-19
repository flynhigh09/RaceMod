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
        private UIText _headsup;
        private UIRectangle _headsupRectangle;
        public UIText _bigmessage;

        public Blip _Start;
        Blip _chks;

        float racercarhealthmax = 0;
        float racerhealthmax = 0;
        float racerhealth = 0;
        float racerSpeed = 100f;

        Vehicle RaceCar01;
        Vehicle RaceCar02;
        Vehicle playerRaceCar;
        Ped playerRacer;
        Ped Racer01;
        Ped Racer02;
        
        TaskSequence Racer01Task;
        TaskSequence Racer02Task;

        private readonly List<Ped> _racers = new List<Ped>();

        public Vector3 SpawnPointPlayer = new Vector3(648.682f, 3502.916f, 33.27346f);
        Vector3 Racer01Spawn = new Vector3(638.395f, 3503.451f, 35.62923f);

        Vector3 RaceCar01Spawn = new Vector3(638.435f, 3518.02f, 33.58217f);
        Vector3 RaceCar02Spawn = new Vector3(638.435f, 3522.02f, 33.58217f);
        Vector3 RaceCar03Spawn = new Vector3(638.435f, 3526.02f, 33.58217f);

        Vector3 Point01 = new Vector3(412.771f, 3502.336f, 34.02979f);
        Vector3 Point02 = new Vector3(330.752f, 3583.387f, 32.6484f);
        Vector3 Point03 = new Vector3(905.267f, 3633.396f, 32.01043f);
        private readonly Vector3[] _checkpoints = {
        new Vector3(412.771f, 3502.336f, 34.02979f),
        new Vector3(330.752f, 3583.387f, 32.6484f),
        new Vector3(905.267f, 3633.396f, 32.01043f)};

        int CurrentCheckpoint;
        
        public Race01()
        {
            Interval = 250;
            Tick += Race01_Tick;       
        }
   
        internal void Initialize(VehicleHash VehicleModel)
        {
            playerRacer = Game.Player.Character;

            //Vehicles
            RaceCar01 = World.CreateVehicle(VehicleModel, RaceCar01Spawn, 98.702f);
            RaceCar02 = World.CreateVehicle(VehicleModel, RaceCar02Spawn, 98.702f);
            playerRaceCar = World.CreateVehicle(VehicleModel, RaceCar03Spawn, 98.702f);

            //PEDS
            Racer01 = Function.Call<Ped>(Hash.CREATE_RANDOM_PED, 637.415f, 3517.02f, 35.62923f);
            Racer02 = Function.Call<Ped>(Hash.CREATE_RANDOM_PED, 638.415f, 3519.02f, 35.62923f);

            //Mods
            RaceCar01.NeonLightsColor = Color.FromArgb(55, 255, 130);
            playerRaceCar.NeonLightsColor = Color.FromArgb(75, 99, 175);
            RaceCar02.NeonLightsColor = Color.FromArgb(50, 190, 30);

            RaceCar01.SetNeonLightsOn(VehicleNeonLight.Left,true);
            RaceCar01.SetNeonLightsOn(VehicleNeonLight.Right, true);

            RaceCar02.SetNeonLightsOn(VehicleNeonLight.Right, true);
            RaceCar02.SetNeonLightsOn(VehicleNeonLight.Left, true);

            playerRaceCar.SetNeonLightsOn(VehicleNeonLight.Left, true);
            playerRaceCar.SetNeonLightsOn(VehicleNeonLight.Right, true);
            playerRaceCar.NumberPlate = "Fly";
            playerRaceCar.CustomPrimaryColor = Color.FromArgb(25, 130, 210);
            playerRaceCar.CustomSecondaryColor = Color.FromArgb(199, 22, 20);

            //Tasks
            playerRacer.Task.WarpIntoVehicle(playerRaceCar, VehicleSeat.Driver);

            RacerTask();
            Racer2Task();

            Racer02.Task.PerformSequence(Racer02Task);
            Racer01.Task.PerformSequence(Racer01Task);

            //Race?
            OnCheckpoint(3);            
        }

        void Racer2Task()
        {
            Racer02Task = new TaskSequence();
            
            Racer02Task.AddTask.WarpIntoVehicle(RaceCar02, VehicleSeat.Driver);
            Racer02Task.AddTask.DriveTo(RaceCar02, Point01, racerSpeed, (int)DrivingStyle.AvoidTraffic);
            Racer02Task.AddTask.DriveTo(RaceCar02, Point02, racerSpeed, (int)DrivingStyle.AvoidTraffic);
           // Racer02Task.AddTask.FollowPointRoute(Point03);
        }
        void RacerTask()
        {
            Racer01Task = new TaskSequence();

            Racer01Task.AddTask.WarpIntoVehicle(RaceCar01, VehicleSeat.Driver);                
            Racer01Task.AddTask.DriveTo(RaceCar01, Point01, racerSpeed, (int)DrivingStyle.AvoidTraffic);
            Racer01Task.AddTask.DriveTo(RaceCar01, Point02, racerSpeed, (int)DrivingStyle.AvoidTraffic);            
           // Racer01Task.AddTask.FollowPointRoute(Point03);
        }

        void Race01_Tick(object sender, EventArgs e)
        {
            if (playerRacer.Position.DistanceTo(Point01) < 10f && CurrentCheckpoint == 0)
            {
                CurrentCheckpoint += 1;
                OnCheckpoint(1);

                UI.Notify("Reached Checkpoint 1/NAN");
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

                UI.Notify("Reached Checkpoint 3/NAN");
            }
        }

        void OnCheckpoint(int cpointID)
        {
            for(int i = 0; i < Getcheckpoints().Length(); i++)
            //_chks = World.CreateBlip(i);
            Function.Call(Hash.ADD_BLIP_FOR_COORD, i);
            Function.Call(Hash.SET_BLIP_COLOUR, _chks.Handle, 1);
            Function.Call(Hash.SET_BLIP_ROUTE, _chks.Handle, true);

             racercarhealthmax = RaceCar01.MaxHealth;
             racerhealthmax = Racer01.MaxHealth;
             racerhealth = Racer01.Health;

            if (cpointID == 0)
            {
                if (racercarhealthmax <= 0)
                    racerhealth = 0;
               // Checkpoint.Position = Point01;     
            }
            if (cpointID == 1 && CurrentCheckpoint == 1)
            {
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
    }
}