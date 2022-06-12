using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Math_Solver.Services
{
    public interface IUserMessage
    {
        Task LongAlert(string message);
        Task ShortAlert(string message);
        void ExitApp();
    }
}
