using System.Collections.Generic;
using System.Reflection;
using System;
using System.Data;
using UnityEngine;
using Oxide.Core;

namespace Oxide.Plugins
{
    [Info("Mind Freeze", "PaiN", "1.0.0", ResourceId = 0)]
    [Description("Allows you to freeze players with a legit way.")]
    public class MindFreeze : RustPlugin
    {
		public void Loaded()
		{
		if (!permission.PermissionExists("canmindfreeze")) permission.RegisterPermission("canmindfreeze", this);	
			//LoadDefaultConfig(); Maybe gonna add this later.
		}
			
			
		private Timer timerrepeat;
		
		[ChatCommand("freeze")]
		void cmdFreeze(BasePlayer player, string command, string[] args)
		{
			string steamId = player.userID.ToString();
            if (permission.UserHasPermission(steamId, "canmindfreeze"))
			{
				var target = BasePlayer.Find(args[0]);
				var position = target.transform.position;
				var configPos = new Vector3(position.x, position.y , position.z);

				if(Vector3.Distance(target.transform.position, configPos) < 1)
				{
				var timerrepeat = timer.Repeat(1, 0, () => target.ClientRPCPlayer(null, target, "ForcePositionTo", new object[] { configPos }));
				player.TransformChanged();

				}
			}
			else
				SendReply(player, "You do not have permission to use this command!");
		
		}
		[ChatCommand("unfreeze")]//I will find another way to unfreeze but for now i cant think another way to do it.
		void cmdUnFreeze(BasePlayer player, string command, string[] args)
		{
		timerrepeat.Destroy();	
		}
		
		
		public void Unloaded()
		{
			timerrepeat.Destroy();			
		}
	}

}
