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

        Button reporteRepuestosButton = new Button("Reporte de Repuestos");
        reporteRepuestosButton.Clicked += (sender, e) => new ReporteRepuestoWindow().Show();

        Button reporteServiciosButton = new Button("Reporte de Servicios");
        reporteServiciosButton.Clicked += (sender, e) => new ReporteServicioWindow().Show();

        Button reporteFacturasButton = new Button("Reporte de Facturación");
        reporteFacturasButton.Clicked += (sender, e) => new ReporteFacturasWindow().Show();

        Button reporteBitacoraButton = new Button("Reporte de Bitácora");
        reporteBitacoraButton.Clicked += (sender, e) => new ReporteBitacoraWindow().Show();

        Button topVehiculosServiciosButton = new Button("Top 5 Vehículos con Más Servicios");
        topVehiculosServiciosButton.Clicked += (sender, e) => new VehiculosMasServicios().Show();

        Button topVehiculosAntiguosButton = new Button("Top 5 Vehículos Más Antiguos");
        topVehiculosAntiguosButton.Clicked += (sender, e) => new VehiculosMasAntiguos().Show();


        
        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(reporteUsuariosButton, false, false, 5);
        vbox.PackStart(reporteVehiculosButton, false, false, 5);
        vbox.PackStart(reporteRepuestosButton, false, false, 5);
        vbox.PackStart(reporteServiciosButton, false, false, 5);
        vbox.PackStart(reporteFacturasButton, false, false, 5);
        vbox.PackStart(reporteBitacoraButton, false, false, 5);
        vbox.PackStart(topVehiculosServiciosButton, false, false, 5);
        vbox.PackStart(topVehiculosAntiguosButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }
}
