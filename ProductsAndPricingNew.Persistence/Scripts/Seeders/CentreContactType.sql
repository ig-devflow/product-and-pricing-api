INSERT INTO [PricingRef].[CentreContactType] ([Id], [Name], [IsDeleted])
VALUES
    (1, N'Centre Director',    0),
    (2, N'Director of Studies', 0);


SET IDENTITY_INSERT [PricingRef].[CentreContactType] ON;

INSERT INTO [PricingRef].[CentreContactType] ([Id], [Name], [IsDeleted])
VALUES
    (1, N'Centre Director',     0),
    (2, N'Director of Studies', 0);

SET IDENTITY_INSERT [PricingRef].[CentreContactType] OFF;