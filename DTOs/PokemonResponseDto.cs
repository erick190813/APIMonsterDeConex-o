namespace ApiMonsterDeConexao.DTOs
{
    public class PokemonResponseDto : PokemonCreateDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTime DataColeta { get; set; }
        public bool EnviadoParaNuvem { get; set; }
    }
}