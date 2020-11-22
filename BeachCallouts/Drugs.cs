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
    
    [CalloutProperties("Drug Deal", "BGHDDevelopment", "0.0.5")]
    public class Drugs : Callout
    {
        private Ped suspect, suspect2;

        public Drugs()
        {
            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if(x <= 40)
            { 
                InitInfo(new Vector3(-1591.88f, -927.26f, 8.98211f));
            }
            else if(x > 40 && x <= 65)
            {
                InitInfo(new Vector3(-1224.92f, -1803.93f, 2.36156f));
            }
            else
            {
                InitInfo(new Vector3(-1579.6f, -1021.78f, 7.64913f));
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
            PedData data1 = await Utilities.GetPedData(suspect.NetworkId);
            string firstname = data1.FirstName;
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

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
            suspect = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            suspect2 = await SpawnPed(RandomUtils.GetRandomPed(), Location);
            
            //Suspect Data
            PedData data = new PedData();
            List<Item> items = new List<Item>();
            data.BloodAlcoholLevel = 0.25;
            Item DrugBags = new Item {
                Name = "Dug Bags",
                IsIllegal = true
            };
            items.Add(DrugBags);
            data.Items = items;
            Utilities.SetPedData(suspect.NetworkId,data);

            //Suspect2 Data
            PedData data2 = new PedData();
            List<Item> items2 = new List<Item>();
            data.BloodAlcoholLevel = 0.18;
            Item Drugs = new Item {
                Name = "Dugs",
                IsIllegal = true
            };
            items.Add(Drugs);
            data.Items = items2;
            Utilities.SetPedData(suspect2.NetworkId,data2);
            
            //Tasks
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            suspect2.AlwaysKeepTask = true;
            suspect2.BlockPermanentEvents = true;
            PlayerData playerData = Utilities.GetPlayerData();
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