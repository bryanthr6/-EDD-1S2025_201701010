using Gtk;
using System;
using System.IO;
using System.Diagnostics;
using System.Text;

public class WindowReportes : Window
{
    public WindowReportes() : base("Generar Reportes")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;
        
        var vbox = new Box(Orientation.Vertical, 10);
        
        Label titulo = new Label("Generar Reportes Gráficos") { 
            MarginBottom = 20,
            Justify = Justification.Center
        };
        
        Button reporteUsuariosBtn = new Button("Reporte de Usuarios");
        Button reporteVehiculosBtn = new Button("Reporte de Vehículos");
        Button reporteRepuestosBtn = new Button("Reporte de Repuestos");
        Button reporteServiciosBtn = new Button("Reporte de Servicios");
        Button regresarBtn = new Button("Regresar") { MarginTop = 20 };

        vbox.PackStart(titulo, false, false, 0);
        vbox.PackStart(reporteUsuariosBtn, false, false, 0);
        vbox.PackStart(reporteVehiculosBtn, false, false, 0);
        vbox.PackStart(reporteRepuestosBtn, false, false, 0);
        vbox.PackStart(reporteServiciosBtn, false, false, 0);
        vbox.PackStart(regresarBtn, false, false, 0);

        Add(vbox);

        // Conectar eventos
        reporteUsuariosBtn.Clicked += (s, e) => GenerarReporteUsuarios();
        reporteVehiculosBtn.Clicked += (s, e) => GenerarReporteVehiculos();
        reporteRepuestosBtn.Clicked += (s, e) => GenerarReporteRepuestos();
        reporteServiciosBtn.Clicked += (s, e) => GenerarReporteServicios();
        regresarBtn.Clicked += OnRegresarClicked;

