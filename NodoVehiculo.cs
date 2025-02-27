using System;
using System.Runtime.InteropServices;

unsafe struct NodoVehiculo {
    public int ID_Vehiculo;
    public int ID_Usuario;
    public fixed char Marca[50];
    public fixed char Modelo[50];
    public fixed char Placa[20];
    public NodoVehiculo* siguiente;
    public NodoVehiculo* anterior; // Puntero para lista doblemente enlazada
}
