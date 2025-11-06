# Azure Key Vault Setup Guide

This guide will help you set up Azure Key Vault and configure your application to use it for secure credential management.

## Prerequisites

- Azure CLI installed and configured
- Azure subscription
- Appropriate permissions to create resources

## Step 1: Create Azure Key Vault

```bash
# Login to Azure
az login

# Set your subscription (if you have multiple)
az account set --subscription "your-subscription-id"

# Create a resource group (if you don't have one)
az group create --name timetracker-rg --location eastus

# Create Key Vault (replace 'your-unique-keyvault-name' with a globally unique name)
az keyvault create \
  --name your-unique-keyvault-name \
  --resource-group timetracker-rg \
  --location eastus
```

## Step 2: Add Secrets to Key Vault

```bash
# Add SQL Connection String (for Azure SQL with Managed Identity)
az keyvault secret set \
  --vault-name your-unique-keyvault-name \
  --name "TimeTrackerDBConnectionString" \
  --value "Server=your-sqlserver.database.windows.net;Database=timetracker;Authentication=Active Directory Default;"

# Add Auth0 Domain
az keyvault secret set \
  --vault-name your-unique-keyvault-name \
  --name "Auth0--Domain" \
  --value "dev-isdsnzl58pgmx3kw.us.auth0.com"

# Add Auth0 Audience
az keyvault secret set \
  --vault-name your-unique-keyvault-name \
  --name "Auth0--Audience" \
  --value "timetracker"
```

**Note:** Key Vault secret names use `--` (double dash) instead of `:` (colon) because colons are not allowed in secret names. The configuration system automatically converts `--` to `:` when reading.

## Step 3: Grant Local Development Access

For local development, grant yourself access to the Key Vault:

```bash
# Get your user's Object ID
az ad signed-in-user show --query id -o tsv

# Grant yourself Key Vault Secrets User role
az role assignment create \
  --role "Key Vault Secrets User" \
  --assignee "your-object-id" \
  --scope "/subscriptions/your-subscription-id/resourceGroups/timetracker-rg/providers/Microsoft.KeyVault/vaults/your-unique-keyvault-name"
```

## Step 4: Configure Local Development

### Option A: Using Azure CLI (Recommended)

```bash
# Sign in with Azure CLI
az login

# Your application will now automatically use your Azure CLI credentials
```

### Option B: Using Visual Studio

1. Open Visual Studio
2. Go to Tools ? Options ? Azure Service Authentication
3. Sign in with your Azure account that has access to the Key Vault

## Step 5: Update appsettings.json

Update your `appsettings.json` and `appsettings.Development.json` files:

```json
{
  "Azure": {
    "KeyVaultName": "your-unique-keyvault-name"
  }
}
```

## Step 6: Deploy to Azure App Service

### Create App Service with Managed Identity

```bash
# Create App Service Plan
az appservice plan create \
  --name timetracker-plan \
  --resource-group timetracker-rg \
  --sku B1 \
  --is-linux

# Create Web App with System-Assigned Managed Identity
az webapp create \
--name your-webapp-name \
  --resource-group timetracker-rg \
  --plan timetracker-plan \
  --runtime "DOTNETCORE:9.0" \
  --assign-identity [system]

# Get the Managed Identity's Principal ID
az webapp identity show \
  --name your-webapp-name \
  --resource-group timetracker-rg \
  --query principalId -o tsv
```

### Grant App Service Managed Identity Access to Key Vault

```bash
# Grant the App Service Managed Identity access to Key Vault
az role assignment create \
  --role "Key Vault Secrets User" \
  --assignee "managed-identity-principal-id" \
  --scope "/subscriptions/your-subscription-id/resourceGroups/timetracker-rg/providers/Microsoft.KeyVault/vaults/your-unique-keyvault-name"
```

### Configure App Service Settings

```bash
# Set the Key Vault name in App Service configuration
az webapp config appsettings set \
  --name your-webapp-name \
  --resource-group timetracker-rg \
  --settings Azure__KeyVaultName="your-unique-keyvault-name"
```

## Step 7: Migrate to Azure SQL with Managed Identity (Optional)

If you want to use Managed Identity with Azure SQL Database:

### Create Azure SQL Server and Database

```bash
# Create Azure SQL Server
az sql server create \
  --name your-sqlserver \
  --resource-group timetracker-rg \
  --location eastus \
  --admin-user sqladmin \
  --admin-password "YourStrongPassword123!"

# Create Database
az sql db create \
  --name timetracker \
  --server your-sqlserver \
  --resource-group timetracker-rg \
  --service-objective S0
```

### Configure Managed Identity for SQL Access

1. **Enable Azure AD Authentication on SQL Server:**

```bash
# Set your Azure AD account as SQL Server admin
az sql server ad-admin create \
  --resource-group timetracker-rg \
  --server-name your-sqlserver \
  --display-name "Your Name" \
  --object-id "your-azure-ad-object-id"
```

2. **Grant App Service Managed Identity SQL Access:**

Connect to your Azure SQL Database using SQL Server Management Studio or Azure Data Studio and run:

```sql
-- Create a user for the App Service Managed Identity
CREATE USER [your-webapp-name] FROM EXTERNAL PROVIDER;

-- Grant appropriate permissions
ALTER ROLE db_datareader ADD MEMBER [your-webapp-name];
ALTER ROLE db_datawriter ADD MEMBER [your-webapp-name];
ALTER ROLE db_ddladmin ADD MEMBER [your-webapp-name];
```

3. **Update Connection String in Key Vault:**

```bash
az keyvault secret set \
  --vault-name your-unique-keyvault-name \
  --name "TimeTrackerDBConnectionString" \
  --value "Server=your-sqlserver.database.windows.net;Database=timetracker;Authentication=Active Directory Default;"
```

## Testing Locally

1. Ensure you're logged in with Azure CLI: `az login`
2. Run your application: `dotnet run`
3. The application will automatically:
   - Use your Azure CLI credentials to authenticate to Key Vault
   - Retrieve secrets from Key Vault
   - Use those secrets for database and Auth0 connections

## Troubleshooting

### Issue: "DefaultAzureCredential failed to retrieve a token"

**Solution:** Make sure you're logged in with Azure CLI:
```bash
az login
az account show
```

### Issue: "Access denied to Key Vault"

**Solution:** Verify you have the correct role assignment:
```bash
az role assignment list \
  --assignee "your-object-id" \
  --scope "/subscriptions/your-subscription-id/resourceGroups/timetracker-rg/providers/Microsoft.KeyVault/vaults/your-unique-keyvault-name"
```

### Issue: "Cannot connect to SQL Server with Managed Identity"

**Solution:** Ensure:
1. Azure AD authentication is enabled on SQL Server
2. Your Managed Identity is created as a user in the database
3. The connection string uses `Authentication=Active Directory Default`

## Security Best Practices

? **DO:**
- Use Managed Identity for Azure-hosted applications
- Use DefaultAzureCredential for seamless local and cloud authentication
- Store all secrets in Key Vault, never in code or config files
- Use separate Key Vaults for dev/staging/production
- Enable Key Vault firewall and private endpoints for production

? **DON'T:**
- Store client secrets or certificates in code
- Use connection strings with passwords in production
- Share Key Vault access across environments
- Grant unnecessary permissions to service principals

## Summary

After completing this setup:
- ? Secrets are stored securely in Azure Key Vault
- ? Local development uses your Azure credentials
- ? Azure App Service uses Managed Identity
- ? No secrets are stored in configuration files
- ? Connection strings use Managed Identity authentication

Your application is now following Azure security best practices!
