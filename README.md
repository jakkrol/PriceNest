# PriceNest

**PriceNest** is an intelligent price tracking and analytics system designed to monitor product price changes across various e-commerce platforms. The project features an **integrated AI Agent** capable of analyzing data and interacting with users in natural language. The project is currently **in active development**, focusing on building a robust microservice architecture.

> **Status:** 🏗️ In Development (AI Agent & Backend integration)

---

## 🏗️ System Architecture

The project follows a distributed model to ensure process isolation and high scalability:

* **Backend & AI Orchestrator:** ASP.NET Core Web API – The central hub responsible for business logic, authentication, and data management. It uses **Semantic Kernel** to orchestrate an **AI Agent** that can autonomously call system services.
* **Scraper Service:** Node.js + Playwright – A dedicated microservice for data extraction, capable of rendering JavaScript and bypassing basic anti-bot mechanisms.
* **Database:** PostgreSQL – Stores structured price history, product relations, and user configurations.
* **Frontend:** Next.js (Planned) – A dashboard featuring interactive price trend charts.

---

## 🛠️ Tech Stack

| Layer | Technology | Status |
| :--- | :--- | :--- |
| **AI / LLM** | **OpenAI (GPT-4o-mini) / Semantic Kernel** | 🟢 Integrated |
| **Backend** | .NET 9 / ASP.NET Core | 🟡 Under Construction |
| **Scraper** | Node.js / Playwright | 🟡 Basic Functionality |
| **Database** | PostgreSQL | 🟡 Schema Design |
| **Frontend** | Next.js (React) | ⚪ Planned |

---

## 🚀 Key Features (Roadmap)

- [x] Initialization of the Scraping Microservice (Node.js).
- [x] Integration of Playwright (Headless Browser) engine.
- [x] **AI Agent Integration:** Natural language querying of the database via Semantic Kernel.
- [ ] Implementation of advanced ASP.NET Core Web API endpoints.
- [ ] Microservice orchestration and PostgreSQL integration.
- [ ] Automated task scheduling (daily price checks).
- [ ] Next.js Dashboard with price history visualization.

---

## ⚙️ Local Setup (Development)

### 1. AI Agent Configuration
To use the AI features, you need to provide an OpenAI API Key using .NET User Secrets:
```bash
cd src/PriceNest.Api
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "your-api-key-here"
