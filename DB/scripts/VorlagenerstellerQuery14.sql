USE VorlaDB

ALTER TABLE Empleado
ADD Editable bit default 1; 
go

CREATE TRIGGER trg_UpdateEditableEmpleado
ON PlanillaMensualEmpleado
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE E
    SET E.Editable = 0
    FROM Empleado E
    INNER JOIN inserted I ON E.CedulaEmpleado = I.CedulaEmpleado
END;