using System;
using System.Collections.Generic;
using System.Linq;
using System.Service.ProcessService;
using System.Threading.Tasks;

namespace Core.Logics.Control
{
    public interface IPlayerControl
    {
        void CreateChar(string name);
        void SetChar(int playerId);
        void MoveForward();
        void MoveBack();
        void AddCommand(List<string> command);
        IPlayerBase MyCharDetail { get; }
        IStageBase CurrentMap { get; }
        ProcessServiceJsonMode MyStatus();
    }
}
