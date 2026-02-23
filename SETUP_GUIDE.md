# Complete Setup Guide - Step by Step

This guide will walk you through setting up and running the entire Hostel Management System project.

## Prerequisites Checklist

Before starting, make sure you have:
- ✅ .NET 8 SDK installed
- ✅ Node.js and npm installed
- ✅ MySQL Server installed and running
- ✅ MySQL Workbench (optional, for database management)
- ✅ Code editor (VS Code, Visual Studio, etc.)

---

## Step 1: Verify Prerequisites

### Check .NET SDK
```bash
dotnet --version
```
Should show version 8.x.x

### Check Node.js and npm
```bash
node --version
npm --version
```
Should show Node.js v16+ and npm v8+

### Check MySQL
```bash
mysql --version
```
Or verify MySQL service is running in Windows Services.

---

## Step 2: Database Setup

### Option A: Using MySQL Workbench (Recommended)

1. **Open MySQL Workbench**
2. **Connect to your MySQL server** (usually localhost, port 3306)
3. **Open the SQL script:**
   - Navigate to: `backend/database.sql`
   - Open it in MySQL Workbench
4. **Execute the script:**
   - Click the "Execute" button (⚡ icon) or press `Ctrl+Shift+Enter`
   - Wait for "Script executed successfully" message
5. **Verify database creation:**
   - In the left panel, refresh the schema list
   - You should see `HostelManagementDB` database

### Option B: Using Command Line

1. **Open Command Prompt or PowerShell**
2. **Navigate to project directory:**
   ```bash
   cd d:\project
   ```
3. **Run the SQL script:**
   ```bash
   mysql -u root -p < backend/database.sql
   ```
4. **Enter your MySQL password when prompted**

### Option C: Let Application Create It (Automatic)

If you skip this step, the application will create the database automatically when you run it for the first time.

---

## Step 3: Configure Backend Connection String

1. **Open:** `backend/appsettings.json`
2. **Update the connection string:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=HostelManagementDB;User=root;Password=YOUR_PASSWORD;Port=3306;"
     }
   }
   ```
   Replace `YOUR_PASSWORD` with your actual MySQL root password.
   
   If your MySQL has no password, use:
   ```json
   "DefaultConnection": "Server=localhost;Database=HostelManagementDB;User=root;Password=;Port=3306;"
   ```

---

## Step 4: Setup Backend (.NET API)

1. **Open Terminal/Command Prompt**
2. **Navigate to backend folder:**
   ```bash
   cd d:\project\backend
   ```
3. **Restore NuGet packages:**
   ```bash
   dotnet restore
   ```
4. **Build the project:**
   ```bash
   dotnet build
   ```
5. **Run the backend:**
   ```bash
   dotnet run
   ```

### Expected Output:
You should see something like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Application started. Press Ctrl+C to shut down.
```

**Keep this terminal window open!** The backend API is now running.

### Verify Backend is Running:
- Open browser and go to: `http://localhost:5000/swagger`
- You should see the Swagger API documentation page

---

## Step 5: Setup Frontend (React)

1. **Open a NEW Terminal/Command Prompt window** (keep backend running in the first one)

2. **Navigate to frontend folder:**
   ```bash
   cd d:\project\frontend
   ```

3. **Install dependencies:**
   ```bash
   npm install
   ```
   This may take a few minutes. Wait for it to complete.

4. **Create environment file (if needed):**
   - Check if `.env` file exists in `frontend` folder
   - If not, create it with:
     ```
     VITE_API_BASE_URL=http://localhost:5000/api
     ```

5. **Start the development server:**
   ```bash
   npm run dev
   ```

### Expected Output:
You should see something like:
```
  VITE v7.x.x  ready in xxx ms

  ➜  Local:   http://localhost:5173/
  ➜  Network: use --host to expose
```

**Keep this terminal window open too!**

---

## Step 6: Access the Application

1. **Open your web browser**
2. **Navigate to:** `http://localhost:5173`
3. **You should see the Home page** of the Hostel Management System

