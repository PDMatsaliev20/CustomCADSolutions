using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Core.Contracts
{
    public interface IDeepAIService
    {
        public Task<string> GenerateImage(string description);
    }
}
