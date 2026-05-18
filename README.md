# 🚀 PokeAPI - Backend de Integração (App 2)

> **"Sistemas distribuídos na prática!"** > Repositório oficial da API de processamento de dados desenvolvida para a Situação de Aprendizagem (SA) do curso Técnico em Desenvolvimento de Sistemas.

## 📖 Sobre o Projeto

Este projeto é o backend de um sistema integrado criado pela nossa squad para validar o fluxo de informações entre diferentes tecnologias. O objetivo foi estabelecer uma comunicação contínua e segura entre três aplicações independentes.

Eu fiquei responsável por arquitetar e desenvolver o **App 2**, uma Web API RESTful robusta que atua como o núcleo do sistema:
1. Recebe os dados de Pokémons coletados pelo **App 1** (Aplicação Desktop).
2. Processa e persiste essas informações em tempo real num banco de dados NoSQL na nuvem.
3. Fornece relatórios estruturados em JSON para serem consumidos e exibidos no **App 3** (Painel de Análise).

## 🛠️ Tecnologias e Ferramentas Utilizadas

Durante o desenvolvimento, busquei aplicar padrões de mercado e tecnologias modernas:

* **C# & .NET 8.0:** Framework principal para a construção da Web API.
* **Google Cloud Firestore:** Banco de dados NoSQL em tempo real para persistência dos dados.
* **Docker:** Para conteinerização da aplicação, garantindo o padrão "funciona na minha máquina e no servidor".
* **Google Cloud Run (Serverless):** Plataforma de hospedagem em nuvem de alta disponibilidade.
* **Swagger (OpenAPI):** Para documentação interativa e testes diretos dos endpoints.

## ☁️ Decisão Arquitetural: A Migração de Infraestrutura

Como estudante, um dos maiores aprendizados desta SA foi lidar com ambientes reais de produção. 
Inicialmente, o deploy estava planejado para uma hospedagem gratuita em servidor Windows IIS. No entanto, após realizar o *troubleshooting* de diversos erros de execução (500.31, 500.38, 500.30), diagnostiquei que as limitações físicas da plataforma (como restrição drástica de RAM) e o bloqueio de portas gRPC do firewall inviabilizavam a conexão segura com o Firebase.

Para garantir a **resiliência, escalabilidade e a entrega do projeto da squad**, tomei a decisão técnica de pivotar a hospedagem. Migrei o núcleo da API para contêineres rodando no **Google Cloud Run**. O resultado foi uma API respondendo em milissegundos, com alta disponibilidade e sem gargalos na integração! 🏆

## 🔗 Endpoints da API

A API foi documentada usando o Swagger. A interface interativa pode ser acessada adicionando `/swagger` à URL base da hospedagem no Google Cloud.

Principais rotas disponíveis:

* `GET /api/PokemonData/ping`: Rota de *Health Check* para verificar o status do servidor.
* `POST /api/PokemonData`: Rota utilizada pelo **App 1** para enviar o payload JSON com os dados do Pokémon recém-coletado.
* `GET /api/PokemonData/relatorios`: Rota utilizada pelo **App 3** para buscar todo o histórico de registros salvos no banco de dados.

## 🚀 Como rodar o projeto localmente

Para testar o projeto no seu ambiente de desenvolvimento, siga os passos:

1. Clone este repositório:
   ```bash
   git clone [https://github.com/SEU-USUARIO/Sua-Repo-PokeAPI.git](https://github.com/SEU-USUARIO/Sua-Repo-PokeAPI.git)