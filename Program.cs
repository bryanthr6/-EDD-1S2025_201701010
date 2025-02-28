using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

unsafe class Program {
    static ListaUsuarios usuarios = new ListaUsuarios();
    static ListaVehiculos vehiculos = new ListaVehiculos();
    static ListaRepuestos repuestos = new ListaRepuestos();
    static ListaServicios servicios = new ListaServicios();
    static ListaFacturas facturas = new ListaFacturas();

    static void Main() {
        // Insertar usuario root
        usuarios.Insertar(0, "root", "", "root@gmail.com", "root123");

        while (true) {
            if (!Login()) {
                Console.WriteLine("Credenciales incorrectas. Intente de nuevo.");
                continue;
            }
            MostrarMenuRoot();
        }
    }

    static bool Login() {
        Console.Write("Correo: ");
        string? correo = Console.ReadLine();
        if (string.IsNullOrEmpty(correo)) return false;

        Console.Write("Contraseña: ");
        string? contrasenia = Console.ReadLine();
        if (string.IsNullOrEmpty(contrasenia)) return false;

        NodoUsuario* usuario = usuarios.BuscarPorCorreo(correo);
        if (usuario == null) return false;

        return GetString(usuario->Contrasenia) == contrasenia;
    }

    static void MostrarMenuRoot() {
        while (true) {
            Console.WriteLine("\n--- Menú Principal (Root) ---");
            Console.WriteLine("1. Carga Masiva");
            Console.WriteLine("2. Ingreso Manual");
            Console.WriteLine("3. Gestión de Usuarios");
            Console.WriteLine("4. Generar Servicio");
            Console.WriteLine("5. Generar Reportes");
            Console.WriteLine("6. Cerrar Sesión");
            Console.Write("Seleccione una opción: ");
            string? opcion = Console.ReadLine();
            if (opcion == null) continue;

            switch (opcion) {
                case "1":
                    Console.Write("Ingrese la ruta del archivo JSON: ");
                    string? ruta = Console.ReadLine();
                    if (!string.IsNullOrEmpty(ruta)) CargarDesdeJson(ruta);
                    break;
                case "2":
                    MostrarSubmenuIngresoManual();
                    break;
                case "3":
                    MostrarSubmenuGestionUsuarios();
                    break;
                case "4":
                    GenerarServicio();
                    break;
                case "5":
                    MostrarSubmenuReportes();
                    break;
                case "6":
                    Console.WriteLine("Cerrando sesión...");
                    usuarios.LiberarMemoria();
                    vehiculos.LiberarMemoria();
                    repuestos.LiberarMemoria();
                    servicios = new ListaServicios(); // Reiniciar lista de servicios
                    facturas = new ListaFacturas();   // Reiniciar pila de facturas
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }
    
    class DatosCargados {
    public Usuario[] Usuarios { get; set; } = Array.Empty<Usuario>();
    public Vehiculo[] Vehiculos { get; set; } = Array.Empty<Vehiculo>();
    public Repuesto[] Repuestos { get; set; } = Array.Empty<Repuesto>();
    }

    static void CargarDesdeJson(string rutaArchivo) {
        if (!File.Exists(rutaArchivo)) {
            Console.WriteLine("Archivo no encontrado.");
            return;
        }

        string json = File.ReadAllText(rutaArchivo);
        try {
            DatosCargados datos = JsonSerializer.Deserialize<DatosCargados>(json) ?? new DatosCargados();

            // Cargar usuarios
            foreach (var u in datos.Usuarios) {
                usuarios.Insertar(u.ID, u.Nombres, u.Apellidos, u.Correo, u.Contrasenia);
            }
            Console.WriteLine($"Usuarios cargados: {datos.Usuarios.Length}");

            // Cargar vehículos
            foreach (var v in datos.Vehiculos) {
                vehiculos.Insertar(v.ID, v.ID_Usuario, v.Marca, v.Modelo, v.Placa, usuarios);
            }
            Console.WriteLine($"Vehículos cargados: {datos.Vehiculos.Length}");

            // Cargar repuestos
            foreach (var r in datos.Repuestos) {
                repuestos.Insertar(r.ID, r.RepuestoNombre, r.Costo);
            }
            Console.WriteLine($"Repuestos cargados: {datos.Repuestos.Length}");

        } catch (Exception e) {
            Console.WriteLine("Error al cargar JSON: " + e.Message);
        }
    }


    static void CargarUsuarios(string json) {
        try {
            Usuario[] usuariosCargados = JsonSerializer.Deserialize<Usuario[]>(json) ?? Array.Empty<Usuario>();

            foreach (var u in usuariosCargados) {
                usuarios.Insertar(u.ID, u.Nombres, u.Apellidos, u.Correo, u.Contrasenia);
            }
            Console.WriteLine("Usuarios cargados correctamente.");
        } catch (Exception e) {
            Console.WriteLine("Error al cargar usuarios: " + e.Message);
        }
    }

    static void CargarVehiculos(string json) {
        try {
            Vehiculo[] vehiculosCargados = JsonSerializer.Deserialize<Vehiculo[]>(json) ?? Array.Empty<Vehiculo>();

            foreach (var v in vehiculosCargados) {
                vehiculos.Insertar(v.ID, v.ID_Usuario, v.Marca, v.Modelo, v.Placa, usuarios);
            }
            Console.WriteLine("Vehículos cargados correctamente.");
        } catch (Exception e) {
            Console.WriteLine("Error al cargar vehículos: " + e.Message);
        }
    }


    static void CargarRepuestos(string json) {
        try {
            Repuesto[] repuestosCargados = JsonSerializer.Deserialize<Repuesto[]>(json) ?? Array.Empty<Repuesto>();

            foreach (var r in repuestosCargados) {
                repuestos.Insertar(r.ID, r.RepuestoNombre, r.Costo);
            }
            Console.WriteLine("Repuestos cargados correctamente.");
        } catch (Exception e) {
            Console.WriteLine("Error al cargar repuestos: " + e.Message);
        }
    }

    static void MostrarSubmenuIngresoManual() {
        while (true) {
            Console.WriteLine("\n--- Ingreso Manual ---");
            Console.WriteLine("1. Usuario");
            Console.WriteLine("2. Vehículo");
            Console.WriteLine("3. Repuesto");
            Console.WriteLine("4. Regresar");
            Console.Write("Seleccione una opción: ");
            string? opcion = Console.ReadLine();
            if (opcion == null) continue;

            switch (opcion) {
                case "1":
                    IngresarUsuario();
                    break;
                case "2":
                    IngresarVehiculo();
                    break;
                case "3":
                    IngresarRepuesto();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    static void MostrarSubmenuGestionUsuarios() {
        while (true) {
            Console.WriteLine("\n--- Gestión de Usuarios ---");
            Console.WriteLine("1. Ver Usuario");
            Console.WriteLine("2. Eliminar Usuario");
            Console.WriteLine("3. Editar Usuario");
            Console.WriteLine("4. Regresar");
            Console.Write("Seleccione una opción: ");
            string? opcion = Console.ReadLine();
            if (opcion == null) continue;

            switch (opcion) {
                case "1":
                    Console.Write("Ingrese el ID del usuario: ");
                    if (!int.TryParse(Console.ReadLine(), out int id)) return;
                    usuarios.VerUsuario(id, vehiculos);
                    break;
                case "2":
                    Console.Write("Ingrese el ID del usuario a eliminar: ");
                    if (!int.TryParse(Console.ReadLine(), out int idEliminar)) return;
                    usuarios.EliminarUsuario(idEliminar);
                    break;
                case "3":
                    EditarUsuario();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    static void IngresarUsuario() {
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;
        Console.Write("Nombres: ");
        string? nombres = Console.ReadLine();
        if (string.IsNullOrEmpty(nombres)) return;
        Console.Write("Apellidos: ");
        string? apellidos = Console.ReadLine();
        if (string.IsNullOrEmpty(apellidos)) return;
        Console.Write("Correo: ");
        string? correo = Console.ReadLine();
        if (string.IsNullOrEmpty(correo)) return;
        Console.Write("Contraseña: ");
        string? contrasenia = Console.ReadLine();
        if (string.IsNullOrEmpty(contrasenia)) return;

        usuarios.Insertar(id, nombres, apellidos, correo, contrasenia);
    }

    static void EditarUsuario() {
        Console.Write("Ingrese el ID del usuario a editar: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;

        NodoUsuario* usuario = usuarios.BuscarPorID(id);
        if (usuario == null) {
        Console.WriteLine("Usuario no encontrado.");
        return;
        }

        Console.WriteLine("\n--- Editar Usuario ---");
        Console.WriteLine($"Nombre actual: {GetString(usuario->Nombres)}");
        Console.Write("Nuevo Nombre (deje vacío para no cambiar): ");
        string? nuevoNombre = Console.ReadLine();
        if (!string.IsNullOrEmpty(nuevoNombre)) CopyString(usuario->Nombres, nuevoNombre);

        Console.WriteLine($"Apellidos actuales: {GetString(usuario->Apellidos)}");
        Console.Write("Nuevos Apellidos (deje vacío para no cambiar): ");
        string? nuevoApellidos = Console.ReadLine();
        if (!string.IsNullOrEmpty(nuevoApellidos)) CopyString(usuario->Apellidos, nuevoApellidos);

        Console.WriteLine($"Correo actual: {GetString(usuario->Correo)}");
        Console.Write("Nuevo Correo (deje vacío para no cambiar): ");
        string? nuevoCorreo = Console.ReadLine();
        if (!string.IsNullOrEmpty(nuevoCorreo)) CopyString(usuario->Correo, nuevoCorreo);

        Console.WriteLine("Usuario actualizado correctamente.");
        }


    static void IngresarVehiculo() {
        Console.Write("ID Vehículo: ");
        if (!int.TryParse(Console.ReadLine(), out int idVehiculo)) return;
        Console.Write("ID Usuario: ");
        if (!int.TryParse(Console.ReadLine(), out int idUsuario)) return;
        Console.Write("Marca: ");
        string? marca = Console.ReadLine();
        if (string.IsNullOrEmpty(marca)) return;
        Console.Write("Modelo: ");
        string? modelo = Console.ReadLine();
        if (string.IsNullOrEmpty(modelo)) return;
        Console.Write("Placa: ");
        string? placa = Console.ReadLine();
        if (string.IsNullOrEmpty(placa)) return;

        vehiculos.Insertar(idVehiculo, idUsuario, marca, modelo, placa, usuarios);
    }

    static void IngresarRepuesto() {
        Console.Write("ID Repuesto: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;
        Console.Write("Nombre del Repuesto: ");
        string? nombre = Console.ReadLine();
        if (string.IsNullOrEmpty(nombre)) return;
        Console.Write("Precio: ");
        if (!float.TryParse(Console.ReadLine(), out float precio)) return;

        repuestos.Insertar(id, nombre, precio);
    }

    static void GenerarServicio() {
        Console.Write("ID del servicio: ");
        if (!int.TryParse(Console.ReadLine(), out int idServicio)) return;

        Console.Write("ID del vehículo: ");
        if (!int.TryParse(Console.ReadLine(), out int idVehiculo)) return;

        Console.Write("ID del repuesto: ");
        if (!int.TryParse(Console.ReadLine(), out int idRepuesto)) return;

        Console.Write("Detalles del servicio: ");
        string? detalles = Console.ReadLine();
        if (string.IsNullOrEmpty(detalles)) return;

        Console.Write("Costo del servicio: ");
        if (!float.TryParse(Console.ReadLine(), out float costoServicio)) return;

        // Validar existencia del vehículo y repuesto
        if (vehiculos.BuscarPorID(idVehiculo) == null) {
            Console.WriteLine("Error: Vehículo no encontrado.");
            return;
        }

        NodoRepuesto* repuesto = repuestos.BuscarPorID(idRepuesto);
        if (repuesto == null) {
            Console.WriteLine("Error: Repuesto no encontrado.");
            return;
        }

        // Insertar servicio en la cola
        servicios.Insertar(idServicio, idRepuesto, idVehiculo, detalles, costoServicio, repuestos, vehiculos);
        Console.WriteLine("Servicio registrado exitosamente.");

        // Generar factura
        float total = costoServicio + repuesto->Precio;
        facturas.GenerarFactura(idServicio, idServicio, total);
        Console.WriteLine($"Factura generada: ID {idServicio}, Total: {total}");
    }

    static void MostrarSubmenuReportes() {
        while (true) {
            Console.WriteLine("\n--- Generar Reportes ---");
            Console.WriteLine("1. Usuarios");
            Console.WriteLine("2. Vehículos");
            Console.WriteLine("3. Regresar");
            Console.Write("Seleccione una opción: ");
            string? opcion = Console.ReadLine();
            if (opcion == null) continue;

            switch (opcion) {
                case "1":
                    usuarios.GenerarReporteUsuarios();
                    break;
                case "2":
                    vehiculos.GenerarReporteVehiculos();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private static string GetString(char* charArray) {
        return new string(charArray).TrimEnd('\0');
    }

    private static void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }
}
