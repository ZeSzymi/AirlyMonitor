CREATE TABLE Installations (
    Id int not null,
    AddressId uniqueidentifier not null,
    Elevation float not null,
    SponsorId UNIQUEIDENTIFIER not null,
    Latitude float not null,
    Longitude float not null,
    Deleted BIT not null DEFAULT 0
);

CREATE TABLE [Addresses] (
    AddressId uniqueidentifier not null,
    Country varchar(MAX) null,
    City varchar(MAX) null,
    Street varchar(MAX) null,
    DisplayAddress1 varchar(MAX) null,
    DisplayAddress2 varchar(MAX) null,
    [Number] varchar(MAX) null,
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
    Id uniqueidentifier not null,
    InstallationId int not null,
    FromDateTime DATETIME2 not null,
    TillDateTime DATETIME2 not null,
    [Values] VARCHAR(MAX) not null
)

CREATE TABLE AlertDefinitions (
    UserId nvarchar(450) not null,
    Id uniqueidentifier not null,
    InstallationId int not null,
    CheckEvery int not null, 
    Rules VARCHAR(MAX) not null,
    Deleted BIT not null DEFAULT 0,
    Active BIT not null DEFAULT 1,
    AQIThreshold Float null,
    [From] DATETIME2 null,
    [To] DATETIME2 null
)

CREATE Table Alerts (
    Id uniqueidentifier not null,
    AlertDefinitionId uniqueidentifier not null,
    InstallationId int not null,
    [DateTime] DATETIME2 not null,
    Reports VARCHAR(MAX) not null,
    AQIReport VARCHAR(MAX) not null,
    RaiseAlert BIT not null DEFAULT 0,
    PreviousRaisedAlert BIT not null DEFAULT 0
)

CREATE TABLE AspNetUsersInstallations (
    UserId nvarchar(450) not null,
    InstallationId int not null
)

ALTER TABLE Installations ADD CONSTRAINT PK_Installations PRIMARY KEY (Id);
ALTER TABLE Addresses ADD CONSTRAINT PK_Addresses PRIMARY KEY (AddressId);
ALTER TABLE Sponsors ADD CONSTRAINT PK_Sponsors PRIMARY KEY (SponsorId);
ALTER TABLE Measurements ADD CONSTRAINT PK_Measurements PRIMARY KEY (Id);
ALTER TABLE AlertDefinitions ADD CONSTRAINT PK_AlertDefinitions PRIMARY KEY (Id);
ALTER TABLE Alerts ADD CONSTRAINT PK_Alerts PRIMARY KEY (Id);
ALTER TABLE AspNetUsersInstallations ADD CONSTRAINT PK_AspNetUsersInstallations PRIMARY KEY (UserId, InstallationId);

ALTER TABLE Installations ADD CONSTRAINT FK_Installations_Addresses FOREIGN KEY (AddressId) REFERENCES [Addresses](AddressId);
ALTER TABLE Installations ADD CONSTRAINT FK_Installations_Sponsors FOREIGN KEY (SponsorId) REFERENCES [Sponsors](SponsorId);

ALTER TABLE AlertDefinitions ADD CONSTRAINT FK_AlertDefinitions_Installations FOREIGN KEY (InstallationId) REFERENCES [Installations](Id);
ALTER TABLE AlertDefinitions ADD CONSTRAINT FK_AlertDefinitions_AspNetUsers FOREIGN KEY (UserId) REFERENCES [AspNetUsers](Id);

ALTER TABLE Alerts ADD CONSTRAINT FK_Alerts_Installations FOREIGN KEY (InstallationId) REFERENCES [Installations](Id);
ALTER TABLE Alerts ADD CONSTRAINT FK_Alerts_AlertDefinitions FOREIGN KEY (AlertDefinitionId) REFERENCES [AlertDefinitions](Id) on delete cascade;;

ALTER TABLE Measurements ADD CONSTRAINT FK_Measurements_Installations FOREIGN KEY (InstallationId) REFERENCES Installations(Id);

ALTER TABLE AspNetUsersInstallations ADD CONSTRAINT FK_AspNetUsersInstallations_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id);
ALTER TABLE AspNetUsersInstallations ADD CONSTRAINT FK_AspNetUsersInstallations_Installations FOREIGN KEY (InstallationId) REFERENCES Installations(Id);