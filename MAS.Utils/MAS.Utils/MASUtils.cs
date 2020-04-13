namespace MAS.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using log4net;

    public class MASUtils
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly IEnumerable<Prop1> Prop1Values = Enum.GetValues(typeof(Prop1)).Cast<Prop1>();
        private static readonly IEnumerable<Prop2> Prop2Values = Enum.GetValues(typeof(Prop2)).Cast<Prop2>();
        private static readonly IEnumerable<Prop3> Prop3Values = Enum.GetValues(typeof(Prop3)).Cast<Prop3>();

        private const double ImmunityWeight = 1.5;
        private const double BaneWeight = 1.5;

        public static DefensivePropertySetCollection ProcessAllDefensivePropertySets()
        {
            var defensivePropertySets = new Dictionary<(Prop1, Prop2, Prop3), DefensivePropertySet>();

            foreach (var defensiveProp1 in Prop1Values)
            {
                foreach (var defensiveProp2 in Prop2Values)
                {
                    foreach (var defensiveProp3 in Prop3Values)
                    {
                        var defensivePropertySet = ProcessAllAttackPropertySets(defensiveProp1, defensiveProp2, defensiveProp3);
                        defensivePropertySets.Add(defensivePropertySet.GetDictionaryKey(), defensivePropertySet);
                    }
                }
            }

            return new DefensivePropertySetCollection(defensivePropertySets);
        }

        public static DefensivePropertySetCollection ProcessDefensivePropertySet(Prop1 defensiveProp1, Prop2 defensiveProp2, Prop3 defensiveProp3)
        {
            var defensivePropertySet = ProcessAllAttackPropertySets(defensiveProp1, defensiveProp2, defensiveProp3);
            return new DefensivePropertySetCollection(defensivePropertySet);
        }

        public static DefensivePropertySet ProcessAllAttackPropertySets(Prop1 defensiveProp1, Prop2 defensiveProp2, Prop3 defensiveProp3)
        {
            var defensivePropertySet = new DefensivePropertySet(defensiveProp1, defensiveProp2, defensiveProp3);

            foreach (var offensiveProp1 in Prop1Values)
            {
                foreach (var offensiveProp2 in Prop2Values)
                {
                    foreach (var offensiveProp3 in Prop3Values)
                    {
                        var offensivePropertySet = ProcessAttackPropertySet(defensiveProp1, defensiveProp2, defensiveProp3, offensiveProp1, offensiveProp2, offensiveProp3);
                        defensivePropertySet.TargetingOffensiveProperties.Add(offensivePropertySet.GetDictionaryKey(), offensivePropertySet);
                    }
                }
            }

            return defensivePropertySet;
        }

        public static OffensivePropertySet ProcessAttackPropertySet(Prop1 defensiveProp1, Prop2 defensiveProp2, Prop3 defensiveProp3, Prop1 offensiveProp1, Prop2 offensiveProp2, Prop3 offensiveProp3)
        {
            double sum1 = 0;
            double sum2 = 0;
            double sum3 = 0;

            var possibleImmunity = false;

            // + means weak

            switch (defensiveProp1)
            {
                case Prop1.Blunt:
                    if (offensiveProp1 == Prop1.Blade)
                        sum1++;
                    if (offensiveProp1 == Prop1.Gun)
                        sum1--;
                    if (offensiveProp2 == Prop2.Fire)
                        sum1--;
                    if (offensiveProp3 == Prop3.Magical)
                        sum1++;
                    break;
                case Prop1.Blade:
                    if (offensiveProp1 == Prop1.Gun)
                        sum1++;
                    if (offensiveProp1 == Prop1.Blunt)
                        sum1--;
                    if (offensiveProp2 == Prop2.Earth)
                        sum1++;
                    if (offensiveProp3 == Prop3.Kinetical)
                        sum1--;
                    break;
                case Prop1.Gun:
                    if (offensiveProp1 == Prop1.Blunt)
                        sum1++;
                    if (offensiveProp1 == Prop1.Blade)
                        sum1--;
                    if (offensiveProp2 == Prop2.Water)
                        sum1++;
                    if (offensiveProp3 == Prop3.Physical)
                        sum1--;
                    if (offensiveProp3 == Prop3.Kinetical)
                    {
                        possibleImmunity = true;
                        sum1 -= ImmunityWeight;
                    }
                    if (offensiveProp2 == Prop2.Fire)
                        sum1 += BaneWeight;
                    break;
                default:
                    break;
            }

            switch (defensiveProp2)
            {
                case Prop2.Earth:
                    if (offensiveProp2 == Prop2.Fire)
                        sum2++;
                    if (offensiveProp2 == Prop2.Water)
                        sum2--;
                    if (offensiveProp1 == Prop1.Blade)
                        sum2--;
                    if (offensiveProp3 == Prop3.Kinetical)
                        sum2++;
                    break;
                case Prop2.Fire:
                    if (offensiveProp2 == Prop2.Water)
                        sum2++;
                    if (offensiveProp2 == Prop2.Earth)
                        sum2--;
                    if (offensiveProp1 == Prop1.Blunt)
                        sum2++;
                    if (offensiveProp3 == Prop3.Magical)
                        sum2--;
                    if (offensiveProp1 == Prop1.Gun)
                    {
                        possibleImmunity = true;
                        sum2 -= ImmunityWeight;
                    }
                    if (offensiveProp3 == Prop3.Kinetical)
                        sum2 += BaneWeight;
                    break;
                case Prop2.Water:
                    if (offensiveProp2 == Prop2.Earth)
                        sum2++;
                    if (offensiveProp2 == Prop2.Fire)
                        sum2--;
                    if (offensiveProp3 == Prop3.Physical)
                        sum2++;
                    if (offensiveProp1 == Prop1.Gun)
                        sum2--;
                    break;
                default:
                    break;
            }

            switch (defensiveProp3)
            {
                case Prop3.Physical:
                    if (offensiveProp3 == Prop3.Magical)
                        sum3++;
                    if (offensiveProp3 == Prop3.Kinetical)
                        sum3--;
                    if (offensiveProp2 == Prop2.Water)
                        sum3--;
                    if (offensiveProp1 == Prop1.Gun)
                        sum3++;
                    break;
                case Prop3.Magical:
                    if (offensiveProp3 == Prop3.Kinetical)
                        sum3++;
                    if (offensiveProp3 == Prop3.Physical)
                        sum3--;
                    if (offensiveProp2 == Prop2.Fire)
                        sum3++;
                    if (offensiveProp1 == Prop1.Blunt)
                        sum3--;
                    break;
                case Prop3.Kinetical:
                    if (offensiveProp3 == Prop3.Physical)
                        sum3++;
                    if (offensiveProp3 == Prop3.Magical)
                        sum3--;
                    if (offensiveProp1 == Prop1.Blade)
                        sum3++;
                    if (offensiveProp2 == Prop2.Earth)
                        sum3--;
                    if (offensiveProp2 == Prop2.Fire)
                    {
                        possibleImmunity = true;
                        sum3 -= ImmunityWeight;
                    }
                    if (offensiveProp1 == Prop1.Gun)
                        sum3 += BaneWeight;
                    break;
                default:
                    break;
            }

            var sum = sum1 + sum2 + sum3;

            var offensivePropertySet = new OffensivePropertySet
            {
                Property1 = offensiveProp1,
                Property2 = offensiveProp2,
                Property3 = offensiveProp3,
                TargetPropertyKey = (defensiveProp1, defensiveProp2, defensiveProp3),
                TargetResistanceValue = Math.Abs(sum) < 1 ? Math.Round(sum, MidpointRounding.AwayFromZero) : sum,
                TargetIsImmune = possibleImmunity && sum <= -1
            };

            return offensivePropertySet;
        }

        public static void PrintHistograms(DefensivePropertySetCollection defensivePropertySetCollection, string name = null, bool printBestOffensiveProperties = false, int bestOffensivePropertiesCount = 24)
        {
            var sb = new StringBuilder();
            var nameProvided = !string.IsNullOrWhiteSpace(name);
            var incrementer = 1;
            var defensivePropertySetCount = defensivePropertySetCollection.Collection.Count;

            foreach (var defensivePropertySet in defensivePropertySetCollection.Collection.Values)
            {
                // Name (if present) and property set
                if (nameProvided)
                {
                    sb.AppendLine($"{name} [{incrementer}/{defensivePropertySetCount}]");
                }
                sb.AppendLine(defensivePropertySet.ToString());

                var offensivePropertySets = defensivePropertySet.TargetingOffensiveProperties.Values;

                var offensivePropertySetsForNonImmunities = offensivePropertySets.Where(c => !c.TargetIsImmune).ToList();
                var offensivePropertySetsForImmunities = offensivePropertySets.Where(c => c.TargetIsImmune).ToList();

                // Cumulative sum
                var offensivePropertyCumulativeSum = offensivePropertySets.Sum(c => c.TargetResistanceValue);
                if (offensivePropertySetsForImmunities.Count != 0)
                {
                    sb.AppendLine("Cumulative Sum:");
                    sb.AppendLine($" Without Immunities: {offensivePropertySetsForNonImmunities.Sum(c => c.TargetResistanceValue)}");
                    sb.AppendLine($" With Immunities: {offensivePropertyCumulativeSum}");
                }
                else
                {
                    sb.AppendLine($"Cumulative Sum: {offensivePropertyCumulativeSum}");
                }

                // Sum and count of targeting resistance values at or above zero
                var offensivePropertySetsWithPositiveOrZeroSums = offensivePropertySets.Where(c => c.TargetResistanceValue >= 0).ToList();
                sb.AppendLine($"Positive Sum With Zero: {offensivePropertySetsWithPositiveOrZeroSums.Sum(c => c.TargetResistanceValue)}");
                sb.AppendLine($"Positive Count With Zero: {offensivePropertySetsWithPositiveOrZeroSums.Count}");

                var offensivePropertySetsForNonImmunitiesGroupedBySum = offensivePropertySetsForNonImmunities.GroupBy(c => c.TargetResistanceValue).OrderByDescending(d => d.Key).ToList();
                var offensivePropertySetsForImmunitiesGroupedBySum = offensivePropertySetsForImmunities.GroupBy(c => c.TargetResistanceValue).OrderByDescending(d => d.Key).ToList();

                // Sum histogram
                sb.AppendLine("Sum Histogram:");
                offensivePropertySetsForNonImmunitiesGroupedBySum.ForEach(c => sb.AppendLine($" Value: {c.Key, -4} | Count: {c.Count(), -2} | Properties: {string.Join(", ", c.Select(d => d.ToString()))}"));

                // Immunity histogram
                if (offensivePropertySetsForImmunitiesGroupedBySum.Count != 0)
                {
                    sb.AppendLine($"Immunities: {offensivePropertySetsForImmunities.Count}");
                    sb.AppendLine("Immunity Histogram:");
                    offensivePropertySetsForImmunitiesGroupedBySum.ForEach(c => sb.AppendLine($" Value: {c.Key, -4} | Count: {c.Count(), -2} | Properties: {string.Join(", ", c.Select(d => d.ToString()))}"));
                }

                Log.Info(sb.ToString());
                sb.Clear();

                incrementer++;
            }

            // Best targeting offensive properties (if present)
            if (printBestOffensiveProperties && defensivePropertySetCount > 0)
            {
                if (nameProvided)
                {
                    sb.AppendLine(name);
                }

                sb.AppendLine("Best Targeting Properties:");
                var bestOffensivePropertySets = defensivePropertySetCollection.GetBestOffensivePropertySets();
                foreach (var bestOffensivePropertySet in bestOffensivePropertySets.Take(bestOffensivePropertiesCount))
                {
                    sb.AppendLine($" {bestOffensivePropertySet}");
                }

                Log.Info(sb.ToString());
                sb.Clear();
            }
        }
    }
}