CREATE TABLE persona(
	
)




CREATE TABLE tutor(
	id INTEGER PRIMARY KEY,
	region VARCHAR(50)
)

CREATE TABLE tutoria(
	id_tutoria INTEGER PRIMARY KEY IDENTITY,
	tipo_sesion VARCHAR(20) NOT NULL,
	modalidad VARCHAR (10) NOT NULL,
	cantidad_estudiantes INTEGER NOT NULL,
	tarifa_individual INTEGER,
	tarifa_grupal INTEGER,
	duracion INTEGER NOT NULL,
	direccion VARCHAR (255),
	link VARCHAR (255),
	tutor_id INTEGER,
	FOREIGN KEY(tutor_id) REFERENCES tutor(id)
)

INSERT INTO tutoria(tipo_sesion,modalidad,cantidad_estudiantes,tarifa_individual, duracion,link) 
VALUES('individual','virtual',1,3000,60,'https://zoom.us/j/92519732767?pwd=cGlpUlJHZDUvWVVRSDRjemphRUltUT09')

SELECT *
FROM tutoria

DROP TABLE tutoria
DROP TABLE tutor
