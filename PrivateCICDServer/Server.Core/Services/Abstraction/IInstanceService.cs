using System;
using System.Collections.Generic;

namespace Server.Core.Services.Abstraction
{
    public interface IInstanceService
    {
        Guid Register(Guid tokenId, string description);
        bool Auth(Guid id, Guid tokenId);
        void EditDescription(Guid id, string newDescription);
        IReadOnlyCollection<Instance> List();
    }
}