using System;
using System.Runtime.InteropServices;

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
