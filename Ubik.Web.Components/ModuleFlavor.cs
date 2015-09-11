using System;
using System.Collections.Generic;

namespace Ubik.Web.Components
{
    public class ModuleFlavor
    {
        private readonly string _flavorDescription = String.Empty;
        private static readonly IDictionary<string, ModuleFlavor> Dict = new Dictionary<string, ModuleFlavor>();

        public static readonly ModuleFlavor Empty         = new ModuleFlavor("Empty");
        public static readonly ModuleFlavor PartialAction = new ModuleFlavor("Partial Action");
        public static readonly ModuleFlavor PartialView   = new ModuleFlavor("Partial View");
        public static readonly ModuleFlavor ViewComponent = new ModuleFlavor("View Component");

        private ModuleFlavor(string flavorDescription)
        {
            _flavorDescription = flavorDescription;
            Dict.Add(flavorDescription, this);
        }

        public override string ToString()
        {
            return _flavorDescription;
        }

        public static ModuleFlavor Parse(string flavorDescription)
        {
            if (Dict.Keys.Contains(flavorDescription))
            {
                return Dict[flavorDescription];
            }
            throw new NotImplementedException("This flavor description is not supported currently.");
        }

        public static bool TryParse(string heightDescription, out ModuleFlavor flavor)
        {
            try
            {
                flavor = Parse(heightDescription);
                return true;
            }
            catch (NotImplementedException ex)
            {
                flavor = null;
                return false;
            }
        }

        public static List<ModuleFlavor> GetMembers()
        {
            return new List<ModuleFlavor>(Dict.Values);
        }
    }
}