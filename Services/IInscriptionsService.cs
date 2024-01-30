using SellIntegro.Models;

namespace SellIntegro.Services
{
    public interface IInscriptionsService
    {
        Task<IEnumerable<Inscription>> GetInscriptions();
        Task<Inscription> GetSingleInscription(int id);
        Task AddInscription(Inscription inscription);
        Task UpdateInscription(Inscription inscription);
        Task DeleteInscription(Inscription inscription);
    }
}
