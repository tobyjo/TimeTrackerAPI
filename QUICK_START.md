# Quick Start: Azure Key Vault Setup

## ?? Fast Track Setup (5 minutes)

### 1. Login to Azure
```bash
az login
```

### 2. Create Key Vault
```bash
# Create resource group
az group create --name timetracker-rg --location eastus

# Create Key Vault (choose a unique name!)
az keyvault create --name YOUR-UNIQUE-NAME-HERE --resource-group timetracker-rg --location eastus
```

### 3. Add Your Secrets
```bash
# SQL Connection String
az keyvault secret set --vault-name YOUR-UNIQUE-NAME-HERE --name "TimeTrackerDBConnectionString" --value "Server=localhost\\SQLEXPRESS01;Database=timetracker;Trusted_Connection=True;TrustServerCertificate=True;"

# Auth0 Domain
az keyvault secret set --vault-name YOUR-UNIQUE-NAME-HERE --name "Auth0--Domain" --value "dev-isdsnzl58pgmx3kw.us.auth0.com"

# Auth0 Audience
az keyvault secret set --vault-name YOUR-UNIQUE-NAME-HERE --name "Auth0--Audience" --value "timetracker"
```

### 4. Grant Yourself Access
```bash
# Get your Object ID
$objectId = az ad signed-in-user show --query id -o tsv

# Grant access
az role assignment create --role "Key Vault Secrets User" --assignee $objectId --scope "/subscriptions/$(az account show --query id -o tsv)/resourceGroups/timetracker-rg/providers/Microsoft.KeyVault/vaults/YOUR-UNIQUE-NAME-HERE"
```

### 5. Update Your Config
Edit `appsettings.json` and `appsettings.Development.json`:
```json
{
  "Azure": {
    "KeyVaultName": "YOUR-UNIQUE-NAME-HERE"
  }
}
```

### 6. Test It!
```bash
dotnet run
```

## ? You're Done!

Your app now:
- ? Loads secrets from Azure Key Vault
- ? Works locally with your Azure credentials
- ? Ready to deploy to Azure with Managed Identity

---

## ?? Need More Detail?
See `AZURE_SETUP_GUIDE.md` for:
- Azure App Service deployment
- Managed Identity configuration
- Azure SQL migration
- Troubleshooting tips

## ?? Quick Troubleshooting

**Can't authenticate?**
```bash
az login
az account show
```

**Can't access Key Vault?**
```bash
# Check your permissions
az role assignment list --assignee $(az ad signed-in-user show --query id -o tsv)
```

**App not finding Key Vault?**
- Check `appsettings.json` has correct `KeyVaultName`
- Verify you're logged in with `az account show`
