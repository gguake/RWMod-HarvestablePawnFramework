using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace HPF
{
    public class CompResourceHarvestable : ThingComp
	{
		public CompProperties_ResourceHarvestable Props
		{
			get
			{
				return (CompProperties_ResourceHarvestable)this.props;
			}
		}

		protected bool Active
		{
			get
			{
				if (this.parent.Faction == null)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				using (List<Constraint>.Enumerator enumerator = this.Props.constraints.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CheckActiveCondition(this, pawn, null))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.Props.saveUniqueKey, 0f, false);
		}

		public override void CompTick()
		{
			if (this.Active)
			{
				float progressPerTick = 1f / (this.Props.intervalDays * 60000f);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					progressPerTick *= PawnUtility.BodyResourceGrowthSpeed(pawn);

					if (Props.affectedStats?.Any() ?? false)
					{
						foreach (var statMultiplier in Props.affectedStats)
						{
							progressPerTick *= pawn.GetStatValue(statMultiplier.statDef) * statMultiplier.multiplier;
						}
					}
				}

				this.fullness += progressPerTick;
				if (this.fullness > 1f)
				{
					this.fullness = 1f;
				}
			}
		}

		public void Gathered(Pawn doer, StatDef stat)
		{
			if (!this.Active)
			{
				Log.Error(doer + " gathered body resources while not Active: " + this.parent);
			}
			if (!Rand.Chance(doer.GetStatValue(stat, true)))
			{
				MoteMaker.ThrowText((doer.DrawPos + this.parent.DrawPos) / 2f, this.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int i = GenMath.RoundRandom((float)this.Props.amount * this.fullness);
				while (i > 0)
				{
					int num = Mathf.Clamp(i, 1, this.Props.thingDef.stackLimit);
					i -= num;
					Thing thing = ThingMaker.MakeThing(this.Props.thingDef, null);
					thing.stackCount = num;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null, null, default(Rot4));
				}
			}
			this.fullness = 0f;
		}

		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			if (this.Props.inspectText == null)
			{
				return null;
			}
			return this.Props.inspectText.Translate() + ": " + this.fullness.ToStringPercent();
		}

		private float fullness;
	}
}
