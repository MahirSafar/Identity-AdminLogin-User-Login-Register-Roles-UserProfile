using Pustok.App.DAL.Context;
using Pustok.App.Models;

namespace Pustok.App.Services
{
    public class LayoutService(PustokDbContext pustokDbContext)
    {
        private readonly PustokDbContext _context = pustokDbContext;
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
