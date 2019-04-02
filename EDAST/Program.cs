using EDAST.Core;
using System;
using System.Threading.Tasks;

namespace EDAST {
    partial class Program {
        static async Task MainAsync(string[] args) {
            if (!await initAsync()) {
                // err.
            }
        }
    }
}
