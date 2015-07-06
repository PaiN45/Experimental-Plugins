using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.Data;
using UnityEngine;
using Oxide.Core;

namespace Oxide.Plugins
{
    [Info("Mind Freeze", "PaiN", "2.0.0", ResourceId = 1198)]
    [Description("Allows you to freeze players with a legit way.")]
    class MindFreeze : RustPlugin
    {
        private Timer _timer;

        private class FrozenPlayerInfo
        {
            public BasePlayer Player { get; set; }
            public Vector3 FrozenPosition { get; set; }

            public FrozenPlayerInfo(BasePlayer player)
            {
                Player = player;
                FrozenPosition = player.transform.position;
            }
        }

        List<FrozenPlayerInfo> frozenPlayers = new List<FrozenPlayerInfo>();

        void Loaded()
        {
            if (!permission.PermissionExists("canmindfreeze")) permission.RegisterPermission("canmindfreeze", this);
            //LoadDefaultConfig(); Maybe gonna add this later.
            _timer= timer.Every(1, OnTimer);
        }

        [ChatCommand("freeze")]
        void cmdFreeze(BasePlayer player, string cmd, string[] args)
        {
			var target = BasePlayer.Find(args[0]);
			string steamId = Convert.ToString(player.userID);
			if (args.Length == 1)
			{
				if (!permission.UserHasPermission(steamId, "canmindfreeze"))
				{
					SendReply(player, "No Permission!");
					return;
				}

				
				if (!target)
				{
					SendReply(player, "Player not found!");
					return;
				}				
				if (target == null) return; // player in arg not found in player list
				if (frozenPlayers.Any(t => t.Player == target)) return; // already in frozen list
				frozenPlayers.Add(new FrozenPlayerInfo(target));
				SendReply(target, "You have been frozen by " + player.displayName);
				SendReply(player, "You have frozen " + target.displayName);
			}
			else
				{
					SendReply(player, "Syntax: /freeze \"player\" ");
					return;
				}
		}
		
		

        [ChatCommand("unfreeze")]
        void cmdUnFreeze(BasePlayer player, string cmd, string[] args)
        {
			var target = BasePlayer.Find(args[0]);
			string steamId = Convert.ToString(player.userID);
			if (args.Length == 1)
			{
				if (!permission.UserHasPermission(steamId, "canmindfreeze"))
				{
				SendReply(player, "No Permission!");
				return;
				}
				
				
				if (!target)
				{
				SendReply(player, "Player not found!");
				return;
				}
				
				
				if (target == null) return; // player in arg not found in player list
				frozenPlayers.RemoveAll(t => t.Player == target);
				SendReply(target, "You have been unfrozen by " + player.displayName);
				SendReply(player, "You have unfrozen " + target.displayName);
			}
				else
				{
					SendReply(player, "Syntax: /unfreeze \"player\" ");
					return;
				}	
		}

        [ChatCommand("unfreezeall")]
        void cmdUnFreezeAll(BasePlayer player, string cmd, string[] args)
        {
			string steamId = Convert.ToString(player.userID);
			if (permission.UserHasPermission(steamId, "canmindfreeze"))
			{
            frozenPlayers.Clear();
			}
			else
			{
				SendReply(player, "No Permission!");
			}
        }

        void OnTimer()
        {
            foreach (FrozenPlayerInfo current in frozenPlayers)
            {
                if (Vector3.Distance(current.Player.transform.position, current.FrozenPosition) < 1) continue; // if distance < 1, player is still about where we froze him, continue skips to next player
                current.Player.ClientRPCPlayer(null, current.Player, "ForcePositionTo", new object[] { current.FrozenPosition });
                current.Player.TransformChanged();
            }
        }

        void Unloaded()
        {
            _timer.Destroy();
            frozenPlayers.Clear();
        }
    }
}