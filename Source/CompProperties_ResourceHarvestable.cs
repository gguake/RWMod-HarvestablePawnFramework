using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HPF
{
	public class StatModifier
    {
		public StatDef statDef;

		public float offset;
		public float multiplier;
    }

    public class CompProperties_ResourceHarvestable : CompProperties
	{
		public CompProperties_ResourceHarvestable()
		{
			this.compClass = typeof(CompResourceHarvestable);
		}

		public JobDef harvestJobDef;

		public ThingDef thingDef;

		public string saveUniqueKey = "";

		public float intervalDays = 15f;

		public int amount = 1;

		public string inspectText;

		public List<Constraint> constraints = new List<Constraint>();

		public List<StatModifier> speedAffectedStats = new List<StatModifier>();
		public List<StatModifier> productAffectedStats = new List<StatModifier>();
	}
}
