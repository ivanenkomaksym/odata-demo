using ODataDemo.Bar;
using ODataDemo.Foo;
using ODataDemo.Models;

namespace ODataDemo.IntegrationTests
{
    public class NodeTests
    {
        public void visit(Foo.Node node)
        {

        }

        [Fact]
        public void FooNode_ShouldHoldCorrectMetadata()
        {
            var fooMetadata = new FooNodeMetadata
            {
                IsSpecial = true,
                IsEnabled = false
            };

            var fooNode = new Foo.Node
            {
                Id = "foo1",
                Metadata = fooMetadata
            };

            visit(fooNode.Children.First());

            Assert.Equal("foo1", fooNode.Id);
            Assert.True(fooNode.Metadata.IsSpecial);
            Assert.False(fooNode.Metadata.IsEnabled);
            Assert.Empty(fooNode.Children);
        }

        [Fact]
        public void BarNode_ShouldSupportChildren()
        {
            var parent = new Bar.Node
            {
                Id = "bar-parent",
                Metadata = new BarNodeMetadata
                {
                    Category = "Main",
                    Label = "Parent"
                },
                Children =
                [
                    new Bar.Node
                    {
                        Id = "bar-child",
                        Metadata = new BarNodeMetadata
                        {
                            Category = "Child",
                            Label = "Leaf"
                        }
                    }
                ]
            };

            Assert.Single(parent.Children);
            var child = Assert.Single(parent.Children); // Also assigns it
            Assert.Equal("bar-child", child.Id);
            Assert.Equal("Leaf", child.Metadata.Label);
        }
    }
}
