using System;

unsafe class Program {
    static ListaUsuarios usuarios = new ListaUsuarios();
    static ListaVehiculos vehiculos = new ListaVehiculos();

    static void Main() {
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
        string correo = Console.ReadLine();
        Console.Write("Contraseña: ");
        string contrasenia = Console.ReadLine();

        NodoUsuario* usuario = usuarios.BuscarPorID(0);
        return usuario != null;
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
            string opcion = Console.ReadLine();

            switch (opcion) {
                case "2":
                    MostrarSubmenuIngresoManual();
                    break;
                case "3":
                    MostrarSubmenuGestionUsuarios();
                    break;
                case "5":
                    Console.WriteLine("Cerrando sesión...");
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
            string opcion = Console.ReadLine();

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
            Console.WriteLine("4. Regresar");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion) {
                case "1":
                    Console.Write("Ingrese el ID del usuario: ");
                    int id = int.Parse(Console.ReadLine());
                    usuarios.VerUsuario(id, vehiculos);
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
        int id = int.Parse(Console.ReadLine());
        Console.Write("Nombres: ");
        string nombres = Console.ReadLine();
        Console.Write("Apellidos: ");
        string apellidos = Console.ReadLine();
        Console.Write("Correo: ");
        string correo = Console.ReadLine();
        Console.Write("Contraseña: ");
        string contrasenia = Console.ReadLine();
        usuarios.Insertar(id, nombres, apellidos, correo, contrasenia);
    }

    static void IngresarVehiculo() {
        Console.Write("ID Vehículo: ");
        int idVehiculo = int.Parse(Console.ReadLine());
        Console.Write("ID Usuario: ");
        int idUsuario = int.Parse(Console.ReadLine());
        Console.Write("Marca: ");
        string marca = Console.ReadLine();
        Console.Write("Modelo: ");
        string modelo = Console.ReadLine();
        Console.Write("Placa: ");
        string placa = Console.ReadLine();
        vehiculos.Insertar(idVehiculo, idUsuario, marca, modelo, placa, usuarios);
    }
}
