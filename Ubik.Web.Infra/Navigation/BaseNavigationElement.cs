using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Web.Infra.Navigation.Contracts;

namespace Ubik.Web.Infra.Navigation
{
    public class BaseNavigationElement : INavigationElement<int>, ITaxonomy
    {
        private const string PathSeperator = @".";
        private readonly IEnumerable<NavigationElementDto> _data;
        private readonly NavigationElementDto _proxy;
        private readonly int _depth;
        private readonly string _path;

        public BaseNavigationElement(IEnumerable<NavigationElementDto> data, int id)
        {
            _data = data as NavigationElementDto[] ?? data.ToArray();
            if (Data.All(x => x.Id != id)) throw new NullReferenceException("proxy");
            _proxy = Data.FirstOrDefault(x => x.Id == id);
            _depth = CalculateDepth();
            _path = CalculatePath();

        }

        private string CalculatePath()
        {
            if (Proxy.ParentId == default (int)) return Proxy.Id.ToString(CultureInfo.InvariantCulture);
            var idBucket = new List<int>();
            var i = Proxy.Id;
            var p = Proxy.ParentId;
            while (p > 0)
            {
                idBucket.Add(i);
                i = Data.FirstOrDefault(x => x.Id == p).Id;
                p = Data.FirstOrDefault(x => x.Id == p).ParentId;
            }
            idBucket.Reverse();
            return string.Join(PathSeperator, idBucket.ToArray());
        }

        private int CalculateDepth()
        {
            var d = 1;
            var id = ParentId;
            while ((id > 0))
            {
                d++;
                id = Convert.ToInt32(Data.FirstOrDefault(x => x.Id == id).ParentId);
            }
            return d;
        }

        public NavigationElementRole Role {
            get { return Proxy.Role; }
        }

        public string AnchorTarget
        {
            get { return Proxy.AnchorTarget; }
        }

        public string Display
        {
            get { return Proxy.Display; }
        }

        public string Href
        {
            get { return Proxy.Href; }
        }

        public int Depth { get { return _depth; } }

        public INavigationGroup Group
        {
            get { return new BaseNavigationGroup(){Description = Proxy.Group.Description, Display = Proxy.Group.Display, Key = Proxy.Group.Key, Weight = Proxy.Group.Weight};  }
        }

        public string IconCssClass { get { return Proxy.IconCssClass; } }

        public bool HasChildren
        {
            get { return Data.Any(x => x.ParentId == Proxy.Id); }
        }

        public string Path
        {
            get { return _path; }
        }

        public object Item
        {
            get { return this; }
        }

        public string Type
        {
            get { return GetType().FullName; }
        }

        public virtual IHierarchicalEnumerable GetChildren()
        {
            var result =  new BaseNavigationElements<BaseNavigationElement>(
                Data.Where(x => x.ParentId == Proxy.Id)
                .OrderBy(x => x.Weight)
                .Select(x => new BaseNavigationElement(Data, x.Id))
                .ToList());
            return result;
        }

        public virtual IHierarchyData GetParent()
        {
            return Data.Any(x => x.Id == Proxy.ParentId) ? new BaseNavigationElement(Data, Proxy.ParentId) : null;
        }

        public double Weight
        {
            get { return Proxy.Weight; }
        }

        public int Id
        {
            get { return Proxy.Id; }
        }

        public int ParentId
        {
            get { return Proxy.ParentId; }
        }

        protected IEnumerable<NavigationElementDto> Data
        {
            get { return _data; }
        }

        protected NavigationElementDto Proxy
        {
            get { return _proxy; }
        }
    }
}
