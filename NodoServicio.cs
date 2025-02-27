using System;
using System.Runtime.InteropServices;

unsafe struct NodoServicio {
    public int ID;
    public int ID_Repuesto;
    public int ID_Vehiculo;
    public fixed char Detalles[100];
    public float Costo;
    public NodoServicio* siguiente;
}
