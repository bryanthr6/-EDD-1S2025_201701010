using System;

unsafe class Program {
    static ListaUsuarios usuarios = new ListaUsuarios();
    static ListaVehiculos vehiculos = new ListaVehiculos();
    static ListaRepuestos repuestos = new ListaRepuestos();

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
            Console.WriteLine("5. Cerrar Sesión");
            Console.Write("Seleccione una opción: ");
            string? opcion = Console.ReadLine();
            if (opcion == null) continue;

            switch (opcion) {
                case "2":
                    MostrarSubmenuIngresoManual();
                    break;
                case "3":
                    MostrarSubmenuGestionUsuarios();
                    break;
                case "5":
                    Console.WriteLine("Cerrando sesión...");
                    usuarios.LiberarMemoria();
                    vehiculos.LiberarMemoria();
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    static void MostrarSubmenuIngresoManual() {
        while (true) {
            Console.WriteLine("\n--- Ingreso Manual ---");
            Console.WriteLine("1. Usuario");
            Console.WriteLine("2. Vehículo");
            Console.WriteLine("5. Regresar");
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
                case "5":
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
            Console.WriteLine("3. Editar Usuario");  // Nueva opción
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
                    EditarUsuario();  // Nueva función
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
