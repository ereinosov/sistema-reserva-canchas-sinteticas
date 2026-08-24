/* Si A11 ya se ejecutó y falla "User does not have permission to perform this action"
   al crear el primer administrador, ejecute solo este lote. */
ALTER DATABASE ReservaCanchasDB SET TRUSTWORTHY ON;
GO
