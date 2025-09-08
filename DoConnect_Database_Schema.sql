-- Database_Schema_DoConnect.sql
CREATE DATABASE DoConnectDb;
GO

USE DoConnectDb;
GO

-- Create Users table
CREATE TABLE Users (
    Id int IDENTITY(1,1) PRIMARY KEY,
    FirstName nvarchar(50) NOT NULL,
    LastName nvarchar(50) NOT NULL,
    Email nvarchar(256) NOT NULL UNIQUE,
    UserName nvarchar(256) NOT NULL UNIQUE,
    PasswordHash nvarchar(max) NOT NULL,
    IsActive bit DEFAULT 1,
    CreatedAt datetime2 DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 DEFAULT GETUTCDATE()
);

-- Create Questions table
CREATE TABLE Questions (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Title nvarchar(200) NOT NULL,
    Content nvarchar(max) NOT NULL,
    Topic nvarchar(100) NOT NULL,
    Status int DEFAULT 0, -- 0=Pending, 1=Approved, 2=Rejected
    UserId int NOT NULL,
    CreatedAt datetime2 DEFAULT GETUTCDATE(),
    UpdatedAt datetime2,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Create Answers table
CREATE TABLE Answers (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Content nvarchar(max) NOT NULL,
    Status int DEFAULT 0, -- 0=Pending, 1=Approved, 2=Rejected
    QuestionId int NOT NULL,
    UserId int NOT NULL,
    CreatedAt datetime2 DEFAULT GETUTCDATE(),
    UpdatedAt datetime2,
    FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Create Images table
CREATE TABLE Images (
    Id int IDENTITY(1,1) PRIMARY KEY,
    FileName nvarchar(255) NOT NULL,
    FilePath nvarchar(255) NOT NULL,
    ContentType nvarchar(100) NOT NULL,
    FileSize bigint NOT NULL,
    QuestionId int NULL,
    AnswerId int NULL,
    UploadedAt datetime2 DEFAULT GETUTCDATE(),
    FOREIGN KEY (QuestionId) REFERENCES Questions(Id),
    FOREIGN KEY (AnswerId) REFERENCES Answers(Id)
);

-- Create additional tables as needed...
