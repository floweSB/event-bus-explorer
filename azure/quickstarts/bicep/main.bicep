@description('App Service Plan Name')
param appServicePlanName string = 'asp-ebe-flowesb-001'

@description('App Service Name')
param appServiceName string = 'as-ebe-flowesb-001'

@description('AppService location')
param location string = resourceGroup().location

@description('')
param serviceBusConnectionString string

module appService 'app-service.bicep' = {
  name: 'appService'

  params: {
    appServicePlanName: appServicePlanName
    appServiceName: appServiceName
    location: location
    serviceBusConnectionString: serviceBusConnectionString
  }
}
