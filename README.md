# ERDM Credit Management Microservice - Complete Documentation

## 📋 Project Overview

### Purpose
The **ERDM Credit Management Microservice** is an enterprise-grade, production-ready solution for managing the complete credit lifecycle - from application submission to approval, underwriting, and disbursement. Built on **.NET 8** with **MongoDB**, it provides a robust foundation for financial institutions, lending platforms, and credit assessment systems.

### Scope
This microservice handles the end-to-end credit management process including:

- ✅ **Credit Application Management** - Create, submit, and track credit applications
- ✅ **Customer Profile Management** - Store and manage customer information
- ✅ **Underwriting Process** - Automated credit assessment and risk evaluation
- ✅ **Decision Engine** - Approve, decline, or refer applications based on rules
- ✅ **Document Management** - Upload and verify supporting documents
- ✅ **Credit Bureau Integration** - Fetch and store credit scores
- ✅ **Fraud Detection** - Basic fraud checks and risk scoring
- ✅ **Audit Trail** - Complete history of application lifecycle
- ✅ **Reporting & Analytics** - Statistics and aggregated data

## 🏗️ Architecture Overview

### Clean Architecture Implementation

```
┌─────────────────────────────────────────────────────────────┐
│                     Presentation Layer                       │
│                    (ERDM.Credit.API)                         │
│                  Controllers, Swagger                        │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    Application Layer                         │
│                 (ERDM.Credit.Application)                    │
│            Services, DTOs, AutoMapper Profiles               │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                     Domain Layer                             │
│                  (ERDM.Credit.Domain)                        │
│         Entities, Value Objects, Interfaces                  │
└─────────────────────────────────────────────────────────────┘
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                  Infrastructure Layer                        │
│               (ERDM.Credit.Infrastructure)                   │
│       MongoDB Repositories, Settings, Indexes                │
└─────────────────────────────────────────────────────────────┘
```

## 🎯 Key Features

### 1. **Credit Application Workflow**
```
Draft → Submitted → Underwriting → Approved/Declined/Referred
```

### 2. **Rich Domain Model**
- Complete credit application data structure
- Customer profiles and employment details
- Credit bureau data integration
- Fraud check results
- Document management
- Underwriting history tracking

### 3. **MongoDB Optimization**
- Proper indexing for query performance
- TTL indexes for auto-expiration of stale applications
- Compound indexes for status-based queries
- Text search capabilities

### 4. **Enterprise Features**
- **Audit Trail**: Automatic tracking of CreatedBy, ModifiedBy, timestamps
- **Soft Delete**: Data retention with IsActive flag
- **Version Control**: Optimistic concurrency with version numbers
- **Correlation ID**: End-to-end request tracking
- **Structured Logging**: Serilog integration with file and console sinks

### 5. **Security & Compliance**
- Input validation
- Business rule enforcement
- Audit logs for compliance
- Role-based access control ready

## 📊 Data Model

### Credit Application Document Structure
```json
{
  "_id": "507f1f77bcf86cd799439011",
  "applicationId": "APP-20240328-ABC123",
  "customerId": "CUST-001",
  "customerProfile": {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phone": "+1234567890"
  },
  "productType": "PERSONAL_LOAN",
  "requestedAmount": 25000.00,
  "requestedTerm": 36,
  "status": "PENDING",
  "decision": {
    "status": "APPROVED",
    "approvedAmount": 20000.00,
    "interestRate": 8.5,
    "riskGrade": "B",
    "decidedBy": "SYSTEM",
    "decidedAt": "2024-03-28T10:30:00Z"
  },
  "applicationData": {
    "employmentStatus": "EMPLOYED",
    "annualIncome": 85000.00,
    "employerName": "Tech Corp",
    "yearsAtEmployer": 3,
    "monthlyExpenses": 3500.00,
    "purpose": "Debt Consolidation"
  },
  "underwritingHistory": [
    {
      "stage": "INITIATION",
      "status": "COMPLETED",
      "startedAt": "2024-03-28T10:00:00Z",
      "result": "Application created"
    },
    {
      "stage": "SUBMISSION",
      "status": "COMPLETED",
      "startedAt": "2024-03-28T10:05:00Z",
      "result": "Application submitted"
    }
  ],
  "createdAt": "2024-03-28T10:00:00Z",
  "updatedAt": "2024-03-28T10:30:00Z",
  "isActive": true,
  "version": 3
}
```

