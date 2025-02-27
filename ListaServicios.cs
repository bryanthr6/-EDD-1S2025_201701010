using System;
using System.Runtime.InteropServices;

unsafe class ListaServicios {
    public NodoServicio* frente = null;
    public NodoServicio* final = null;

    public void Insertar(int id, int idRepuesto, int idVehiculo, string detalles, float costo, ListaRepuestos repuestos, ListaVehiculos vehiculos) {
        // Validar existencia del repuesto y del vehículo
        if (repuestos.BuscarPorID(idRepuesto) == null) {
            Console.WriteLine("ID de repuesto no encontrado.");
            return;
        }
        if (vehiculos.BuscarPorID(idVehiculo) == null) {
            Console.WriteLine("ID de vehículo no encontrado.");
            return;
        }

        NodoServicio* nuevo = (NodoServicio*)Marshal.AllocHGlobal(sizeof(NodoServicio));
        nuevo->ID = id;
        nuevo->ID_Repuesto = idRepuesto;
        nuevo->ID_Vehiculo = idVehiculo;
        nuevo->Costo = costo;
        CopyString(nuevo->Detalles, detalles);
        nuevo->siguiente = null;

        if (final == null) {
            frente = final = nuevo;
        } else {
            final->siguiente = nuevo;
            final = nuevo;
        }
    }

    public NodoServicio* AtenderServicio() {
        if (frente == null) {
            Console.WriteLine("No hay servicios en espera.");
            return null;
        }

        NodoServicio* servicio = frente;
        frente = frente->siguiente;
        if (frente == null) {
            final = null;
        }
        return servicio;
    }

    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 99; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }
}
