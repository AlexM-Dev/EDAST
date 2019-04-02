using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Core.Data {
    public class AddressResult {
        public Address Address { get; }
        public Dictionary<string, object> Failures { get; }

        public AddressResult(Address addr) {
            this.Address = addr;
            this.Failures = new Dictionary<string, object>();
        }
    }
}
