# User Credentials Guide

## How to Create Users and Login

### Step 1: Create Users via Swagger API

Go to: `http://localhost:5000/swagger`

Find: `POST /api/Auth/register` → Click "Try it out"

---

## Admin User

**Create Admin:**
```json
{
  "username": "admin",
  "email": "admin@hostel.com",
  "password": "Admin123!",
  "role": "Admin",
  "name": "Admin User"
}
```

**Login Credentials:**
- Username: `admin`
- Password: `Admin123!`
- Role: `Admin`

**Login URL:** `http://localhost:5173` → Click "Admin Login"

---

## Student User

**Create Student:**
```json
{
  "username": "student1",
  "email": "student1@hostel.com",
  "password": "Student123!",
  "role": "Student",
  "name": "John Doe",
  "rollNumber": "STU001",
  "phone": "1234567890",
  "address": "123 Main Street"
}
```

**Login Credentials:**
- Username: `student1`
- Password: `Student123!`
- Role: `Student`

**Login URL:** `http://localhost:5173` → Click "Student Login"

---

## Warden User

**Create Warden:**
```json
{
  "username": "warden1",
  "email": "warden1@hostel.com",
  "password": "Warden123!",
  "role": "Warden",
  "name": "Jane Smith",
  "department": "Computer Science",
  "phone": "0987654321"
}
```

**Login Credentials:**
- Username: `warden1`
- Password: `Warden123!`
- Role: `Warden`

**Login URL:** `http://localhost:5173` → Click "Warden Login`

---

## Quick Reference

| Role | Username | Password | Dashboard URL |
|------|----------|----------|---------------|
| Admin | `admin` | `Admin123!` | After login → `/admin/dashboard` |
| Student | `student1` | `Student123!` | After login → `/student/dashboard` |
| Warden | `warden1` | `Warden123!` | After login → `/warden/dashboard` |

---

## Step-by-Step Login Process

1. **Go to:** `http://localhost:5173`
2. **Click** the appropriate login button (Admin Login, Student Login, or Warden Login)
3. **Enter credentials:**
   - Username
   - Password
   - Role (must match: Admin, Student, or Warden)
4. **Click "Login"**
5. **You'll be redirected** to the appropriate dashboard

---

## Notes

- **Role is case-insensitive** - You can use "admin", "Admin", or "ADMIN"
- **Password requirements:**
  - Minimum 6 characters
  - Must contain uppercase, lowercase, and digit
- **If login fails:** Check that the user exists and credentials are correct
- **First time:** You must create users via Swagger before you can login

---

## Troubleshooting

**Problem: "Invalid credentials"**
- Make sure the user was created successfully in Swagger
- Check username, password, and role are correct
- Role must match exactly (case-insensitive)

**Problem: "Invalid role for this login"**
- Make sure the role matches the login type
- Admin login → role must be "Admin"
- Student login → role must be "Student"
- Warden login → role must be "Warden"

**Problem: Can't create user**
- Check if username or email already exists
- Make sure all required fields are filled
- Check password meets requirements (6+ chars, uppercase, lowercase, digit)
