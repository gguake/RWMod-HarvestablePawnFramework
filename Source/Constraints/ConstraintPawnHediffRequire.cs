using Verse;

namespace HPF
{
    public class ConstraintPawnHediffRequire : ConstraintPawnHediff
	{
		public override bool CheckActiveCondition(ThingComp comp, Pawn pawn, ThingWithComps equipment)
		{
			return pawn != null && pawn.health != null && pawn.health.hediffSet.HasHediff(this.hediff, false);
		}
	}
}
