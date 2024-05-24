# ShoppinCart

## Desarrollador: Fernando Garcia

## Descripción
Este documento detalla los pasos necesarios para iniciar y utilizar el proyecto "Nombre del Proyecto". Soy el desarrollador principal de este proyecto y he creado esta guía para facilitar su implementación y uso.

### 1. Configurar la cadena de conexión
La cadena de conexión a la base de datos donde se almacenarán los diferentes registros, esta cadena tiene la siguiente estructura:

```
"DefaultConnection": "Server=NombreServidor; Database=ShoppingCart; User=Usuario; Password=Clave; MultipleActiveResultSets=True; Encrypt=True;TrustServerCertificate=True"`
```

Esta cadena se encuentra en el archivo `appsetting.json`, asegúrese de cabiar los valores de NombreServidor por el nombre del servidor a la base de datos a la que se quiere conectar, al igual que el usuario y la contraseña. Se recomienda dejar el nombre de la base de datos tal y como está, si ya tiene una base de datos con ese nombre entonce si cambiarla.

### 2. Hacer la migracion a la base de datos
Una vez hecho el cambio en la cadena de conexión se debe hacer la migración esto con el objetivo que se cree la base de datos, con las tablas correspondientes a cada modelo establecido en el proyecto. 

Los pasos son los siguientes:

1. Abrir la Consola de Administrador de paquetes. Herramientas > Administrador de paquetes Nugets >  Consola de Administrador de paquetes. Y escribir los siguientes comandos:
```
PM> Add-Migration NombreMigracion
```
Cambiar el nombre de la migración a la que usted considere. Luego hacer el update a la base de datos para que se reflejen los cambios en la base de datos en SQLServer con el siguiente comando:
```
PM> Update-Database
```
Y con eso la Base de datos se creará con los diferente tablas que corresponden a los modelos del proyecto.

### 3. Correr el proyecto
Este proyecto tiene una interfaz super sencilla, pero amigable en la que se pueden hacer los diferentes peticiones http.

Ya que este proyecto utiliza la autorización entoces primero debemos crear un usuario para poder usar cualquier metodo de los controladores. Para ello seguir lo siguiente:

1. Ve a la parte donde se encuentran los metodos del Usuario, estos están ubicados hasta el final de la página, a continuación crearas un nuevo usuario en el metodo de Regiter, cambiarás los valores de nombre de usuario y contraseña por los que tu quieras, usa una contraseña segura y que puedas recordar.

2. Una vez creado el usuario la respuesta de devolverá un token, este token es único y se renueva cada día, tendras que copiar el token sin las comillas.

3. Ve a la parte superior de la página verás un botón verde con un candado, dará click ahí y le pedirá el token, deberás ingresar en el formato que se requiere el cual es:
  ```
  bearer {token}
  ```
  No olvide quitar las llaves, solo debe ir la palabra `bearer [espacio] token`

### 4. Guardar Registros
Para probar los métodos el mismo programa le da un ejemplo de como ingresar el JSON para hacer las peticiones POST y PUT, en la petición POST remover el id, ya que las tablas en la base de datos tienen la propiedad IDENTITY entonces no es necesario poner el id. Sin embargo para guardar un producto se necesita ponerlo ya que así es como lo busca en [fakestoreapi.com](https://fakestoreapi.com/).

Se recomineda seguir esta lógica para guardar un registro:

1. Guardar Client
2. Guardar Product
3. Guardar Order
4. Guardar OrderDetail
5. Guardar Payment

Con esto se guardarán los registros y a su vez a la cual hacen referencia, gracias al modelo de la base de datos.

### 5. Preba haciendo una consulta en SQLServer
Acá dejo un ejemplo de una consulta para ver un registro:

```
  SELECT 
     od.Id
    ,o.OrderDate
	,c.Name
    ,p.title
	,p.description
	,p.category
    ,od.Quantity
  FROM [ShoppingCart].[dbo].[OrderDetails] od
  LEFT JOIN [ShoppingCart].dbo.Orders o ON o.Id = od.OrderId
  LEFT JOIN ShoppingCart.dbo.Clients c ON c.Id = o.ClientId
  LEFT JOIN ShoppingCart.dbo.Products p ON p.id = od.ProductId
```

## Contacto 📞

Para cualquier consulta o soporte técnico, no dudes en contactarme:

- **Correo electrónico:** fgarcia0300@gmail.com
