using System;
using System.Linq;
using System.Threading.Tasks;
using Ezley.Domain;
using Ezley.EncyrptionKeyStore;
using Ezley.EventSourcing;
using Ezley.EventStore;

namespace ES.Domain
{
    public interface IRepository
    {
        Task<T> Load<T>(Guid id)
            where T : AggregateBase;

        Task<bool> Save<T>(EventUserInfo eventUserInfo, T aggregate)
            where T : AggregateBase;
        Task<AesKeyInfo> LoadKeyInfoAsync(string streamId); 
        byte[] LoadKeyAsync(string streamId);
        Task<bool> SaveKeyInfo(AesKeyInfo key);
    }

    public class Repository : IRepository
    {
        private readonly IEventStore _eventStore;
        private readonly IKeyStore _keyStore;
        public Repository(IEventStore eventStore, IKeyStore keyStore)
        {
            _eventStore = eventStore;
            _keyStore = keyStore;
        }

        public async Task<T> Load<T>(Guid id)
            where T : AggregateBase
        {
            var stream = await _eventStore.LoadStreamAsyncOrThrowNotFound(id.ToString());
            return (T) Activator.CreateInstance(typeof(T), stream.Events);
        }

        public async Task<bool> Save<T>(EventUserInfo eventUserInfo, T aggregate)
            where T : AggregateBase
        {
            if (eventUserInfo == null)
            {
                throw new ApplicationException("EventUserInfo was expected but not provided.");
            }
            if (!aggregate.Changes.Any())
                return true;

            var streamId = aggregate.Id;
            // save all events
            bool savedEvents = await _eventStore.AppendToStreamAsync(eventUserInfo, streamId,
                aggregate.Version,
                aggregate.Changes);

            return savedEvents;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Null if key is not found.</returns>
        public async Task<AesKeyInfo> LoadKeyInfoAsync(string id)
        {
            var streamId =  $"KeyInfo:{id.ToString()}";
            var key = await _keyStore.LoadKeyAsync(streamId);

            return key?.KeyData.ToObject<AesKeyInfo>();
        }

        public byte[] LoadKeyAsync(string id)
        {
            var keyData = LoadKeyInfoAsync(id).GetAwaiter().GetResult();
            return keyData.Key;
        }
        public async Task<bool> SaveKeyInfo(AesKeyInfo key)
        {
            var streamId = key.Id;
            await _keyStore
                .SaveKeyAsync(streamId, key);
            return true;
        }
    }
}