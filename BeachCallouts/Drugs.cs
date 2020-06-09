using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;

namespace BeachCallouts
{
    
    [CalloutProperties("Drug Deal", "BGHDDevelopment", "0.0.3", Probability.Medium)]
    public class Drugs : Callout
    {
        private Ped suspect, suspect2;
        List<object> items = new List<object>();
        List<object> items2 = new List<object>();
        
        public Drugs()
        {
            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if(x <= 40)
            { 
                InitBase(new Vector3(-1591.88f, -927.26f, 8.98211f));
            }
            else if(x > 40 && x <= 65)
            {
                InitBase(new Vector3(-1224.92f, -1803.93f, 2.36156f));
            }
            else
            {
                InitBase(new Vector3(-1579.6f, -1021.78f, 7.64913f));
            }
            ShortName = "Drug Deal";
            CalloutDescription = "A person has been seen selling drugs.";
            ResponseCode = 2;
            StartDistance = 200f;
        }
        
        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            suspect.AttachBlip();
            suspect2.AttachBlip();
            dynamic data1 = await GetPedData(suspect.NetworkId);
            string firstname = data1.Firstname;
            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if(x <= 40)
            { 
                DrawSubtitle("~r~[" + firstname + "] ~s~SHOOT COPS ARE HERE!", 5000);
                suspect.Task.ReactAndFlee(player);
                suspect2.Task.ShootAt(player);
            }
            else if(x > 40 && x <= 65)
            {
                suspect.Task.ReactAndFlee(player);
                suspect2.Task.ReactAndFlee(player); 
                DrawSubtitle("~r~[" + firstname + "] ~s~BAIL! RUN!", 5000);
            }
            else
            {
                DrawSubtitle("~r~[" + firstname + "] ~s~Shoot, I give up!", 5000);
                suspect.Task.HandsUp(100000); 
                suspect2.Task.ReactAndFlee(player);
            }
        }

        public async override Task Init()
        {
            OnAccept();
            suspect = await SpawnPed(GetRandomPed(), Location);
            suspect2 = await SpawnPed(GetRandomPed(), Location);
            //Suspect Data
            dynamic data = new ExpandoObject();
            data.alcoholLevel = 0.25;
            object DrugBags = new {
                Name = "Dug Bags",
                IsIllegal = true
            };
            items.Add(DrugBags);
            data.items = items;
            SetPedData(suspect.NetworkId,data);

            //Suspect2 Data
            dynamic data2 = new ExpandoObject();
            data.alcoholLevel = 0.18;
            object Drugs = new {
                Name = "Drugs",
                IsIllegal = true
            };
            items.Add(Drugs);
            data.items2 = items2;
            SetPedData(suspect2.NetworkId,data2);
            
            //Tasks
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            suspect2.AlwaysKeepTask = true;
            suspect2.BlockPermanentEvents = true;
            dynamic playerData = GetPlayerData();
            string displayName = playerData.DisplayName;
            Notify("~r~[BeachCallouts] ~y~Officer ~b~" + displayName + ",~y~ the suspects have been reported to have");
            Notify("~y~been exchanging baggies!");
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