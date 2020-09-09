using System.Collections.Generic;
using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class PackingInfo
    {
        [XmlAttribute]
        public int SalesProductId { get; set; }

        [XmlAttribute]
        public double PackingWeight { get; set; } = 1;

        [XmlIgnore]
        public List<PackResource> Resources { get; set; } = new List<PackResource>();
    }
}