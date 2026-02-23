-- =============================================
-- Check and Fix Database Tables
-- Run this step by step
-- =============================================

USE HostelManagementDB;

-- Step 1: Check the current structure of Students table
SELECT 'Current Students table structure:' AS Info;
DESCRIBE Students;

-- Step 2: Check if RoomChangeRequests exists
SELECT 'Checking RoomChangeRequests table:' AS Info;
SHOW TABLES LIKE 'RoomChangeRequests';

-- Step 3: If Students table doesn't have StudentId, we need to fix it
-- First, let's see what columns it has
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_KEY
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'HostelManagementDB' 
  AND TABLE_NAME = 'Students';
