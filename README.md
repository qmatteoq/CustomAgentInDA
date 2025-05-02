# Travel Agency Copilot Sample

This repository demonstrates how you can wrap a custom agent built with any technology into a declarative agent for Microsoft 365 Copilot. This technique empowers to use any type of agent through the Copilot Chat interface, as long as they expose an API that the declarative agent can call.

## Overview

The solution includes:

- **TravelAgency-DA**: A Declarative Agent for Microsoft 365 Copilot, forwarding user prompts to the backend API exposed by the custom agent and returning responses.
- **TravelAgency-Custom**: A custom agent exposed as an API that leverages Semantic Kernel to generate travel plans, suggest destinations, and provide weather information.

## Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio Code](https://code.visualstudio.com/)
- [Teams Toolkit for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=TeamsDevApp.ms-teams-vscode-extension)
- Access to a Microsoft 365 tenant with a Copilot license
- The option to [sideload Teams apps](https://learn.microsoft.com/microsoftteams/platform/concepts/deploy-and-publish/apps-upload) enabled for the Microsoft 365 tenant

## Setup

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio Code.
3. Open the `appsettings.json` file in the **TravelAgency.API** project. You'll find the following entry:
    
    ```json
    "AIServices": {
        "AzureOpenAI": {
            "DeploymentName": "",
            "Endpoint": "",
            "ApiKey": ""
        }
    }
    ```
    
    Fill in the values for `DeploymentName`, `Endpoint`, and `ApiKey` with your Azure OpenAI service connection details. You can find these values in the Azure portal under your OpenAI resource.
4. Launch the **TravelAgency.AppHost** project. This will start the backend API for the custom agent and the .NET Aspire dashboard to monitor it. The API will be available at `http://localhost:5175` by default.
5. Use the [Port Forwarding feature in Visual Studio Code](https://code.visualstudio.com/docs/debugtest/port-forwarding) to expose your local 5175 port to the Internet. Make sure to set its visibility to public. Take note of the public URL assigned to your local port. 
6. Open the openapi.json file in the **TravelAgency-DA -> appPackage -> apiSpecificationFile** folder. You will find the following entry:
    
    ```json
    "servers": [
        {
        "url": "<YOUR_PUBLIC_URL>",
        }
    ]
    ```
    
    Replace `<YOUR_PUBLIC_URL>` with the public URL assigned to your local port in the previous step. Save the file.
7. Now open a dedicated Visual Studio Code instance on the **TravelAgency-DA** folder. 
8. Open the Teams Toolkit panel and click on **Provision**. The declarative agent will be deployed to your Microsoft 365 tenant. 
9. Open Microsoft 365 Copilot and try the agent with a prompt like:
    
    ```
    Please suggest me a few travel destinations if I'm interested into art and museums.
    ```
