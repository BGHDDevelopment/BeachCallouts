using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace BeachCallouts
{
    
    [CalloutProperties("Boat Ashore", "BGHDDevelopment", "1.0.0")]
    public class BoatOnShore : Callout
    {
        private Vehicle car;
        Ped driver, passenger;
        private string[] carList = { "tug", "Dinghy", "Jetmax", "Speeder", "Speeder2", "Squalo", "Submersible", "Submersible2", "Suntrap", "Toro", "Tropic", "Tropic2"};

        public BoatOnShore()
        {
            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if(x <= 40)
            { 
                InitInfo(new Vector3(-1576.69f, -1221.67f, 1.46055f));
            }
            else if(x > 40 && x <= 65)
            {
                InitInfo(new Vector3(-1435.31f, -1554.3f, 1.5076f));
            }
            else
            {
                InitInfo(new Vector3(-1767.66f, -1014.86f, 1.93437f));
            }
            ShortName = "Boat Ashore";
            CalloutDescription = "A boat has run aground.";
            ResponseCode = 2;
            StartDistance = 150f;
        }
        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            driver = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            passenger = await SpawnPed(RandomUtils.GetRandomPed(), Location + 1);
            Random random = new Random();
            string cartype = carList[random.Next(carList.Length)];
            VehicleHash Hash = (VehicleHash) API.GetHashKey(cartype);
            car = await SpawnVehicle(Hash, Location);
            PlayerData playerData = Utilities.GetPlayerData();
            string displayName = playerData.DisplayName;
            Notify("~r~[BeachCallouts] ~y~Officer ~b~" + displayName + ",~y~ a " + cartype + " has washed ashore!");
           
            
            //Driver Data
            PedData data = new PedData();
            data.BloodAlcoholLevel = 0.09;
            Utilities.SetPedData(driver.NetworkId,data);
            
            //Passenger Data
            PedData data2 = new PedData();
            List<Item> items = new List<Item>();
            data2.BloodAlcoholLevel = 0.01;
            Item Pistol = new Item {
                Name = "Pistol",
                IsIllegal = true
            };
            items.Add(Pistol);
            data2.Items = items;
            Utilities.SetPedData(passenger.NetworkId,data2);
            //Tasks
            driver.AlwaysKeepTask = true;
            driver.BlockPermanentEvents = true;
            passenger.AlwaysKeepTask = true;
            passenger.BlockPermanentEvents = true;
            passenger.Weapons.Give(WeaponHash.Pistol, 20, true, true);
            driver.Task.WanderAround();
            car.AttachBlip();
            driver.AttachBlip();
            passenger.AttachBlip();
            API.Wait(6000);
            PedData data4 = await Utilities.GetPedData(passenger.NetworkId);
            PedData data1 = await Utilities.GetPedData(driver.NetworkId);
            string firstname2 = data4.FirstName;
            string firstname = data1.FirstName;
            DrawSubtitle("~r~[" + firstname2 + "] ~s~I am sorry I can't be blamed for this!", 5000);
            passenger.Kill();
            API.Wait(2000);
            DrawSubtitle("~r~[" + firstname + "] ~s~NO WHY WOULD YOU DO THAT!", 5000);
            API.Wait(5000);
            DrawSubtitle("~r~[" + firstname + "] ~s~I need to get out of here....", 5000);
            driver.SetIntoVehicle(car, VehicleSeat.Driver);
            driver.Task.FleeFrom(player);
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
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