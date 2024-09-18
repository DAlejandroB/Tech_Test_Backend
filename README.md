# Backend Engineer Technical Test API

## Description
This API is developed as part of a technical test for the role of Backend Engineer. The application is a simple backend using **MongoDB** as the database and follows common architecture patterns to ensure a clean, maintainable, and scalable design. It handles user authentication, registration, and management of providers' information.

The project demonstrates the usage of **DTOs**, **Services**, and **Controllers**, which are essential components for structuring a robust API in modern software development.

## Architecture
The application is organized with the following patterns and practices:
- **DTO (Data Transfer Objects):** Used to encapsulate the data exchanged between the client and the server, minimizing data exposure and ensuring validation is properly handled.
- **Service Layer:** Encapsulates business logic, including user authentication and provider management. It interacts with the database layer through repositories and the controllers layer to respond to HTTP requests.
- **Controllers:** Handle HTTP requests, convert DTOs to domain models, and call the appropriate services to process requests.

The architecture promotes **separation of concerns** and **single responsibility** principles, making the code more maintainable and testable.

## Models

### 1. **Provider**
This model represents a provider entity in the system. Each provider is registered with the following attributes:
- **TaxId**: A unique identifier for the provider.
- **BusinessName**: The legal business name of the provider.
- **Address**: The street address where the provider operates.
- **City**: The city where the provider is located.
- **Department**: The region or department associated with the provider's location.
- **Email**: The primary email address for the provider.
- **IsActive**: A boolean flag indicating if the provider is currently active.
- **CreatedAt**: A timestamp for when the provider was registered in the system.
- **ContactEmail**: An alternate email for direct contact with the provider.

### 2. **User**
The user model is used to handle authentication and user management within the application. It includes the following properties:
- **Username**: The chosen name for user identification within the system.
- **Email**: The user's email address, used for communication and as a unique identifier.
- **Creation Date**: The date and time when the user was created.
- **Password Hash**: A securely stored hash of the user's password, used for authentication purposes.

## Key Features
- **User Authentication:** The system allows users to register and authenticate using JWT (JSON Web Tokens).
- **Provider Management:** Providers can be managed through CRUD operations (Create, Read, Update, Delete) via the API.
- **Swagger Documentation:** The API is documented using Swagger, making it easy to explore and test endpoints.

## Technologies Used
- **.NET 6**: The backend framework used to build the API.
- **MongoDB**: A NoSQL database used to store users and provider information.
- **JWT (JSON Web Tokens)**: Used for user authentication.
- **Swagger**: Provides API documentation and a user-friendly interface to test the endpoints.
- **Docker**: The application is dockerized for easy setup and deployment.

## Setup Instructions
Clone the repository:
   ```bash
   git clone https://github.com/DAlejandroB/Tech_Test_Backend
