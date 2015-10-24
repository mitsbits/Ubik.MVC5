using System.Linq;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Infra.Services
{
    public class SystemSlugWordRplacer : ISlugWordReplacer
    {
        private static readonly string[] _excludedWords = { "...", "…" };

        public string Replace(string source)
        {
            return _excludedWords.Aggregate(source, (current, throwAway) => current.Replace(throwAway, " "));
        }
    }
}