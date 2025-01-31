# API Gateway with Ocelot and Authentication

This project demonstrates how to build an **API Gateway** using **Ocelot** and implement **authentication** using **IdentityServer4**,  **API Key**, **JWT** and **OAuth2.0**.

## Features

- **API Gateway**: Built using **Ocelot** to route requests to downstream services. Supports multiple authentication schemes.
- **Authentication**: 
  - **API Key Authentication**: Requests must include a valid API key in the `X-API-KEY` header
  - **JWT Authentication**: Secure endpoints that do  require JWT authentication
  - **Identity Server4 Authentication**: Creates a bearer token, then the gateway uses that token

## Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Postman](https://www.postman.com/downloads/) (or any API testing tool)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or any code editor)
- [Google.console](https://cloud.google.com)



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

### Identity Server Implementation Summary

This section explains how we implemented Identity Server integration with our API Gateway.

## Implementation Steps

### 1. Identity Server Setup
- Established an OAuth2.0 Authorization Server
- Configured Client Credentials flow
- Defined API resource (`demoapi`) and scope (`demoapi.read`)
- Registered client (`gateway`)

### 2. Token Acquisition
- Endpoint: `POST https://localhost:5005/connect/token`
- Request body (x-www-form-urlencoded):

- - Identity Server validates client
- Checks scopes
- Generates access token

### 3. API Access
- Endpoint: `GET https://localhost:7295/gateway/ids-test`
- Add token to Authorization header: `Bearer {token}`
- Token validation
- Claims returned:
* `iss` (Issuer): Identity Server
* `aud` (Audience): demoapi
* `scope`: demoapi.read
* `client_id`: gateway
* `exp` (Expiration)
* `iat` (Issue time)

## Testing Flow
1. Get token from Identity Server
2. Use token to access protected API through Gateway
3. Receive authenticated response with claims

## Response Example
```json
{
"message": "Authenticated with Identity Server!",
"claims": [
  {"type": "iss", "value": "https://localhost:5005"},
  {"type": "aud", "value": "demoapi"},
  {"type": "scope", "value": "demoapi.read"},
  {"type": "client_id", "value": "gateway"}
]
```



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

# OAuth 2.0 Authentication Steps (Including Postman Testing)

## Step 1: Open Postman and Create a New Request

1. Open **Postman**.
2. Create a new request to a **protected endpoint**.

---

## Step 2: Configure Authorization

1. Go to the **Authorization** tab.
2. Select **OAuth 2.0** as the type.
3. Click **"Configure New Token"**.

---

## Step 3: Fill in OAuth 2.0 Configuration

Enter the following details:

```text
Token Name: Any name
Grant Type: Authorization Code
Callback URL: https://oauth.pstmn.io/v1/callback
Auth URL: https://accounts.google.com/o/oauth2/v2/auth
Access Token URL: https://oauth2.googleapis.com/token
Client ID: Your Google Client ID
Client Secret: Your Google Client Secret
Scope: email profile openid
```

---

## Step 4: Get New Access Token
Click "Get New Access Token".
The Google login page will open.
Sign in with your Google account.
Grant permissions when prompted.
You will be redirected back to Postman.

---

## Step 5: Postman Receives Tokens

Postman will receive the following tokens:

Access Token (Starts with ya29...)
ID Token (Starts with eyJ...)
Select the ID Token (not the Access Token).
Click "Use Token".

## Step 6: Send Request to Endpoint
The token will be added as Bearer in the Authorization header.
The request will be authenticated.
The API will return the protected resource.

## Configuration

### Ocelot Configuration (ocelot.json) [This file can be updated while testing the program you can change the paths and save it and continue with requests]
```json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/test/google-verify",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/gateway/google-test",
      "UpstreamHttpMethod": [ "GET" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "GoogleBearer"
      }
    },
    {
      "DownstreamPathTemplate": "/api/demo/secure/test/jwt",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/gateway/jwt-test",
      "UpstreamHttpMethod": [ "GET" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "CustomBearer"
      }
    },
    {
      "DownstreamPathTemplate": "/api/demo/secure/ids",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 5001
        }
      ],
      "UpstreamPathTemplate": "/gateway/ids-test",
      "UpstreamHttpMethod": [ "GET" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "IdentityServer"
      }
    }
  ]
}


```

The API key is validated in the `ApiKeyAuthenticationHandler` class. Default API key: `test-api-key-123`

