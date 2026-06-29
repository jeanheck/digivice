using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public interface IJournalProvider
    {
        Journal Get();
    }
}
