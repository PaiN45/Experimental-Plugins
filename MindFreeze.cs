using System.Collections.Generic;
using System.Reflection;
using System;
using System.Data;
using UnityEngine;
using Oxide.Core;

namespace Oxide.Plugins
{
    [Info("Mind Freeze", "PaiN", "1.2.0", ResourceId = 1198)]
    [Description("Allows you to freeze players with a legit way.")]
    class MindFreeze : RustPlugin
    {
		
        void Loaded()
        {
        if (!permission.PermissionExists("canmindfreeze")) permission.RegisterPermission("canmindfreeze", this);
            //LoadDefaultConfig(); Maybe gonna add this later.
        }
           
       List<BasePlayer> frozenPlayers = new List<BasePlayer>();  
		   
        private Timer timerrepeat;
       
        [ChatCommand("freeze")]
		void cmdFreeze(BasePlayer player, string cmd, string[] args)
        {
			var target = BasePlayer.Find(args[0]);
			frozenPlayers.Add(target);
			if (frozenPlayers.Contains(target)) 
			{
                var position = target.transform.position;
                var configPos = new Vector3(position.x, position.y , position.z);

                if(Vector3.Distance(target.transform.position, configPos) < 1)
                {
                var timerrepeat = timer.Repeat(1, 0, () => target.ClientRPCPlayer(null, target, "ForcePositionTo", new object[] { configPos }));
                target.TransformChanged();

                }
			}
		}
  
		
        [ChatCommand("unfreeze")]
        void cmdUnFreeze(BasePlayer player, string command, string[] args)
        {
			var target = BasePlayer.Find(args[0]);
			if (frozenPlayers.Contains(target)) 
			{
				frozenPlayers.Remove(target);
			}
					
        }
		
		[ChatCommand("unfreezeall")]
		void cmdUnFreezeAll(BasePlayer player, string command, string[] args)
		{
			frozenPlayers.Clear();		
		}
       
       
        public void Unloaded()
        {
            timerrepeat.Destroy(); 
			frozenPlayers.Clear();
        }
    }

}