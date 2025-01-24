# API Gateway with Ocelot and Authentication

This project demonstrates how to build an **API Gateway** using **Ocelot** and implement **authentication** using **API Key** and **JWT**. Future updates will include **OAuth2** authentication.

## Features

- **API Gateway**: Built using **Ocelot** to route requests to downstream services. Supports multiple authentication schemes.
- **Authentication**: 
  - **API Key Authentication**: Requests must include a valid API key in the `X-API-KEY` header
  - **JWT Authentication**: Secure endpoints that do  require JWT authentication
  - **Future Additions**: JWT Authentication and OAuth2 Authentication integration

## Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Postman](https://www.postman.com/downloads/) (or any API testing tool)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or any code editor)



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

### JWT Authentication
**Request**:
```
Retrieve Access Token from login, and then insert the token to the Header ( Bearer )
GET http://localhost:7295/gateway/secure
```

**Response**:
```json
{
  "message": "Hello from Demo API (JWT Auth)!"
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
      "DownstreamPathTemplate": "/api/demo/test",
      "DownstreamScheme": "http",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5000
        }
      ],
      "UpstreamPathTemplate": "/gateway/demo",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ]
    },
    {
      "DownstreamPathTemplate": "/api/demo/secure/test",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/gateway/secure",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      }
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

