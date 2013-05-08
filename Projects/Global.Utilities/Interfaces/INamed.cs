using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Utilities.Interfaces
{
    public interface INamed
    {
        /// <summary>
        /// The item's name.
        /// </summary>
        string Name { get; set; }
    }
}