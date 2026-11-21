-- create DATABASE Ewallet

-- drop TABLE Users
-- DROP TABLE Wallets
-- drop TABLE Transactions

-- CREATE TABLE Users(
--     Id int IDENTITY(1,1) PRIMARY key,
--     Name NVARCHAR(50) NOT NULL,
--     Email NVARCHAR(100) NOT NULL UNIQUE,
--     Phone NVARCHAR(20) not NULL,
--     CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
-- )

-- CREATE Table Wallets(
--     Id int IDENTITY(1,1) PRIMARY KEY,
--     UsersId int not null UNIQUE,
--     WalletNumber NVARCHAR(50) NOT NULL UNIQUE,
--     Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
--     CreatedAt DATETIME NOT NULL DEFAULT GETDATE()

--     CONSTRAINT FK_Wallets_Users Foreign Key (UsersId) REFERENCES Users(Id)
-- )

-- CREATE Table Transactions(
--     Id int IDENTITY(1,1) PRIMARY KEY,
--     SenderWalletId int,
--     ReceiverWalletId int NOT NULL,
--     Type NVARCHAR(50) not NULL,
--     Amount DECIMAL(18,2) NOT NULL DEFAULT 0,
--     Description TEXT,
--     CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

--     CONSTRAINT FK_Transactions_Sender Foreign Key (SenderWalletId) REFERENCES Wallets(Id),
--     CONSTRAINT FK_Transactions_Receiver Foreign Key (ReceiverWalletId) REFERENCES Wallets(Id)
-- )

-- select * from Users
-- select * from Wallets
-- select * from Transactions


create DATABASE EwalletWeekly6

EXEC sp_help 'Users';


select * from Users
select * from Wallets
select * from PaymentMethods

select * from Transactions

select
    t.ReferenceNo,
    t.Type,
    t.Amount,
    t.Status,
    pm.Code
from Transactions t
join TopUpRequests tp on tp.TransactionId = t.Id
join PaymentMethods pm on pm.Id = tp.PaymentMethodId


select * from AuditLogs
select * from WalletMutations
select * from TopUpRequests


SELECT *
FROM Wallets
WHERE Id = 10;


create DATABASE EwalletWeekly7
GO
EXEC sp_tables @table_owner = 'dbo';

select * from dbo.AspNetUsers;
