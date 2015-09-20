using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ubik.Infra.Contracts;
using Ubik.Web.Infra.Navigation.Contracts;

namespace Ubik.Web.Infra.Navigation
{
    public class BaseNavigationElement : INavigationElement<int>
    {
        private const string PathSeperator = @".";
        private readonly IEnumerable<NavigationElementDto> _data;
        private readonly NavigationElementDto _proxy;
        private readonly int _depth;
        private readonly string _path;

        public BaseNavigationElement(IEnumerable<NavigationElementDto> data, int id)
        {
            _data = data as NavigationElementDto[] ?? data.ToArray();
            if (_data.All(x => x.Id != id)) throw new NullReferenceException("proxy");
            _proxy = _data.FirstOrDefault(x => x.Id == id);
            _depth = CalculateDepth();
            _path = CalculatePath();

        }

        private string CalculatePath()
        {
            if (_proxy.ParentId == default (int)) return _proxy.Id.ToString(CultureInfo.InvariantCulture);
            var idBucket = new List<int>();
            var i = _proxy.Id;
            var p = _proxy.ParentId;
            while (p > 0)
            {
                idBucket.Add(i);
                i = _data.FirstOrDefault(x => x.Id == p).Id;
                p = _data.FirstOrDefault(x => x.Id == p).ParentId;
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
                id = Convert.ToInt32(_data.FirstOrDefault(x => x.Id == id).ParentId);
            }
            return d;
        }

        public NavigationElementRole Role {
            get { return _proxy.Role; }
        }

        public string AnchorTarget
        {
            get { return _proxy.AnchorTarget; }
        }

        public string Display
        {
            get { return _proxy.Display; }
        }

        public string Href
        {
            get { return _proxy.Href; }
        }

        public int Depth { get { return _depth; } }

        public INavigationGroup Group
        {
            get { return new BaseNavigationGroup(){Description = _proxy.Group.Description, Display = _proxy.Group.Display, Key = _proxy.Group.Key, Weight = _proxy.Group.Weight};  }
        }

        public bool HasChildren
        {
            get { return _data.Any(x => x.ParentId == _proxy.Id); }
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

        public IHierarchicalEnumerable GetChildren()
        {
            var result =  new BaseNavigationElements<BaseNavigationElement>(
                _data.Where(x => x.ParentId == _proxy.Id)
                .OrderByDescending(x => x.Weight)
                .Select(x => new BaseNavigationElement(_data, x.Id))
                .ToList());
            return result;
        }

        public IHierarchyData GetParent()
        {
            return _data.Any(x => x.Id == _proxy.ParentId) ? new BaseNavigationElement(_data, _proxy.ParentId) : null;
        }

        public double Weight
        {
            get { return _proxy.Weight; }
        }

        public int Id
        {
            get { return _proxy.Id; }
        }

        public int ParentId
        {
            get { return _proxy.ParentId; }
        }
    }
}
