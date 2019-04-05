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

        public AddressResult Merge(params AddressResult[] results) {
            Address addr = null;
            AddressResult addrResult = null;

            foreach (var result in results) {
                if (addr == null) {
                    addr = result.Address;
                    addrResult = new AddressResult(addr);
                } else if (addr != result.Address) return null;

                addrResult.Failures.AddRange(result.Failures);
            }

            return addrResult;
        }
    }
}
