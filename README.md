# API Gateway with Ocelot and Authentication

This project demonstrates how to build an **API Gateway** using **Ocelot** and implement **authentication** using **API Key** and **No Authentication**. Future updates will include **JWT** and **OAuth2** authentication.

## Features

- **API Gateway**: Built using **Ocelot** to route requests to downstream services. Supports multiple authentication schemes.
- **Authentication**: 
  - **API Key Authentication**: Requests must include a valid API key in the `X-API-KEY` header
  - **No Authentication**: Public endpoints that do not require authentication
  - **Future Additions**: JWT Authentication and OAuth2 Authentication integration

## Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Postman](https://www.postman.com/downloads/) (or any API testing tool)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or any code editor)

## Project Structure
1. **Demo.API**:
   - Simple API with public and authenticated endpoints
   - Endpoints:
     - `/api/demo/public` (No Authentication)
     - `/api/demo/test` (API Key Authentication)
2. **OcelotApiGatewayDemo**:
   - API Gateway built using Ocelot
   - Handles API key authentication and routes requests

## Setup

### Clone the Repository
```bash
git clone https://github.com/your-username/your-repo.git
cd your-repo
```

### Run the Projects

#### Demo.API
```bash
cd Demo.API
dotnet run
```
The API will be available at: `http://localhost:5000`

#### OcelotApiGatewayDemo
```bash
cd OcelotApiGatewayDemo
dotnet run
```
The API Gateway will be available at: `http://localhost:7295`

## Testing the API

### Public Endpoint (No Authentication)
**Request**:
```
GET http://localhost:7295/gateway/public
```

**Response**:
```json
{
  "message": "Hello from Demo API (Public Endpoint)!"
}
```

### API Key Authentication
**Request**:
```
GET http://localhost:7295/gateway/test
Headers:
X-API-KEY: test-api-key-123
```

**Response**:
```json
{
  "message": "Hello from Demo API (API Key Authenticated)!"
}
```

Note: If the `X-API-KEY` header is missing or invalid, the gateway will return a `401 Unauthorized` response.

## Configuration

### Ocelot Configuration (ocelot.json)
```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/demo/{everything}",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5000
        }
      ],
      "UpstreamPathTemplate": "/gateway/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ]
    }
  ],
  "GlobalConfiguration": {
    "BaseUrl": "http://localhost:7295"
  }
}
```

The API key is validated in the `ApiKeyAuthenticationHandler` class. Default API key: `test-api-key-123`

## Future Updates

- OAuth2 Authentication: Integration with OAuth2 providers ( google console)

