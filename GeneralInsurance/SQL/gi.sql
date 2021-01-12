create database GeneralInsurance

create table USERS(
UserId int Identity not null PRIMARY KEY,
Name varchar(255),
Email varchar(255),
ContactNo bigint,
DateOfBirth datetime,
Address varchar(255),
DrivingLiscence varchar(255),
Password varchar(255)
)

create table MOTOR(
MotorId int Identity not null PRIMARY KEY,
ManufactureYear int,
Model varchar(255),
Type varchar(255),
PurchaseDate datetime,
RegNo int,
EngineNo int,
ChasisNo int,
UserId int,
FOREIGN KEY (UserId) REFERENCES USERS(UserId)
)
create table INSURANCE(
InsuranceId int Identity(1000,1) not null PRIMARY KEY,
Plans varchar(255),
Duration int,
PolicyStartDate datetime,
PolicyEndDate datetime,
Amount int,
UserId int,
MotorId int,
FOREIGN KEY (UserId) REFERENCES USERS(UserId),
FOREIGN KEY (MotorId) REFERENCES MOTOR(MotorId)
)
create table CLAIM(
ClaimId int Identity(1000,1) not null PRIMARY KEY,
ClaimDate datetime,
ApprovalStatus varchar(255),
ClaimAmount int,
ReasonToClaim varchar(255),
InsuranceId int,
UserId int,
FOREIGN KEY (UserId) REFERENCES USERS(UserId),
FOREIGN KEY (InsuranceId) REFERENCES INSURANCE(InsuranceId)
)
create table ADMINS(
AdminId int Identity not null PRIMARY KEY,
Password varchar(30),
EmailId varchar(50)
)

create table Transactions(
TransactionId int Identity(1000,1) primary key,
Amount int,
Date datetime,
UserId int,
FOREIGN KEY (UserId) REFERENCES USERS(UserId)
)