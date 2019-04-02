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
        /// The base address Url to use for processing.
        /// </summary>
        [JsonProperty("URL")]
        public string Url { get; }

        /// <summary>
        /// All parameters as processed by each appropriate addon.
        /// </summary>
        public dynamic Parameters { get; }
    }
}
