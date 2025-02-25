using System;
using System.Runtime.InteropServices;

unsafe class ListaUsuarios {
    public NodoUsuario* cabeza;

    public void Insertar(int id, string nombres, string apellidos, string correo, string contrasenia) {
        if (BuscarPorID(id) != null) {
            Console.WriteLine("ID de usuario ya registrado.");
            return;
        }
        NodoUsuario* nuevo = (NodoUsuario*)Marshal.AllocHGlobal(sizeof(NodoUsuario));
        nuevo->ID = id;
        CopyString(nuevo->Nombres, nombres);
        CopyString(nuevo->Apellidos, apellidos);
        CopyString(nuevo->Correo, correo);
        CopyString(nuevo->Contrasenia, contrasenia);
        nuevo->siguiente = cabeza;
        cabeza = nuevo;
    }

    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }

    public NodoUsuario* BuscarPorID(int id) {
        NodoUsuario* actual = cabeza;
        while (actual != null) {
            if (actual->ID == id) return actual;
            actual = actual->siguiente;
        }
        return null;
    }
}
