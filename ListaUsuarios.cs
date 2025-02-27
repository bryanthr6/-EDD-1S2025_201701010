using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

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

        nuevo->siguiente = null;

        if (cabeza == null) {
            cabeza = nuevo;
        } else {
            NodoUsuario* actual = cabeza;
            while (actual->siguiente != null) {
                actual = actual->siguiente;
            }
            actual->siguiente = nuevo;
        }
    }

    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }

    public void VerUsuario(int id, ListaVehiculos vehiculos) {
        NodoUsuario* usuario = BuscarPorID(id);
        if (usuario == null) {
            Console.WriteLine("Usuario no encontrado.");
            return;
        }

        Console.WriteLine("\n--- Información del Usuario ---");
        Console.WriteLine($"ID: {usuario->ID}");
        Console.WriteLine($"Nombres: {GetString(usuario->Nombres)}");
        Console.WriteLine($"Apellidos: {GetString(usuario->Apellidos)}");
        Console.WriteLine($"Correo: {GetString(usuario->Correo)}");

        Console.WriteLine("\n--- Vehículos del Usuario ---");
        NodoVehiculo* vehiculo = vehiculos.cabeza;
        bool tieneVehiculos = false;

        while (vehiculo != null) {
            if (vehiculo->ID_Usuario == usuario->ID) {
                Console.WriteLine($"ID Vehículo: {vehiculo->ID_Vehiculo}");
                Console.WriteLine($"Marca: {GetString(vehiculo->Marca)}");
                Console.WriteLine($"Modelo: {GetString(vehiculo->Modelo)}");
                Console.WriteLine($"Placa: {GetString(vehiculo->Placa)}");
                Console.WriteLine("----------------------------");
                tieneVehiculos = true;
            }
            vehiculo = vehiculo->siguiente;
        }

        if (!tieneVehiculos) {
            Console.WriteLine("Este usuario no tiene vehículos registrados.");
        }
    }


    public NodoUsuario* BuscarPorID(int id) {
        NodoUsuario* actual = cabeza;
        while (actual != null) {
            if (actual->ID == id) return actual;
            actual = actual->siguiente;
        }
        return null;
    }

    public NodoUsuario* BuscarPorCorreo(string correo) {
        NodoUsuario* actual = cabeza;
        while (actual != null) {
            if (GetString(actual->Correo) == correo) return actual;
            actual = actual->siguiente;
        }
        return null;
    }

    public void EliminarUsuario(int id) {
        NodoUsuario* actual = cabeza;
        NodoUsuario* anterior = null;

        while (actual != null) {
            if (actual->ID == id) {
                if (anterior == null) {
                    cabeza = actual->siguiente;
                } else {
                    anterior->siguiente = actual->siguiente;
                }
                Marshal.FreeHGlobal((IntPtr)actual);
                Console.WriteLine($"Usuario con ID {id} eliminado.");
                return;
            }
            anterior = actual;
            actual = actual->siguiente;
        }
        Console.WriteLine("Usuario no encontrado.");
    }

    public void GenerarReporteUsuarios() {
        string dotPath = "usuarios.dot";
        string imagePath = "usuarios.png";
        using (StreamWriter writer = new StreamWriter(dotPath)) {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=LR;");
            writer.WriteLine("  node [shape=box, style=filled, color=lightblue];");
            
            NodoUsuario* actual = cabeza;
            while (actual != null) {
                if (actual->ID != 0) { // Excluir el usuario root
                    string userLabel = $"\"{actual->ID}\" [label=\"ID: {actual->ID}\\nNombre: {GetString(actual->Nombres)} {GetString(actual->Apellidos)}\\nCorreo: {GetString(actual->Correo)}\"]";
                    writer.WriteLine(userLabel);

                    if (actual->siguiente != null && actual->siguiente->ID != 0) {
                        writer.WriteLine($"\"{actual->ID}\" -> \"{actual->siguiente->ID}\";");
                    }
                }
                actual = actual->siguiente;
            }

            writer.WriteLine("}");
        }
        
        Process.Start("dot", $"-Tpng {dotPath} -o {imagePath}");
        Console.WriteLine("Reporte generado: " + imagePath);
    }





    public void LiberarMemoria() {
        NodoUsuario* actual = cabeza;
        while (actual != null) {
            NodoUsuario* temp = actual;
            actual = actual->siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        }
        cabeza = null;
    }

    private string GetString(char* charArray) {
        return new string(charArray).TrimEnd('\0');
    }
}
