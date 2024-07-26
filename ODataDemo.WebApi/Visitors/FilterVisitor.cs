using Microsoft.OData.UriParser;
using ODataDemo.Models;

namespace ODataDemo.Visitors
{
    /// <summary>
    /// Collects ids used in the filter clause, e.g. "?$filter=Name in ('Customer 1', 'Customer 3')" will produce ["Customer 1", "Customer 3"]
    /// </summary>
    public class FilterVisitor : QueryNodeVisitor<bool>
    {
        public List<string> Names { get; private set; } = new List<string>();

        public override bool Visit(InNode node)
        {
            if (node.Left is SingleValuePropertyAccessNode propertyNode)
            {
                if (string.Equals(propertyNode.Property.Name, nameof(Customer.Name), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (node.Right is CollectionConstantNode collectionNode)
                    {
                        foreach (var constantNode in collectionNode.Collection.Cast<ConstantNode>())
                        {
                            Names.Add(constantNode.Value.ToString());
                        }
                    }
                }
            }
            return true;
        }
    }
}