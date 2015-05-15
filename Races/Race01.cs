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
        Vector3 Racer01Spawn = new Vector3(637.395f, 3505.451f, 35.62923f);
        Vector3 RaceCar01Spawn = new Vector3(638.435f, 3502.02f, 33.58217f);
        TaskSequence Racer01Task;





        Vector3 Point01 = new Vector3(412.771f, 3502.336f, 34.02979f);
        Vector3 Point02 = new Vector3(330.752f, 3583.387f, 32.6484f);

        public Race01()
        {
            Interval = 250;
            Tick += Race01_Tick;
        }

       

        internal void Initialize(int NumberOfRacers, string VehicleModel)
        {

            RaceCar01 = World.CreateVehicle(VehicleModel, RaceCar01Spawn, 95f);
            Racer01 = World.CreatePed(GTA.Native.PedHash.Barry, Racer01Spawn);
            Racer01.DrivingSpeed = 100f;
            Racer01.Task.WarpIntoVehicle(RaceCar01, VehicleSeat.Driver);
            RaceCar01.MarkAsNoLongerNeeded();
            Racer01.MarkAsNoLongerNeeded();
            Racer01.Task.PerformSequence(Racer01Task);
            
        }
        void SetupTasksequences()
        {
            Racer01Task = new TaskSequence();
            Racer01Task.AddTask.DriveTo(RaceCar01, Point01, 5f, 100 / 2f, (int)DrivingStyle.AvoidTrafficExtremely);
            Racer01Task.AddTask.DriveTo(RaceCar01, Point02, 5f, 100 / 2f, (int)DrivingStyle.AvoidTrafficExtremely);
            Racer01Task.Close();
        }



        void Race01_Tick(object sender, EventArgs e)
        {
            if(Racer01Task.Count == 1)
            {
                RaceCar01.PrimaryColor = VehicleColor.UtilSilver;
            }
            if (Racer01Task.Count == 2)
            {
                RaceCar01.PrimaryColor = VehicleColor.UtilRed;
            }
        }

    }
}
