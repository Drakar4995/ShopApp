SET IDENTITY_INSERT [dbo].[Compra] ON
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (1, 50, '10/02/2019', 'calle la paloma 2', NULL, NULL)
INSERT INTO [dbo].[Compra] ([Id], [PrecioTotal], [FechaCompra], [DireccionEnvio], [ClienteId], [MetodoPagoID]) VALUES (2, 50, '10/03/2017', 'calle luis badia 1', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Compra] OFF