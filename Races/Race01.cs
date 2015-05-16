using System;
using GTA;
using GTA.Native;
using GTA.Math;

namespace SPraceMod.Races
{
    class Race01 : Script
    {

        


        Vehicle RaceCar01;
        Ped Racer01;
        Vector3 SpawnPointPlayer = new Vector3(648.682f, 3502.916f, 33.27346f);
        Vector3 Racer01Spawn = new Vector3(637.395f, 3505.451f, 35.62923f);
        Vector3 RaceCar01Spawn = new Vector3(638.435f, 3502.02f, 33.58217f);
        TaskSequence Racer01Task;


        float racerSpeed = 200f;


        Vector3 Point01 = new Vector3(412.771f, 3502.336f, 34.02979f);
        Vector3 Point02 = new Vector3(330.752f, 3583.387f, 32.6484f);
        Vector3 Point03 = new Vector3(905.267f, 3633.396f, 32.01043f);

       
        int CurrentCheckpoint;

        public Race01()
        {
            Interval = 250;
            Tick += Race01_Tick;


           
        }

       

        internal void Initialize(int NumberOfRacers, string VehicleModel)
        {

            RaceCar01 = World.CreateVehicle(VehicleModel, RaceCar01Spawn, 95f);
            Racer01 = World.CreatePed(GTA.Native.PedHash.Barry, Racer01Spawn);
            SetupTasksequences();
            RaceCar01.MarkAsNoLongerNeeded();
            Racer01.MarkAsNoLongerNeeded();
            Racer01.Task.PerformSequence(Racer01Task);
            OnCheckpoint(0);
            
        }
        void SetupTasksequences()
        {
            Racer01Task = new TaskSequence();
            Racer01Task.AddTask.WarpIntoVehicle(RaceCar01, VehicleSeat.Driver);
            Racer01Task.AddTask.DriveTo(RaceCar01, Point01, 300f, (int)DrivingStyle.AvoidTrafficExtremely);
            Racer01Task.AddTask.DriveTo(RaceCar01, Point02, 300f, (int)DrivingStyle.AvoidTrafficExtremely);
          //  Racer01Task.AddTask.DriveTo(RaceCar01, Point03, 300f, (int)DrivingStyle.AvoidTrafficExtremely);
            Racer01Task.Close();
        }



        void Race01_Tick(object sender, EventArgs e)
        {
            if (Game.Player.Character.Position.DistanceTo(Point01) < 10f && CurrentCheckpoint == 0)
            {

                CurrentCheckpoint += 1;
                OnCheckpoint(1);

                UI.Notify("Reached Checkpoint 1/NAN");
            }
            if (Game.Player.Character.Position.DistanceTo(Point02) < 10f && CurrentCheckpoint == 1)
            {

                CurrentCheckpoint += 1;
                OnCheckpoint(2);


                UI.Notify("Reached Checkpoint 2/NAN");
            }
            if (Game.Player.Character.Position.DistanceTo(Point03) < 10f && CurrentCheckpoint == 1)
            {

                CurrentCheckpoint += 1;
                OnCheckpoint(3);


                UI.Notify("Reached Checkpoint 3/NAN");
            }
        }

        void OnCheckpoint(int cpointID)
        {

           // Blip Checkpoint = World.CreateBlip(new Vector3(0f, 0f, 0f), 8f);

            if(cpointID == 0)
            {
                //Checkpoint.Position = Point01;
                if (Game.Player.Character.IsInVehicle())
                {
                    Vehicle oldCar = Game.Player.Character.CurrentVehicle;
                    oldCar.Delete();
                }

                Vehicle NewCar = World.CreateVehicle("BTYPE", SpawnPointPlayer, 98.702f);
                Game.Player.Character.Task.WarpIntoVehicle(NewCar, VehicleSeat.Driver);

                
            }
            if (cpointID == 1 && CurrentCheckpoint == 1)
            {
                //Checkpoint.Position = Point02;
            }
            if (cpointID == 2 && CurrentCheckpoint == 2)
            {
                //Checkpoint.Remove();
            }
        }

    }
}
