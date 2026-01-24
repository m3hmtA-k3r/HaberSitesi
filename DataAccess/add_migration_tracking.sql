-- Add EF Core Migration Tracking to Existing Database
-- This script PRESERVES all existing data and tables
-- SQL Server 2022 / SQL Server Express
-- Generated: 2026-01-23

USE HABER_SITESI;
GO

PRINT 'Starting migration tracking setup...';
GO

-- Step 1: Create __EFMigrationsHistory table if it doesn't exist
IF OBJECT_ID('__EFMigrationsHistory', 'U') IS NULL
BEGIN
    PRINT 'Creating __EFMigrationsHistory table...';

    CREATE TABLE __EFMigrationsHistory (
        MigrationId NVARCHAR(150) NOT NULL,
        ProductVersion NVARCHAR(32) NOT NULL,
        CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
    );

    PRINT '__EFMigrationsHistory table created successfully.';
END
ELSE
BEGIN
    PRINT '__EFMigrationsHistory table already exists.';
END
GO

-- Step 2: Add migration record (tells EF Core the migration is already applied)
IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260123050942_InitialSqlServerMigration')
BEGIN
    PRINT 'Adding migration record...';

    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion)
    VALUES ('20260123050942_InitialSqlServerMigration', '9.0.8');

    PRINT 'Migration record added successfully.';
END
ELSE
BEGIN
    PRINT 'Migration record already exists.';
END
GO

-- Step 3: Verify setup
PRINT '----------------------------------------';
PRINT 'Migration Tracking Setup Complete!';
PRINT '----------------------------------------';
PRINT '';
PRINT 'Current migrations in database:';
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory;
GO

PRINT '';
PRINT 'Existing tables in database:';
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
GO

PRINT '';
PRINT '✅ SUCCESS: Database is now ready for EF Core!';
PRINT 'All existing data has been preserved.';
GO
