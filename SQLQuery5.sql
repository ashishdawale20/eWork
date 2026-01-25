USE CrimePortalDb;
GO

CREATE TABLE dbo.CrimeRegister (
    CrimeRegisterId INT IDENTITY(1,1) PRIMARY KEY,
    Year INT NULL,
    CaseNumber NVARCHAR(50) NULL,
    CrimeDate DATE NULL,
    TimeFrom TIME NULL,
    TimeTo TIME NULL,
    Complainant NVARCHAR(150) NULL,
    AccusedAge INT NULL,
    AccusedGender NVARCHAR(20) NULL,
    AccusedRelativeName NVARCHAR(150) NULL,
    AccusedAddress NVARCHAR(MAX) NULL,
    PlaceOfIncident NVARCHAR(MAX) NULL,
    Act NVARCHAR(200) NULL,
    Court NVARCHAR(100) NULL,
    PoliceStation NVARCHAR(100) NULL,
    InvestigatingOfficerName NVARCHAR(150) NULL,
    InvestigatingOfficerRank NVARCHAR(100) NULL,
    UserId NVARCHAR(50) NULL,
    OfficerRank NVARCHAR(100) NULL,
    OfficeName NVARCHAR(150) NULL,
    OfficeAddress NVARCHAR(MAX) NULL,
    District NVARCHAR(100) NULL,
    Division NVARCHAR(100) NULL
);
GO
