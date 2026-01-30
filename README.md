# PetStore - Microservices E-Commerce Platform

A modern, distributed e-commerce platform built with .NET 10, demonstrating cloud-native microservices architecture patterns including event-driven communication, domain-driven design, and saga-based distributed transactions.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Services](#services)
- [Technology Stack](#technology-stack)
- [Prerequisites](#prerequisites)
- [Setup & Installation](#setup--installation)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## 🎯 Overview

PetStore is a comprehensive microservices demonstration platform that showcases best practices in distributed system design. It implements a complete e-commerce workflow including:

- **Product Catalog Management** - Browse and manage product inventory
- **Shopping Basket** - Add/remove items with Redis-backed persistence
- **Order Processing** - Complex order orchestration with payment and stock management
- **Identity & Security** - OAuth 2.0 and JWT-based authentication
- **Payment Processing** - Integrated payment handling
- **Event-Driven Integration** - Asynchronous communication between services via RabbitMQ

### Target Audience

- Developers learning microservices architecture patterns
- Teams evaluating .NET-based distributed systems
- Organizations building scalable e-commerce platforms

## 🏗️ Architecture

### High-Level Design

```
┌─────────────────────────────────────────────────────────────────┐
│                    API Gateway / Client                         │
└─────────────────────────────────────────────────────────────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        │                     │                     │
   ┌────▼─────┐        ┌─────▼────┐        ┌──────▼──────┐
   │ Basket    │        │ Catalog  │        │  Identity   │
   │ Service   │        │ Service  │        │  Service    │
   │ (Redis)   │        │ (PgSQL)  │        │  (PgSQL)    │
   └────┬─────┘        └─────┬────┘        └──────┬──────┘
        │                     │                     │
        └─────────────────────┼─────────────────────┘
                              │
                    ┌─────────▼──────────┐
                    │   RabbitMQ         │
                    │   Event Bus        │
                    └─────────┬──────────┘
                              │
        ┌─────────────────────┼─────────────────────┐
        │                     │                     │
   ┌────▼──────────┐    ┌─────▼────┐        ┌──────▼──────┐
   │ Ordering      │    │ Payment  │        │ OrderProc   │
   │ Service       │    │ Service  │        │ (Background)│
   │ (PgSQL/Saga)  │    │          │        │             │
   └───────────────┘    └──────────┘        └─────────────┘
```

### Key Architectural Patterns

1. **Microservices Architecture** - Each service owns its data and business logic
2. **Event-Driven Architecture** - RabbitMQ enables async communication and eventual consistency
3. **Saga Pattern** - OrderProcessor manages distributed transactions across services
4. **Domain-Driven Design (DDD)** - Clear domain models in Ordering service with aggregates and domain events
5. **CQRS Pattern** - Command/Query separation using MediatR in Ordering service
6. **Service Discovery & Configuration** - .NET Aspire orchestration

## 🔧 Services

### Core Services

| Service            | Purpose                                        | Tech Stack                                       | Database   |
| ------------------ | ---------------------------------------------- | ------------------------------------------------ | ---------- |
| **Basket.API**     | Shopping cart management, item persistence     | ASP.NET Core, gRPC                               | Redis      |
| **Catalog.API**    | Product information, inventory management      | ASP.NET Core, EF Core, AutoMapper                | PostgreSQL |
| **Ordering.API**   | Order creation, orchestration, CQRS            | ASP.NET Core, EF Core, MediatR, FluentValidation | PostgreSQL |
| **Identity.API**   | Authentication, authorization, JWT tokens      | ASP.NET Core Identity, IdentityServer            | PostgreSQL |
| **Payment.API**    | Payment processing and authorization           | ASP.NET Core                                     | PostgreSQL |
| **OrderProcessor** | Background job: grace period, event processing | Hosted Service, RabbitMQ consumer                | PostgreSQL |

### Shared Projects

| Project                      | Purpose                                               |
| ---------------------------- | ----------------------------------------------------- |
| **EventBus**                 | Abstract event interfaces and abstractions            |
| **EventBus.RabbitMQ**        | RabbitMQ implementation of event bus                  |
| **IntegrationEventLogEF**    | Outbox pattern implementation for reliable publishing |
| **PetStore.ServiceDefaults** | Common configuration and middleware                   |

## 💾 Technology Stack

### Core Framework

- **.NET 10** - Latest .NET runtime
- **ASP.NET Core** - Web framework for all services

### Data Access

- **Entity Framework Core** - ORM with PostgreSQL provider (Npgsql)
- **PostgreSQL** - Primary relational database
- **Redis** - Cache and session storage

### Communication & Messaging

- **RabbitMQ** - Event bus for inter-service communication
- **gRPC** - Synchronous inter-service communication (Basket)
- **OpenAPI/Swagger** - REST API documentation

### Domain & Application Patterns

- **MediatR** - CQRS command/query dispatcher
- **FluentValidation** - Request validation
- **AutoMapper** - Object-to-object mapping

### Identity & Security

- **IdentityServer** - OAuth 2.0 / OpenID Connect provider
- **ASP.NET Core Identity** - User and role management
- **JWT** - Stateless authentication tokens

### Cloud-Native & Observability

- **.NET Aspire** - Orchestration and cloud-native stack
- **OpenTelemetry** - Distributed tracing and metrics
- **Activity Extensions** - Request correlation

## 📋 Prerequisites

### Required Software

- **.NET 10 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Docker & Docker Compose** - For PostgreSQL, Redis, and RabbitMQ
- **Git** - Version control
- **Visual Studio Code** or **Visual Studio 2022+** - Code editor

### Recommended Tools

- **Postman** or **Thunder Client** - API testing
- **pgAdmin** - PostgreSQL administration (optional)
- **.NET Aspire Dashboard** - Local monitoring and diagnostics

## 🚀 Setup & Installation

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/petstore.git
cd PetStore
```

### 2. Start Infrastructure Services

Using Docker Compose to run PostgreSQL, Redis, and RabbitMQ:

```bash
docker-compose up -d
```

**Services will be available at:**

- PostgreSQL: `localhost:5432`
- Redis: `localhost:6379`
- RabbitMQ: `localhost:5672` (AMQP), `localhost:15672` (Management UI)

### 3. Create and Migrate Databases

Navigate to the solution root and run:

```bash
cd src

# Restore all NuGet packages
dotnet restore

# Apply Entity Framework Core migrations
dotnet ef database update --project ./Catalog.API
dotnet ef database update --project ./Ordering.API
dotnet ef database update --project ./Identity.API
dotnet ef database update --project ./Payment.API
```

### 4. Configure Environment Variables

Create `.env` files in each service directory with necessary configurations:

```env
# Catalog.API/.env
ConnectionStrings__catalogdb=Host=localhost;Port=5432;Database=catalogdb;Username=postgres;Password=yourpassword

# Ordering.API/.env
ConnectionStrings__orderingdb=Host=localhost;Port=5432;Database=orderingdb;Username=postgres;Password=yourpassword

# Basket.API/.env
ConnectionStrings__redis=localhost:6379

# Identity.API/.env
ConnectionStrings__identitydb=Host=localhost;Port=5432;Database=identitydb;Username=postgres;Password=yourpassword
```

## 🎮 Running the Application

### Option 1: Using .NET Aspire (Recommended)

```bash
cd src/PetStore.AppHost
dotnet run
```

This launches the Aspire dashboard at `http://localhost:8080` with all services orchestrated and monitored.

### Option 2: Run Individual Services

From the solution root:

```bash
# Terminal 1: Catalog.API
dotnet run --project src/Catalog.API

# Terminal 2: Ordering.API
dotnet run --project src/Ordering.API

# Terminal 3: Basket.API
dotnet run --project src/Basket.API

# Terminal 4: Identity.API
dotnet run --project src/Identity.API

# Terminal 5: Payment.API
dotnet run --project src/Payment.API

# Terminal 6: OrderProcessor
dotnet run --project src/OrderProcessor
```

**Services will be available at:**

- Catalog API: `https://localhost:7001`
- Ordering API: `https://localhost:7002`
- Basket API: `https://localhost:7003`
- Identity API: `https://localhost:7004`
- Payment API: `https://localhost:7005`

## 📚 API Documentation

### Authentication

All APIs require Bearer tokens from Identity.API. First obtain a token:

```bash
curl -X POST https://localhost:7004/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=client_credentials&client_id=petstore&client_secret=secret"
```

Use the token in subsequent requests:

```bash
curl -H "Authorization: Bearer <token>" https://localhost:7001/api/products
```

### Swagger/OpenAPI Documentation

Each service exposes Swagger UI:

- **Catalog**: https://localhost:7001/swagger
- **Ordering**: https://localhost:7002/swagger
- **Basket**: https://localhost:7003/swagger
- **Identity**: https://localhost:7004/swagger
- **Payment**: https://localhost:7005/swagger

### Key Endpoints

**Catalog Service:**

- `GET /api/products` - List all products
- `GET /api/products/{id}` - Get product details
- `POST /api/products` - Create product (admin)
- `PUT /api/products/{id}` - Update product (admin)

**Basket Service:**

- `GET /api/basket/{userId}` - Get user's basket
- `POST /api/basket/{userId}/items` - Add item to basket
- `DELETE /api/basket/{userId}/items/{productId}` - Remove item
- `DELETE /api/basket/{userId}` - Clear basket

**Ordering Service:**

- `POST /api/orders` - Create new order
- `GET /api/orders/{orderId}` - Get order details
- `GET /api/orders` - List user's orders
- `PUT /api/orders/{orderId}/cancel` - Cancel order

**Identity Service:**

- `POST /connect/token` - Get access token
- `GET /.well-known/openid-configuration` - OIDC metadata

## 📁 Project Structure

```
PetStore/
├── src/
│   ├── Basket.API/              # Shopping cart microservice
│   ├── Catalog.API/             # Product catalog microservice
│   ├── Ordering.API/            # Order processing microservice (DDD/CQRS)
│   ├── Identity.API/            # Authentication/Authorization service
│   ├── Payment.API/             # Payment processing service
│   ├── OrderProcessor/          # Background job processor (Saga)
│   ├── EventBus/                # Abstract event bus interfaces
│   ├── EventBus.RabbitMQ/       # RabbitMQ implementation
│   ├── IntegrationEventLogEF/   # Outbox pattern (EF Core)
│   ├── Ordering.Domain/         # Domain models and events
│   ├── Ordering.Infrastructure/ # Database and repositories
│   ├── PetStore.AppHost/        # .NET Aspire orchestration
│   └── PetStore.ServiceDefaults/# Shared configuration
├── docker-compose.yml           # Infrastructure definition
├── README.md                    # This file
└── LICENSE.txt                  # MIT License
```

## 🔄 Integration Event Flow

1. **Order Created** → Ordering.API publishes `OrderStartedIntegrationEvent`
2. **Stock Reserved** → Catalog.API consumes and publishes `OrderStockConfirmedIntegrationEvent`
3. **Payment Processed** → Payment.API processes and publishes `OrderPaymentSucceededIntegrationEvent`
4. **Grace Period** → OrderProcessor manages state transitions and timeouts
5. **Order Completed** → Final event triggers order fulfillment

## 🤝 Contributing

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Commit changes: `git commit -am 'Add feature'`
3. Push to branch: `git push origin feature/your-feature`
4. Submit a Pull Request

### Code Standards

- Follow C# naming conventions and Microsoft guidelines
- Include unit tests for business logic
- Add XML documentation comments to public APIs
- Maintain consistent code formatting

## 📖 Additional Resources

- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Microservices Patterns](https://microservices.io/)
- [Domain-Driven Design](https://www.domainlanguage.com/ddd/)
- [CQRS Pattern](https://www.martinfowler.com/bliki/CQRS.html)

## 📄 License

This project is licensed under the MIT License - see [LICENSE.txt](LICENSE.txt) for details.
