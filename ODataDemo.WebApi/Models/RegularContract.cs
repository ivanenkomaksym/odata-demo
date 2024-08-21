namespace ODataDemo.Models
{
    public class RegularContract: IContract
    {
        public string ContractId { get; init; }
        public Address Address { get; set; }
    }
}
