INSERT INTO [ReferenceData].[UnitType] ([Name], [Description], [IsDateBased], [CalculationKind], [MinMajorUnits], [MinMinorUnits], [IsDeleted])
VALUES
    (N'Any', N'', 0, 0, 0, 0, 0),
    (N'Fixed price', N'', 0, 1, 1, 0, 0),
    (N'Days', N'', 1, 2, 1, 0, 0),
    (N'Calendar Weeks', N'', 1, 3, 0, 1, 0),
    (N'Calendar Night Weeks', N'', 1, 4, 0, 1, 0),
    (N'Working Weeks', N'', 1, 5, 0, 1, 0),
    (N'Months',  N'', 1, 6, 1, 0, 0);