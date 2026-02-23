-- =============================================
-- Hostel Management System Database Schema
-- MySQL Database Script
-- =============================================
-- NOTE: If you get errors about indexes already existing, you can safely ignore them.
-- The tables will still be created correctly.

-- Create Database
CREATE DATABASE IF NOT EXISTS HostelManagementDB;
USE HostelManagementDB;

-- =============================================
-- ASP.NET Core Identity Tables
-- =============================================

-- AspNetUsers table (extends IdentityUser with Role)
CREATE TABLE IF NOT EXISTS AspNetUsers (
    Id VARCHAR(255) NOT NULL PRIMARY KEY,
    UserName VARCHAR(256) NULL,
    NormalizedUserName VARCHAR(256) NULL,
    Email VARCHAR(256) NULL,
    NormalizedEmail VARCHAR(256) NULL,
    EmailConfirmed TINYINT(1) NOT NULL DEFAULT 0,
    PasswordHash LONGTEXT NULL,
    SecurityStamp LONGTEXT NULL,
    ConcurrencyStamp LONGTEXT NULL,
    PhoneNumber LONGTEXT NULL,
    PhoneNumberConfirmed TINYINT(1) NOT NULL DEFAULT 0,
    TwoFactorEnabled TINYINT(1) NOT NULL DEFAULT 0,
    LockoutEnd DATETIME(6) NULL,
    LockoutEnabled TINYINT(1) NOT NULL DEFAULT 0,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    Role VARCHAR(50) NULL
);

-- Create indexes (you can ignore errors if indexes already exist)
CREATE INDEX EmailIndex ON AspNetUsers(NormalizedEmail);
CREATE UNIQUE INDEX UserNameIndex ON AspNetUsers(NormalizedUserName);

-- AspNetRoles table
CREATE TABLE IF NOT EXISTS AspNetRoles (
    Id VARCHAR(255) NOT NULL PRIMARY KEY,
    Name VARCHAR(256) NULL,
    NormalizedName VARCHAR(256) NULL,
    ConcurrencyStamp LONGTEXT NULL
);

CREATE UNIQUE INDEX RoleNameIndex ON AspNetRoles(NormalizedName);

