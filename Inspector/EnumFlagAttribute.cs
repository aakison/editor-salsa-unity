using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Relentless {

    [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag")]
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class EnumFlagAttribute : PropertyAttribute {

        public EnumFlagAttribute() { }

        public EnumFlagAttribute(string enumName) {
            EnumName = enumName;
        }

        public string EnumName { get; private set; }

    }

}
