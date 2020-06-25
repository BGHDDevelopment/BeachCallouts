using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;

namespace BeachCallouts
{
    [CalloutProperties("Possible Fight", "BGHDDevelopment", "0.0.3")]
    public class Fight : Callout
    {
        Ped suspect, suspect2, suspect3, suspect4, suspect5, suspect6, suspect7, suspect8, suspect9, suspect10;
        List<object> items = new List<object>();
        
        public Fight()
        {
            Random random = new Random();
            int x = random.Next(1, 100 + 1);
            if(x <= 40)
            { 
                InitInfo(new Vector3(-1464.61f, -1471.88f, 2.15028f));
            }
            else if(x > 40 && x <= 65)
            {
                InitInfo(new Vector3(-1290.43f, -1759.8f, 2.14623f));
            }
            else
            {
                InitInfo(new Vector3(-1453.23f, -991.816f, 6.1983f));
            }
            ShortName = "Fight in Progress";
            CalloutDescription = "A fight is taking place.";
            ResponseCode = 3;
            StartDistance = 200f;
        }

        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            suspect.AttachBlip();
            suspect2.AttachBlip();
            suspect3.AttachBlip();
            suspect4.AttachBlip();
            suspect5.AttachBlip();
            suspect6.AttachBlip();
            suspect7.AttachBlip();
            suspect8.AttachBlip();
            suspect9.AttachBlip();
            suspect10.AttachBlip();
            suspect.Task.FightAgainst(suspect2);
            suspect2.Task.FightAgainst(suspect3);
            suspect3.Task.FightAgainst(suspect4);
            suspect4.Task.FightAgainst(suspect5);
            suspect5.Task.FightAgainst(suspect6);
            suspect6.Task.FightAgainst(suspect7);
            suspect7.Task.FightAgainst(suspect8);
            suspect8.Task.FightAgainst(suspect9);
            suspect9.Task.FightAgainst(suspect10);
            suspect10.Task.FightAgainst(suspect);
            dynamic data1 = await Utilities.GetPedData(suspect.NetworkId);
            string firstname = data1.Firstname;
            dynamic data2 = await Utilities.GetPedData(suspect2.NetworkId);
            string firstname2 = data2.Firstname;
            dynamic data3 = await Utilities.GetPedData(suspect3.NetworkId);
            string firstname3 = data3.Firstname;
            dynamic data4 = await Utilities.GetPedData(suspect4.NetworkId);
            string firstname4 = data4.Firstname;
            API.Wait(6000);
            DrawSubtitle("~r~[" + firstname + "] ~s~Why does my day at the beach always turn into this?", 5000);
            API.Wait(6000);
            DrawSubtitle("~r~[" + firstname2 + "] ~s~You are not my friend anymore!", 5000);
            API.Wait(6000);
            DrawSubtitle("~r~[" + firstname3 + "] ~s~YOUR MEAN!", 5000);
            API.Wait(6000);
            DrawSubtitle("~r~[" + firstname4 + "] ~s~DIE!", 5000);
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
            suspect = await SpawnPed(GetRandomPed(), Location + 1);
            suspect2 = await SpawnPed(GetRandomPed(), Location - 1);
            suspect3 = await SpawnPed(GetRandomPed(), Location + 2);
            suspect4 = await SpawnPed(GetRandomPed(), Location - 2);
            suspect5 = await SpawnPed(GetRandomPed(), Location + 1);
            suspect6 = await SpawnPed(GetRandomPed(), Location - 1);
            suspect7 = await SpawnPed(GetRandomPed(), Location + 3);
            suspect8 = await SpawnPed(GetRandomPed(), Location - 3);
            suspect9 = await SpawnPed(GetRandomPed(), Location + 1);
            suspect10 = await SpawnPed(GetRandomPed(), Location - 1);
            
            //Suspect 1
            dynamic data = new ExpandoObject();
            data.alcoholLevel = 0.08;
            data.drugsUsed = new bool[] {false,false,true};
            Utilities.SetPedData(suspect.NetworkId,data);
            
            //Suspect 2
            dynamic data2 = new ExpandoObject();
            data2.alcoholLevel = 0.05;
            data2.drugsUsed = new bool[] {true,false,false};
            Utilities.SetPedData(suspect2.NetworkId,data2);
            
            //Suspect 3
            dynamic data3 = new ExpandoObject();
            data3.alcoholLevel = 0.02;
            data3.drugsUsed = new bool[] {false,false,false};
            Utilities.SetPedData(suspect3.NetworkId,data3);
            
            //Suspect 4
            dynamic data4 = new ExpandoObject();
            data4.alcoholLevel = 0.00;
            data4.drugsUsed = new bool[] {false,false,false};
            object Cash = new {
                Name = "$500 Cash",
                IsIllegal = false
            };
            items.Add(Cash);
            Utilities.SetPedData(suspect4.NetworkId,data4);
            
            //Suspect 5
            dynamic data5 = new ExpandoObject();
            data5.alcoholLevel = 0.00;
            data5.drugsUsed = new bool[] {true,true,true};
            Utilities.SetPedData(suspect5.NetworkId,data5);
            
            //Suspect 6
            dynamic data6 = new ExpandoObject();
            data6.alcoholLevel = 0.20;
            data6.drugsUsed = new bool[] {false,false,false};
            Utilities.SetPedData(suspect6.NetworkId,data6);
            
            //Suspect 7
            dynamic data7 = new ExpandoObject();
            data7.alcoholLevel = 0.01;
            data7.drugsUsed = new bool[] {false,false,false};
            Utilities.SetPedData(suspect7.NetworkId,data7);
            
            //Suspect 8
            dynamic data8 = new ExpandoObject();
            data8.alcoholLevel = 0.08;
            data8.drugsUsed = new bool[] {false,false,false};
            Utilities.SetPedData(suspect8.NetworkId,data8);
            
            //Suspect 9
            dynamic data9 = new ExpandoObject();
            data9.alcoholLevel = 0.00;
            data9.drugsUsed = new bool[] {false,true,false};
            Utilities.SetPedData(suspect9.NetworkId,data9);
            
            //Suspect 10
            dynamic data10 = new ExpandoObject();
            data10.alcoholLevel = 0.00;
            data10.drugsUsed = new bool[] {false,false,true};
            object Cash2 = new {
                Name = "$500 Cash",
                IsIllegal = false
            };
            items.Add(Cash);
            Utilities.SetPedData(suspect10.NetworkId,data10);
            
            
            //TASKS
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
            suspect2.AlwaysKeepTask = true;
            suspect2.BlockPermanentEvents = true;
            suspect3.AlwaysKeepTask = true;
            suspect3.BlockPermanentEvents = true;
            suspect4.AlwaysKeepTask = true;
            suspect4.BlockPermanentEvents = true;
            suspect5.AlwaysKeepTask = true;
            suspect5.BlockPermanentEvents = true;
            suspect6.AlwaysKeepTask = true;
            suspect6.BlockPermanentEvents = true;
            suspect7.AlwaysKeepTask = true;
            suspect7.BlockPermanentEvents = true;
            suspect8.AlwaysKeepTask = true;
            suspect8.BlockPermanentEvents = true;
            suspect9.AlwaysKeepTask = true;
            suspect9.BlockPermanentEvents = true;
            suspect10.AlwaysKeepTask = true;
            suspect10.BlockPermanentEvents = true;
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