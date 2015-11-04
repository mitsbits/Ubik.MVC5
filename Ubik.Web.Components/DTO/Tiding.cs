using System;
using System.Xml.Serialization;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Components.DTO
{
    [XmlRoot(Namespace = "http://Ubik/Web/Components/DTO/")]
    [Serializable]
    public class Tiding : Catalogued, IWeighted
    {
        private readonly Tidings _children;

        public Tiding()
        {
            _children = new Tidings();
        }

        public override string Key { get; set; }
        public override string HumanKey { get; set; }
        public override string Value { get; set; }
        public override string Hint { get; set; }
        public override string Flag { get; set; }
        public virtual double Weight { get; set; }

        public virtual Tidings Children
        {
            get { return _children; }
        }
    }
}