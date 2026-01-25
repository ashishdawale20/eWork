-- 🔹 Check and Add Missing Columns in CrimeRegister table

-- अगर CrimeRegister टेबल में AccusedName नहीं है तो Add करें
IF COL_LENGTH('dbo.CrimeRegister', 'AccusedName') IS NULL
BEGIN
    ALTER TABLE dbo.CrimeRegister
    ADD AccusedName NVARCHAR(150) NULL;
    PRINT '✅ AccusedName column added successfully.';
END
ELSE
BEGIN
    PRINT '⚠️ AccusedName already exists.';
END

-- अगर Complainant कॉलम नहीं है तो Add करें
IF COL_LENGTH('dbo.CrimeRegister', 'Complainant') IS NULL
BEGIN
    ALTER TABLE dbo.CrimeRegister
    ADD Complainant NVARCHAR(150) NULL;
    PRINT '✅ Complainant column added successfully.';
END
ELSE
BEGIN
    PRINT '⚠️ Complainant already exists.';
END
