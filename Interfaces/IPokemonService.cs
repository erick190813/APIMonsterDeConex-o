using PokeApiBackend.DTOs;

namespace PokeApiBackend.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonResponseDto> ProcessAndSaveAsync(PokemonCreateDto data);
        Task<IEnumerable<PokemonResponseDto>> GetAllProcessedDataAsync();
    }
}