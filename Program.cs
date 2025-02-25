using System;
using System.Runtime.InteropServices;

unsafe struct NodoUsuario {
    public int ID;
    public fixed char Nombres[50];
    public fixed char Apellidos[50];
    public fixed char Correo[50];
    public fixed char Contrasenia[20];
    public NodoUsuario* siguiente;
}

unsafe struct NodoVehiculo {
    public int ID_Vehiculo;
    public int ID_Usuario;
    public fixed char Marca[50];
    public fixed char Modelo[50];
    public fixed char Placa[20];
    public NodoVehiculo* siguiente;
}

unsafe class ListaUsuarios {
    public NodoUsuario* cabeza;
    
    public void Insertar(int id, string nombres, string apellidos, string correo, string contrasenia) {
        if (BuscarPorID(id) != null) {
            Console.WriteLine("ID de usuario ya registrado.");
            return;
        }
        NodoUsuario* nuevo = (NodoUsuario*)Marshal.AllocHGlobal(sizeof(NodoUsuario));
        nuevo->ID = id;
        CopyString(nuevo->Nombres, nombres);
        CopyString(nuevo->Apellidos, apellidos);
        CopyString(nuevo->Correo, correo);
        CopyString(nuevo->Contrasenia, contrasenia);
        nuevo->siguiente = cabeza;
        cabeza = nuevo;
    }
    
    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }
    
    public NodoUsuario* BuscarPorID(int id) {
        NodoUsuario* actual = cabeza;
        while (actual != null) {
            if (actual->ID == id) return actual;
            actual = actual->siguiente;
        }
        return null;
    }
}

unsafe class ListaVehiculos {
    public NodoVehiculo* cabeza;
    
    public void Insertar(int idVehiculo, int idUsuario, string marca, string modelo, string placa, ListaUsuarios usuarios) {
        if (BuscarPorID(idVehiculo) != null) {
            Console.WriteLine("ID de vehículo ya registrado.");
            return;
        }
        if (usuarios.BuscarPorID(idUsuario) == null) {
            Console.WriteLine("ID de usuario no encontrado.");
            return;
        }
        NodoVehiculo* nuevo = (NodoVehiculo*)Marshal.AllocHGlobal(sizeof(NodoVehiculo));
        nuevo->ID_Vehiculo = idVehiculo;
        nuevo->ID_Usuario = idUsuario;
        CopyString(nuevo->Marca, marca);
        CopyString(nuevo->Modelo, modelo);
        CopyString(nuevo->Placa, placa);
        nuevo->siguiente = cabeza;
        cabeza = nuevo;
    }
    
    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }
    
    public NodoVehiculo* BuscarPorID(int id) {
        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            if (actual->ID_Vehiculo == id) return actual;
            actual = actual->siguiente;
        }
        return null;
    }
}

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
