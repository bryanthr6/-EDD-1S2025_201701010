# Manual de Usuario

## Introducción
El presente manual tiene como objetivo guiar al usuario en el uso del sistema de gestión de usuarios, vehículos y repuestos. La aplicación permite la administración eficiente de estos elementos a través de una interfaz gráfica e integración con Graphviz para generar reportes visuales.

## Requisitos del Sistema
- **Sistema Operativo:** Linux (Ubuntu recomendado) o Windows con WSL habilitado.
- **Dependencias:**
  - .NET Core instalado
  - GTK para la interfaz gráfica
  - Graphviz para la generación de reportes visuales
- **Ejecución:** `dotnet run` desde la terminal en la carpeta del proyecto.

## Instalación y Ejecución
1. Clonar o descargar el repositorio del proyecto.
2. Abrir una terminal en la carpeta del proyecto.
3. Ejecutar el comando `dotnet run`.
4. La aplicación se iniciará con la interfaz gráfica habilitada.

## Menús y Funcionalidades

### Menú Principal
Al ejecutar el programa, el usuario accederá a la pantalla principal donde podrá elegir entre las siguientes opciones:

1. **Gestión de Usuarios**
2. **Gestión de Vehículos**
3. **Gestión de Repuestos**
4. **Generación de Reportes**
5. **Salir**

### Gestión de Usuarios
Permite administrar los usuarios en el sistema. Las opciones disponibles son:
- **Crear usuario:** Se ingresa un ID, nombre y tipo de usuario.
- **Buscar usuario:** Permite buscar un usuario por ID.
- **Eliminar usuario:** Se puede eliminar un usuario del sistema.
- **Mostrar lista de usuarios:** Visualiza todos los usuarios registrados.

### Gestión de Vehículos
Opciones para administrar vehículos en el sistema:
- **Agregar vehículo:** Se solicita ID, marca, modelo y año.
- **Buscar vehículo:** Encuentra un vehículo por su ID.
- **Eliminar vehículo:** Borra un vehículo registrado.
- **Mostrar lista de vehículos:** Lista todos los vehículos registrados.

### Gestión de Repuestos
Manejo de repuestos disponibles en el sistema:
- **Añadir repuesto:** Se ingresa ID, nombre y detalles.
- **Buscar repuesto por ID:** Encuentra información específica.
- **Eliminar repuesto:** Borra un repuesto del sistema.
- **Mostrar lista de repuestos:** Despliega todos los repuestos registrados.

### Generación de Reportes
La aplicación permite la visualización de datos mediante Graphviz:
- **Reporte de usuarios:** Muestra la estructura de datos de los usuarios en forma de nodos.
- **Reporte de vehículos:** Genera un diagrama con la relación de vehículos registrados.
- **Reporte de repuestos:** Representa gráficamente los repuestos en la base de datos.

## Explicación de las Imágenes Generadas con Graphviz
Cada reporte genera un diagrama en formato `.dot` que puede ser convertido en una imagen (`.png`).
- Los nodos representan los elementos del sistema (usuarios, vehículos o repuestos).
- Las conexiones muestran relaciones entre los elementos.
- La visualización facilita la interpretación de la estructura de datos.

## Ejemplo de Uso
1. El usuario accede a la aplicación y selecciona **Gestión de Vehículos**.
2. Agrega un vehículo ingresando los datos solicitados.
3. Busca el vehículo recién agregado para verificar la información.
4. Genera un reporte visual con Graphviz.
5. Consulta el archivo de salida `.png` para ver la estructura gráfica.

## Cierre
Este manual proporciona una guía completa para el uso de la aplicación. Para cualquier error o problema, se recomienda revisar la terminal en busca de mensajes del sistema o revisar las dependencias instaladas.

Para cualquier consulta ingresar al repositorio en Github: https://github.com/bryanthr6/-EDD-1S2025_201701010