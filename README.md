# 🛒 E-Commerce Web API 

A high-performance, secure, and scalable Backend API built with **ASP.NET Core**. This project is a real-world application designed to master professional software engineering patterns and provide a complete e-commerce experience.

## 📖 Project Overview
This project manages the core functionalities of an online store, from product management to secure user authentication. It is built with a focus on **Clean Code**, **Performance**, and **Scalability**, applying the latest industry standards learned in the "Mastering ASP.NET Core" course.

---

## 🚀 Functional Features (What it does)
- **User Identity:** Secure Register/Login system with JWT Authentication.
- **Product Management:** Full CRUD operations for products, categories, and brands.
- **Shopping Cart:** Real-time management of user baskets.
- **Order Processing:** Managing customer orders and checkout workflows.
- **Advanced Search:** Filtering, sorting, and pagination for product catalogs.

---

## 🛠 Technical Stack & Implementation
This project showcases advanced backend development skills:

### 1. Core Architecture
- **ASP.NET Core Web API:** The foundation of the system.
- **Minimal APIs:** Used for high-performance and lightweight endpoints.
- **Dependency Injection:** Centralized service management for loose coupling.

### 2. Security & Performance
- **Identity & JWT:** Secure token-based authentication.
- **Refresh Tokens:** For maintaining user sessions securely.
- **Cancellation Tokens:** Optimized server resources by cancelling long-running tasks when a user disconnects.
- **CORS:** Configured for secure cross-origin communication.

### 3. Reliability & Validation
- **Global Exception Handling:** Centralized Middleware to handle errors gracefully.
- **Fluent Validation:** Advanced data validation to ensure data integrity.
- **RFC Standard:** Consistent error responses following global API standards.
- **Filters:** (Action, Exception, and Global) to manage cross-cutting concerns.

### 4. Documentation
- **Swagger (OpenAPI):** Interactive API documentation to explore and test endpoints easily.

---

## 🏗 Roadmap (Future Enhancements)
This project is continuously evolving. Upcoming features include:
- [ ] **Clean Architecture** refactoring.
- [ ] **MediatR & CQRS** for better command/query separation.
- [ ] **SignalR** for real-time order tracking.
- [ ] **Microservices** transition.

---

## 💻 Getting Started
1. **Clone the repo:** `git clone https://github.com/Gamal-Elnagar11/E-Commerce-API-Version3.git`
2. **Setup Database:** Update the connection string in `appsettings.json`.
3. **Run Migrations:** Execute `dotnet ef database update`.
4. **Launch:** Run `dotnet run` and navigate to `/swagger` to explore the API.

---
**Developed by [Gamal Saeed]** – *Committed to building better software, one commit at a time.*
