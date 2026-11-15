
-- Create the database 
IF DB_ID('ShoeInventoryDB') IS NULL
BEGIN
    CREATE DATABASE ShoeInventoryDB;
END;
GO

USE ShoeInventoryDB;
GO


IF OBJECT_ID('Brands', 'U') IS NULL
BEGIN
    CREATE TABLE Brands
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(80) NOT NULL UNIQUE
    );
END;
GO


IF OBJECT_ID('Sizes', 'U') IS NULL
BEGIN
    CREATE TABLE Sizes
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Number INT NOT NULL UNIQUE,
        CONSTRAINT CK_Sizes_Number CHECK (Number > 0)
    );
END;
GO


IF OBJECT_ID('Shoes', 'U') IS NULL
BEGIN
    CREATE TABLE Shoes
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Model NVARCHAR(100) NOT NULL,
        BrandId INT NOT NULL,
        SizeId INT NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        Stock INT NOT NULL,

        CONSTRAINT FK_Shoes_Brands
            FOREIGN KEY (BrandId) REFERENCES Brands(Id),

        CONSTRAINT FK_Shoes_Sizes
            FOREIGN KEY (SizeId) REFERENCES Sizes(Id),

        CONSTRAINT CK_Shoes_Price
            CHECK (Price >= 0),

        CONSTRAINT CK_Shoes_Stock
            CHECK (Stock >= 0)
    );
END;
GO
