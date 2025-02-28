using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

unsafe class ListaFacturas {
    public NodoFactura* tope = null;

    public void GenerarFactura(int id, int idOrden, float total) {
        NodoFactura* nueva = (NodoFactura*)Marshal.AllocHGlobal(sizeof(NodoFactura));
        nueva->ID = id;
        nueva->ID_Orden = idOrden;
        nueva->Total = total;
        nueva->siguiente = tope;
        tope = nueva;
    }

    public void GenerarReporteFacturas()
    {
        if (tope == null)
        {
            Console.WriteLine("No hay facturas registradas.");
            return;
        }

        string rutaDot = "reporte_facturas.dot";
        string rutaImagen = "reporte_facturas.png";

        using (StreamWriter writer = new StreamWriter(rutaDot))
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=TB;"); // Orientación de arriba hacia abajo
            writer.WriteLine("    node [shape=box, style=filled, color=lightblue];");

            // Contar la cantidad total de facturas para numerarlas de forma descendente
            NodoFactura* actual = tope;
            int totalFacturas = 0;
            while (actual != null)
            {
                totalFacturas++;
                actual = actual->siguiente;
            }

            // Reiniciar el puntero al tope y asignar números descendentes
            actual = tope;
            int contador = totalFacturas; // Comenzamos con el número más alto

            while (actual != null)
            {
                string nodoActual = $"factura{contador}";
                writer.WriteLine($"    {nodoActual} [label=\"Factura {contador}\\nID Factura: {actual->ID}\\nID Orden: {actual->ID_Orden}\\nTotal: {actual->Total}\"];");

                if (actual->siguiente != null)
                {
                    string nodoSiguiente = $"factura{contador - 1}";
                    writer.WriteLine($"    {nodoActual} -> {nodoSiguiente} [dir=forward];"); // 🔹 Flechas apuntan hacia abajo
                }

                actual = actual->siguiente;
                contador--; // 🔹 Ahora numeramos en orden descendente
            }

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

            Console.WriteLine("Reporte de facturación generado correctamente: " + rutaImagen);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al generar el reporte: " + ex.Message);
        }
    }





    public void CancelarFactura() {
        if (tope == null) {
            Console.WriteLine("No hay facturas pendientes.");
            return;
        }

        // Guardamos la factura que vamos a eliminar
        NodoFactura* facturaEliminada = tope;
        Console.WriteLine($"Factura ID: {facturaEliminada->ID} cancelada. Total: Q{facturaEliminada->Total}");

        // Movemos el tope al siguiente nodo
        tope = tope->siguiente;

        // Liberamos la memoria de la factura eliminada
        Marshal.FreeHGlobal((IntPtr)facturaEliminada);
    }
}
