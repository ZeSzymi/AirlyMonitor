CREATE TABLE Installations (
    Id int not null,
    AddressId uniqueidentifier not null,
    Elevation float not null,
    SponsorId UNIQUEIDENTIFIER not null,
    Latitude float not null,
    Longitude float not null
);

CREATE TABLE [Addresses] (
    AddressId uniqueidentifier not null,
    Country varchar(MAX) null,
    City varchar(MAX) null,
    Street varchar(MAX) null,
    DisplayAddress1 varchar(MAX) null,
    DisplayAddress2 varchar(MAX) null,
    [Number] int null,
);

CREATE TABLE Sponsors (
    SponsorId UNIQUEIDENTIFIER not null,
    [Name] varchar(MAX) null,
    Logo varchar(MAX) null,
    Link varchar(MAX) null,
    DisplayName varchar(MAX) null,
    Id int not null,
    [Number] int null,
);

CREATE TABLE Measurements (
    InstallationId int not null,
    FromDateTime DATETIME2 not null,
    TillDateTime DATETIME2 not null,
    [Values] VARCHAR(MAX) not null
)


ALTER TABLE Installations ADD CONSTRAINT PK_Installations PRIMARY KEY (Id);
ALTER TABLE Addresses ADD CONSTRAINT PK_Addresses PRIMARY KEY (AddressId);
ALTER TABLE Sponsors ADD CONSTRAINT PK_Sponsors PRIMARY KEY (SponsorId);
ALTER TABLE Measurements ADD CONSTRAINT PK_Measurements PRIMARY KEY (InstallationId);
ALTER TABLE Installations ADD CONSTRAINT FK_Installations_Addresses FOREIGN KEY (AddressId) REFERENCES [Addresses](AddressId);
ALTER TABLE Installations ADD CONSTRAINT FK_Installations_Sponsors FOREIGN KEY (SponsorId) REFERENCES [Sponsors](SponsorId);
ALTER TABLE Measurements ADD CONSTRAINT FK_Measurements_Installations FOREIGN KEY (InstallationId) REFERENCES Installations(Id);
