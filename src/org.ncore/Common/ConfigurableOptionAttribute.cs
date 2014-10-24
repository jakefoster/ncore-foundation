using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.ncore.Common
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = true, Inherited = true )]
    public class ConfigurableOptionAttribute : Attribute
    {
        public string Name { get; private set; }

        public ConfigurableOptionAttribute( string name )
        {
            this.Name = name;
        }
    }
}
