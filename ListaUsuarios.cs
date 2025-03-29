using System;
using System.Runtime.InteropServices;

public unsafe struct NodoUsuario 
{
    public int Id;
    public fixed char Nombres[50];
    public fixed char Apellidos[50];
    public fixed char Correo[50];
    public int Edad;
    public fixed char Contrasenia[50];
    public NodoUsuario* Siguiente;

    public NodoUsuario(int id, string nombres, string apellidos, string correo, int edad, string contrasenia) 
    {
        Id = id;
        Edad = edad;
        Siguiente = null;

        fixed (char* ptr = Nombres) {
            CopyToFixedArray(ptr, nombres);
        }
        fixed (char* ptr = Apellidos) {
            CopyToFixedArray(ptr, apellidos);
        }
        fixed (char* ptr = Correo) {
            CopyToFixedArray(ptr, correo);
        }
        fixed (char* ptr = Contrasenia) {
            CopyToFixedArray(ptr, contrasenia);
        }
    }

    private static void CopyToFixedArray(char* destination, string source) 
    {
        for (int i = 0; i < source.Length && i < 50; i++) 
        {
            destination[i] = source[i];
        }
    }
}

public unsafe class ListaUsuarios 
{
    private NodoUsuario* cabeza;

    public ListaUsuarios() 
    {
        cabeza = null;
    }

    public NodoUsuario* GetCabeza() => cabeza;

    public void AgregarUsuario(int id, string nombres, string apellidos, string correo, int edad, string contrasenia) 
    {
        NodoUsuario* nuevo = (NodoUsuario*)Marshal.AllocHGlobal(sizeof(NodoUsuario));
        *nuevo = new NodoUsuario(id, nombres, apellidos, correo, edad, contrasenia);

        if (cabeza == null) 
        {
            cabeza = nuevo;
        } 
        else 
        {
            NodoUsuario* actual = cabeza;
            while (actual->Siguiente != null) 
            {
                actual = actual->Siguiente;
            }
            actual->Siguiente = nuevo;
        }
    }

    public NodoUsuario* BuscarPorCorreo(string correo) 
    {
        NodoUsuario* actual = cabeza;
        while (actual != null) 
        {
            string correoActual = PtrToString(actual->Correo);
            if (correoActual == correo) 
            {
                return actual;
            }
            actual = actual->Siguiente;
        }
        return null;
    }

    public NodoUsuario* BuscarPorId(int id) 
    {
        NodoUsuario* actual = cabeza;
        while (actual != null) 
        {
            if (actual->Id == id) 
            {
                return actual;
            }
            actual = actual->Siguiente;
        }
        return null;
    }

    public void EliminarPorId(int id) 
    {
        if (cabeza == null) return;

        if (cabeza->Id == id) 
        {
            NodoUsuario* temp = cabeza;
            cabeza = cabeza->Siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
            Console.WriteLine("Usuario eliminado.");
            return;
        }

        NodoUsuario* actual = cabeza;
        while (actual->Siguiente != null) 
        {
            if (actual->Siguiente->Id == id) 
            {
                NodoUsuario* temp = actual->Siguiente;
                actual->Siguiente = temp->Siguiente;
                Marshal.FreeHGlobal((IntPtr)temp);
                Console.WriteLine("Usuario eliminado.");
                return;
            }
            actual = actual->Siguiente;
        }

        Console.WriteLine("Usuario no encontrado.");
    }

    public string PtrToString(char* ptr) 
    {
        var resultado = new System.Text.StringBuilder();
        for (int i = 0; i < 50 && ptr[i] != '\0'; i++) 
        {
            resultado.Append(ptr[i]);
        }
        return resultado.ToString();
    }

    public void MostrarUsuarios() 
    {
        if (cabeza == null) 
        {
            Console.WriteLine("No hay usuarios registrados.");
            return;
        }

        NodoUsuario* actual = cabeza;
        Console.WriteLine("\n=== Lista de Usuarios ===");
        while (actual != null) 
        {
            Console.WriteLine($"ID: {actual->Id}, Nombre: {PtrToString(actual->Nombres)} {PtrToString(actual->Apellidos)}, Correo: {PtrToString(actual->Correo)}, Edad: {actual->Edad}");
            actual = actual->Siguiente;
        }
        Console.WriteLine();
    }
}