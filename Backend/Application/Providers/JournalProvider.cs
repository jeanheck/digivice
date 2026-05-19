using Backend.Domain.Models;
using Backend.Domain.Assemblers;
using Backend.Application.Loaders;

namespace Backend.Application.Providers
{
    public class JournalProvider(IJournalLoader journalLoader) : IJournalProvider
    {
        public Journal Get()
        {
            return JournalAssembler.Assemble(journalLoader.Load());
        }
    }
}
