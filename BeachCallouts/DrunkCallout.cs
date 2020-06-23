using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;

namespace BeachCallouts
{
    [CalloutProperties("Drunk Person", "BGHDDevelopment", "0.0.3")]
    public class DrunkCallout : Callout
    {
        private Ped suspect, suspect2;
        List<object> items = new List<object>();
        List<object> items2 = new List<object>();
        
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
            UpdateData();
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
            dynamic data1 = await Utilities.GetPedData(suspect.NetworkId);
            string firstname = data1.Firstname;
            DrawSubtitle("~r~[" + firstname + "] ~s~Can I have a beer?", 5000);
            dynamic data2 = await Utilities.GetPedData(suspect2.NetworkId);
            string firstname2 = data2.Firstname;
            DrawSubtitle("~r~[" + firstname + "] ~s~SURE!", 5000);
            suspect.Task.FleeFrom(player);
        }

        public async override Task OnAccept()
        {
            InitBlip();
            suspect = await SpawnPed(GetRandomPed(), Location);
            suspect2 = await SpawnPed(GetRandomPed(), Location);
            //Suspect Data
            dynamic data = new ExpandoObject();
            data.alcoholLevel = 0.25;
            object SixPack = new {
                Name = "Six Pack",
                IsIllegal = false
            };
            items.Add(SixPack);
            data.items = items;
            Utilities.SetPedData(suspect.NetworkId,data);

            //Suspect2 Data
            dynamic data2 = new ExpandoObject();
            data.alcoholLevel = 0.18;
            object Beer = new {
                Name = "Beer",
                IsIllegal = false
            };
            items.Add(Beer);
            data.items2 = items2;
            Utilities.SetPedData(suspect2.NetworkId,data2);
            
            //Tasks
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            suspect2.AlwaysKeepTask = true;
            suspect2.BlockPermanentEvents = true;
            dynamic playerData = Utilities.GetPlayerData();
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