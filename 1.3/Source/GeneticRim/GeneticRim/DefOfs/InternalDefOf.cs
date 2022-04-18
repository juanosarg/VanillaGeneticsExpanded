using System;
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

        public static ThingDef GR_UrsineGenetic;
        public static ThingDef GR_AvianGenetic;
        public static ThingDef GR_BoomalopeGenetic;
        public static ThingDef GR_MuffaloGenetic;
        public static ThingDef GR_CanineGenetic;
        public static ThingDef GR_RodentGenetic;
        public static ThingDef GR_FelineGenetic;
        public static ThingDef GR_EquineGenetic;
        public static ThingDef GR_InsectoidGenetic;
        public static ThingDef GR_ReptileGenetic;
        public static ThingDef GR_HumanoidGenetic;
        public static ThingDef GR_ThrumboGenetic;
        public static ThingDef GR_TemplateGenetic;
        public static ThingDef GR_GrowthCell;
        public static ThingDef GR_DNAStorageBank;
        public static ThingDef GR_ArchocentipedeFormer;
        public static ThingDef GR_ArchoGrowthcell;
        public static ThingDef GR_ArchoWomb;
        public static ThingDef GR_Wolfchicken;
        public static ThingDef GR_Catffalo;
        public static ThingDef GR_Boomsnake;
        public static ThingDef GR_ParagonIguana;
        public static ThingDef GR_EggBomb;
        public static ThingDef GR_GenePod;
        public static ThingDef GR_Gas_Dust;
        public static ThingDef GR_Fleshling;
        public static ThingDef GR_FleshMonstrosity;
        public static ThingDef GR_ParagonInsectoid;
        public static ThingDef GR_TissueGrowingVat;
        public static ThingDef GR_SlowFlyer;
        public static ThingDef GR_StraightFlyer;
        public static ThingDef GR_BiomechanicalLabBeacon;
        public static ThingDef GR_Turkeyman;
        public static ThingDef GR_Muffaloman;
        public static ThingDef GR_Dogman;
        public static ThingDef GR_Moleman;
        public static ThingDef GR_Catman;
        public static ThingDef GR_Manscarab;
        public static ThingDef GR_Lizardman;
        public static ThingDef GR_Thrumboman;
        public static ThingDef GR_Manbear;

        public static PawnKindDef GR_ArchotechCentipede;

        public static HediffDef GR_AnimalControlHediff;
        public static HediffDef GR_ExtractedBrain;
        public static HediffDef GR_Frenzied;
        public static HediffDef GR_RecentlyHatched;
        public static HediffDef GR_MuscleNecrosis;
        public static HediffDef GR_AnimalTuberculosis;
        public static HediffDef GR_AnimalAbasia;
        public static HediffDef GR_SargSyndrome;
        public static HediffDef GR_GreaterScaria;
        public static HediffDef GR_SadisticAdrenaline;

        public static ThingCategoryDef GR_GeneticMaterial;
        public static ThingCategoryDef GR_GeneticMaterialTierOne;
        public static ThingCategoryDef GR_GeneticMaterialTierTwoOrThree;
        public static ThingCategoryDef GR_Genoframes;
        public static ThingCategoryDef GR_Boosters;
        public static ThingCategoryDef GR_GenomeExcavators;
        public static ThingCategoryDef GR_ImplantCategory;

        public static JobDef GR_InsertIngredients;
        public static JobDef GR_InsertGrowthCell;
        public static JobDef GR_InsertArchotechGrowthCell;
        public static JobDef GR_ArchotechDNAExtractionJob;
        public static JobDef GR_ParagonConversionJob;
        public static JobDef GR_HumanoidHybridTalk;
        public static JobDef GR_AnimalDeconstructJob;
        public static JobDef GR_AnimalHuntJob;
        public static JobDef GR_AnimalHarvestJob;
        public static JobDef GR_HumanoidHybridRecruit;

        public static SoundDef GR_Pop;
        public static SoundDef GR_Beep;

        public static DamageDef GR_Acid;

        public static FactionDef GR_RoamingMonstrosities;

        public static IncidentDef GR_ManhunterMonstrosities;

        public static InteractionDef GR_TalkingToHumans;

        public static QuestScriptDef GR_OpportunitySite_AbandonedLab;
        public static QuestScriptDef GR_OpportunitySite_BiomechanicalLab;

        [MayRequireIdeology]
        public static PreceptDef GR_WorktableSpeeds_Implants;
        [MayRequireIdeology]
        public static PreceptDef GR_WorktableSpeeds_Genomorpher;

        public static AbilityDef GR_DeathRay;



    }
}
