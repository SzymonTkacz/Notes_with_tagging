using Notes_with_tagging.Models;

namespace Notes_with_tagging.Services
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