## 🏛️ Design Patterns Implemented

| Pattern | Purpose |
|---------|---------|
| **Domain-Driven Design (DDD)** | Encapsulates business logic in domain entities |
| **Repository Pattern** | Abstracts data access for MongoDB |
| **Unit of Work** | Ensures transactional consistency |
| **CQRS** | Separates read and write operations |
| **Dependency Injection** | Loose coupling and testability |
| **Template Method** | Reusable base repository functionality |
| **Specification Pattern** | Dynamic query building |
| **Builder Pattern** | Complex object construction |
| **Factory Pattern** | Centralized object creation |
| **Strategy Pattern** | Configurable algorithms |
| **Decorator Pattern** | Cross-cutting concerns via middleware |
| **Option Pattern** | Strongly-typed configuration |

## 📦 Project Structure

```
ERDM.Credit/
├── ERDM.Credit.API/                    # Presentation Layer
│   ├── Controllers/                    # API endpoints
│   │   └── CreditApplicationsController.cs
│   ├── Program.cs                      # Application entry point
│   └── appsettings.json                # Configuration
│
├── ERDM.Credit.Application/            # Application Layer
│   ├── Services/                       # Business orchestration
│   │   └── CreditApplicationService.cs
│   └── Mappings/                       # AutoMapper profiles
│       └── CreditApplicationProfile.cs
│
├── ERDM.Credit.Domain/                 # Domain Layer
│   ├── Entities/                       # Domain entities
│   │   ├── BaseEntity.cs
│   │   └── CreditApplication.cs
│   └── Interfaces/                     # Repository contracts
│       └── ICreditApplicationRepository.cs
│
├── ERDM.Credit.Infrastructure/         # Infrastructure Layer
│   ├── Settings/                       # Configuration
│   │   └── MongoDbSettings.cs
│   ├── Repositories/                   # Data access
│   │   ├── MongoRepository.cs
│   │   └── CreditApplicationRepository.cs
│   └── DependencyInjection.cs          # DI registration
│
└── ERDM.Credit.Contracts/              # Shared DTOs
    └── DTOs/
        └── CreditApplicationDtos.cs
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- MongoDB (Local or Atlas)
- Visual Studio 2022 / VS Code / Rider

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/yourcompany/ERDM.Credit.git
cd ERDM.Credit
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Configure MongoDB connection**
Update `appsettings.json` in ERDM.Credit.API:
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ERDM_Credit",
    "CollectionPrefix": "credit"
  }
}
```

4. **Run the application**
```bash
cd ERDM.Credit.API
dotnet run
```

5. **Access Swagger UI**
```
https://localhost:5001/swagger
```

## 🔌 API Endpoints

