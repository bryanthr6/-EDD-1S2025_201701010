using System;
using Gtk;

public class GenerarReporteWindow : Window
{
    public GenerarReporteWindow() : base("Generar Reporte")
    {
        SetDefaultSize(400, 250);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Seleccione el tipo de reporte:");

        Button reporteUsuariosButton = new Button("Reporte de Usuarios");
        reporteUsuariosButton.Clicked += (sender, e) => new ReporteUsuarioWindow().Show();

        Button reporteVehiculosButton = new Button("Reporte de Vehículos");
        reporteVehiculosButton.Clicked += (sender, e) => new ReporteVehiculoWindow().Show();

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(reporteUsuariosButton, false, false, 5);
        vbox.PackStart(reporteVehiculosButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }
}
