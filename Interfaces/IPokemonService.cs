using ApiMonsterDeConexao.DTOs;

namespace ApiMonsterDeConexao.Interfaces
{
    public interface IPokemonService
    {
        Task<PokemonResponseDto> ProcessAndSaveAsync(PokemonCreateDto data);
        Task<IEnumerable<PokemonResponseDto>> GetAllProcessedDataAsync();
    }
}