using System.Collections.Generic;

namespace Ubik.Web.Infra.Contracts
{
    public interface IInternationalCharToAsciiProvider
    {
        IReadOnlyDictionary<char, char[]> Refernce { get; }

        char[] Remap(char c);
    }
}