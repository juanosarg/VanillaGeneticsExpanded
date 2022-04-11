using System;
using UnityEngine;
using Verse;

namespace GeneticRim
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {
       
       

        public static readonly Graphic graphicTopLarge = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/Electrowombs/ElectrowombLarge_Top", ShaderDatabase.CutoutComplex, Vector2.one, Color.white);
        public static readonly Graphic graphicTopSmall = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/Electrowombs/Electrowomb_Top", ShaderDatabase.CutoutComplex, Vector2.one, Color.white);
        public static readonly Graphic graphicTopArcho = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/Endgame/Archowomb_Top", ShaderDatabase.CutoutComplex, Vector2.one, Color.white);
        public static readonly Graphic workingMechahybridizer = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/Mech/Mechahybridizer_Closed", ShaderDatabase.CutoutComplex, Vector2.one*5, Color.white);
        public static readonly Graphic spentfuse = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Building/Mech/MechafuseBlown", ShaderDatabase.CutoutComplex, Vector2.one, Color.white);

        public static readonly Graphic spark = (Graphic_Single)GraphicDatabase.Get<Graphic_Single>("Things/Mote/GR_YellowSparkFlash", ShaderDatabase.MoteGlow, Vector2.one*4, Color.white);

        public static readonly Material barFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));
        public static readonly Material barUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

    }
}
