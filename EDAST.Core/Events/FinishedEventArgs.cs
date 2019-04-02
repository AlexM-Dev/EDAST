using EDAST.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Core.Events {
    public class FinishedEventArgs : EventArgs {
        public AddressResult[] Results { get; }
        public FinishedEventArgs(AddressResult[] results) {
            this.Results = results;
        }
    }
}
