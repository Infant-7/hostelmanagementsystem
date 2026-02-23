-- =============================================
-- Fix Students Table Structure
-- This script will fix the Students table to have StudentId as primary key
-- =============================================

USE HostelManagementDB;

-- Step 1: Check current structure
SELECT 'Current Students table structure:' AS Info;
DESCRIBE Students;

-- Step 2: Check if there's any data (backup first if needed)
SELECT COUNT(*) AS StudentCount FROM Students;

-- Step 3: Drop dependent tables that reference Students
DROP TABLE IF EXISTS RoomChangeRequests;
DROP TABLE IF EXISTS Attendances;
DROP TABLE IF EXISTS GatePassRequests;
DROP TABLE IF EXISTS Grievances;

-- Step 4: Drop the Students table
DROP TABLE IF EXISTS Students;

-- Step 5: Recreate Students table with correct structure
CREATE TABLE Students (
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

-- Step 6: Create indexes
CREATE INDEX IX_Students_UserId ON Students(UserId);
CREATE INDEX IX_Students_RollNumber ON Students(RollNumber);

-- Step 7: Recreate dependent tables
CREATE TABLE Attendances (
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

CREATE TABLE GatePassRequests (
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

CREATE TABLE Grievances (
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

-- Step 8: Create RoomChangeRequests table
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

-- Step 9: Verify the structure
SELECT 'Students table structure after fix:' AS Info;
DESCRIBE Students;

SELECT 'All tables created successfully!' AS Status;
