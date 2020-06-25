using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;

namespace BeachCallouts
{
    
    [CalloutProperties("Active Shooter", "BGHDDevelopment", "0.0.3")]
    public class ActiveShooter : Callout
    {
        private Ped suspect, vic1, vic2, vic3, vic4, vic5;
        List<object> items = new List<object>();
        public ActiveShooter()
        {
            InitInfo(new Vector3(-1688.4f, -1059.91f, 13.0558f));
            ShortName = "Active Shooter";
            CalloutDescription = "Reports of an active shooter at the pier!";
            ResponseCode = 3;
            StartDistance = 300f;
        }
        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            suspect.AttachBlip();
            suspect.Weapons.Give(WeaponHash.MarksmanRifle, 1000, true, true);
            suspect.Task.ShootAt(player);
            vic1.Kill();
            vic2.Kill();
            vic3.Kill();
            vic4.Kill();
            vic5.Kill();
            vic1.AttachBlip();
            vic2.AttachBlip();
            vic3.AttachBlip();
            vic4.AttachBlip();
            vic5.AttachBlip();
            dynamic data1 = await Utilities.GetPedData(suspect.NetworkId);
            string firstname = data1.Firstname;
            DrawSubtitle("~r~[" + firstname + "] ~s~I knew this was coming... DIE!", 5000);
        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
            dynamic playerData = Utilities.GetPlayerData();
            string displayName = playerData.DisplayName;
            Notify("~r~[BeachCallouts] ~y~Officer ~b~" + displayName + ",~y~ reports show multiple victims down!");
            suspect = await SpawnPed(GetRandomPed(), Location);
            vic1 = await SpawnPed(GetRandomPed(), Location + 1);
            vic2 = await SpawnPed(GetRandomPed(), Location + 2);
            vic3 = await SpawnPed(GetRandomPed(), Location + 4);
            vic4 = await SpawnPed(GetRandomPed(), Location - 5);
            vic5 = await SpawnPed(GetRandomPed(), Location - 1);
            //Suspect 1
            dynamic data = new ExpandoObject();
            data.alcoholLevel = 0.08;
            object Rifle = new {
                Name = "Rifle",
                IsIllegal = true
            };
            items.Add(Rifle);
            data.items = items;
            Utilities.SetPedData(suspect.NetworkId,data);
            suspect.AlwaysKeepTask = true;
            suspect.BlockPermanentEvents = true;
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