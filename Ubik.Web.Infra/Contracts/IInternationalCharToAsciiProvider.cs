using System.Collections.Generic;

namespace Ubik.Web.Infra.Contracts
{
    public interface IInternationalCharToAsciiProvider
    {
        IReadOnlyDictionary<char, char[]> Reference { get; }

        char[] Remap(char c);
    }
}