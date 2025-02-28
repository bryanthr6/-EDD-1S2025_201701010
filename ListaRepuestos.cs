using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

unsafe class ListaRepuestos {
    public NodoRepuesto* cabeza = null;

    public void Insertar(int id, string nombre, float precio) {
        if (BuscarPorID(id) != null) {
            Console.WriteLine("ID de repuesto ya registrado.");
            return;
        }

        NodoRepuesto* nuevo = (NodoRepuesto*)Marshal.AllocHGlobal(sizeof(NodoRepuesto));
        nuevo->ID = id;
        nuevo->Precio = precio;
        CopyString(nuevo->Nombre, nombre);

        if (cabeza == null) {
            cabeza = nuevo;
            cabeza->siguiente = cabeza; // Circular
        } else {
            NodoRepuesto* actual = cabeza;
            while (actual->siguiente != cabeza) {
                actual = actual->siguiente;
            }
            actual->siguiente = nuevo;
            nuevo->siguiente = cabeza; // Cierra el ciclo
        }
    }

    public void GenerarReporteRepuestos()
    {
        if (cabeza == null)
        {
            Console.WriteLine("No hay repuestos registrados.");
            return;
        }

        string rutaDot = "reporte_repuestos.dot";
        string rutaImagen = "reporte_repuestos.png";

        using (StreamWriter writer = new StreamWriter(rutaDot))
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=LR;"); // Orientación de izquierda a derecha
            writer.WriteLine("    node [shape=box, style=filled, color=lightblue];");

            NodoRepuesto* actual = cabeza;
            int contador = 0;

            do
            {
                string nodoActual = $"repuesto{contador}";
                writer.WriteLine($"    {nodoActual} [label=\"ID: {actual->ID}\\nNombre: {GetString(actual->Nombre)}\\nPrecio: {actual->Precio}\"];");

                actual = actual->siguiente;
                contador++;
            } while (actual != cabeza);

            // Conectar los nodos en orden
            actual = cabeza;
            for (int i = 0; i < contador - 1; i++)
            {
                writer.WriteLine($"    repuesto{i} -> repuesto{i + 1};");
            }

            // Conectar el último nodo con el primero para cerrar el ciclo
            writer.WriteLine($"    repuesto{contador - 1} -> repuesto0;");

            writer.WriteLine("}");
        }

        // Generar la imagen con Graphviz
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

            Console.WriteLine("Reporte de repuestos generado correctamente: " + rutaImagen);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al generar el reporte: " + ex.Message);
        }
    }


    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 49; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }

    public NodoRepuesto* BuscarPorID(int id) {
        if (cabeza == null) return null;
        
        NodoRepuesto* actual = cabeza;
        do {
            if (actual->ID == id) return actual;
            actual = actual->siguiente;
        } while (actual != cabeza);

        return null;
    }

    public void MostrarRepuestos() {
        if (cabeza == null) {
            Console.WriteLine("No hay repuestos registrados.");
            return;
        }

        NodoRepuesto* actual = cabeza;
        do {
            Console.WriteLine($"ID: {actual->ID}, Nombre: {GetString(actual->Nombre)}, Precio: {actual->Precio}");
            actual = actual->siguiente;
        } while (actual != cabeza);
    }

    public void EliminarRepuesto(int id) {
        if (cabeza == null) return;

        NodoRepuesto* actual = cabeza;
        NodoRepuesto* anterior = null;

        do {
            if (actual->ID == id) {
                if (actual == cabeza && actual->siguiente == cabeza) {
                    Marshal.FreeHGlobal((IntPtr)actual);
                    cabeza = null;
                } else {
                    if (actual == cabeza) {
                        NodoRepuesto* ultimo = cabeza;
                        while (ultimo->siguiente != cabeza) {
                            ultimo = ultimo->siguiente;
                        }
                        cabeza = cabeza->siguiente;
                        ultimo->siguiente = cabeza;
                    } else {
                        anterior->siguiente = actual->siguiente;
                    }
                    Marshal.FreeHGlobal((IntPtr)actual);
                }
                Console.WriteLine($"Repuesto con ID {id} eliminado.");
                return;
            }
            anterior = actual;
            actual = actual->siguiente;
        } while (actual != cabeza);

        Console.WriteLine("Repuesto no encontrado.");
    }

    public void LiberarMemoria() {
        if (cabeza == null) return;

        NodoRepuesto* actual = cabeza;
        do {
            NodoRepuesto* temp = actual;
            actual = actual->siguiente;
            Marshal.FreeHGlobal((IntPtr)temp);
        } while (actual != cabeza);

        cabeza = null;
    }

    private string GetString(char* charArray) {
        return new string(charArray).TrimEnd('\0');
    }
}
