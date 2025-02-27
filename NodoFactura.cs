using System;
using System.Runtime.InteropServices;

unsafe struct NodoFactura {
    public int ID;
    public int ID_Orden;
    public float Total;
    public NodoFactura* siguiente;
}
