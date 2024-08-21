namespace ODataDemo.Models
{
    public class CombinedContract : IContract
    {
        public string PrimaryContractId { get; init; }
        public string SecondaryContractId { get; init; }
        public Address Address { get; set; }
    }
}
