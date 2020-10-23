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
    [CalloutProperties("Drunk Person", "BGHDDevelopment", "0.0.4")]
    public class DrunkCallout : Callout
    {
        private Ped suspect, suspect2;

        public DrunkCallout()
        {
            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if(x <= 40)
            { 
                InitInfo(new Vector3(-1646.12f, -1115.24f, 13.0266f));
            }
            else if(x > 40 && x <= 65)
            {
                InitInfo(new Vector3(-1325.32f, -1535.14f, 4.32976f));
            }
            else
            {
                InitInfo(new Vector3(-2165.49f, -463.562f, 2.45656f));
            }
            ShortName = "Drunk Person Causing Issues";
            CalloutDescription = "A drunk person is causing issues at the beach.";
            ResponseCode = 2;
            StartDistance = 200f;
        }
        
        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            API.SetPedIsDrunk(suspect.GetHashCode(), true);
            API.SetPedIsDrunk(suspect2.GetHashCode(), true);
            suspect.Task.WanderAround();
            suspect2.Task.WanderAround();
            suspect.AttachBlip();
            suspect2.AttachBlip();
            PedData data1 = await Utilities.GetPedData(suspect.NetworkId);
            string firstname = data1.FirstName;
            DrawSubtitle("~r~[" + firstname + "] ~s~Can I have a beer?", 5000);
            PedData data2 = await Utilities.GetPedData(suspect2.NetworkId);
            string firstname2 = data2.FirstName;
            DrawSubtitle("~r~[" + firstname + "] ~s~SURE!", 5000);
            suspect.Task.FleeFrom(player);
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
            suspect = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            suspect2 = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            
            
            //Suspect Data
            PedData data = new PedData();
            List<Item> items = data.Items;
            data.BloodAlcoholLevel = 0.25;
            Item SixPack = new Item {
                Name = "Six Pack",
                IsIllegal = false
            };
            items.Add(SixPack);
            data.Items = items;
            Utilities.SetPedData(suspect.NetworkId,data);

            //Suspect2 Data
            PedData data2 = new PedData();
            List<Item> items2 = data.Items;
            data2.BloodAlcoholLevel = 0.18;
            Item Beer = new Item {
                Name = "Beer",
                IsIllegal = false
            };
            items.Add(Beer);
            data2.Items = items2;
            Utilities.SetPedData(suspect2.NetworkId,data2);
            
            //Tasks
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            suspect2.AlwaysKeepTask = true;
            suspect2.BlockPermanentEvents = true;
            PlayerData playerData = Utilities.GetPlayerData();
            string displayName = playerData.DisplayName;
            Notify("~r~[BeachCallouts] ~y~Officer ~b~" + displayName + ",~y~ the suspects have been reported to be");
            Notify("~y~causing issues with other people and falling down!");
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