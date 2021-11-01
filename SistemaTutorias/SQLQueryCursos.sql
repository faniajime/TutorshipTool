CREATE PROCEDURE Courses_View_Edit
@mode NVARCHAR(MAX),
@nombre NVARCHAR(100) = NULL,
@detalles NVARCHAR(255) = NULL,
@area_especialidad INT = NULL

AS 
BEGIN
	SET NOCOUNT ON;
	IF(@mode ='getCoursesList')
	BEGIN
		SELECT 
		@nombre,
		@detalles,
		@area_especialidad
		FROM CURSO
	END	
END


DROP PROCEDURE EmployeeViewOrInsert

Select * FROM CURSO