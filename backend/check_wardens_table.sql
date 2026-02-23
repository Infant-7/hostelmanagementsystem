-- Script to check and verify the Wardens table structure
-- Run this in MySQL Workbench to verify the table exists and has the correct structure

USE HostelManagementDB;

-- Check if the table exists
SHOW TABLES LIKE 'Wardens';

-- Check the table structure
DESCRIBE Wardens;

-- Check if there are any wardens
SELECT COUNT(*) as WardenCount FROM Wardens;

-- If the table doesn't exist or has wrong structure, run this:
DROP TABLE IF EXISTS Wardens;

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

-- Verify the table was created
DESCRIBE Wardens;







