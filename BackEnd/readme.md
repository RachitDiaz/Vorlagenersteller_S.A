# Back-end Volagenersteller

## Paquetes necesarios para correr el proyecto back-end

Colocarse en la carpeta raiz del proyecto ".../backend-planilla" y correr los siguientes comandos:
  * `dotnet add package System.Data.SqlClient`
  * `dotnet add package Microsoft.AspNetCore.Mvc.Core`
  * `dotnet add package Microsoft.AspNetCore.Http.Abstractions`
  * `dotnet add package Microsoft.Data.SqlClient`

## Volagenersteller Planilla 

Este proyecto de back-end desarrollado con .NET(8.0) se complementa con la parte front-end (más información en [readme.md](/FrontEnd/readme.md)) desarrollada en la carpeta [FrontEnd](/FrontEnd).

### Para correr el proyecto back-end de la planilla

Desde Visual Studio Community 2022 abra el [archivo de solución del proyecto en la carpeta backend-planilla](/BackEnd/backend-planilla) llamado backend-planilla.sln y presionar f5 para correr el proyecto.

## Volagenersteller API-MediSeguro

Este proyecto de back-end desarrollado con .NET(8.0) calcula el beneficio de seguro y retorna un monto en colones correspondiente a la aplicación de ciertos criterios definidos por los PO, se reciben 3 parámetros que indican sexo, fecha de nacimiento y cantidad de dependientes.

### Para correr el proyecto back-end de la API-MediSeguro

Desde Visual Studio Community 2022 abra el [archivo de solución del proyecto en la carpeta backend-planilla](/BackEnd/API-MediSeguro/) llamado API-MediSeguro.sln y presionar f5 para correr el proyecto.
