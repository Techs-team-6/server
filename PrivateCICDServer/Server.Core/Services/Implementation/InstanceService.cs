using System;
using System.Collections.Generic;
using System.Linq;
using Server.Core.Services.Abstraction;

namespace Server.Core.Services.Implementation
{
    public class InstanceService : IInstanceService
    {
        private readonly List<Instance> _instances;
        public InstanceService()
        {
            _instances = new List<Instance>();
        }
        public Guid Register(Guid tokenId, string description)
        {
            var id = Guid.NewGuid();
            var instance = new Instance(id, tokenId, description);
            _instances.Add(instance);
            return id;
        }
        public bool Auth(Guid id, Guid tokenId)
        {
            var instance = _instances.FirstOrDefault(x => x.Id == id);
            return instance is not null && instance.TokenId == tokenId;
        }
        public void EditDescription(Guid id, string newDescription)
        {
            var instance = _instances.FirstOrDefault(x => x.Id == id) ?? throw new ArgumentOutOfRangeException(nameof(id));
            instance.Description = newDescription;
        }
        public IReadOnlyCollection<Instance> List()
        {
            return _instances;
        }
    }
}