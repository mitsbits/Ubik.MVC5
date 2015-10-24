using System.Linq;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Infra.Services
{
    public class SystemSlugCharReplacer : ISlugCharOmmiter
    {
        private static readonly char[] excludedChars =
        {
            ' '
            , ','
            , '.'
            , '/'
            , '\\'
            , '-'
            , '_'
            , '='
            , '«'
            , '»'
            , '~'
            ,'\''
            ,'"'
            ,'*'
            ,'+'
            ,';'
            ,'&'
            ,':'
            ,'¨'
            ,'…'
        };

        public bool Ommit(char source)
        {
            return excludedChars.Contains(source);
        }
    }
}