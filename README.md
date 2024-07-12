# pruebaFGRP
Backend desarrollado en C# para las historias de usuario

### HISTORIA DE USUARIO: API GATEWAY
### HISTORIA DE USUARIO: CREACIÓN DE WORKER SERVICE PARA PROCESAMIENTO DE ÓRDENES EN SEGUNDO PLANO

Para inicializar el proyecto se debe clonar el proyecto y ejecutar en la carpeta raíz `docker compose up`, debería inicializar todos los procesos y empezar su funcionamiento con una llamada a un servicio en donde se podrá validar el API Gateway Swagger y en la consola, se observara los mensajes de las ordenes funcionando en segundo plano.

Tanto el backend como la base de datos, están dentro del mismo contenedor y configurados por default.

**Los accesos para generar el token JWT son:** `admin@domain.com /	admin$2020` aunque utilizando el mismo API se pueden registrar usuarios nuevos.
