using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Core.Data {
    /// <summary>
    /// Provides the base for each website to check.
    /// This type is the heart of the program, being dynamic.
    /// </summary>
    public class Address {
        /// <summary>
        /// The base address Location to use for processing.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// The minimum state in order to process as "down".
        /// </summary>
        public State StateSensitivity { get; set; }

        /// <summary>
        /// All parameters as processed by each appropriate addon.
        /// </summary>
        public dynamic Parameters { get; set; }
    }
}
