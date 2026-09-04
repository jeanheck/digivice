using System.Text.Json.Serialization;
using Backend.Events.DTO.Interfaces;
using Backend.Events.DTO.Shared;

namespace Backend.Events.DTO;

public record class AuctionsDTO : IDTO
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<bool> DivineBarrier { get; init; } = Optional<bool>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<bool> HazardShield { get; init; } = Optional<bool>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<bool> SniperShield { get; init; } = Optional<bool>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<bool> DramonShield { get; init; } = Optional<bool>.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Optional<bool> YinYangWand { get; init; } = Optional<bool>.Empty;
}
