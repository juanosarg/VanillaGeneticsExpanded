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


    }
}
