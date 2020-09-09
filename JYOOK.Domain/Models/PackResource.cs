using System;
using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class PackResource
    {
        [XmlAttribute]
        public int SalesProductId { get; set; }

        [XmlAttribute]
        public Guid ExtraResourceId { get; set; }

        [XmlIgnore]
        public ExtraResource Resource { get; set; }

        [XmlAttribute]
        public ushort Count { get; set; }
    }
}