namespace MAS.Utils
{
    public class OffensivePropertySet : PropertySet
    {
        public (Prop1, Prop2, Prop3) TargetPropertyKey { get; set; }

        public double TargetResistanceValue { get; set; }

        public bool TargetIsImmune { get; set; }
    }
}