### Credit Applications

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/creditapplications` | Create new credit application |
| GET | `/api/creditapplications/{id}` | Get application by ID |
| GET | `/api/creditapplications/number/{applicationNumber}` | Get by application number |
| GET | `/api/creditapplications` | Get all applications (paginated) |
| POST | `/api/creditapplications/{id}/submit` | Submit application |
| POST | `/api/creditapplications/{id}/approve` | Approve application |
| POST | `/api/creditapplications/{id}/decline` | Decline application |
| GET | `/api/creditapplications/statistics` | Get application statistics |

### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| Page | int | Page number (default: 1) |
| PageSize | int | Items per page (default: 10) |
| Status | string | Filter by status |
| CustomerId | string | Filter by customer |
| SortBy | string | Sort field (CreatedAt, Amount, Status) |
| SortDescending | bool | Sort direction |

## 🔒 Business Rules

### Application Status Flow
1. **PENDING** - Initial state after creation
2. **SUBMITTED** - Customer submits for review
3. **UNDERWRITING** - Underwriter/System reviews
4. **APPROVED** - Application approved
5. **DECLINED** - Application rejected
6. **REFERRED** - Requires manual review

### Validation Rules
- Requested amount must be > 0
- Requested term must be between 1-360 months
- Customer profile must be complete
- Only PENDING applications can be submitted
- Only SUBMITTED applications can go to underwriting
- Only UNDERWRITING applications can be approved/declined

## 📊 Database Indexes

| Index Name | Fields | Type | Purpose |
|------------|--------|------|---------|
| idx_application_id | applicationId | Unique | Fast lookup by application number |
| idx_customer_id | customerId | Standard | Find all applications for a customer |
| idx_status_created_at | status, createdAt | Compound | Pagination and filtering |
| idx_expires_at_ttl | expiresAt | TTL | Auto-delete expired applications |

## 🧪 Testing

### Sample Request - Create Application
```json
POST /api/creditapplications
{
  "customerId": "CUST-001",
  "customerProfile": {
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@example.com",
    "phone": "+1234567890"
  },
  "productType": "PERSONAL_LOAN",
  "requestedAmount": 25000,
  "requestedTerm": 36,
  "applicationData": {
    "employmentStatus": "EMPLOYED",
    "annualIncome": 85000,
    "employerName": "Tech Corp",
    "yearsAtEmployer": 3,
    "housingStatus": "OWN",
    "monthlyExpenses": 3500,
    "existingDebts": 15000,
    "purpose": "Debt Consolidation"
  },
  "metadata": {
    "ipAddress": "192.168.1.100",
    "userAgent": "Mozilla/5.0",
    "deviceId": "device-123",
    "source": "WEB"
  }
}
```

### Sample Response
```json
{
  "success": true,
  "message": "Application created successfully",
  "data": {
    "id": "67e5f8a1b3c4d5e6f7a8b9c0",
    "applicationId": "APP-20240328-ABC123",
    "status": "PENDING",
    "createdAt": "2024-03-28T10:00:00Z"
  }
}
```

## 🛠️ Configuration

### MongoDB Settings
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ERDM_Credit",
    "CollectionPrefix": "credit",
    "MinPoolSize": 10,
    "MaxPoolSize": 100,
    "ConnectionTimeoutSeconds": 30,
    "SocketTimeoutSeconds": 60,
    "WriteConcern": "majority",
    "JournalEnabled": true,
    "ReadPreferenceMode": "Primary",
    "RetryWrites": true,
    "RetryReads": true
  }
}
```

## 📈 Performance Considerations

### Optimizations
- Connection pooling for MongoDB
- Proper indexing strategies
- Pagination for list endpoints
- Async operations throughout
- Caching-ready architecture

### Scaling
- Horizontal scaling via MongoDB replica sets
- Stateless microservice design
- Read replicas for reporting queries
- Event-driven architecture ready

## 🔐 Security Considerations

- Connection string encryption
- Input validation and sanitization
- Business rule enforcement
- Audit trail for compliance
- Role-based access control ready
- Soft delete for data retention

## 📝 Logging & Monitoring

### Serilog Configuration
- Console logging for development
- File logging with daily rotation
- Structured JSON output ready
- Correlation ID for request tracking

### Health Checks
- MongoDB connection health
- Application health status
- Readiness probes for Kubernetes

## 🚦 Future Enhancements

1. **Event Sourcing** - Complete audit trail of all changes
2. **Background Processing** - Hangfire for delayed tasks
3. **Message Bus** - RabbitMQ/Kafka for async communication
4. **API Versioning** - Support multiple API versions
5. **Rate Limiting** - Prevent abuse
6. **Caching** - Redis for frequently accessed data
7. **SignalR** - Real-time notifications
8. **ElasticSearch** - Advanced search capabilities

## 📚 Related Documentation

- [MongoDB .NET Driver Documentation](https://www.mongodb.com/docs/drivers/csharp/)
- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Serilog Documentation](https://serilog.net/)
- [AutoMapper Documentation](https://docs.automapper.org/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Team

- Architecture & Design: ERDM Team
- Development: Credit Management Team
- QA: Quality Assurance Team

## 📞 Support

- **GitHub Issues**: [Repository Issues](https://github.com/yourcompany/ERDM.Credit/issues)
- **Email**: support@erdm.com
- **Documentation**: [Wiki Pages](https://github.com/yourcompany/ERDM.Credit/wiki)

---

**Version**: 1.0.0  
**Last Updated**: March 2026  
**Status**: Production Ready ✅

## 🎯 Key Takeaways

This microservice provides:

1. **Complete Credit Management** - Full lifecycle from application to approval
2. **Production-Ready Architecture** - Clean architecture with best practices
3. **Scalable Design** - Built for growth with MongoDB
4. **Enterprise Features** - Audit trails, soft delete, version control
5. **Developer Friendly** - Clean code, documentation, and testing support

The ERDM Credit Management Microservice is ready to handle your credit operations at scale!
