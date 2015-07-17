using UnityEngine;
using Rust;
using Oxide.Core.Plugins;

namespace Oxide.Plugins
{

    [Info("Hitmarker GUI", "PaiN", 0.1, ResourceId = 0)]
    [Description("This plugin informs the attacker/player if he hit someone..")]
	class HitmarkerGUI : RustPlugin
    {     
	
        string json = @"[
                       {
                            ""name"": ""HitMarker"",
                            ""parent"": ""Overlay"",
                            ""components"":
                            [
                                {
                                     ""type"":""UnityEngine.UI.Image"",
                                     ""color"":""0.0 0.0 0.0 0.0"",
                                },
                                {
                                    ""type"":""RectTransform"",
                                    ""anchormin"": ""0.50 0.49"",
                                    ""anchormax"": ""0.60 0.51""
                                }
                            ]
                        },
                        {
                            ""parent"": ""HitMarker"",
                            ""components"":
                            [
                                {
                                    ""type"":""UnityEngine.UI.Text"",
                                    ""text"":""{hitmarker}"",
                                    ""fontSize"":20,
									""color"":""1 0.0 0.0 2"", 
                                    ""align"": ""MiddleCenter"",
									""anchormin"": ""0.50 0.50"",
                                    ""anchormax"": ""0.50 0.50""
                                }
                            ]
                        },
                    ]
                    ";


		void OnPlayerAttack(BasePlayer attacker, HitInfo hitinfo)
		{
			var gettingdmg = hitinfo.HitEntity;
			if(gettingdmg && gettingdmg.ToPlayer())
			{
				CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = attacker.net.connection }, null, "AddUI", json.Replace("{hitmarker}", "HIT"));
				timer.In(1, () => CommunityEntity.ServerInstance.ClientRPCEx(new Network.SendInfo() { connection = attacker.net.connection }, null, "DestroyUI", "HitMarker"));
			
			}
		}
	}
}