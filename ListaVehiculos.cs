using System;
using System.Runtime.InteropServices;

unsafe class ListaVehiculos {
    public NodoVehiculo* cabeza;
    public NodoVehiculo* cola;

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
        nuevo->anterior = null;
        
        if (cabeza != null) {
            cabeza->anterior = nuevo;
        } else {
            cola = nuevo; // Si la lista estaba vacía, también es la cola
        }
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

    public void EliminarVehiculo(int id) {
        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            if (actual->ID_Vehiculo == id) {
                if (actual->anterior != null) {
                    actual->anterior->siguiente = actual->siguiente;
                } else {
                    cabeza = actual->siguiente;
                }

                if (actual->siguiente != null) {
                    actual->siguiente->anterior = actual->anterior;
                } else {
                    cola = actual->anterior;
                }
                
                Marshal.FreeHGlobal((IntPtr)actual);
                Console.WriteLine($"Vehículo con ID {id} eliminado.");
                return;
            }
            actual = actual->siguiente;
        }
        Console.WriteLine("Vehículo no encontrado.");
    }

    public void LiberarMemoria() {
        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            NodoVehiculo* temp = actual;
            actual = actual->siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        }
        cabeza = null;
        cola = null;
    }
}
