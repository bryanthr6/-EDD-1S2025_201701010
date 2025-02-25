using System;
using System.Runtime.InteropServices;

unsafe struct NodoUsuario {
    public int ID;
    public fixed char Nombres[50];
    public fixed char Apellidos[50];
    public fixed char Correo[50];
    public fixed char Contrasenia[20];
    public NodoUsuario* siguiente;
}
