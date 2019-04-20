using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Core.Data {
    public enum State {
        /// <summary>
        /// The issue arisen is just information; i.e. harmless.
        /// </summary>
        Information = 0,
        /// <summary>
        /// An issue has arisen, but isn't critical.
        /// </summary>
        NonCritical = 1,
        /// <summary>
        /// A critical issue has arisen and should be cause for concern.
        /// </summary>
        Critical = 2
    }
}
