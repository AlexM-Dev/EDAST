using EDAST.Core;
using EDAST.Core.Data;
using System;
using System.Threading.Tasks;

namespace EDAST {
    partial class Program {
        async Task MainAsync(string[] args) {
            if (!await initAsync()) {
                // err.
            }
            Console.WriteLine("Yo");

            await Task.Delay(-1);
        }
    }
}
