using EDAST.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDAST.Core.Events {
    public class CheckingEventArgs : EventArgs {
        public AddressResult Result { get; }
        public CheckingEventArgs(AddressResult result) {
            this.Result = result;
        }
    }
}
