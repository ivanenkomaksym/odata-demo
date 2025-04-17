using ODataDemo.Foo;
using ODataDemo.Models;

namespace ODataDemo.Bar
{
    public class BarNodeMetadata
    {
        public string Category { get; set; }
        public string Label { get; set; }
    }

    public class Node : Node<BarNodeMetadata> { }
}
