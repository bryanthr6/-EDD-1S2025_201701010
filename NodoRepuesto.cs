using System;
using System.Runtime.InteropServices;

unsafe struct NodoRepuesto {
    public int ID;
    public fixed char Nombre[50];
    public int Cantidad;
    public float Precio;
    public NodoRepuesto* siguiente;
}
