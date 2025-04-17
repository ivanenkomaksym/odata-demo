namespace ODataDemo.Models
{
    public class Node<T, TChilren>
    {
        public string Id { get; set; }
        public IEnumerable<TChilren> Children { get; set; } = new List<TChilren>();

        public T Metadata { get; set; }
    }
}
