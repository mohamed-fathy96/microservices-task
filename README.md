# E-Commerce app using Microservices Architecture

## This is an e-commerce app built using Microservices Architecture, containerized, orchestrated using Docker and Docker Compose inspired by [Microsoft's eShopOnContainers app](https://github.com/dotnet-architecture/eShopOnContainers) and Microsoft's Official Book on Microservices Architecture

## Installing and Running

1. At the root directory which include docker-compose.yml files, run the command:
```
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
```
2. The microservices will be available at the following Urls:
- **Catalog API:** http://host.docker.internal:8000/swagger/index.html
- **Basket API:** http://host.docker.internal:8001/swagger/index.html
- **Ordering API:** http://host.docker.internal:8002/swagger/index.html
- **Ocelot API Gateway:** http://host.docker.internal:8010/Catalog
- **Portainer:** http://host.docker.internal:9000   || Username: admin , Password: admin1234567
- **RabbitMQ Dashboard:** http://host.docker.internal:15672  || Username: guest , Password: guest
**Application Architecture Diagram**

![image](https://user-images.githubusercontent.com/94698429/174092398-93f4ae06-5e13-4307-8e2a-84c5a075b769.png)

## Documentation and Details:
**Design and Architectual Patterns used:**
- N-Tier Architecture in both Catalog and Basket Services
- Generic Repository Pattern (with ability to add query expressions for specification)
- Clean Architecture and Domain Driven Design in Order Service
- Event-driven Architecture and Async communication between services using RabbitMQ as the Message Broker
- Mediator and CQRS pattern using MediatR NuGet package in the Ordering Service
- Dependency Injection is used to ensure separation of concerns and loose coupling between all services and classes
- API Gateway Routing Pattern or BFF (Backend for frontend) which was implemented using Ocelot NuGet Package

**The app is composed of 3 main services:**

- Catalog Service
- Basket Service
- Ordering Service

### Catalog Service

**The catalog service is responsible for CRUD operations on the products. It uses MongoDB as the data store to store products.**

### Basket Service

**The basket service is responsible for operations on the user basket such as creating a user basket, adding item to basket, etc... It uses Redis as the data store to store user basket, because Redis is a best fit for caching information and it acts as a distributed cache in the application. It is also responsible for sending the basket checkout event to RabbitMQ which is later consumed by the Ordering Service to create an order for the user.**

### Ordering Service

**The ordering service is responsible for performing CRUD operations on user orders. It is also responsible for consuming checkout event from the basket service and creating an order for the user. It uses SQL Server for storing data.**

### Ordering Service Implementation Details:
**Ordering Service was implemented using Clean Architecture, which decouples use cases from external details. The benefit of this is that we achieve separation of concerns, for example, any change in the infrastructure layer does not affect application layer.**

**Ordering Service has 4 main layers:**
- Ordering.Domain: Which is the middle layer and has no references to any other layers.
- Ordering.Application: Covers business use cases (contracts, features and behaviors), also represents business requirements and business foundation.
- Ordering.Infrastructure: Contains the infrastructure details of the application layer, and it is loosely coupled from application layer and can be changed easily.
- Ordering.API: The presentation layer. 

**Ordering Service also applies CQRS and Mediator design patterns. The query and commands are separated and sent to different handlers.**
**Event-driven architecture is applied when ordering service consumes a message from the basket service, and creates an order for the user.**

![Screenshot_1](https://user-images.githubusercontent.com/94698429/174103097-d182b834-ee74-4432-848e-cc9d4b4c02c8.png)

![Screenshot_2](https://user-images.githubusercontent.com/94698429/174103766-6d79105d-e439-479a-a5f6-7241441ad8ed.png)


### Tools used
- **Docker : For containerizing services**
- **Portainer: Managing containers**
- **Docker Compose: Container orechestrator**
- **Ocelot API Gateway: Acts as the gateway to all services**
- **RabbitMQ with MassTransit: Allows Async communication between services**


