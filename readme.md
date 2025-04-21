# Contentful Integration with .NET

This repository provides resources and guidance for integrating Contentful into a .NET application. It is intended to help developers understand the APIs available, learn how to use the official .NET SDK, and implement Contentful-powered features efficiently.

---

## 📚 Official Documentation & Resources

### 1. **Contentful API Concepts**

🔗 [Contentful API Concepts Documentation](https://www.contentful.com/developers/docs/concepts/apis/)  
Understand the core concepts behind Contentful's APIs:

- Content Delivery API (CDA)
- Content Management API (CMA)
- Content Preview API (CPA)
- GraphQL API

This page explains what each API does and when to use them in your applications.

---

### 2. **Getting Started with .NET SDK**

🔗 [Using .NET CDA SDK Tutorial](https://www.contentful.com/developers/docs/net/tutorials/using-net-cda-sdk/)  
Learn how to use the Contentful .NET SDK to fetch content from Contentful using the Content Delivery API.

Topics covered:

- Installing the SDK via NuGet
- Setting up the client
- Fetching entries and assets
- Working with content models

---

### 3. **Contentful .NET SDK (GitHub)**

🔗 [GitHub - contentful.net](https://github.com/contentful/contentful.net)  
This is the official .NET SDK for accessing Contentful APIs. Features:

- Open-source and community-supported
- Contains source code, usage examples, and issues
- Supports CDA, CMA, and CPA

---

### 4. **All .NET Tutorials for Contentful**

🔗 [Contentful .NET Tutorials Index](https://www.contentful.com/developers/docs/net/tutorials/)  
Explore a range of tutorials for various use cases in .NET:

- Basic integration
- Advanced querying
- Sync API usage
- Rich text rendering
- Managing entries via CMA

---

## 🛠 Prerequisites

- .NET Framework or .NET Core/6+
- Contentful account with a space and access token
- Visual Studio or any .NET-compatible IDE

---

## 📦 Installation

Install the Contentful SDK via NuGet:

```bash
dotnet add package contentful.aspnetcore
```

## ⚙️ Configuration

To integrate Contentful into your .NET application, you need to configure your API keys and other settings. Add the following section to your `appsettings.json` file:

```json
"ContentfulOptions": {
  "DeliveryApiKey": "<Your Delivery API Key>",
  "ManagementApiKey": "<Your Management API Key>",
  "PreviewApiKey": "<Your Preview API Key>",
  "SpaceId": "<Your Space ID>",
  "UsePreviewApi": false,
  "MaxNumberOfRateLimitRetries": 0
}
```

Explanation of Configuration Options:

- DeliveryApiKey: API key for accessing published content via the Content Delivery API (CDA).
- ManagementApiKey: API key for managing content via the Content Management API (CMA).
- PreviewApiKey: API key for accessing draft content via the Content Preview API (CPA).
- SpaceId: The unique identifier for your Contentful space.
- UsePreviewApi: Set to true if you want to fetch draft content using the Preview API.
- MaxNumberOfRateLimitRetries: The number of retries the SDK should attempt when encountering rate limits.