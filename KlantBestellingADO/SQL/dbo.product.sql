CREATE TABLE [dbo].[product] (
    [productID] INT             IDENTITY (1, 1) NOT NULL,
    [naam]      NVARCHAR (50)   NOT NULL,
    [prijs]     FLOAT NOT NULL,
    PRIMARY KEY CLUSTERED ([productID] ASC),
    UNIQUE NONCLUSTERED ([naam] ASC)
);

