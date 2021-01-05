CREATE TABLE [dbo].[bestellingDetails]
(
	[productID]	INT NOT NULL,
	[bestellingID] INT NOT NULL,
	PRIMARY KEY CLUSTERED ([productID] ASC, [bestellingID] ASC),
    CONSTRAINT [productID] FOREIGN KEY ([productID]) REFERENCES [dbo].[product]([productID]), 
	CONSTRAINT [bestellingID] FOREIGN KEY ([bestellingID]) REFERENCES [dbo].[bestelling]([bestellingID])
)
