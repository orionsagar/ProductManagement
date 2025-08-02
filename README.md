# Backend

# 🏗️ Product Management API (.NET 8 + PostgreSQL)

This is a backend API for managing **Products**, **Projects**, and **Product Items** in an imaginary company's production workflow. Built with **ASP.NET Core 8**, **Entity Framework Core**, and **PostgreSQL**.

## ✅ Features

* JWT Authentication
* Role-based access (Product Manager, Project Manager, Production Engineer)
* Audit/History Logging via Middleware
* Project and Item summary endpoints
* PostgreSQL with EF Core (Code First)
* CORS enabled for frontend development

---

## 🚀 Getting Started

### 1. 📦 Prerequisites

Make sure you have the following installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [PostgreSQL](https://www.postgresql.org/download/)
* [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

---

### 2. 🔧 Configuration

Update `appsettings.Development.json` with your PostgreSQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ProductManagementDb;Username=postgres;Password=yourpassword"
  },
  "Jwt": {
    "Key": "YourSuperSecureKeyHereAtLeast32Characters",
    "Issuer": "ProductManagementApi"
  }
}
```

---

### 3. 💠 Apply EF Core Migrations

If migrations are not added yet:

```bash
dotnet ef migrations add InitialCreate
```

Then apply them:

```bash
dotnet ef database update
```

This creates the PostgreSQL database and schema.

---

### 4. 🌱 (Optional) Seed Dummy Data

If you have a seeding mechanism (like in `DbInitializer`):

```bash
dotnet run
```

Or ensure the logic in `Program.cs` calls seeding on startup:

```csharp
await DbInitializer.SeedAsync(app.Services);
```

---

### 5. ▶️ Run the API

```bash
dotnet run
```

By default, it will run on:

* [http://localhost:5000](http://localhost:5000)
* [https://localhost:7000](https://localhost:7000)

Swagger UI available at:

```
https://localhost:7000/swagger
```

---

## 📦 API Structure

| Entity      | Description                          |
| ----------- | ------------------------------------ |
| Product     | Represents a product version/design  |
| Project     | Created under a product              |
| ProductItem | Units/items created within a project |


## 📡 API Endpoints

### Products
| Method | Endpoint          | Description                      |
|--------|-------------------|---------------------------------|
| GET    | `/api/products`    | List all products (supports search/filter via query params) |
| POST   | `/api/products`    | Create a new product            |
| GET    | `/api/products/:id`| Get details of a specific product |

### Projects
| Method | Endpoint           | Description                     |
|--------|--------------------|--------------------------------|
| GET    | `/api/projects`    | List all projects (supports search/filter) |
| POST   | `/api/projects`    | Create a new project           |
| GET    | `/api/projects/:id`| Get details of a specific project |

### Project Items
| Method | Endpoint                 | Description                          |
|--------|--------------------------|------------------------------------|
| GET    | `/api/projects/:id/items`| List all items under a specific project |
| POST   | `/api/projects/:id/items`| Create a new item under a specific project |
| PATCH  | `/api/items/:id`         | Update an existing item (e.g., status) |

### Summaries
| Method | Endpoint                    | Description                         |
|--------|-----------------------------|-----------------------------------|
| GET    | `/api/summary/product/:id`  | Get aggregated summary for a product (e.g., item status distribution across projects) |
| GET    | `/api/summary/project/:id`  | Get aggregated summary for a project (e.g., status breakdown of product items) |

---

## 🔒 Roles

| Role                | Permissions                           |
| ------------------- | ------------------------------------- |
| Product Manager     | Create/Edit Products, Create Projects |
| Project Manager     | Create Projects, View Summaries       |
| Production Engineer | Create/Update Product Items           |

---

## 🔍 Development Notes

* CORS is configured for `http://localhost:3000` (React/Next frontend).
* Authentication uses JWT bearer tokens.
* Middleware logs all changes for audit/history purposes.

---

## 🧪 Testing

Use Swagger or Postman to test API endpoints.

---

## 🛯️ Troubleshooting

| Issue                      | Action to fix                                                  |
| -------------------------- | -------------------------------------------------------------- |
| `invalid_token` error      | Check frontend sends a valid JWT token                         |
| CORS setup                 | Confirm backend allows your frontend origin                    |
| Token expired or malformed | Renew token or fix token generation                            |
| Ambiguous HTTP Method      | Add `[HttpGet]`, `[HttpPost]`, etc. to your controller methods |

---

## 📂 Folder Structure (example)

```
ProductManagementBackend/
├── ProductManagement.Api/              # ASP.NET Core Web API
├── ProductManagement.Application/      # Application logic (CQRS, DTOs, Services)
├── ProductManagement.Domain/           # Domain models & enums
├── ProductManagement.Infrastructure/   # EF Core, DB context, PostgreSQL
└── ProductManagement.Shared/           # Common contracts, constants, etc.
```

---

## Data Model Diagram or UML Diagram

![Diagram](https://raw.githubusercontent.com/orionsagar/ProductManagement/637399a851e66fa1ba6a95f3a80991c1f19fae5d/Untitled.png)

## 📝 License

MIT — free to use and modify.
