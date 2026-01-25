USE CrimePortalDb;
GO

/* 1️⃣ पंचांचे तपशील */
IF OBJECT_ID('dbo.Panchas', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Panchas (
        PanchaId INT IDENTITY(1,1) PRIMARY KEY,
        CrimeRegisterId INT NOT NULL,
        Name NVARCHAR(150) NULL,
        Age INT NULL,
        Address NVARCHAR(MAX) NULL,
        FOREIGN KEY (CrimeRegisterId) REFERENCES dbo.CrimeRegister(CrimeRegisterId)
    );
END
GO

/* 2️⃣ जप्त मुद्देमाल */
IF OBJECT_ID('dbo.SeizedItems', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SeizedItems (
        SeizedItemId INT IDENTITY(1,1) PRIMARY KEY,
        CrimeRegisterId INT NOT NULL,
        PropertyClass NVARCHAR(100) NULL,
        PropertyType NVARCHAR(100) NULL,
        Description NVARCHAR(MAX) NULL,
        ApproxValue DECIMAL(18,2) NULL,
        FOREIGN KEY (CrimeRegisterId) REFERENCES dbo.CrimeRegister(CrimeRegisterId)
    );
END
GO

/* 3️⃣ नमुना माहिती */
IF OBJECT_ID('dbo.Samples', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Samples (
        SampleId INT IDENTITY(1,1) PRIMARY KEY,
        CrimeRegisterId INT NOT NULL,
        CarrierName NVARCHAR(150) NULL,
        SampleNumber INT NULL,
        SampleType NVARCHAR(300) NULL,
        FOREIGN KEY (CrimeRegisterId) REFERENCES dbo.CrimeRegister(CrimeRegisterId)
    );
END
GO
