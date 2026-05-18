using PokeApiBackend.DTOs;
using PokeApiBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PokeApiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonDataController : ControllerBase
    {
        private readonly IPokemonService _service;

        public PokemonDataController(IPokemonService service)
        {
            _service = service;
        }

        /// <summary>
        /// Realiza a ingestão de dados brutos filtrados originados do App 1 (WPF).
        /// </summary>
        /// <param name="data">Payload contendo os atributos mapeados da PokeAPI.</param>
        /// <returns>Objeto contendo o ID gerado pelo barramento da PokeApi Backend.</returns>
        /// <response code="201">Sucesso. Retorna o contrato completo processado e pronto para exibição.</response>
        /// <response code="400">Falha estrutural. O payload enviado pelas ViewModels do App 1 está nulo ou corrompido.</response>
        /// <remarks>
        /// Exemplo estrutural de requisição assíncrona (POST) aplicável no HttpClient do WPF:
        /// 
        ///     POST /api/PokemonData/coleta
        ///     {
        ///        "altura": 1.7,
        ///        "peso": 90.5,
        ///        "tipos": ["Fire", "Flying"],
        ///        "spriteUrl": "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/6.png",
        ///        "hp": 78,
        ///        "attack": 84,
        ///        "defense": 78,
        ///        "spAttack": 109,
        ///        "spDefense": 85,
        ///        "speed": 100,
        ///        "baseStatTotal": 534,
        ///        "competitiveRole": "Wallbreaker"
        ///     }
        /// </remarks>
        [HttpPost("coleta")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PokemonResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReceiveData([FromBody] PokemonCreateDto data)
        {
            if (data == null)
                return BadRequest(new { error = "O payload JSON enviado não pode ser processado como uma entidade válida." });

            var result = await _service.ProcessAndSaveAsync(data);
            return CreatedAtAction(nameof(GetDataForDashboard), new { id = result.Id }, result);
        }

        /// <summary>
        /// Recupera a listagem agregada de registros para alimentação dos Dashboards analíticos do App 3 (WPF).
        /// </summary>
        /// <returns>Coleção estruturada de objetos de auditoria global.</returns>
        /// <response code="200">Sucesso. Retorna a coleção de dados recuperados do cluster NoSQL cloud.</response>
        /// <response code="204">Sem conteúdo. A consulta foi executada com sucesso, mas o banco de dados está vazio.</response>
        [HttpGet("relatorios")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PokemonResponseDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetDataForDashboard()
        {
            var results = await _service.GetAllProcessedDataAsync();
            if (results == null || !results.Any())
                return NoContent();

            return Ok(results);
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new
            {
                mensagem = "PokeApi Backend a funcionar perfeitamente!",
                status = 200,
                ambiente = "Google Cloud Run (Serverless)"
            });
        }
    }
}