using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EDAST {
    partial class Program {
        static void Main(string[] args) =>
            new Program().MainAsync(args).Wait();

        async Task<bool> initAsync() {
            return true;
        }
    }
}
