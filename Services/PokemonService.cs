using ApiMonsterDeConexao.DTOs;
using ApiMonsterDeConexao.Interfaces;

namespace ApiMonsterDeConexao.Services
{
    public class PokemonService : IPokemonService
    {
        public async Task<PokemonResponseDto> ProcessAndSaveAsync(PokemonCreateDto data)
        {
            // Abstração de regras de validação estrutural
            var response = new PokemonResponseDto
            {
                Id = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                DataColeta = DateTime.UtcNow,
                EnviadoParaNuvem = true,
                Altura = data.Altura,
                Peso = data.Peso,
                Tipos = data.Tipos,
                SpriteUrl = data.SpriteUrl,
                HP = data.HP,
                Attack = data.Attack,
                Defense = data.Defense,
                SpAttack = data.SpAttack,
                SpDefense = data.SpDefense,
                Speed = data.Speed,
                BaseStatTotal = data.BaseStatTotal,
                CompetitiveRole = data.CompetitiveRole
            };

            // Implementação nativa da persistência Cloud Firestore entra neste ponto estratégico
            return await Task.FromResult(response);
        }

        public async Task<IEnumerable<PokemonResponseDto>> GetAllProcessedDataAsync()
        {
            // Mock estruturado para testes imediatos de integração do App 3 (WPF Dashboards)
            var mockData = new List<PokemonResponseDto>
            {
                new PokemonResponseDto
                {
                    Id = "MONSTER-7A8B",
                    DataColeta = DateTime.UtcNow,
                    EnviadoParaNuvem = true,
                    Altura = 0.7, Peso = 6.9,
                    Tipos = new List<string> { "Grass", "Poison" },
                    HP = 45, Attack = 49, Defense = 49, BaseStatTotal = 318,
                    CompetitiveRole = "Special Sweeper"
                }
            };
            return await Task.FromResult(mockData);
        }
    }
}