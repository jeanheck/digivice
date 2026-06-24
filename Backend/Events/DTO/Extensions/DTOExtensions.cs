using Backend.Events.DTO.Interfaces;

namespace Backend.Events.DTO.Extensions;

public static class DTOExtensions
{
    public static bool IsEmpty<T>(this T dto) where T : IDTO, new()
    {
        return EqualityComparer<T>.Default.Equals(dto, new T());
    }

    public static bool IsNotEmpty<T>(this T dto) where T : IDTO, new()
    {
        return !dto.IsEmpty();
    }
}
