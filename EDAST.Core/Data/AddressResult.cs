using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Core.Data {
    public class AddressResult {
        public Address Address { get; }
        public List<string> Failures { get; }
        public AddressResult(Address addr) {
            this.Address = addr;
            this.Failures = new List<string>();
        }
    }
}
