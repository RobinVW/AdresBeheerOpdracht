CREATE TABLE [dbo].[klant] (
    [klantID] INT            IDENTITY (1, 1) NOT NULL,
    [naam]    NVARCHAR (50)  NOT NULL,
    [adres]   NVARCHAR (400) NOT NULL,
    PRIMARY KEY CLUSTERED ([KlantID] ASC),
	UNIQUE (naam,adres)
);

