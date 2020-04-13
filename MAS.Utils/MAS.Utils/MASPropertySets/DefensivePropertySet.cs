namespace MAS.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public class DefensivePropertySet : PropertySet
    {
        public DefensivePropertySet() { }

        public DefensivePropertySet(Prop1 defensiveProp1, Prop2 defensiveProp2, Prop3 defensiveProp3)
            : base(defensiveProp1, defensiveProp2, defensiveProp3) { }

        public Dictionary<(Prop1 OffensiveProp1, Prop2 OffensiveProp2, Prop3 OffensiveProp3), OffensivePropertySet> TargetingOffensiveProperties { get; set; } = new Dictionary<(Prop1, Prop2, Prop3), OffensivePropertySet>();

    }
}
