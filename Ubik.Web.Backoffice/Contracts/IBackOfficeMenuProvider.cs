using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Ubik.Web.Infra.Navigation;
using Ubik.Web.Infra.Navigation.Contracts;

namespace Ubik.Web.Backoffice.Contracts
{
    public interface IBackOfficeMenuProvider : IMenuProvider<BackofficeNavigationElements>
    {
    }

    public class XmlBackOfficeMenuProvider : IBackOfficeMenuProvider
    {
        private readonly XDocument _document;
        private readonly ICollection<NavigationElementDto> _data;
        private BackofficeNavigationElements _menu;
        internal XmlBackOfficeMenuProvider(string content)
        {
            _document = XDocument.Load(new StringReader(content));
            _data = new HashSet<NavigationElementDto>();
            Parse();
        }
        public ICollection<NavigationElementDto> Raw { get { return _data; } }

        public BackofficeNavigationElements Menu
        {
            get
            {
                if (_menu == null)
                {            
                    var roots =
                        _data.Where(x => x.ParentId == 0)
                            .OrderByDescending(x => x.Weight)
                            .Select(x => new BackofficeNavigationElement(_data, x.Id){IconCssClass = x.IconCssClass})
                            .ToList();
                    _menu = new BackofficeNavigationElements(roots);
                }
                return _menu;
            }
        }

        private void Parse()
        {
            var id = 1;
            var groups = from c in _document.Descendants("group") select c;
            foreach (var xGroup in groups)
            {
                var @group = new NavigationGroupDto()
                {
                    Description = xGroup.EmptyIfNull("description"),
                    Display = xGroup.EmptyIfNull("display"),
                    Key = xGroup.EmptyIfNull("key")
                };
                if (xGroup.Descendants("icon").Any())
                {
                    var icon = xGroup.Descendants("icon").First();
                    if (icon.Descendants("cssclass").Any())
                    {
                        @group.IconCssClass = icon.Descendants("cssclass").Single().Value;
                    }
                }
                foreach (var xElement in xGroup.Descendants("element"))
                {
                    var element = NavigationElementDto(xElement, id, @group);
                    if (xElement.Descendants("icon").Any())
                    {
                        var icon = xElement.Descendants("icon").First();
                        if (icon.Descendants("cssclass").Any())
                        {
                            element.IconCssClass = icon.Descendants("cssclass").Single().Value;
                        }
                    }
                    _data.Add(element);
                    Traverse(xElement, @group, ref id);
                }
            }
        }

        protected virtual NavigationElementDto NavigationElementDto(XElement xElement, int id, NavigationGroupDto @group)
        {
            var element = new NavigationElementDto()
            {
                Display = xElement.EmptyIfNull("display"),
                Id = id,
                Href = xElement.EmptyIfNull("href"),
                Group = @group,
                ParentId = 0,
                Weight = id * 10,
                Role = NavigationElementRole.Anchor
            };
            return element;
        }

        protected virtual void Traverse(XContainer xElement, NavigationGroupDto @group, ref int seq)
        {
            var parentId = seq;
            foreach (var descendant in xElement.Descendants("element"))
            {
                seq++;
                var element = NavigationElementDto(descendant, seq, @group);
                element.ParentId = parentId;
                _data.Add(element);
                Traverse(descendant, @group, ref seq);
            }
        }

        public static IBackOfficeMenuProvider FromInternalConfig()
        {
            var content = string.Empty;

            using (var stream = Assembly.GetExecutingAssembly().
                       GetManifestResourceStream("Ubik.Web.Backoffice.Configuration.MainMenu.xml"))
            {
                if (stream != null)
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
            }
            return new XmlBackOfficeMenuProvider(content);
        }
    }

    internal static class ext
    {
        public static string EmptyIfNull(this XElement element, string attrName)
        {
            return element.Attributes(attrName).Any() ? element.Attribute(attrName).Value : string.Empty;
        }
    }
}