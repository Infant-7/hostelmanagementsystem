<<<<<<< HEAD
# Hostel Management System

A comprehensive full-stack hostel management system built with React frontend, .NET 8 Web API backend, and MySQL database.

## Features

### Public Pages
- **Home Page**: Welcome page with feature overview
- **About Us**: Information about the system
- **Contact**: Contact form and information

### Authentication
- Three login types: Student, Admin, Warden
- JWT-based authentication
- Role-based access control

### Admin Dashboard
- **Manage Students**: CRUD operations for student records
- **Manage Wardens**: CRUD operations for warden records
- **Attendance Management**: View, add, edit, and delete attendance records
- **Gate Pass Requests**: View and approve/reject gate pass requests
- **Room Change Requests**: View and approve/reject room change requests
- **Grievances**: View and resolve student grievances

### Warden Dashboard
- **Manage Students**: CRUD operations for student records
- **Attendance Management**: View, add, edit, and delete attendance records
- **Gate Pass Requests**: View and approve/reject gate pass requests
- **Room Change Requests**: View and approve/reject room change requests
- **Grievances**: View and resolve student grievances

### Student Dashboard
- **View Attendance**: Display personal attendance records
- **Request Gate Pass**: Submit gate pass requests
- **Request Room Change**: Submit room change requests
- **Grievances**: View own grievances and submit new ones

## Technology Stack

### Frontend
- React 19 with Vite
- React Router for navigation
- Tailwind CSS for styling
- Axios for API calls

### Backend
- .NET 8 Web API
- Entity Framework Core
- ASP.NET Core Identity
- MySQL Database (Pomelo.EntityFrameworkCore.MySql)
- JWT Authentication

## Project Structure

```
project/
├── frontend/              # React application
│   ├── src/
│   │   ├── components/   # Reusable components
│   │   ├── pages/        # Page components
│   │   ├── services/     # API service calls
│   │   ├── context/      # Auth context
│   │   └── utils/        # Utilities
│   └── package.json
├── backend/              # .NET 8 Web API
│   ├── Controllers/      # API controllers
│   ├── Models/           # Data models
│   ├── Data/             # DbContext and migrations
│   ├── Services/         # Business logic
│   ├── DTOs/             # Data transfer objects
│   └── Program.cs
└── README.md
```

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- Node.js and npm
- MySQL Server
- MySQL Workbench (optional, for database management)

### Backend Setup

1. Navigate to the backend directory:
```bash
cd backend
```

2. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=HostelManagementDB;User=root;Password=yourpassword;Port=3306;"
  }
}
```

3. Restore packages and build:
```bash
dotnet restore
dotnet build
```

4. Run the application:
```bash
dotnet run
```

The API will be available at `http://localhost:5000` (or the port specified in launchSettings.json).

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd frontend
```

2. Install dependencies:
```bash
npm install
```

3. Create a `.env` file (if not exists) and set the API URL:
```
VITE_API_BASE_URL=http://localhost:5000/api
```

4. Run the development server:
```bash
npm run dev
```

The frontend will be available at `http://localhost:5173`.

### Database Setup

1. Create a MySQL database named `HostelManagementDB` (or update the connection string).

2. The database will be automatically created when you run the backend application for the first time (using `EnsureCreated()` in Program.cs).

3. Alternatively, you can create migrations:
```bash
cd backend
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login
- `GET /api/auth/me` - Get current user (requires authentication)

### Students
- `GET /api/students` - Get all students (Admin/Warden only)
- `GET /api/students/{id}` - Get student by ID
- `POST /api/students` - Create student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

### Wardens
- `GET /api/wardens` - Get all wardens (Admin only)
- `GET /api/wardens/{id}` - Get warden by ID
- `POST /api/wardens` - Create warden
- `PUT /api/wardens/{id}` - Update warden
- `DELETE /api/wardens/{id}` - Delete warden

### Attendance
- `GET /api/attendance` - Get all attendance records (Admin/Warden)
- `GET /api/attendance/student/{studentId}` - Get student attendance
- `POST /api/attendance` - Create attendance record
- `PUT /api/attendance/{id}` - Update attendance
- `DELETE /api/attendance/{id}` - Delete attendance

### Gate Pass
- `GET /api/gatepass` - Get all gate pass requests (Admin/Warden)
- `GET /api/gatepass/student/{studentId}` - Get student requests
- `POST /api/gatepass` - Create gate pass request (Student)
- `PUT /api/gatepass/{id}/approve` - Approve/Reject request
- `DELETE /api/gatepass/{id}` - Delete request

### Room Change
- `GET /api/roomchange` - Get all room change requests (Admin/Warden)
- `GET /api/roomchange/student/{studentId}` - Get student requests
- `POST /api/roomchange` - Create room change request (Student)
- `PUT /api/roomchange/{id}/approve` - Approve/Reject request
- `DELETE /api/roomchange/{id}` - Delete request

### Grievances
- `GET /api/grievance` - Get all grievances (Admin/Warden)
- `GET /api/grievance/student/{studentId}` - Get student grievances
- `POST /api/grievance` - Create grievance (Student)
- `PUT /api/grievance/{id}/resolve` - Resolve grievance
- `DELETE /api/grievance/{id}` - Delete grievance

### Rooms
- `GET /api/rooms` - Get all rooms
- `GET /api/rooms/{id}` - Get room by ID
- `POST /api/rooms` - Create room
- `PUT /api/rooms/{id}` - Update room
- `DELETE /api/rooms/{id}` - Delete room

## Default Roles

The system supports three roles:
- **Student**: Can view attendance, request gate passes, request room changes, and submit grievances
- **Warden**: Can manage students, attendance, approve requests, and resolve grievances
- **Admin**: Full access including managing wardens

## Security

- JWT tokens for authentication
- Role-based authorization on backend controllers
- Protected routes on frontend
- Password hashing using ASP.NET Core Identity

## Development Notes

- The backend uses Entity Framework Core with MySQL
- CORS is configured to allow requests from the React frontend
- JWT tokens expire after 60 minutes (configurable in appsettings.json)
- The frontend stores tokens in localStorage

## License

This project is for educational purposes.



=======
# hostelmanagementsystem
This Hostel Management project was developed by Front -End React Js and the Back-End is .NET web api and the Database is Mysql Workbench
>>>>>>> c651bd1591dcafdf200cccf9b3324eca49b580dd
