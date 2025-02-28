using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
unsafe struct NodoMatriz {
    public int ID_Vehiculo;
    public int ID_Repuesto;
    public NodoMatriz* derecha;
    public NodoMatriz* abajo;
    public fixed char Detalle[100];  // Nuevo campo para el detalle del servicio
}
