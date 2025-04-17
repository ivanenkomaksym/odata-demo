using ODataDemo.Models;

namespace ODataDemo.Foo
{
    public class FooNodeMetadata
    {
        public bool IsSpecial { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class Node : Node<FooNodeMetadata, Node> { }
}
