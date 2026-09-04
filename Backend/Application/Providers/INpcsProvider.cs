using Backend.Domain.Models;

namespace Backend.Application.Providers
{
    public interface INpcsProvider
    {
        Npcs Get();
    }
}
