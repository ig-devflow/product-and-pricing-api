SET IDENTITY_INSERT [PricingRef].[PrintFormat] ON;

INSERT INTO [PricingRef].[PrintFormat] ([Id], [Name], [IsDeleted])
VALUES
    (1, N'A4', 0),
    (2, N'Letter', 0);

SET IDENTITY_INSERT [PricingRef].[PrintFormat] OFF;