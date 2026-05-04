# PriceNest

**PriceNest** is an intelligent price tracking and analytics system designed to monitor product price changes across various e-commerce platforms. The project is currently **in active development**, focusing on building a robust microservice architecture.

> **Status:** 🏗️ In Development (Backend & Scraper core)

---

## 🏗️ System Architecture

The project follows a distributed model to ensure process isolation and high scalability:

*   **Backend:** ASP.NET Core Web API – The central hub responsible for business logic, authentication, and data management.
*   **Scraper Service:** Node.js + Playwright – A dedicated microservice for data extraction, capable of rendering JavaScript and bypassing basic anti-bot mechanisms.
*   **Database:** PostgreSQL – Stores structured price history, product relations, and user configurations.
*   **Frontend:** Next.js (Planned) – A dashboard featuring interactive price trend charts.

---

## 🛠️ Tech Stack

| Layer | Technology | Status |
| :--- | :--- | :--- |
| **Backend** | .NET 8 / ASP.NET Core | 🟡 Under Construction |
| **Scraper** | Node.js / Playwright | 🟡 Basic Functionality |
| **Database** | PostgreSQL | 🟡 Schema Design |
| **Frontend** | Next.js (React) | ⚪ Planned |

---

## 🚀 Key Features (Roadmap)

- [x] Initialization of the Scraping Microservice (Node.js).
- [x] Integration of Playwright (Headless Browser) engine.
- [ ] Implementation of ASP.NET Core Web API endpoints.
- [ ] Microservice orchestration and PostgreSQL integration.
- [ ] Automated task scheduling (daily price checks).
- [ ] Next.js Dashboard with price history visualization.

---

## ⚙️ Local Setup (Development)

### 1. Scraper (Node.js)
Requires Node.js v18+.
```bash
cd src/scraper
npm install
npx playwright install chromium
node index.js
