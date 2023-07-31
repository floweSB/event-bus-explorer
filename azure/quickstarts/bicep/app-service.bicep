@description('App Service Plan Name')
param appServicePlanName string = 'asp-ebe-flowesb-001'

@description('App Service Name')
param appServiceName string = 'as-ebe-flowesb-001'

@description('The app service location')
param location string = '[resourceGroup().location]'

@description('Service Bus root connection string')
param serviceBusConnectionString string

@description('App Service container settings')
param linuxFxVersion string = 'DOCKER|flowesb/event-bus-explorer:latest'

resource appServiceAppPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: 'F1'
    tier: 'Free'
  }
  kind: 'linux'
}

resource appService 'Microsoft.Web/sites@2022-09-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServiceAppPlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      appSettings: [
        {
          name: 'ConnectionStrings__ServiceBus'
          value: serviceBusConnectionString
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Development'
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_URL'
          value: 'https://ghcr.io'
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_USERNAME'
          value: ''
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_PASSWORD'
          value: null
        }
        {
          name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
          value: 'false'
        }
      ]
    }
  }

  identity: {
    type: 'SystemAssigned'
  }
}
