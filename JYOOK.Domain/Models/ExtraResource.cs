using System;
using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class ExtraResource
    {
        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public ResourceType Type { get; set; }

        [XmlAttribute]
        public double Price { get; set; }
    }
}