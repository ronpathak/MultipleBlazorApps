using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiBlazorApps.Components.Helpers
{
    public class MultipleSelectorModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public MultipleSelectorModel(string key, string value)
        {
            Key = key;
            Value = value;
        }

    }
}