---

## Step 7: Test the Application

### Create Test Users

Since there are no users yet, you'll need to register or create them through the API.

#### Option A: Register via API (Using Swagger)

1. Go to: `http://localhost:5000/swagger`
2. Find `POST /api/auth/register`
3. Click "Try it out"
4. Use this example to create an Admin:
   ```json
   {
     "username": "admin",
     "email": "admin@hostel.com",
     "password": "Admin123!",
     "role": "Admin",
     "name": "Admin User"
   }
   ```
5. Click "Execute"
6. Copy the token from the response

#### Option B: Register via Frontend (if register page exists)

Or you can add a register endpoint to the frontend.

#### Option C: Use MySQL Workbench to Insert Admin

Run this SQL in MySQL Workbench:
```sql
USE HostelManagementDB;

-- Create Admin User (password: Admin123!)
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, Role)
VALUES (
    UUID(),
    'admin',
    'ADMIN',
    'admin@hostel.com',
    'ADMIN@HOSTEL.COM',
    1,
    'AQAAAAIAAYagAAAAE...', -- This is a hashed password, you'll need to generate it
    UUID(),
    UUID(),
    0,
    0,
    1,
    0,
    'Admin'
);
```

**Note:** For production, always use the registration API endpoint.

---

## Step 8: Login and Test

1. **Go to:** `http://localhost:5173`
2. **Click "Admin Login"** (or Student/Warden Login)
3. **Enter credentials:**
   - Username: `admin`
   - Password: `Admin123!` (or whatever you set)
   - Role: `Admin`
4. **Click Login**
5. **You should be redirected to the Admin Dashboard**

---

## Troubleshooting

### Backend Issues

**Problem: Cannot connect to database**
- ✅ Check MySQL service is running
- ✅ Verify connection string in `appsettings.json`
- ✅ Check MySQL username and password
- ✅ Ensure database `HostelManagementDB` exists

**Problem: Port 5000 already in use**
- Change port in `backend/Properties/launchSettings.json`
- Or kill the process using port 5000

**Problem: Build errors**
- Run `dotnet clean` then `dotnet restore`
- Check .NET SDK version: `dotnet --version`

### Frontend Issues

**Problem: Cannot connect to API**
- ✅ Check backend is running on port 5000
- ✅ Verify `.env` file has correct API URL
- ✅ Check browser console for CORS errors
- ✅ Restart frontend: `Ctrl+C` then `npm run dev`

**Problem: npm install fails**
- Clear npm cache: `npm cache clean --force`
- Delete `node_modules` folder and `package-lock.json`
- Run `npm install` again

**Problem: Port 5173 already in use**
- Vite will automatically use next available port
- Or specify port: `npm run dev -- --port 3000`

### Database Issues

**Problem: Database doesn't exist**
- Run the SQL script manually in MySQL Workbench
- Or let the application create it automatically

**Problem: Tables missing**
- Check if SQL script executed successfully
- Or run: `dotnet ef database update` (if using migrations)

---

## Quick Start Commands Summary

### Terminal 1 (Backend):
```bash
cd d:\project\backend
dotnet restore
dotnet run
```

### Terminal 2 (Frontend):
```bash
cd d:\project\frontend
npm install
npm run dev
```

### Browser:
```
http://localhost:5173
```

---

## Stopping the Application

1. **Stop Frontend:** Press `Ctrl+C` in the frontend terminal
2. **Stop Backend:** Press `Ctrl+C` in the backend terminal
3. **Stop MySQL:** (Optional) Stop MySQL service if needed

---

## Next Steps

1. Create test users (Admin, Warden, Student)
2. Add sample data (Rooms, Students, etc.)
3. Test all features:
   - Login for each role
   - CRUD operations
   - Request approvals
   - Attendance tracking

---

## Need Help?

- Check the main `README.md` for more details
- Review `backend/README_DATABASE.md` for database setup
- Check browser console (F12) for frontend errors
- Check backend terminal for API errors

