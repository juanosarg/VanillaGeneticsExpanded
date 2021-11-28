using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace GeneticRim
{
	public static class SuperItemSpawner
	{
		private static Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		[DebugAction("Autotests", "Make all possible items", false, false, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MakeAllItems()
		{
			SuperItemSpawner.MakeColonyItems(new ColonyMakerFlag[1]);
		}

		
		public static void MakeColonyItems(params ColonyMakerFlag[] flags)
		{
			bool godMode = DebugSettings.godMode;
			DebugSettings.godMode = true;
			Thing.allowDestroyNonDestroyable = true;
			if (SuperItemSpawner.usedCells == null)
			{
				SuperItemSpawner.usedCells = new BoolGrid(SuperItemSpawner.Map);
			}
			else
			{
				SuperItemSpawner.usedCells.ClearAndResizeTo(SuperItemSpawner.Map);
			}
			SuperItemSpawner.overRect = new CellRect(SuperItemSpawner.Map.Center.x - (Map.Size.x/2)+10, SuperItemSpawner.Map.Center.z - (Map.Size.z / 2)+10, Map.Size.x-20, Map.Size.z-20);
			SuperItemSpawner.DeleteAllSpawnedPawns();
			GenDebug.ClearArea(SuperItemSpawner.overRect, Find.CurrentMap);
			
			
				List<ThingDef> itemDefs = (from def in DefDatabase<ThingDef>.AllDefs
										   where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.category == ThingCategory.Item
										   select def).ToList<ThingDef>();
				SuperItemSpawner.FillWithItems(overRect, itemDefs);
			
			
			
			SuperItemSpawner.ClearAllHomeArea();
			SuperItemSpawner.FillWithHomeArea(SuperItemSpawner.overRect);
			DebugSettings.godMode = godMode;
			Thing.allowDestroyNonDestroyable = false;
		}

		private static void FillWithItems(CellRect rect, List<ThingDef> itemDefs)
		{
			int num = 0;
			int itemCounter = itemDefs.Count;
			foreach (IntVec3 intVec in rect)
			{
				
				if (intVec.x % 6 != 0 && intVec.z % 6 != 0)
				{
					DebugThingPlaceHelper.DebugSpawn(itemDefs[num], intVec, -1, true);
					itemCounter--;
					num++;
					if (num >= itemDefs.Count)
					{
						num = 0;
					}
					if (itemCounter < 0)
                    {
						break;
                    }
				}
			}
		}

		

		private static bool TryGetFreeRect(int width, int height, out CellRect result)
		{
			for (int i = SuperItemSpawner.overRect.minZ; i <= SuperItemSpawner.overRect.maxZ - height; i++)
			{
				for (int j = SuperItemSpawner.overRect.minX; j <= SuperItemSpawner.overRect.maxX - width; j++)
				{
					CellRect cellRect = new CellRect(j, i, width, height);
					bool flag = true;
					for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
					{
						for (int l = cellRect.minX; l <= cellRect.maxX; l++)
						{
							if (SuperItemSpawner.usedCells[l, k])
							{
								flag = false;
								break;
							}
						}
						if (!flag)
						{
							break;
						}
					}
					if (flag)
					{
						result = cellRect;
						for (int m = cellRect.minZ; m <= cellRect.maxZ; m++)
						{
							for (int n = cellRect.minX; n <= cellRect.maxX; n++)
							{
								IntVec3 c = new IntVec3(n, 0, m);
								SuperItemSpawner.usedCells.Set(c, true);
								if (c.GetTerrain(Find.CurrentMap).passability == Traversability.Impassable)
								{
									SuperItemSpawner.Map.terrainGrid.SetTerrain(c, TerrainDefOf.Concrete);
								}
							}
						}
						return true;
					}
				}
			}
			result = new CellRect(0, 0, width, height);
			return false;
		}

		

		private static void DeleteAllSpawnedPawns()
		{
			foreach (Pawn pawn in SuperItemSpawner.Map.mapPawns.AllPawnsSpawned.ToList<Pawn>())
			{
				pawn.Destroy(DestroyMode.Vanish);
				pawn.relations.ClearAllRelations();
			}
			Find.GameEnder.gameEnding = false;
		}

		private static void ClearAllHomeArea()
		{
			foreach (IntVec3 c in SuperItemSpawner.Map.AllCells)
			{
				SuperItemSpawner.Map.areaManager.Home[c] = false;
			}
		}

		private static void FillWithHomeArea(CellRect r)
		{
			new Designator_AreaHomeExpand().DesignateMultiCell(r.Cells);
		}

		private static CellRect overRect;

		private static BoolGrid usedCells;

	}
}

