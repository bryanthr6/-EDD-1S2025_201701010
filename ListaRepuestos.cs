using System;
using System.Runtime.InteropServices;

unsafe class ListaRepuestos {
    public NodoRepuesto* cabeza = null;

    public void Insertar(int id, string nombre, int cantidad, float precio) {
        if (BuscarPorID(id) != null) {
            Console.WriteLine("ID de repuesto ya registrado.");
            return;
        }

        NodoRepuesto* nuevo = (NodoRepuesto*)Marshal.AllocHGlobal(sizeof(NodoRepuesto));
        nuevo->ID = id;
        nuevo->Cantidad = cantidad;
        nuevo->Precio = precio;
        CopyString(nuevo->Nombre, nombre);

        if (cabeza == null) {
            cabeza = nuevo;
            cabeza->siguiente = cabeza; // Circular
        } else {
            NodoRepuesto* actual = cabeza;
            while (actual->siguiente != cabeza) {
                actual = actual->siguiente;
            }
            actual->siguiente = nuevo;
            nuevo->siguiente = cabeza; // Cierra el ciclo
        }
    }

    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }

    public NodoRepuesto* BuscarPorID(int id) {
        if (cabeza == null) return null;
        
        NodoRepuesto* actual = cabeza;
        do {
            if (actual->ID == id) return actual;
            actual = actual->siguiente;
        } while (actual != cabeza);

        return null;
    }

    public void MostrarRepuestos() {
        if (cabeza == null) {
            Console.WriteLine("No hay repuestos registrados.");
            return;
        }

        NodoRepuesto* actual = cabeza;
        do {
            Console.WriteLine($"ID: {actual->ID}, Nombre: {GetString(actual->Nombre)}, Cantidad: {actual->Cantidad}, Precio: {actual->Precio}");
            actual = actual->siguiente;
        } while (actual != cabeza);
    }

    public void EliminarRepuesto(int id) {
        if (cabeza == null) return;

        NodoRepuesto* actual = cabeza;
        NodoRepuesto* anterior = null;

        do {
            if (actual->ID == id) {
                if (actual == cabeza && actual->siguiente == cabeza) {
                    Marshal.FreeHGlobal((IntPtr)actual);
                    cabeza = null;
                } else {
                    if (actual == cabeza) {
                        NodoRepuesto* ultimo = cabeza;
                        while (ultimo->siguiente != cabeza) {
                            ultimo = ultimo->siguiente;
                        }
                        cabeza = cabeza->siguiente;
                        ultimo->siguiente = cabeza;
                    } else {
                        anterior->siguiente = actual->siguiente;
                    }
                    Marshal.FreeHGlobal((IntPtr)actual);
                }
                Console.WriteLine($"Repuesto con ID {id} eliminado.");
                return;
            }
            anterior = actual;
            actual = actual->siguiente;
        } while (actual != cabeza);

        Console.WriteLine("Repuesto no encontrado.");
    }

    public void LiberarMemoria() {
        if (cabeza == null) return;

        NodoRepuesto* actual = cabeza;
        do {
            NodoRepuesto* temp = actual;
            actual = actual->siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        } while (actual != cabeza);

        cabeza = null;
    }

    private string GetString(char* charArray) {
        return new string(charArray).TrimEnd('\0');
    }
}
