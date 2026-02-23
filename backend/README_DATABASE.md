# Database Setup Guide

## SQL Script Location

The database SQL script is located at: `backend/database.sql`

## Two Ways to Create the Database

### Option 1: Using the SQL Script (Manual)

1. Open MySQL Workbench or any MySQL client
2. Connect to your MySQL server
3. Open the `backend/database.sql` file
4. Execute the entire script
5. The database `HostelManagementDB` will be created with all tables

### Option 2: Using Entity Framework Core (Automatic)

The application is configured to automatically create the database when it runs for the first time using `EnsureCreated()` in `Program.cs`.

**Steps:**
1. Make sure your MySQL server is running
2. Update the connection string in `backend/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=HostelManagementDB;User=root;Password=yourpassword;Port=3306;"
     }
   }
   ```
3. Run the backend application:
   ```bash
   cd backend
   dotnet run
   ```
4. The database and tables will be created automatically

## Using EF Core Migrations (Recommended for Production)

For better control and versioning, you can use EF Core migrations:

1. Install EF Core Tools (if not already installed):
   ```bash
   dotnet tool install --global dotnet-ef
   ```

2. Create initial migration:
   ```bash
   cd backend
   dotnet ef migrations add InitialCreate
   ```

3. Apply migration to database:
   ```bash
   dotnet ef database update
   ```

This will create a `Migrations` folder with SQL files that you can review and version control.

## Database Schema Overview

### Identity Tables (ASP.NET Core Identity)
- `AspNetUsers` - User accounts with Role field
- `AspNetRoles` - User roles
- `AspNetUserRoles` - User-Role mapping
- `AspNetUserClaims` - User claims
- `AspNetUserLogins` - External login providers
- `AspNetUserTokens` - User tokens
- `AspNetRoleClaims` - Role claims

### Application Tables
- `Students` - Student information
- `Wardens` - Warden information
- `Rooms` - Room details
- `Attendances` - Attendance records
- `GatePassRequests` - Gate pass requests
- `RoomChangeRequests` - Room change requests
- `Grievances` - Student grievances

## Connection String Format

```
Server=localhost;Database=HostelManagementDB;User=root;Password=yourpassword;Port=3306;
```

Replace:
- `localhost` with your MySQL server address
- `root` with your MySQL username
- `yourpassword` with your MySQL password
- `3306` with your MySQL port (if different)

## Notes

- The SQL script includes `IF NOT EXISTS` clauses to prevent errors if tables already exist
- All foreign key constraints are set up for referential integrity
- Indexes are created for better query performance
- Unique constraints are applied where needed (e.g., RollNumber, RoomNumber)



