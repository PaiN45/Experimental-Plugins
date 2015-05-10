using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Barricade Remover", "bawNg", 0.2)]
    class BarricadeRemover : RustPlugin
    {
        const float cupboardDistance = 60f;
        
        [ConsoleCommand("barricades.count")]
        void cmdCountbarricades(ConsoleSystem.Arg arg)
        {
            if (arg.Player() && !arg.Player().IsAdmin())
            {
                SendReply(arg, "You need to be admin to use that command");
                return;
            }
            var barricade.wood = FindAllCupboardlessbarricade.woodBlocks();
            SendReply(arg, $"There are {barricade.wood.Count} barricade blocks outside of cupboard range");
        }

        [ConsoleCommand("barricades.remove")]
        void cmdRemovebarricades(ConsoleSystem.Arg arg)
        {
            if (arg.Player() && !arg.Player().IsAdmin())
            {
                SendReply(arg, "You need to be admin to use that command");
                return;
            }
            PrintToChat("<color=red>Admin is removing all barricade blocks outside of cupboard range...</color>");
            var barricade.wood = FindAllCupboardlessbarricade.woodBlocks();
            var started_at = Time.realtimeSinceStartup;
            foreach (var building_block in barricade.wood)
                building_block.Kill();
            Puts($"[BarricadeRemover] Destroyed {barricade.wood.Count} barricade blocks in {Time.realtimeSinceStartup - started_at:0.000} seconds");
            PrintToChat($"<color=yellow>Admin has removed {barricade.wood.Count} barricade blocks from the map</color>");
        }

        HashSet<BuildingBlock> FindAllCupboardlessbarricade.woodBlocks()
        {
            var tool_cupboards = FindAllToolCupboards();
            var barricade.wood = FindAllbarricade.woodBuildingBlocks();                        
            var started_at = Time.realtimeSinceStartup;
            Puts($"[BarricadeRemover] Checking {barricade.wood.Count} barricade blocks against {tool_cupboards.Length} tool cupboards...");
            foreach (var cupboard in tool_cupboards)
            {
                foreach (var collider in Physics.OverlapSphere(cupboard.transform.position, cupboardDistance))
                {
                    var building_block = collider.GetComponentInParent<BuildingBlock>();
                    if (building_block) barricade.wood.Remove(building_block);
                }
            }
            Puts($"[BarricadeRemover] Finding {barricade.wood.Count} cupboardless barricade blocks took {Time.realtimeSinceStartup - started_at:0.000} seconds");
            return barricade.wood;
        }
        
        HashSet<BuildingBlock> FindAllbarricade.woodBuildingBlocks()
        {
            var started_at = Time.realtimeSinceStartup;
            Puts("[BarricadeRemover] Finding all barricade blocks...");
            var building_blocks = UnityEngine.Object.FindObjectsOfType<BuildingBlock>();
            var barricade.wood = new HashSet<BuildingBlock>(building_blocks.Where(block => block.grade == BuildingGrade.Enum.Wood));
            Puts($"[BarricadeRemover] Finding {barricade.wood.Count} barricade blocks took {Time.realtimeSinceStartup - started_at:0.000} seconds");
            return barricade.wood;
        }

        BaseCombatEntity[] FindAllToolCupboards()
        {
            var started_at = Time.realtimeSinceStartup;
            Puts("[BarricadeRemover] Finding all tool cupboards...");
            var combat_entities = UnityEngine.Object.FindObjectsOfType<BaseCombatEntity>();
            var tool_cupboards = combat_entities.Where(entity => entity.LookupPrefabName() == "items/cupboard.tool.deployed").ToArray();
            Puts($"[BarricadeRemover] Finding {tool_cupboards.Length} tool cupboards took {Time.realtimeSinceStartup - started_at:0.000} seconds");
            return tool_cupboards;
        }
    }
}