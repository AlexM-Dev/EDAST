using EDAST.Core;
using System;
using System.Threading.Tasks;

namespace EDAST {
    partial class Program {
        async Task MainAsync(string[] args) {
            if (!await initAsync()) {
                // err.
            }
        }
    }
}
