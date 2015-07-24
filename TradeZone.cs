using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using Oxide.Core;
using System.Linq;
using Oxide.Plugins;

namespace Oxide.Plugins
{
    [Info("Trade Zone", "PaiN", 0.1, ResourceId = 714)]
    [Description("This plugin gives you the ability to make trading zones with two sides.")]
    public class TradeZone : RustPlugin
    {
        void Loaded()
		{
			LoadDefaultConfig();
		
		}
		
		
		protected override void LoadDefaultConfig()
        {
			
			if(Config["TradeZone", "Side1X"] == null) Config["TradeZone", "Side1X"] = 0;
			if(Convert.ToInt32(Config["TradeZone", "Side1X"]) != 0) return;
			
			if(Config["TradeZone", "Side1Y"] == null) Config["TradeZone", "Side1Y"] = 10;
			if(Convert.ToInt32(Config["TradeZone", "Side1Y"]) != 10) return;
			
			if(Config["TradeZone", "Side1Z"] == null) Config["TradeZone", "Side1Z"] = 0;
			if(Convert.ToInt32(Config["TradeZone", "Side1Z"]) != 0) return;
			////////////////////////////////////////////////////////////////////////////
			if(Config["TradeZone", "Side2X"] == null) Config["TradeZone", "Side2X"] = 3;
			if(Convert.ToInt32(Config["TradeZone", "Side2X"]) != 3) return;
			
			if(Config["TradeZone", "Side2Y"] == null) Config["TradeZone", "Side2Y"] = 10;
			if(Convert.ToInt32(Config["TradeZone", "Side2Y"]) != 10) return;
			
			if(Config["TradeZone", "Side2Z"] == null) Config["TradeZone", "Side2Z"] = 30;
			if(Convert.ToInt32(Config["TradeZone", "Side2Z"]) != 30) return;
			
			if(Config["TradeZone", "TPtimer"] == null) Config["TradeZone", "TPtimer"] = 2;
			if(Convert.ToInt32(Config["TradeZone", "TPtimer"]) != 2) return;

		
            SaveConfig();
		}
		
		[ChatCommand("tradezone")]
		void cmdTradeZone(BasePlayer player, string cmd, string[] args)
		{
			if(args.Length < 4 )
			{
				SendReply(player, "Syntax: /tradezone <add> <name> <side1/side2> <teleportTimer>");
				return;
			}
			if(args[0] == "add")
			{
				if(args[2] == "side1")
				{
					var pos = player.transform.position;
					Config[args[1], "Side1X"] = Convert.ToInt32(pos.x);
					Config[args[1], "Side1Y"] = Convert.ToInt32(pos.y);
					Config[args[1], "Side1Z"] = Convert.ToInt32(pos.z);
					Config[args[1], "TPtimer"] = Convert.ToInt32(args[3]);
					SaveConfig();
					SendReply(player, "Added new trade zone (SIDE1) " + args[1].ToString());
				}
				else if(args[2] == "side2")
				{
					var pos = player.transform.position;
					Config[args[1], "Side2X"] = Convert.ToInt32(pos.x);
					Config[args[1], "Side2Y"] = Convert.ToInt32(pos.y);
					Config[args[1], "Side2Z"] = Convert.ToInt32(pos.z);
					Config[args[1], "TPtimer"] = Convert.ToInt32(args[3]);
					SaveConfig();
					SendReply(player, "Added new trade zone (SIDE2) " + args[1].ToString());
				}	
				return;
			}
		}
			[ChatCommand("tradezonelist")]
			void cmdTradeList(BasePlayer player, string cmd, string[] args)
			{
				if(args.Length > 0)
				{
					SendReply(player, "Syntax: /tradezonelist");
					return;
				}
					foreach(var trade in Config)
					{
						string tradezoneName = trade.Key.ToString();
						if(tradezoneName == "TradeZone") continue;
						{
							
							var Side1 = new Vector3(Convert.ToInt32(Config[tradezoneName, "Side1X"]), Convert.ToInt32(Config[tradezoneName, "Side1Y"]), Convert.ToInt32(Config[tradezoneName, "Side1Z"]));
							var Side2 = new Vector3(Convert.ToInt32(Config[tradezoneName, "Side2X"]), Convert.ToInt32(Config[tradezoneName, "Side2Y"]), Convert.ToInt32(Config[tradezoneName, "Side2Z"]));

							SendReply(player, "Current Trade-Zones: " + tradezoneName + "\n" +
							"Side1: " + Side1 + "\n" +
							"Side2: " + Side2);
				
				
						}
					}
					return;
			}
		
		
		[ChatCommand("tradeto")]
		void cmdTradeTo(BasePlayer player, string cmd, string[] args)
		{
			if(args.Length == 1)
			{
					
				foreach(var trade in Config)
				{
					string tradezoneName = trade.Key.ToString();
					if(tradezoneName == "TradeZone") continue;
					{
						var TPtoSide1 = new Vector3(Convert.ToInt32(Config[args[0], "Side1X"]), Convert.ToInt32(Config[args[0], "Side1Y"]), Convert.ToInt32(Config[args[0], "Side1Z"]));
						var TPtoSide2 = new Vector3(Convert.ToInt32(Config[args[0], "Side2X"]), Convert.ToInt32(Config[args[0], "Side2Y"]), Convert.ToInt32(Config[args[0], "Side2Z"]));							 if(Config[args[0]] == null)
						{
							SendReply(player, "This Trade-Zone does not exists. Please check the current Trade-Zones: /tradezonelist");
							return;				
						}
						if(player.IsWounded() || player.IsSleeping() || player.IsSpectating()) return;
						SendReply(player, "Teleporting in " + Config[args[0], "TPtimer"].ToString() + " seconds!");
						timer.Once(Convert.ToInt32(Config[args[0], "TPtimer"]), () => {
							if(player.IsWounded() || player.IsSleeping() || player.IsSpectating()) return;
							player.ClientRPCPlayer(null, player, "ForcePositionTo", TPtoSide2);
							});
						foreach(BasePlayer current in BasePlayer.activePlayerList)
						{
							if(Vector3.Distance(current.transform.position, TPtoSide1) < 2)
							{	
								
								timer.Once(Convert.ToInt32(Config[args[0], "TPtimer"]), () => {
								if(player.IsWounded() || player.IsSleeping() || player.IsSpectating()) return;
								player.ClientRPCPlayer(null, player, "ForcePositionTo", TPtoSide2);
								});
							
							}
						}
					}
				}					
			}
			else
			{
				SendReply(player, "Syntax: /tradeto <tradezonename> || /tradezone list");
				return;		
			}
		}
	}		
}

