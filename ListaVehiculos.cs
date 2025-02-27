using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

unsafe class ListaVehiculos {
    public NodoVehiculo* cabeza;
    public NodoVehiculo* cola;

    public void Insertar(int idVehiculo, int idUsuario, string marca, string modelo, string placa, ListaUsuarios usuarios) {
        if (BuscarPorID(idVehiculo) != null) {
            Console.WriteLine("ID de vehículo ya registrado.");
            return;
        }
        if (usuarios.BuscarPorID(idUsuario) == null) {
            Console.WriteLine("ID de usuario no encontrado.");
            return;
        }
        NodoVehiculo* nuevo = (NodoVehiculo*)Marshal.AllocHGlobal(sizeof(NodoVehiculo));
        nuevo->ID_Vehiculo = idVehiculo;
        nuevo->ID_Usuario = idUsuario;

        CopyString(nuevo->Marca, marca);
        CopyString(nuevo->Modelo, modelo);
        CopyString(nuevo->Placa, placa);

        nuevo->siguiente = null;
        nuevo->anterior = cola;

        if (cola != null) {
            cola->siguiente = nuevo;
        } else {
            cabeza = nuevo; // Si la lista estaba vacía, también es la cabeza
        }
        cola = nuevo;
    }

    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }

    public NodoVehiculo* BuscarPorID(int id) {
        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            if (actual->ID_Vehiculo == id) return actual;
            actual = actual->siguiente;
        }
        return null;
    }

    public void EliminarVehiculo(int id) {
        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            if (actual->ID_Vehiculo == id) {
                if (actual->anterior != null) {
                    actual->anterior->siguiente = actual->siguiente;
                } else {
                    cabeza = actual->siguiente;
                }

                if (actual->siguiente != null) {
                    actual->siguiente->anterior = actual->anterior;
                } else {
                    cola = actual->anterior;
                }
                
                Marshal.FreeHGlobal((IntPtr)actual);
                Console.WriteLine($"Vehículo con ID {id} eliminado.");
                return;
            }
            actual = actual->siguiente;
        }
        Console.WriteLine("Vehículo no encontrado.");
    }

    public void GenerarReporteVehiculos()
    {
        string rutaDot = "reporte_vehiculos.dot";
        string rutaImagen = "reporte_vehiculos.png";

        using (StreamWriter writer = new StreamWriter(rutaDot))
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=LR;");
            writer.WriteLine("    node [shape=box, style=filled, color=lightblue];");

            NodoVehiculo* actual = cabeza;
            int contador = 0;

            while (actual != null)
            {
                string nodoActual = $"vehiculo{contador}";
                writer.WriteLine($"    {nodoActual} [label=\"ID Vehículo: {actual->ID_Vehiculo}\\nID Usuario: {actual->ID_Usuario}\\nPlaca: {GetString(actual->Placa, 20)}\\nMarca: {GetString(actual->Marca, 50)}\\nModelo: {GetString(actual->Modelo, 50)}\"];");

                if (actual->siguiente != null)
                {
                    string nodoSiguiente = $"vehiculo{contador + 1}";
                    writer.WriteLine($"    {nodoActual} -> {nodoSiguiente} [dir=forward];");
                    writer.WriteLine($"    {nodoActual} -> {nodoSiguiente} [dir=back];");
                }

                actual = actual->siguiente;
                contador++;
            }

            writer.WriteLine("}");
        }


        // Generar imagen con Graphviz
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "dot",
                Arguments = $"-Tpng {rutaDot} -o {rutaImagen}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();
                process.WaitForExit();
            }

            Console.WriteLine("Reporte generado correctamente: " + rutaImagen);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al generar el reporte: " + ex.Message);
        }
    }
    private string GetString(char* charArray, int length)
    {
        return new string(charArray, 0, length).Split('\0')[0]; // Elimina caracteres nulos
    }



    public void LiberarMemoria() {
        NodoVehiculo* actual = cabeza;
        while (actual != null) {
            NodoVehiculo* temp = actual;
            actual = actual->siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        }
        cabeza = null;
        cola = null;
    }
    
}

