using System;
using System.Runtime.InteropServices;

unsafe class ListaFacturas {
    public NodoFactura* tope = null;

    public void GenerarFactura(int id, int idOrden, float total) {
        NodoFactura* nueva = (NodoFactura*)Marshal.AllocHGlobal(sizeof(NodoFactura));
        nueva->ID = id;
        nueva->ID_Orden = idOrden;
        nueva->Total = total;
        nueva->siguiente = tope;
        tope = nueva;
    }

    public void CancelarFactura() {
        if (tope == null) {
            Console.WriteLine("No hay facturas pendientes.");
            return;
        }

        NodoFactura* temp = tope;
        tope = tope->siguiente;
        Console.WriteLine($"Factura ID: {temp->ID} pagada. Total: {temp->Total}");
        Marshal.FreeHGlobal((IntPtr)temp);
    }
}
