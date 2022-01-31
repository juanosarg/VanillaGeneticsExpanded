﻿using System;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace GeneticRim
{
	[DefOf]
	public static class InternalDefOf
	{
		static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}

		public static ThingDef         GR_UrsineGenetic;
		public static ThingDef         GR_AvianGenetic;
		public static ThingDef         GR_BoomalopeGenetic;
		public static ThingDef         GR_MuffaloGenetic;
		public static ThingDef         GR_CanineGenetic;
		public static ThingDef         GR_RodentGenetic;
		public static ThingDef         GR_FelineGenetic;
		public static ThingDef         GR_EquineGenetic;
		public static ThingDef         GR_InsectoidGenetic;
		public static ThingDef         GR_ReptileGenetic;
		public static ThingDef         GR_HumanoidGenetic;
		public static ThingDef         GR_ThrumboGenetic;
		public static ThingDef         GR_MechanoidGenetic;
		public static ThingDef         GR_NonSpecializedGenetic;
		public static ThingDef         GR_RuinedGenetic;
        public static ThingDef         GR_GrowthCell;
		public static ThingCategoryDef GR_GeneticMaterial;
		public static ThingCategoryDef GR_Genoframes;
		public static ThingCategoryDef GR_Boosters;
    }
}
