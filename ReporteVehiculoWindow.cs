using System;
using Gtk;

public class ReporteVehiculoWindow : Window
{
    public ReporteVehiculoWindow() : base("Reporte de Vehículos")
    {
        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Generar Reporte de Vehículos:");

        Button generarReporteButton = new Button("Generar Reporte");
        generarReporteButton.Clicked += (sender, e) => OnGenerarReporte();

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(generarReporteButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private void OnGenerarReporte()
    {
        Program.vehiculos.GenerarReporteVehiculos();

        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
            "Reporte de vehículos generado exitosamente.");
        successDialog.Run();
        successDialog.Destroy();
    }
}
