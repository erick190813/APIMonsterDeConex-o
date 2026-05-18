# 🚀 PokeAPI - Backend Core de Integração (App 2)

> **"Arquitetura de microsserviços e sistemas distribuídos na prática!"**

Repositório oficial da Web API de processamento e persistência de dados desenvolvida para a Situação de Aprendizagem (SA) do curso Técnico em Desenvolvimento de Sistemas (SENAI).

---

# 📖 Visão Geral do Ecossistema

Este projeto não é uma aplicação isolada; ele é o coração (**Backend Core**) de um sistema integrado criado pela nossa Squad.

O objetivo principal do ecossistema é validar o fluxo contínuo de dados entre diferentes aplicações (WPF Desktop e Nuvem), garantindo **resiliência, segurança e alta disponibilidade**.

## 🧩 Estrutura do Sistema

O sistema completo é composto por três nós:

### 🖥️ App 1 — Coletor Desktop WPF
Consome a PokeAPI pública, filtra os dados brutos e realiza o envio (**Ingestão**) para o nosso Backend.

### ⚙️ App 2 — Web API .NET (Este Projeto)
Recebe o tráfego do App 1, valida a estrutura, injeta metadados (como data de coleta e IDs únicos) e persiste as informações num cluster NoSQL.

### 📊 App 3 — Dashboard Analítico WPF
Consome os relatórios consolidados desta API para renderizar gráficos e painéis gerenciais.

---

# 🛠️ Stack Tecnológico e Padrões

| Categoria | Tecnologia |
|---|---|
| **Backend** | C# com ASP.NET Core 8.0 |
| **Banco de Dados** | Google Cloud Firestore (NoSQL, Serverless) |
| **Conteinerização** | Docker (multi-stage builds) |
| **Infraestrutura** | Google Cloud Run (PaaS escalável) |
| **Documentação** | Swagger / OpenAPI 3.0 |

## 📐 Padrões de Projeto Utilizados

- Injeção de Dependência (DI)
- DTOs (Data Transfer Objects)
- Repository/Service Pattern
- Middlewares Globais

---

# ☁️ O Desafio Arquitetural (E a Solução)

Um dos maiores aprendizados deste projeto foi o **troubleshooting de infraestrutura real**.

Inicialmente, o deploy da API estava planejado para um servidor Windows com IIS numa hospedagem gratuita. Durante os testes de carga, enfrentamos:

- Gargalos de RAM
- Bloqueios de firewall
- Portas gRPC fechadas exigidas pelo Firebase
- Erros fatais HTTP 500.30 e 500.38

## 🚨 A Decisão

Para garantir a entrega da Squad com padrão de mercado, foi arquitetada uma migração imediata da aplicação.

A API foi:

- Conteinerizada com Docker
- Migrada para Google Cloud Run
- Adaptada para ambiente Serverless

## 🏆 Resultado

- Escalonamento automático de instâncias
- Resolução imediata de conectividade com o Firestore
- Respostas de endpoints em milissegundos
- Maior estabilidade e disponibilidade

---

# 📁 Estrutura do Projeto

A organização das pastas foi pensada para garantir escalabilidade e manutenção do código.

```plaintext
📦 PokeApiBackend
 ┣ 📂 Controllers   -> Controladores HTTP (Rotas e Status Codes)
 ┣ 📂 DTOs          -> Data Transfer Objects
 ┣ 📂 Interfaces    -> Contratos de abstração (IPokemonService)
 ┣ 📂 Middlewares   -> Interceptadores de requisições
 ┣ 📂 Services      -> Regras de negócio e Firestore
 ┣ 📜 Dockerfile    -> Empacotamento da aplicação
 ┣ 📜 Program.cs    -> Configuração da aplicação
 ┗ 📜 README.md     -> Documentação do projeto
