using System.Threading.Tasks;

namespace Ezley.EncyrptionKeyStore
{
    public interface IKeyStore
    {
        Task<Key> LoadKeyAsync(string streamId);
  
        Task SaveKeyAsync(string streamId, object key);
    }
}