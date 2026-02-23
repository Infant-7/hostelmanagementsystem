-- =============================================
-- Fix Database Tables Script
-- Run this to fix the Students and RoomChangeRequests tables
-- =============================================

USE HostelManagementDB;

-- First, check if Students table has StudentId column
-- If not, we need to drop and recreate it

-- Drop RoomChangeRequests first (if it exists) since it depends on Students
DROP TABLE IF EXISTS RoomChangeRequests;

-- Drop Students table to recreate with correct structure
DROP TABLE IF EXISTS Students;

-- Recreate Students table with correct structure
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

-- Create indexes for Students
CREATE INDEX IX_Students_UserId ON Students(UserId);
CREATE INDEX IX_Students_RollNumber ON Students(RollNumber);

-- Now create RoomChangeRequests table
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

-- Create indexes for RoomChangeRequests
CREATE INDEX IX_RoomChangeRequests_StudentId ON RoomChangeRequests(StudentId);
CREATE INDEX IX_RoomChangeRequests_CurrentRoomId ON RoomChangeRequests(CurrentRoomId);
CREATE INDEX IX_RoomChangeRequests_RequestedRoomId ON RoomChangeRequests(RequestedRoomId);
CREATE INDEX IX_RoomChangeRequests_Status ON RoomChangeRequests(Status);

-- Verify tables were created
SELECT 'Students table structure:' AS Info;
DESCRIBE Students;

SELECT 'RoomChangeRequests table structure:' AS Info;
DESCRIBE RoomChangeRequests;
