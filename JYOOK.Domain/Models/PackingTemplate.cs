using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JYOOK.Domain
{
    public class PackingTemplate
    {
        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public double PackingWeight { get; set; } = 1;

        [XmlIgnore]
        public List<PackResourceTemplate> Resources { get; set; } = new List<PackResourceTemplate>();
    }
}