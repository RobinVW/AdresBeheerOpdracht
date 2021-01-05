CREATE TABLE [dbo].[bestelling] (
    [bestellingID] INT             IDENTITY (1, 1) NOT NULL,
    [betaald]      BIT             NULL,
    [prijsBetaald] FLOAT           NOT NULL,
    [tijdstip]     DATETIME        NOT NULL,
    [klantID]      INT             NULL,
    PRIMARY KEY CLUSTERED ([bestellingID] ASC),
    FOREIGN KEY ([klantID]) REFERENCES [dbo].[klant] ([klantID])
);

