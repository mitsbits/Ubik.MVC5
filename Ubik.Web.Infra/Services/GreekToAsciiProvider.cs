﻿using System.Collections.Generic;
using System.Linq;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Infra.Services
{
    public class GreekToAsciiProvider : IInternationalCharToAsciiProvider
    {
        private static readonly IDictionary<char, char> _ticksDict = new Dictionary<char, char>
        {
            {'Ά', 'Α'},
            {'ά', 'α'},
            {'Έ', 'Ε'},
            {'έ', 'ε'},
            {'Ί', 'Ι'},
            {'ί', 'ι'},
            {'ΐ', 'ι'},
            {'Ή', 'Η'},
            {'ή', 'η'},
            {'Ό', 'Ο'},
            {'ό', 'ο'},
            {'Ύ', 'Υ'},
            {'ύ', 'υ'},
            {'ΰ', 'υ'},
            {'Ώ', 'Ω'},
            {'ώ', 'ω'}
        };

        private static readonly IDictionary<char, string> _reference = new Dictionary<char, string>
        {
            {'α', "a"},
            {'β', "v"},
            {'γ', "g"},
            {'δ', "d"},
            {'ε', "e"},
            {'ζ', "z"},
            {'η', "i"},
            {'θ', "th"},
            {'ι', "i"},
            {'κ', "k"},
            {'λ', "l"},
            {'μ', "m"},
            {'ν', "n"},
            {'ξ', "x"},
            {'ο', "o"},
            {'π', "p"},
            {'ρ', "r"},
            {'σ', "s"},
            {'τ', "t"},
            {'υ', "u"},
            {'φ', "f"},
            {'χ', "ch"},
            {'ψ', "ps"},
            {'ω', "o"},
            {'Α', "A"},
            {'Β', "V"},
            {'Γ', "G"},
            {'Δ', "D"},
            {'Ε', "E"},
            {'Ζ', "Z"},
            {'Η', "H"},
            {'Θ', "TH"},
            {'Ι', "I"},
            {'Κ', "K"},
            {'Λ', "L"},
            {'Μ', "M"},
            {'Ν', "N"},
            {'Ξ', "X"},
            {'Ο', "O"},
            {'Π', "P"},
            {'Ρ', "R"},
            {'Σ', "S"},
            {'Τ', "T"},
            {'Υ', "Y"},
            {'Φ', "F"},
            {'Χ', "CH"},
            {'Ψ', "PS"},
            {'Ω', "O"},
            {'ς', "s"}
        };

        public IReadOnlyDictionary<char, char[]> Refernce { get { return _reference.ToDictionary(x => x.Key, x => x.Value.ToCharArray()); } }

        public char[] Remap(char c)
        {
            c = StripTicksFromGreekVowel(c);
            return _reference.Keys.Contains(c) ? _reference[c].ToCharArray() : new[] { c };
        }

        private static char StripTicksFromGreekVowel(char c)
        {
            return _ticksDict.Keys.Contains(c) ? _ticksDict[c] : c;
        }
    }
}