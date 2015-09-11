using System;
using System.Collections.Generic;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.DTO;
using Ubik.Web.Components.Ext;

namespace Ubik.Web.Components
{
    [Serializable]
    public abstract class BasePartialModule : IModule<Tidings>
    {
        protected BasePartialModule(string friendlyName)
            : this()
        {
            FriendlyName = friendlyName;
        }

        protected BasePartialModule(string friendlyName, IDictionary<string, object> parameters)
            : this(friendlyName)
        {
            Parameters.AppendAndUpdate(parameters);
        }

        private BasePartialModule()
        {
            Parameters = new Tidings();
        }

        //TODO: find a way so ModuleFlavor can be extended by calling code
        public abstract ModuleFlavor Flavor { get; }

        public string FriendlyName { get; private set; }

        public Tidings Parameters { get; private set; }

        protected object GetInternalValue(string keyPrefix, string key)
        {
            var dickey = string.Format("{0}{1}", keyPrefix, key);
            return Parameters.ContainsKey(dickey) ? Parameters[dickey] : string.Empty as object;
        }

        protected void SetInternalValue(string keyPrefix, string key, string value)
        {
            var dickey = string.Format("{0}{1}", keyPrefix, key);
            if (!Parameters.ContainsKey(dickey))
            {
                Parameters.Add(dickey, value);
            }
            else
            {
                Parameters[dickey] = value;
            }
        }
    }
}