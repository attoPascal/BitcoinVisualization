using Bitcoin;
using FileHelpers;
//using System.ComponentModel.DataAnnotations;

namespace Test
{
    class FileHelpersTest
    {
        static void Main()
        {
            System.Diagnostics.Debug.WriteLine("hallo");

            var engine = new FileHelperEngine<Address>();
            var fileName = @"..\..\..\..\..\blocks_1_180000\addresses.csv";
            var addresses = engine.ReadFile(fileName);

            for (int i = 0; i < 10; i++)
            {
                Address address = addresses[i];
                System.Diagnostics.Debug.WriteLine(address.ID);
            }
        }
    }

    
    //[MetadataType(typeof(AddressRecordMetadata))]
    //[DelimitedRecord(",")]
    //public class AddressRecord : Address
    //{
        
    //}

    
    //public class AddressRecordMetadata
    //{
    //    [FieldHidden]
    //    public Output Output;
    //}
}
