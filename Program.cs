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

unsafe class ListaUsuarios {
    public NodoUsuario* cabeza;
    
    public void Insertar(int id, string nombres, string apellidos, string correo, string contrasenia) {
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

unsafe class Program {
    static ListaUsuarios usuarios = new ListaUsuarios();
    
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
        return usuario != null && CompareString(usuario->Correo, correo) && CompareString(usuario->Contrasenia, contrasenia);
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
                case "1":
                    Console.WriteLine("Funcionalidad de Carga Masiva en desarrollo...");
                    break;
                case "2":
                    Console.WriteLine("Funcionalidad de Ingreso Manual en desarrollo...");
                    break;
                case "3":
                    Console.WriteLine("Funcionalidad de Gestión de Usuarios en desarrollo...");
                    break;
                case "4":
                    Console.WriteLine("Funcionalidad de Generar Servicio en desarrollo...");
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
    
    private static bool CompareString(char* stored, string input) {
        int i = 0;
        while (stored[i] != '\0' && i < input.Length) {
            if (stored[i] != input[i]) return false;
            i++;
        }
        return stored[i] == '\0' && i == input.Length;
    }
}
