namespace MAS.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using log4net;

    public class DefensivePropertySetCollection
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DefensivePropertySetCollection() { }

        public DefensivePropertySetCollection(Dictionary<(Prop1, Prop2, Prop3), DefensivePropertySet> collection)
        {
            this.Collection = collection;
        }

        public DefensivePropertySetCollection(DefensivePropertySet defensivePropertySet)
        {
            this.Collection = new Dictionary<(Prop1, Prop2, Prop3), DefensivePropertySet>
            {
                {
                    defensivePropertySet.GetDictionaryKey(),
                    defensivePropertySet
                }
            };
        }

        public DefensivePropertySetCollection(DefensivePropertySetCollection defensivePropertySetCollection)
        {
            this.Collection = defensivePropertySetCollection.Collection;
        }
        
        public Dictionary<(Prop1 Property1, Prop2 Property2, Prop3 Property3), DefensivePropertySet> Collection { get; set; } = new Dictionary<(Prop1, Prop2, Prop3), DefensivePropertySet>();

        public DefensivePropertySetCollection Clone()
        {
            return new DefensivePropertySetCollection(this);
        }

        /// <summary>
        /// Filters the active collection based on a set of defensive properties.
        /// </summary>
        public DefensivePropertySetCollection FilterBy(Prop1? defensiveProp1 = null, Prop2? defensiveProp2 = null, Prop3? defensiveProp3 = null)
        {
            var newCollection = this.Collection.AsEnumerable();
            if (defensiveProp1 != null)
            {
                newCollection = newCollection.Where(defensiveSetKvp => defensiveSetKvp.Key.Property1 == defensiveProp1);
            }
            if (defensiveProp2 != null)
            {
                newCollection = newCollection.Where(defensiveSetKvp => defensiveSetKvp.Key.Property2 == defensiveProp2);
            }
            if (defensiveProp3 != null)
            {
                newCollection = newCollection.Where(defensiveSetKvp => defensiveSetKvp.Key.Property3 == defensiveProp3);
            }

            this.Collection = newCollection.ToDictionary(k => k.Key, k => k.Value);
            return this;
        }

        /// <summary>
        /// Returns a subset of the active collection, where the resistance values of the collection's defensive property are evaluated by a comparison operator with a given value.
        /// </summary>
        public DefensivePropertySetCollection EvaluateOffensiveCondition(Prop1 offensiveProp1, Prop2 offensiveProp2, Prop3 offensiveProp3, Comparator comparator, double value = 0)
        {
            var offensivePropertyKey = (offensiveProp1, offensiveProp2, offensiveProp3);
            var newCollection = this.Collection.AsEnumerable();
            switch (comparator)
            {
                case Comparator.eq:
                    newCollection = this.Collection.Where(defensiveSet =>
                        Math.Abs(defensiveSet.Value.TargetingOffensiveProperties[offensivePropertyKey].TargetResistanceValue - value) < 1e-4);
                    break;
                case Comparator.ge:
                    newCollection = this.Collection.Where(defensiveSet =>
                        defensiveSet.Value.TargetingOffensiveProperties[offensivePropertyKey].TargetResistanceValue >= value);
                    break;
                case Comparator.gt:
                    newCollection = this.Collection.Where(defensiveSet =>
                        defensiveSet.Value.TargetingOffensiveProperties[offensivePropertyKey].TargetResistanceValue > value);
                    break;
                case Comparator.le:
                    newCollection = this.Collection.Where(defensiveSet =>
                        defensiveSet.Value.TargetingOffensiveProperties[offensivePropertyKey].TargetResistanceValue <= value);
                    break;
                case Comparator.lt:
                    newCollection = this.Collection.Where(defensiveSet =>
                        defensiveSet.Value.TargetingOffensiveProperties[offensivePropertyKey].TargetResistanceValue < value);
                    break;
                case Comparator.IMMUNE:
                    newCollection = this.Collection.Where(defensiveSet =>
                        defensiveSet.Value.TargetingOffensiveProperties[offensivePropertyKey].TargetIsImmune);
                    break;
                default:
                    Log.Error($"Unrecognized comparison operator '{comparator.ToString()}', ignoring...");
                    return this;
            }

            this.Collection = newCollection.ToDictionary(k => k.Key, k => k.Value);
            return this;
        }

        public IOrderedEnumerable<OffensivePropertySetAverage> GetBestOffensivePropertySets()
        {
            var allOffensivePropertySetsByKey = this.Collection.SelectMany(defensiveSetKvp => defensiveSetKvp.Value.TargetingOffensiveProperties)
                .GroupBy(k => k.Key);

            var offensivePropertySetAverages = allOffensivePropertySetsByKey.Select(g =>
            {
                var (offensiveProp1, offensiveProp2, offensiveProp3) = g.Key;
                var allOffensiveProperties = g.Select(offensiveSetKvp => offensiveSetKvp.Value);
                return new OffensivePropertySetAverage(offensiveProp1, offensiveProp2, offensiveProp3, allOffensiveProperties, this.Collection.Keys);
            });

            return offensivePropertySetAverages
                .OrderByDescending(oa => oa.Average)
                .ThenBy(oa => oa.Variance)
                .ThenByDescending(oa => oa.Min)
                .ThenByDescending(oa => oa.Max);
        }
    }
}
