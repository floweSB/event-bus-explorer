# event-bus-explorer

## Getting Started

### Run locally

#### Docker

You can easily start the Event Bus Explorer locally using Docker:
`docker run -d -p 8080:80 -e ASPNETCORE_ENVIRONMENT=Development -e ConnectionStrings__ServiceBus=<your-sb-cs> ghcr.io/flowesb/event-bus-explorer:latest`

### Deploy on Azure

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FfloweSB%2Fevent-bus-explorer%2Fmain%2Fazure%2Fquickstarts%2Farm%2Fazuredeploy.json)
[![Visualize](https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/1-CONTRIBUTION-GUIDE/images/visualizebutton.svg?sanitize=true)](http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2FfloweSB%2Fevent-bus-explorer%2Fmain%2Fazure%2Fquickstarts%2Farm%2Fazuredeploy.json)

This template creates an Azure Container App (Consumption) to test `event-bus-explorer` with your Azure Service Bus instance
