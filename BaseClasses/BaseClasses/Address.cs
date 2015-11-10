namespace Bitcoin
{
    [FileHelpers.DelimitedRecord(",")]
    public class Address
    {
        public string ID;

        [FileHelpers.FieldHidden]
        public Output Output;
    }
}
