# MSSQL to PostgreSQL/Supabase Migration - Setup Guide

## ✅ Completed Changes

Your CrimePortal application has been successfully configured for PostgreSQL. Here's what was changed:

### 1. **NuGet Package Updated** ✓
- **Removed**: `Microsoft.EntityFrameworkCore.SqlServer` (SQL Server provider)
- **Added**: `Npgsql.EntityFrameworkCore.PostgreSQL` (PostgreSQL provider)
- File: [CrimePortal.csproj](CrimePortal.csproj)

### 2. **Database Configuration Updated** ✓
- Changed from `UseSqlServer()` to `UseNpgsql()` for both DbContexts
- File: [Program.cs](Program.cs)

### 3. **Connection Strings Updated** ✓
- **Production**: [appsettings.json](appsettings.json)
- **Development**: [appsettings.Development.json](appsettings.Development.json)

### 4. **Fresh Migrations Created** ✓
- Old SQL Server-specific migrations cleaned up
- New PostgreSQL migrations generated:
  - `InitialPostgresqlCRDb` - for CRDbContext (Crime Register data)
  - `InitialPostgresqlAppDb` - for AppDbContext (Application data)
- Location: [Migrations/](Migrations/)

### 5. **Build Status**: ✅ SUCCESS (0 Errors)

---

## 🚀 Next Steps - Deploy to Supabase

### Step 1: Set Up Supabase Project
1. Go to [supabase.com](https://supabase.com)
2. Sign in and create a new project
3. Choose a region and strong password
4. Wait for project to initialize (2-3 minutes)

### Step 2: Get Your Connection String
1. In Supabase dashboard, go to **Settings → Database**
2. Find the connection string section
3. Copy the **URI** format connection string

### Step 3: Update Connection String
In [appsettings.json](appsettings.json), replace:
```json
"DefaultConnection": "Host=your-project.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true",
"CrimeRegisterConnection": "Host=your-project.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
```

Replace:
- `your-project` with your Supabase project name
- `YOUR_PASSWORD` with the password you set during project creation

### Step 4: Apply Migrations
```powershell
cd "c:\Users\nadey\source\repos\CrimePortal\CrimePortal"
dotnet ef database update -c CRDbContext
dotnet ef database update -c AppDbContext
```

### Step 5: Test Connection
Run your application:
```powershell
dotnet run
```

If you see your login page without database errors, the migration was successful! ✅

---

## 📋 Key Differences: MSSQL vs PostgreSQL

| Feature | MSSQL | PostgreSQL |
|---------|-------|------------|
| **Identity/Auto-increment** | `IDENTITY(1,1)` | `SERIAL` / `BIGSERIAL` |
| **String Concatenation** | `+` operator | `\|\|` operator (EF handles this) |
| **Boolean** | `BIT` (0/1) | `BOOLEAN` (native) |
| **Datetime** | `DATETIME` | `TIMESTAMP` |
| **Decimal** | Fixed precision | Same - EF handles |
| **Case Sensitivity** | Not case-sensitive by default | Case-sensitive (quoted identifiers) |

**Good news**: EF Core 9.0 handles all these differences automatically! ✓

---

## ⚠️ Important Notes

1. **Connection String Security**: 
   - Never commit `appsettings.json` with real passwords to Git
   - Use User Secrets in development:
     ```powershell
     dotnet user-secrets init
     dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_connection_string"
     ```

2. **SSL/TLS Connections**:
   - Supabase requires SSL for production (SSL Mode=Require)
   - Development can use SSL Mode=Disable for localhost

3. **First-Time Setup**:
   - Supabase automatically creates a default `postgres` database
   - Your migrations will create all tables

4. **Data Migration** (if needed):
   - Export data from MSSQL using SQL Server Management Studio
   - Import to PostgreSQL using pgAdmin or Supabase interface

---

## 🔧 Troubleshooting

### Error: "connection refused"
- Verify connection string is correct
- Check Supabase project status is "Available"
- Ensure network allows outbound HTTPS on port 5432

### Error: "password authentication failed"
- Double-check the password from Supabase Settings
- Make sure you're using the default `postgres` user

### Error: "database does not exist"
- Supabase creates `postgres` database automatically
- Run migrations to create your schema tables

### Migration Issues
- Clear migrations: `dotnet ef migrations remove -f`
- Recreate: `dotnet ef migrations add InitialMigration`

---

## 📚 Useful Resources

- [Npgsql EF Core Documentation](https://www.npgsql.org/efcore/)
- [Supabase PostgreSQL Docs](https://supabase.com/docs/guides/database)
- [Entity Framework Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [PostgreSQL vs MSSQL Guide](https://wiki.postgresql.org/wiki/Number_of_users)

---

**Status**: ✅ Migration Complete - Ready for Supabase Deployment
