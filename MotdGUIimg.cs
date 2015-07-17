using UnityEngine;
using Rust;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{
    [Info("Motd GUI", "PaiN", 0.1, ResourceId = 0)]
    [Description("Simple Motd on the screen.")]
    public class MotdGUI : RustPlugin
    {      
	
		System.Collections.Generic.List<ulong> guioff = new System.Collections.Generic.List<ulong>();	
		
        void Loaded()
        {
			LoadDefaultConfig();
			foreach(BasePlayer player in BasePlayer.activePlayerList)
			{
			string text = Config["Motd", "Message"].ToString();
			string title = Config["Motd", "Title"].ToString();
			CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = player.net.connection }, null, "AddUI", json.Replace("{text}", text).Replace("{title}", title));
			}
        }
		
		protected override void LoadDefaultConfig()
		{
		
		if(Config["Motd", "Message"] == null) Config["Motd", "Message"] = "<color=yellow>Today</color> is a beautiful day!";
		if(Config["Motd", "Message"].ToString() != "<color=yellow>Today</color> is a beautiful day!") return;

		if(Config["Motd", "Title"] == null) Config["Motd", "Title"] = "<color=red>Motd</color>";
		if(Config["Motd", "Title"].ToString() != "<color=red>Motd</color>") return;
		
		
		SaveConfig();
		}
		 
        #region JSON
        string json = @"[
                       { 
                            ""name"": ""Motd"",
                            ""parent"": ""Overlay"",
                            ""components"":
                            [ 
                                {
									""type"":""UnityEngine.UI.RawImage"",
									""imagetype"": ""Tiled"",
									""color"": ""1.0 1.0 1.0 1.0"",
									""url"": ""http://www.wallspick.com/wp-content/uploads/2014/03/Black-background-set-wood-on-chanconsultants-jpg-728x455.jpg"",
								},
                                {
                                    ""type"":""RectTransform"",
                                    ""anchormin"": ""0.001 0.65"",
                                    ""anchormax"": ""0.25 0.85""
                                }
                            ]
                        },
                        {
                            ""parent"": ""Motd"",
                            ""components"":
                            [
                                {
                                    ""type"":""UnityEngine.UI.Text"",
                                    ""text"":""{title}"",
                                    ""fontSize"":20,
                                    ""align"": ""MiddleCenter"",
                                },
                                {
                                    ""type"":""RectTransform"",
                                    ""anchormin"": ""0 0.7"",
                                    ""anchormax"": ""1 1""
                                }
                            ]
                        },
                        {
                            ""parent"": ""Motd"",
                            ""components"":
                            [
                                {
                                    ""type"":""UnityEngine.UI.Text"",
                                    ""text"":""{text}"",
                                    ""fontSize"":15,
                                    ""align"": ""MiddleCenter"",
                                },
                                {
                                    ""type"":""RectTransform"",
                                    ""anchormin"": ""0 0.1"",
                                    ""anchormax"": ""1 1.2""
                                }
                            ]
                        }
                    ]
                    ";
        #endregion
      
        [ChatCommand("motd")]
		void cmdMotdShow(BasePlayer player, string cmd, string[] args)
		{
			if(guioff.Contains(player.userID)) 
			{				
			string text = Config["Motd", "Message"].ToString();
			string title = Config["Motd", "Title"].ToString();
			CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = player.net.connection }, null, "AddUI", json.Replace("{text}", text).Replace("{title}", title));
				guioff.Remove(player.userID);
			}
			else 
			{
				CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = player.net.connection }, null, "DestroyUI", "Motd");
				guioff.Add(player.userID);
			}	
		
		
		}
         
        [HookMethod("OnPlayerInit")]
        void OnPlayerInit(BasePlayer player)
        {
			string text = Config["Motd", "Message"].ToString();
			string title = Config["Motd", "Title"].ToString();
			CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = player.net.connection }, null, "AddUI", json.Replace("{text}", text).Replace("{title}", title));
        }
		
		void Unloaded(BasePlayer player)
		{
			foreach (BasePlayer current in BasePlayer.activePlayerList)
			{
				CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = current.net.connection }, null, "DestroyUI", "Motd");
			
			}
		
		}
		void OnPlayerDisconnected(BasePlayer player)
		{
			CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = player.net.connection }, null, "DestroyUI", "Motd");
		}
    }
}