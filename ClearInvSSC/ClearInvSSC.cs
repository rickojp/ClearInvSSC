using System;
using TShockAPI;
using Terraria;
using TerrariaApi.Server;
using Terraria.Localization;

namespace ClearInvSSC
{
    [ApiVersion(2, 1)]

    public class ClearInvSSC : TerrariaPlugin
    {
        public override Version Version
        {
            get { return new Version("2.0"); }
        }
        public override string Name
        {
            get { return "ClearInvSSC"; }
        }
        public override string Author
        {
            get { return "IcyPhoenix"; }
        }
        public override string Description
        {
            get { return "Clear Inventory/buffs if SSC Activated"; }
        }
        public ClearInvSSC(Main game)
            : base(game)
        {
            Order = 4;
        }
        public override void Initialize()
        {
            ServerApi.Hooks.NetGetData.Register(this, OnGetData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ServerApi.Hooks.NetGetData.Deregister(this, OnGetData);
            base.Dispose(disposing);
        }

        private void OnGetData(GetDataEventArgs args)
        {
            if (args.MsgID == PacketTypes.TileGetSection)
            {
                if (Netplay.Clients[args.Msg.whoAmI].State == 2)
                {
                    CleanInventory(args.Msg.whoAmI);
                }
            }
        }
        private void CleanInventory(int Who)
        {

            if (TShock.ServerSideCharacterConfig.Enabled && !TShock.Players[Who].IsLoggedIn)
            {
                var player = TShock.Players[Who];
                player.TPlayer.SpawnX = -1;
                player.TPlayer.SpawnY = -1;
                player.sX = -1;
                player.sY = -1;

                for (int i = 0; i < NetItem.MaxInventory; i++)
                {
                    if (i < NetItem.InventorySlots)
                    {
                        //0-58
                        player.TPlayer.inventory[i].netDefaults(0);
                    }
                    else if (i < NetItem.InventorySlots + NetItem.ArmorSlots)
                    {
                        //59-78
                        var index = i - NetItem.InventorySlots;
                        player.TPlayer.armor[index].netDefaults(0);
                    }
                    else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots)
                    {
                        //79-88
                        var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots);
                        player.TPlayer.dye[index].netDefaults(0);
                    }
                    else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots)
                    {
                        //89-93
                        var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots);
                        player.TPlayer.miscEquips[index].netDefaults(0);
                    }
                    else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots)
                    {
                        //93-98
                        var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots);
                        player.TPlayer.miscDyes[index].netDefaults(0);
                    }
                    else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots + NetItem.PiggySlots)
                    {
                        //98-138
                        var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots);
                        player.TPlayer.bank.item[index].netDefaults(0);
                    }
                    else if (i < NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots + NetItem.PiggySlots + NetItem.SafeSlots)
                    {
                        //138-178
                        var index = i - (NetItem.InventorySlots + NetItem.ArmorSlots + NetItem.DyeSlots + NetItem.MiscEquipSlots + NetItem.MiscDyeSlots + NetItem.PiggySlots);
                        player.TPlayer.bank2.item[index].netDefaults(0);
                    }
                    else
                    {
                        //179
                        player.TPlayer.trashItem.netDefaults(0);
                    }

                }

                for (int k = 0; k < NetItem.MaxInventory; k++)
                {
                    NetMessage.SendData(5, -1, -1, NetworkText.Empty, player.Index, (float)k, 0f, 0f, 0);
                }

                for (int k = 0; k < Player.maxBuffs; k++)
                {
                    player.TPlayer.buffType[k] = 0;
                }

                NetMessage.SendData(4, -1, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(42, -1, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(16, -1, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(50, -1, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);

                for (int k = 0; k < NetItem.MaxInventory; k++)
                {
                    NetMessage.SendData(5, player.Index, -1, NetworkText.Empty, player.Index, (float)k, 0f, 0f, 0);
                }

                for (int k = 0; k < Player.maxBuffs; k++)
                {
                    player.TPlayer.buffType[k] = 0;
                }

                NetMessage.SendData(4, player.Index, -1, NetworkText.FromLiteral(player.Name), player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(42, player.Index, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(16, player.Index, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);
                NetMessage.SendData(50, player.Index, -1, NetworkText.Empty, player.Index, 0f, 0f, 0f, 0);

            }
        }
    }
}