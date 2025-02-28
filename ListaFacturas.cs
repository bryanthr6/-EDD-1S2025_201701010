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

            NodoFactura* actual = tope;
            int contador = 1; // Para numerar las facturas como Factura 1, Factura 2, etc.

            while (actual != null)
            {
                string nodoActual = $"factura{contador}";
                writer.WriteLine($"    {nodoActual} [label=\"Factura {contador}\\nID Factura: {actual->ID}\\nID Orden: {actual->ID_Orden}\\nTotal: {actual->Total}\"];");

                if (actual->siguiente != null)
                {
                    string nodoSiguiente = $"factura{contador + 1}";
                    writer.WriteLine($"    {nodoActual} -> {nodoSiguiente} [dir=back];");
                }

                actual = actual->siguiente;
                contador++;
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

        NodoFactura* temp = tope;
        tope = tope->siguiente;
        Console.WriteLine($"Factura ID: {temp->ID} pagada. Total: {temp->Total}");
        Marshal.FreeHGlobal((IntPtr)temp);
    }
}
