using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.JobServer.Abstractions.Jobs
{
    public interface IInitializeOnStartup
    {
        void InitializeOnStartup();
    }
}
