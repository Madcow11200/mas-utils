namespace MAS.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public class OffensivePropertySetAverage : PropertySet
    {
        public OffensivePropertySetAverage(Prop1 offensiveProp1, Prop2 offensiveProp2, Prop3 offensiveProp3, IEnumerable<OffensivePropertySet> allOffensiveProperties, IEnumerable<(Prop1 prop1, Prop2 prop2, Prop3 prop3)> allTargetPropertyKeys) 
            : base(offensiveProp1, offensiveProp2, offensiveProp3)
        {
            this.TargetPropertyKeys = allTargetPropertyKeys.ToList();

            this.Average = allOffensiveProperties.Average(offensivePropSet => offensivePropSet.TargetResistanceValue);
            this.Variance = allOffensiveProperties.Average(offensivePropSet => Math.Pow(offensivePropSet.TargetResistanceValue - this.Average, 2));
            this.StandardDeviation = Math.Sqrt(this.Variance);
            this.Min = allOffensiveProperties.Min(offensivePropSet => offensivePropSet.TargetResistanceValue);
            this.Max = allOffensiveProperties.Max(offensivePropSet => offensivePropSet.TargetResistanceValue);
            
            this.AllResistanceValues = allOffensiveProperties.Select(offensivePropSet => offensivePropSet.TargetResistanceValue).OrderBy(val => val).ToList();
        }

        public List<(Prop1 prop1, Prop2 prop2, Prop3 prop3)> TargetPropertyKeys { get; set; }

        public double Average { get; set; }

        public double Variance { get; set; }

        public double StandardDeviation { get; set; }

        public double Min { get; set; }

        public double Max { get; set; }

        public List<double> AllResistanceValues { get; set; }

        public override string ToString()
        {
            return $"{base.ToString(), -21} | AVG: {this.Average, -4} | STD_DEV: {this.StandardDeviation}";
        }
    }
}
