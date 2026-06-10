# E-Commerce Web API - Version 2

A high-performance, secure, and scalable Backend API built with **ASP.NET Core 8.0**. This project is a professional-grade application designed to master advanced software engineering patterns and provide a complete e-commerce experience.

## 📖 Project Overview
This project manages the core functionalities of an online store, from product and order management to secure user authentication. It is built with a focus on **Clean Code**, **Performance**, and **Scalability**, applying the latest industry standards.

## 🚀 Key Functional Features
* **User Identity:** Secure Register/Login system with JWT Authentication & Refresh Tokens.
* **Product Management:** Full CRUD operations for products, categories, and brands.
* **Shopping Cart:** Real-time management of user baskets with "Out of Stock" handling.
* **Admin Order Management:** Complete control over the order lifecycle:
    * Real-time order tracking and history.
    * Status transitions (Pending, Shipped, Delivered).
    * Payment status synchronization (Paid/Not Paid).
* **Advanced Search:** Filtering, sorting, and pagination for product catalogs.

## 🛠 Technical Stack & Implementation
This project showcases advanced backend development skills:

### 1. Core Architecture
* **ASP.NET Core Web API:** The robust foundation of the system.
* **Minimal APIs:** Used for high-performance and lightweight endpoints organized in logical groups.
* **Dependency Injection:** Centralized service management for loose coupling.
* **Repository & Unit of Work:** Professional patterns for database abstraction.

### 2. Security & Performance
* **Authentication & Authorization:** Secure token-based access with JWT.
* **Refresh Tokens:** Maintaining user sessions securely.
* **Cancellation Tokens:** Optimized server resources by cancelling long-running tasks.
* **CORS:** Configured for secure cross-origin communication.

### 3. Reliability & Validation
* **Global Exception Handling:** Centralized Middleware following **RFC 7807** standards.
* **Fluent Validation:** Advanced data validation to ensure data integrity.
* **Filters:** Extensive use of Action, Exception, and Global Filters to manage cross-cutting concerns efficiently.

### 4. Documentation
* **Swagger (OpenAPI):** Interactive API documentation to explore and test endpoints easily.



## 🏗 Roadmap
This project is continuously evolving. Upcoming features include:
* Clean Architecture refactoring.
* MediatR & CQRS for better command/query separation.
* SignalR for real-time order tracking.
* Microservices transition.

## 💻 Getting Started
1. **Clone the repo:** `git clone https://github.com/Gamal-Elnagar11/E-Commerce-API-Version2.git`
2. **Setup Database:** Update the connection string in `appsettings.json`.
3. **Run Migrations:** Execute `dotnet ef database update`.
4. **Launch:** Run `dotnet run` and navigate to `/swagger` to explore the API.

---
*Developed as a high-end production-ready backend, focusing on Clean Code principles and system performance.*
