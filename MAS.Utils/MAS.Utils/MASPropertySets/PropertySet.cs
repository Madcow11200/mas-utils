namespace MAS.Utils
{
    public class PropertySet
    {
        public PropertySet() { }

        public PropertySet(Prop1 prop1, Prop2 prop2, Prop3 prop3)
        {
            this.Property1 = prop1;
            this.Property2 = prop2;
            this.Property3 = prop3;
        }

        public Prop1 Property1 { get; set; }

        public Prop2 Property2 { get; set; }

        public Prop3 Property3 { get; set; }

        public (Prop1 Property1, Prop2 Property2, Prop3 Property3) GetDictionaryKey()
        {
            return (this.Property1, this.Property2, this.Property3);
        }

        public override string ToString()
        {
            return $"{this.Property1} {this.Property2} {this.Property3}";
        }
    }
}
