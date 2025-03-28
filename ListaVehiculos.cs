using System;
using System.Runtime.InteropServices;

unsafe struct NodoVehiculo {
    public int Id;
    public int IdUsuario;
    public fixed char Marca[30];
    public int Modelo;
    public fixed char Placa[10];
    public NodoVehiculo* Anterior;
    public NodoVehiculo* Siguiente;

    public NodoVehiculo(int id, int idUsuario, string marca, int modelo, string placa) {
        Id = id;
        IdUsuario = idUsuario;
        Modelo = modelo;
        Anterior = null;
        Siguiente = null;

        // Copiar datos a los buffers fijos
        fixed (char* ptr = Marca) {
            CopyToFixedArray(ptr, marca);
        }
        fixed (char* ptr = Placa) {
            CopyToFixedArray(ptr, placa);
        }
    }

    private static void CopyToFixedArray(char* destination, string source) {
        for (int i = 0; i < source.Length && i < 30; i++) {
            destination[i] = source[i];
        }
    }
}

unsafe class ListaVehiculos {
    private NodoVehiculo* cabeza;

    public ListaVehiculos() {
        cabeza = null;
    }

    public void AgregarVehiculo(int id, int idUsuario, string marca, int modelo, string placa) {
        NodoVehiculo* nuevo = (NodoVehiculo*)Marshal.AllocHGlobal(sizeof(NodoVehiculo));
        *nuevo = new NodoVehiculo(id, idUsuario, marca, modelo, placa);

        if (cabeza == null) {
            cabeza = nuevo;
        } else {
            NodoVehiculo* actual = cabeza;
            while (actual->Siguiente != null) {
                actual = actual->Siguiente;
            }
            actual->Siguiente = nuevo;
            nuevo->Anterior = actual;
        }
    }

    public void MostrarVehiculos() {
        if (cabeza == null) {
            Console.WriteLine("No hay vehículos registrados.");
            return;
        }

        NodoVehiculo* actual = cabeza;
        Console.WriteLine("\n=== Lista de Vehículos ===");
        while (actual != null) {
            Console.WriteLine($"ID: {actual->Id}, UsuarioID: {actual->IdUsuario}, Marca: {PtrToString(actual->Marca)}, Modelo: {actual->Modelo}, Placa: {PtrToString(actual->Placa)}");
            actual = actual->Siguiente;
        }
        Console.WriteLine();
    }

    public NodoVehiculo* BuscarPorId(int id) {
        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            if (actual->Id == id) return actual;
            actual = actual->Siguiente;
        }
        return null;
    }

    public void EliminarPorId(int id) {
        if (cabeza == null) return;

        if (cabeza->Id == id) {
            NodoVehiculo* temp = cabeza;
            cabeza = cabeza->Siguiente;
            if (cabeza != null) cabeza->Anterior = null;
            Marshal.FreeHGlobal((IntPtr)temp);
            Console.WriteLine("Vehículo eliminado.");
            return;
        }

        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            if (actual->Id == id) {
                if (actual->Anterior != null)
                    actual->Anterior->Siguiente = actual->Siguiente;
                if (actual->Siguiente != null)
                    actual->Siguiente->Anterior = actual->Anterior;

                Marshal.FreeHGlobal((IntPtr)actual);
                Console.WriteLine("Vehículo eliminado.");
                return;
            }
            actual = actual->Siguiente;
        }

        Console.WriteLine("Vehículo no encontrado.");
    }

    public string ObtenerInfoVehiculo(NodoVehiculo* vehiculo) {
        if (vehiculo == null) return "Vehículo no encontrado";
        return $"ID: {vehiculo->Id}\n" +
            $"ID Usuario: {vehiculo->IdUsuario}\n" +
            $"Marca: {PtrToString(vehiculo->Marca)}\n" +
            $"Modelo: {vehiculo->Modelo}\n" +
            $"Placa: {PtrToString(vehiculo->Placa)}";
    }

    private string PtrToString(char* ptr) {
        var resultado = new System.Text.StringBuilder();
        for (int i = 0; i < 50 && ptr[i] != '\0'; i++) {
            resultado.Append(ptr[i]);
        }
        return resultado.ToString();
    }

}
