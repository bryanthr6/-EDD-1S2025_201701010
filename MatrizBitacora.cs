using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

unsafe class MatrizBitacora {
    public NodoMatriz* cabeza = null;

    public void Insertar(int idVehiculo, int idRepuesto, string detalle) {
        NodoMatriz* nuevo = (NodoMatriz*)Marshal.AllocHGlobal(sizeof(NodoMatriz));
        *nuevo = new NodoMatriz();  
        nuevo->ID_Vehiculo = idVehiculo;
        nuevo->ID_Repuesto = idRepuesto;
        nuevo->derecha = null;
        nuevo->abajo = null;
        CopyString(nuevo->Detalle, detalle); // Guardar el detalle del servicio

        if (cabeza == null) {
            cabeza = nuevo;
            return;
        }

        NodoMatriz* vehiculoActual = cabeza;
        NodoMatriz* anteriorVehiculo = null;

        while (vehiculoActual != null && vehiculoActual->ID_Vehiculo != idVehiculo) {
            anteriorVehiculo = vehiculoActual;
            vehiculoActual = vehiculoActual->abajo;
        }

        if (vehiculoActual == null) {
            anteriorVehiculo->abajo = nuevo;
        } else {
            NodoMatriz* repuestoActual = vehiculoActual;
            NodoMatriz* anteriorRepuesto = null;

            while (repuestoActual != null && repuestoActual->ID_Repuesto != idRepuesto) {
                anteriorRepuesto = repuestoActual;
                repuestoActual = repuestoActual->derecha;
            }

            if (repuestoActual == null) {
                anteriorRepuesto->derecha = nuevo;
            } else {
                Console.WriteLine($"El servicio entre el vehículo {idVehiculo} y el repuesto {idRepuesto} ya está registrado.");
                Marshal.FreeHGlobal((IntPtr)nuevo);
            }
        }
    }

    // Función para copiar strings a `char[]`
    private void CopyString(char* destination, string source) {
        int i;
        for (i = 0; i < source.Length && i < 99; i++) {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }


    public void GenerarReporteMatriz() {
        if (cabeza == null) {
            Console.WriteLine("No hay registros en la bitácora.");
            return;
        }

        string rutaDot = "reporte_bitacora.dot";
        string rutaImagen = "reporte_bitacora.png";

        using (StreamWriter writer = new StreamWriter(rutaDot)) {
            writer.WriteLine("digraph G {");
            writer.WriteLine("    rankdir=TB;"); // Orientación de arriba hacia abajo
            writer.WriteLine("    node [shape=plaintext];");
            writer.WriteLine("    matriz [label=<");

            // Iniciar la tabla
            writer.WriteLine("<TABLE BORDER=\"1\" CELLBORDER=\"1\" CELLSPACING=\"0\">");

            // Encabezado con los repuestos
            writer.Write("<TR><TD></TD>"); // Esquina vacía
            NodoMatriz* repuestoActual = cabeza;
            while (repuestoActual != null) {
                writer.Write($"<TD>Repuesto {repuestoActual->ID_Repuesto}</TD>");
                repuestoActual = repuestoActual->derecha;
            }
            writer.WriteLine("</TR>");

            // Recorrer la matriz y generar filas
            NodoMatriz* vehiculoActual = cabeza;
            while (vehiculoActual != null) {
                writer.Write($"<TR><TD>Vehículo {vehiculoActual->ID_Vehiculo}</TD>");

                NodoMatriz* servicioActual = vehiculoActual;
                while (servicioActual != null) {
                    string detalle = GetString(servicioActual->Detalle);
                    writer.Write($"<TD>{detalle}</TD>");
                    servicioActual = servicioActual->derecha;
                }
                writer.WriteLine("</TR>");
                vehiculoActual = vehiculoActual->abajo;
            }

            // Cerrar la tabla
            writer.WriteLine("</TABLE>");
            writer.WriteLine(">];");
            writer.WriteLine("}");
        }

        // Generar imagen con Graphviz
        try {
            ProcessStartInfo psi = new ProcessStartInfo {
                FileName = "dot",
                Arguments = $"-Tpng {rutaDot} -o {rutaImagen}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = psi }) {
                process.Start();
                process.WaitForExit();
            }

            Console.WriteLine("Reporte de bitácora generado correctamente: " + rutaImagen);
        } catch (Exception ex) {
            Console.WriteLine("Error al generar el reporte: " + ex.Message);
        }
    }

    private string GetString(char* charArray) {
        return new string(charArray).TrimEnd('\0');
    }



    public void MostrarMatriz() {
    NodoMatriz* fila = cabeza;
    while (fila != null) {
        NodoMatriz* columna = fila;
        Console.Write($"Vehículo {fila->ID_Vehiculo}: ");
        while (columna != null) {
            Console.Write($"[Repuesto {columna->ID_Repuesto}] -> ");
            columna = columna->derecha;
        }
        Console.WriteLine("NULL");
        fila = fila->abajo;
    }
}

}
