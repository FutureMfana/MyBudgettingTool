CREATE SCHEMA Bugdet;
GO

/*CREATE TABLE Budget.UserInfo(
	UserID INT IDENTITY(1,1) PRIMARY KEY,
	Salary INT NOT NULL,
	Campony NVARCHAR(40) NOT NULL,
	HireDate DATE,
	Position NVARCHAR(40),
	Email NVARCHAR(40),
	Password CHAR(14)
);
GO*/

CREATE TABLE MyBudget(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Expenses NVARCHAR(40) NOT NULL,
	Costs INT NOT NULL,
	CostsInPercent AS ((Costs / 7302) * 100),
	Month_ TinyInt NOT NULL,
	Year_ INT NOT NULL
);
GO

CREATE VIEW Budget_TBL
AS
	SELECT ID AS [ROWID], Expenses AS [Expense], Costs AS [Total Price], CostsInPercent AS [Total Price In Percent], Month_ AS [Month], Year_ AS [Year]  
	FROM MyBudget
GO