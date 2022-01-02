﻿using Verse;

namespace HPF
{
    public class ConstraintPawnRaceRestrict : ConstraintPawnRace
	{
		public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
		{
			return pawn != null && pawn.kindDef.race != null && pawn.kindDef.race.defName != this.race;
		}
	}
}
