using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EDAST.Core {
    public interface IAddon {
        string Name { get; }
        bool Initialise(Manager manager, object conf);
        Task<object> SendMessageAsync<>(IAddon source, object data);
    }
}
