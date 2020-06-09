using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;

namespace BeachCallouts
{
    
    [CalloutProperties("Boat Ashore", "BGHDDevelopment", "0.0.3", Probability.High)]
    public class BoatOnShore : Callout
    {
        private Vehicle car;
        Ped driver, passenger;
        List<object> items = new List<object>();
        List<object> items2 = new List<object>();
        private string[] carList = { "tug", "Dinghy", "Jetmax", "Speeder", "Speeder2", "Squalo", "Submersible", "Submersible2", "Suntrap", "Toro", "Tropic", "Tropic2"};

        public BoatOnShore()
        {
            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if(x <= 40)
            { 
                InitBase(new Vector3(-1576.69f, -1221.67f, 1.46055f));
            }
            else if(x > 40 && x <= 65)
            {
                InitBase(new Vector3(-1435.31f, -1554.3f, 1.5076f));
            }
            else
            {
                InitBase(new Vector3(-1767.66f, -1014.86f, 1.93437f));
            }
            ShortName = "Boat Ashore";
            CalloutDescription = "A boat has run aground.";
            ResponseCode = 2;
            StartDistance = 150f;
        }
        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            passenger.Weapons.Give(WeaponHash.Pistol, 20, true, true);
            driver.Task.WanderAround();
            car.AttachBlip();
            driver.AttachBlip();
            passenger.AttachBlip();
            API.Wait(6000);
            dynamic data2 = await GetPedData(passenger.NetworkId);
            dynamic data1 = await GetPedData(driver.NetworkId);
            string firstname2 = data2.Firstname;
            string firstname = data1.Firstname;
            DrawSubtitle("~r~[" + firstname2 + "] ~s~I am sorry I can't be blamed for this!", 5000);
            //API.ExplodePedHead(passenger.GetHashCode(), 0x1B06D57);
            passenger.Kill();
            API.Wait(2000);
            DrawSubtitle("~r~[" + firstname + "] ~s~NO WHY WOULD YOU DO THAT!", 5000);
            API.Wait(5000);
            DrawSubtitle("~r~[" + firstname + "] ~s~I need to get out of here....", 5000);
            driver.SetIntoVehicle(car, VehicleSeat.Driver);
            driver.Task.FleeFrom(player);
        }

        public async override Task Init()
        {
            OnAccept();
            driver = await SpawnPed(GetRandomPed(), Location + 2);
            passenger = await SpawnPed(GetRandomPed(), Location + 1);
            Random random = new Random();
            string cartype = carList[random.Next(carList.Length)];
            VehicleHash Hash = (VehicleHash) API.GetHashKey(cartype);
            car = await SpawnVehicle(Hash, Location);
            dynamic playerData = GetPlayerData();
            string displayName = playerData.DisplayName;
            Notify("~r~[BeachCallouts] ~y~Officer ~b~" + displayName + ",~y~ a " + cartype + " has washed ashore!");
            //Driver Data
            dynamic data = new ExpandoObject();
            data.alcoholLevel = 0.09;
            SetPedData(driver.NetworkId,data);
            
            //Passenger Data
            dynamic data2 = new ExpandoObject();
            data2.alcoholLevel = 0.01;
            object Pistol = new {
                Name = "Pistol",
                IsIllegal = true
            };
            items2.Add(Pistol);
            data2.items = items2;
            SetPedData(passenger.NetworkId,data2);
            //Tasks
            driver.AlwaysKeepTask = true;
            driver.BlockPermanentEvents = true;
            passenger.AlwaysKeepTask = true;
            passenger.BlockPermanentEvents = true;
        }
        
        private void Notify(string message)
        {
            API.BeginTextCommandThefeedPost("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandThefeedPostTicker(false, true);
        }
        private void DrawSubtitle(string message, int duration)
        {
            API.BeginTextCommandPrint("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandPrint(duration, false);
        }
    }
}