-- AspNetUserRoles table
CREATE TABLE IF NOT EXISTS AspNetUserRoles (
    UserId VARCHAR(255) NOT NULL,
    RoleId VARCHAR(255) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

CREATE INDEX IX_AspNetUserRoles_RoleId ON AspNetUserRoles(RoleId);

-- AspNetUserClaims table
CREATE TABLE IF NOT EXISTS AspNetUserClaims (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(255) NOT NULL,
    ClaimType LONGTEXT NULL,
    ClaimValue LONGTEXT NULL,
    CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

CREATE INDEX IX_AspNetUserClaims_UserId ON AspNetUserClaims(UserId);

-- AspNetUserLogins table
CREATE TABLE IF NOT EXISTS AspNetUserLogins (
    LoginProvider VARCHAR(255) NOT NULL,
    ProviderKey VARCHAR(255) NOT NULL,
    ProviderDisplayName LONGTEXT NULL,
    UserId VARCHAR(255) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey),
    CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

CREATE INDEX IX_AspNetUserLogins_UserId ON AspNetUserLogins(UserId);

-- AspNetUserTokens table
CREATE TABLE IF NOT EXISTS AspNetUserTokens (
    UserId VARCHAR(255) NOT NULL,
    LoginProvider VARCHAR(255) NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Value LONGTEXT NULL,
    PRIMARY KEY (UserId, LoginProvider, Name),
    CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

-- AspNetRoleClaims table
CREATE TABLE IF NOT EXISTS AspNetRoleClaims (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    RoleId VARCHAR(255) NOT NULL,
    ClaimType LONGTEXT NULL,
    ClaimValue LONGTEXT NULL,
    CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

CREATE INDEX IX_AspNetRoleClaims_RoleId ON AspNetRoleClaims(RoleId);

-- =============================================
-- Custom Application Tables
-- =============================================

-- Students table
CREATE TABLE IF NOT EXISTS Students (
    StudentId INT AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(255) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    RollNumber VARCHAR(50) NOT NULL,
    RoomNumber VARCHAR(20) NULL,
    Phone VARCHAR(15) NULL,
    Address VARCHAR(500) NULL,
    CONSTRAINT FK_Students_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Students_RollNumber UNIQUE (RollNumber),
    CONSTRAINT UQ_Students_UserId UNIQUE (UserId)
);

CREATE INDEX IX_Students_UserId ON Students(UserId);
CREATE INDEX IX_Students_RollNumber ON Students(RollNumber);

-- Wardens table
CREATE TABLE IF NOT EXISTS Wardens (
    WardenId INT AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(255) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NULL,
    Phone VARCHAR(15) NULL,
    Department VARCHAR(100) NULL,
    CONSTRAINT FK_Wardens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Wardens_UserId UNIQUE (UserId)
);

CREATE INDEX IX_Wardens_UserId ON Wardens(UserId);

-- Rooms table
CREATE TABLE IF NOT EXISTS Rooms (
    RoomId INT AUTO_INCREMENT PRIMARY KEY,
    RoomNumber VARCHAR(20) NOT NULL,
    Floor INT NOT NULL,
    Capacity INT NOT NULL,
    Occupied INT NOT NULL DEFAULT 0,
    Status VARCHAR(50) NOT NULL DEFAULT 'Available',
    CONSTRAINT UQ_Rooms_RoomNumber UNIQUE (RoomNumber)
);

CREATE INDEX IX_Rooms_RoomNumber ON Rooms(RoomNumber);

-- Attendance table
CREATE TABLE IF NOT EXISTS Attendances (
    AttendanceId INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT NOT NULL,
    Date DATETIME(6) NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Present',
    MarkedBy VARCHAR(450) NULL,
    CONSTRAINT FK_Attendances_Students_StudentId FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
    CONSTRAINT UQ_Attendances_StudentId_Date UNIQUE (StudentId, Date)
);

CREATE INDEX IX_Attendances_StudentId ON Attendances(StudentId);
CREATE INDEX IX_Attendances_Date ON Attendances(Date);

-- GatePassRequests table
CREATE TABLE IF NOT EXISTS GatePassRequests (
    RequestId INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT NOT NULL,
    RequestDate DATETIME(6) NOT NULL,
    Purpose VARCHAR(500) NOT NULL,
    OutTime DATETIME(6) NOT NULL,
    InTime DATETIME(6) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    ApprovedBy VARCHAR(450) NULL,
    CONSTRAINT FK_GatePassRequests_Students_StudentId FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
);

CREATE INDEX IX_GatePassRequests_StudentId ON GatePassRequests(StudentId);
CREATE INDEX IX_GatePassRequests_Status ON GatePassRequests(Status);

-- RoomChangeRequests table
-- Drop table if it exists to recreate with correct structure
DROP TABLE IF EXISTS RoomChangeRequests;

CREATE TABLE RoomChangeRequests (
    RequestId INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT NOT NULL,
    CurrentRoomId INT NOT NULL,
    RequestedRoomId INT NOT NULL,
    Reason VARCHAR(1000) NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    ApprovedBy VARCHAR(450) NULL,
    CONSTRAINT FK_RoomChangeRequests_Students_StudentId FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE,
    CONSTRAINT FK_RoomChangeRequests_Rooms_CurrentRoomId FOREIGN KEY (CurrentRoomId) REFERENCES Rooms(RoomId) ON DELETE CASCADE,
    CONSTRAINT FK_RoomChangeRequests_Rooms_RequestedRoomId FOREIGN KEY (RequestedRoomId) REFERENCES Rooms(RoomId) ON DELETE CASCADE
);

CREATE INDEX IX_RoomChangeRequests_StudentId ON RoomChangeRequests(StudentId);
CREATE INDEX IX_RoomChangeRequests_CurrentRoomId ON RoomChangeRequests(CurrentRoomId);
CREATE INDEX IX_RoomChangeRequests_RequestedRoomId ON RoomChangeRequests(RequestedRoomId);
CREATE INDEX IX_RoomChangeRequests_Status ON RoomChangeRequests(Status);

-- Grievances table
CREATE TABLE IF NOT EXISTS Grievances (
    GrievanceId INT AUTO_INCREMENT PRIMARY KEY,
    StudentId INT NOT NULL,
    Title VARCHAR(200) NOT NULL,
    Description VARCHAR(2000) NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    CreatedDate DATETIME(6) NOT NULL,
    ResolvedDate DATETIME(6) NULL,
    ResolvedBy VARCHAR(450) NULL,
    CONSTRAINT FK_Grievances_Students_StudentId FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
);

CREATE INDEX IX_Grievances_StudentId ON Grievances(StudentId);
CREATE INDEX IX_Grievances_Status ON Grievances(Status);
CREATE INDEX IX_Grievances_CreatedDate ON Grievances(CreatedDate);

-- =============================================
-- Sample Data (Optional - for testing)
-- =============================================

-- Insert sample roles
INSERT IGNORE INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES
('1', 'Student', 'STUDENT', UUID()),
('2', 'Admin', 'ADMIN', UUID()),
('3', 'Warden', 'WARDEN', UUID());

-- =============================================
-- End of Script
-- =============================================



