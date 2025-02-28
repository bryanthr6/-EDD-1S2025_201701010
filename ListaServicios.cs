using System;
using System.Runtime.InteropServices;
using System.Diagnostics;


unsafe class ListaServicios {
    public NodoServicio* frente = null;
    public NodoServicio* final = null;

    public void Insertar(int id, int idRepuesto, int idVehiculo, string detalles, float costo, ListaRepuestos repuestos, ListaVehiculos vehiculos) {
        if (repuestos.BuscarPorID(idRepuesto) == null) {
            Console.WriteLine("ID de repuesto no encontrado.");
            return;
        }
        if (vehiculos.BuscarPorID(idVehiculo) == null) {
            Console.WriteLine("ID de vehículo no encontrado.");
            return;
        }

        NodoServicio* nuevo = (NodoServicio*)Marshal.AllocHGlobal(sizeof(NodoServicio));
        nuevo->ID = id;
        nuevo->ID_Repuesto = idRepuesto;
        nuevo->ID_Vehiculo = idVehiculo;
        nuevo->Costo = costo;
        CopyString(nuevo->Detalles, detalles);
        nuevo->siguiente = null;

        if (final == null) {
            frente = final = nuevo;
        } else {
            final->siguiente = nuevo;
            final = nuevo;
        }

        //Insertar en la matriz dispersa de la bitácora
        Program.bitacora.Insertar(idVehiculo, idRepuesto, detalles);
    }



    public void GenerarReporteServicios()
    {
        if (frente == null)
        {
            Console.WriteLine("No hay servicios registrados.");
            return;
        }

        string rutaDot = "reporte_servicios.dot";
        string rutaImagen = "reporte_servicios.png";

        using (StreamWriter writer = new StreamWriter(rutaDot))
        {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=LR;"); // Orientación de izquierda a derecha
            writer.WriteLine("    node [shape=box, style=filled, color=lightblue];");

            NodoServicio* actual = frente;
            int contador = 1; // Para numerar los servicios como Servicio 1, Servicio 2, etc.

            while (actual != null)
            {
                string nodoActual = $"servicio{contador}";
                writer.WriteLine($"    {nodoActual} [label=\"Servicio {contador}\\nID Servicio: {actual->ID}\\nID Vehículo: {actual->ID_Vehiculo}\\nID Repuesto: {actual->ID_Repuesto}\\nDetalles: {GetString(actual->Detalles, 100)}\\nCosto: {actual->Costo}\"];");

                if (actual->siguiente != null)
                {
                    string nodoSiguiente = $"servicio{contador + 1}";
                    writer.WriteLine($"    {nodoActual} -> {nodoSiguiente};");
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

            Console.WriteLine("Reporte de servicios generado correctamente: " + rutaImagen);
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


    public NodoServicio* AtenderServicio() {
        if (frente == null) {
            Console.WriteLine("No hay servicios en espera.");
            return null;
        }

        NodoServicio* servicio = frente;
        frente = frente->siguiente;
        if (frente == null) {
            final = null;
        }
        return servicio;
    }

    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 99; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }
}