        ShowAll();
    }

    private void OnRegresarClicked(object? sender, EventArgs e)
    {
        WindowAdmin adminWindow = new WindowAdmin();
        adminWindow.Show();
        this.Destroy();
    }

    private void GenerarReporteUsuarios()
    {
        try
        {
            // Crear directorio si no existe
            string reportesDir = "Reportes";
            if (!Directory.Exists(reportesDir))
                Directory.CreateDirectory(reportesDir);

            string dotContent = "digraph G {\n";
            dotContent += "  node [shape=record, fontname=Arial];\n";
            dotContent += "  rankdir=LR;\n\n";
            dotContent += "  label = \"Reporte de Usuarios\";\n";
            dotContent += "  fontsize = 20;\n\n";

            // Recorrer lista de usuarios
            unsafe
            {
                NodoUsuario* actual = Program.listaUsuarios.GetCabeza();
                while (actual != null)
                {
                    string nombres = Program.listaUsuarios.PtrToString(actual->Nombres);
                    string apellidos = Program.listaUsuarios.PtrToString(actual->Apellidos);
                    string correo = Program.listaUsuarios.PtrToString(actual->Correo);
                    
                    dotContent += $"  usuario{actual->Id} [label=\"{{ID: {actual->Id}|Nombre: {nombres} {apellidos}|Correo: {correo}|Edad: {actual->Edad}}}\"];\n";
                    
                    if (actual->Siguiente != null)
                        dotContent += $"  usuario{actual->Id} -> usuario{actual->Siguiente->Id} [color=\"blue\"];\n";
                    
                    actual = actual->Siguiente;
                }
            }

            dotContent += "}";

            string dotFileName = "usuarios.dot";
            string pngFileName = "reporteUsuarios.png";
            
            string dotFilePath = System.IO.Path.Combine(reportesDir, dotFileName);
            string pngFilePath = System.IO.Path.Combine(reportesDir, pngFileName);

            File.WriteAllText(dotFilePath, dotContent);
            
            // Ejecutar Graphviz
            var process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{pngFilePath}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.WaitForExit();

            MostrarMensaje("Reporte generado", $"Se ha generado el reporte de usuarios en: {pngFilePath}");
        }
        catch (Exception ex)
        {
            MostrarError($"Error al generar reporte: {ex.Message}");
        }
    }

    private void GenerarReporteVehiculos()
    {
        try
        {
            string reportesDir = "Reportes";
            if (!Directory.Exists(reportesDir))
                Directory.CreateDirectory(reportesDir);

            string dotContent = "digraph G {\n";
            dotContent += "  node [shape=record, fontname=Arial];\n";
            dotContent += "  rankdir=LR;\n\n";
            dotContent += "  label = \"Reporte de Vehículos\";\n";
            dotContent += "  fontsize = 20;\n\n";

            // Recorrer lista de vehículos
            unsafe
            {
                NodoVehiculo* actual = Program.listaVehiculos.GetCabeza();
                while (actual != null)
                {
                    string marca = Program.listaVehiculos.PtrToString(actual->Marca);
                    string placa = Program.listaVehiculos.PtrToString(actual->Placa);
                    
                    dotContent += $"  vehiculo{actual->Id} [label=\"{{ID: {actual->Id}|UsuarioID: {actual->IdUsuario}|Marca: {marca}|Modelo: {actual->Modelo}|Placa: {placa}}}\"];\n";
                    
                    if (actual->Anterior != null)
                        dotContent += $"  vehiculo{actual->Id} -> vehiculo{actual->Anterior->Id} [dir=both, color=\"green\"];\n";
                    
                    actual = actual->Siguiente;
                }
            }

            dotContent += "}";

            string dotFileName = "vehiculos.dot";
            string pngFileName = "reporteVehiculos.png";
            
            string dotFilePath = System.IO.Path.Combine(reportesDir, dotFileName);
            string pngFilePath = System.IO.Path.Combine(reportesDir, pngFileName);

            File.WriteAllText(dotFilePath, dotContent);
            
            var process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{pngFilePath}\"";
            process.Start();
            process.WaitForExit();

            MostrarMensaje("Reporte generado", $"Se ha generado el reporte de vehículos en: {pngFilePath}");
        }
        catch (Exception ex)
        {
            MostrarError($"Error al generar reporte: {ex.Message}");
        }
    }

    private void GenerarReporteRepuestos()
    {
        try
        {
            string reportesDir = "Reportes";
            if (!Directory.Exists(reportesDir))
                Directory.CreateDirectory(reportesDir);

            string dotContent = "digraph G {\n";
            dotContent += "  node [shape=record, fontname=Arial];\n";
            dotContent += "  rankdir=TB;\n\n";
            dotContent += "  label = \"Reporte de Repuestos (Árbol AVL)\";\n";
            dotContent += "  fontsize = 20;\n\n";

            // Generar contenido del árbol AVL
            dotContent += Program.arbolRepuestos.GenerarDot();

            dotContent += "}";

            string dotFileName = "repuestos.dot";
            string pngFileName = "reporteRepuestos.png";
            
            string dotFilePath = System.IO.Path.Combine(reportesDir, dotFileName);
            string pngFilePath = System.IO.Path.Combine(reportesDir, pngFileName);

            File.WriteAllText(dotFilePath, dotContent);
            
            var process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{pngFilePath}\"";
            process.Start();
            process.WaitForExit();

            MostrarMensaje("Reporte generado", $"Se ha generado el reporte de repuestos en: {pngFilePath}");
        }
        catch (Exception ex)
        {
            MostrarError($"Error al generar reporte: {ex.Message}");
        }
    }

    private void GenerarReporteServicios()
    {
        try
        {
            string reportesDir = "Reportes";
            if (!Directory.Exists(reportesDir))
                Directory.CreateDirectory(reportesDir);

            string dotContent = "digraph G {\n";
            dotContent += "  node [shape=record, fontname=Arial];\n";
            dotContent += "  rankdir=TB;\n\n";
            dotContent += "  label = \"Reporte de Servicios (Árbol Binario)\";\n";
            dotContent += "  fontsize = 20;\n\n";

            // Generar contenido del árbol binario de servicios
            dotContent += Program.arbolServicios.GenerarDot();

            dotContent += "}";

            string dotFileName = "servicios.dot";
            string pngFileName = "reporteServicios.png";
            
            string dotFilePath = System.IO.Path.Combine(reportesDir, dotFileName);
            string pngFilePath = System.IO.Path.Combine(reportesDir, pngFileName);

            File.WriteAllText(dotFilePath, dotContent);
            
            var process = new Process();
            process.StartInfo.FileName = "dot";
            process.StartInfo.Arguments = $"-Tpng \"{dotFilePath}\" -o \"{pngFilePath}\"";
            process.Start();
            process.WaitForExit();

            MostrarMensaje("Reporte generado", $"Se ha generado el reporte de servicios en: {pngFilePath}");
        }
        catch (Exception ex)
        {
            MostrarError($"Error al generar reporte: {ex.Message}");
        }
    }

    private void MostrarMensaje(string titulo, string mensaje)
    {
        using (var dialog = new MessageDialog(
            this, 
            DialogFlags.Modal, 
            MessageType.Info, 
            ButtonsType.Ok, 
            mensaje))
        {
            dialog.Title = titulo;
            dialog.Run();
        }
    }

    private void MostrarError(string mensaje)
    {
        using (var dialog = new MessageDialog(
            this, 
            DialogFlags.Modal, 
            MessageType.Error, 
            ButtonsType.Ok, 
            mensaje))
        {
            dialog.Title = "Error";
            dialog.Run();
        }
    }
}