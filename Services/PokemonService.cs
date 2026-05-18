using ApiMonsterDeConexao.DTOs;
using ApiMonsterDeConexao.Interfaces;
using Google.Cloud.Firestore;

namespace ApiMonsterDeConexao.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly FirestoreDb _db;

        private const string ProjectId = "apimonsterdeconexao";

        public PokemonService()
        {
            try
            {
                string diretorioBase = AppDomain.CurrentDomain.BaseDirectory;
                string caminhoJson = System.IO.Path.Combine(diretorioBase, "apimonsterdeconexao-firebase-adminsdk-fbsvc-77bdc406a2.json");

                if (System.IO.File.Exists(caminhoJson))
                {
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", caminhoJson);
                }

                _db = FirestoreDb.Create(ProjectId); // O erro fatal acontece aqui no MonsterASP
            }
            catch (Exception ex)
            {
                // Se der erro (como no MonsterASP), a API engole a exceção e continua ligando!
                Console.WriteLine($"Bypass de Firebase ativado. Motivo: {ex.Message}");
            }
        }
        public async Task<PokemonResponseDto> ProcessAndSaveAsync(PokemonCreateDto data)
        {
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

            // Criando o documento NoSQL como um Dicionário (Evita "sujar" o DTO com atributos de banco)
            var pokemonDocument = new Dictionary<string, object>
            {
                { "Id", response.Id },
                { "DataColeta", Timestamp.FromDateTime(response.DataColeta) }, // O Firebase usa um tipo próprio de data
                { "Altura", response.Altura },
                { "Peso", response.Peso },
                { "Tipos", response.Tipos },
                { "SpriteUrl", response.SpriteUrl },
                { "HP", response.HP },
                { "Attack", response.Attack },
                { "Defense", response.Defense },
                { "SpAttack", response.SpAttack },
                { "SpDefense", response.SpDefense },
                { "Speed", response.Speed },
                { "BaseStatTotal", response.BaseStatTotal },
                { "CompetitiveRole", response.CompetitiveRole }
            };

            // Salva na coleção "PokemonsProcessados"
            await _db.Collection("PokemonsProcessados").Document(response.Id).SetAsync(pokemonDocument);

            return response;
        }

        public async Task<IEnumerable<PokemonResponseDto>> GetAllProcessedDataAsync()
        {
            // Busca todos os documentos da coleção no Firebase
            var snapshot = await _db.Collection("PokemonsProcessados").GetSnapshotAsync();
            var lista = new List<PokemonResponseDto>();

            foreach (var document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    var dict = document.ToDictionary();
                    lista.Add(new PokemonResponseDto
                    {
                        Id = dict["Id"].ToString(),
                        DataColeta = ((Timestamp)dict["DataColeta"]).ToDateTime(),
                        EnviadoParaNuvem = true,
                        Altura = Convert.ToDouble(dict["Altura"]),
                        Peso = Convert.ToDouble(dict["Peso"]),
                        // O Firestore salva listas como 'List<object>', precisamos converter de volta para string
                        Tipos = ((List<object>)dict["Tipos"]).Select(x => x.ToString()).ToList(),
                        SpriteUrl = dict["SpriteUrl"].ToString(),
                        HP = Convert.ToInt32(dict["HP"]),
                        Attack = Convert.ToInt32(dict["Attack"]),
                        Defense = Convert.ToInt32(dict["Defense"]),
                        SpAttack = Convert.ToInt32(dict["SpAttack"]),
                        SpDefense = Convert.ToInt32(dict["SpDefense"]),
                        Speed = Convert.ToInt32(dict["Speed"]),
                        BaseStatTotal = Convert.ToInt32(dict["BaseStatTotal"]),
                        CompetitiveRole = dict["CompetitiveRole"].ToString()
                    });
                }
            }

            return lista;
        }
    }
}