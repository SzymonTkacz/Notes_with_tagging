using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SellIntegro.Data;
using SellIntegro.Models;
using System.Text.RegularExpressions;

namespace SellIntegro.Services
{
    public class InscriptionsService: IInscriptionsService
    {
        private readonly AppDbContext _context;
        public InscriptionsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddInscription(Inscription inscription)
        {
            AssignTags(inscription);
            _context.Inscriptions.Add(inscription);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inscription>> GetInscriptions()
        {
            return await _context.Inscriptions.ToListAsync();
        }

        public async Task<Inscription?> GetSingleInscription(int id)
        {
            return await _context.Inscriptions.FindAsync(id);
        }

        public async Task UpdateInscription(Inscription inscription)
        {
            AssignTags(inscription);
            _context.Entry(inscription).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteInscription(Inscription inscription)
        {
            _context.Inscriptions.Remove(inscription);
            await _context.SaveChangesAsync();
        }

        private void AssignTags(Inscription inscription)
        {
            const string MatchPhonePattern = @"[\s]+(\d{9})+[\s.]"; // 9 digits next to space and previous space or dot
            const string MatchEmailPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"; // common email pattern

            var tags = new List<string>();

            if (Regex.IsMatch(inscription.Text, MatchPhonePattern)) 
            {
                tags.Add("PHONE");
            }

            if (Regex.IsMatch(inscription.Text, MatchEmailPattern))
            {
                tags.Add("EMAIL");
            }

            if(tags.Any())
            {
                inscription.Tags = tags.ToArray();
            }            
            else
            {
                inscription.Tags = null;
            }
        }
    }
}
