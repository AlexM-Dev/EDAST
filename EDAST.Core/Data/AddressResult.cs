using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Core.Data {
    public class AddressResult {
        public Address Address { get; }
        public Dictionary<string, State> States { get; }
        public bool ProcessErrorOccurred { get; set; }

        public AddressResult(Address addr) {
            this.Address = addr;
            this.States = new Dictionary<string, State>();
        }

        public bool IsAddressUp() {
            var sensitivity = Address.StateSensitivity;

            foreach (var state in States) {
                if (state.Value >= sensitivity)
                    return false;
            }

            return true;
        }

        public static AddressResult Merge(params AddressResult[] results) {
            Address addr = null;
            AddressResult addrResult = null;

            foreach (var result in results) {
                if (addr == null) {
                    addr = result.Address;
                    addrResult = new AddressResult(addr);
                } else if (addr != result.Address) return null;

                foreach (var s in result.States) 
                    addrResult.States.Add(s.Key, s.Value);
                
                if (result.ProcessErrorOccurred)
                    addrResult.ProcessErrorOccurred = true;
            }

            return addrResult;
        }
    }
}
