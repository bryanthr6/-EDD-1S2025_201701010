using System;
using Gtk;

public class ReporteBitacoraWindow : Window
{
    public ReporteBitacoraWindow() : base("Reporte de Bitácora")
    {
        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Generando reporte de bitácora...");

        Button generarReporteButton = new Button("Generar Reporte");
        generarReporteButton.Clicked += (sender, e) => GenerarReporte();

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(generarReporteButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    void GenerarReporte()
    {
        Program.bitacora.GenerarReporteMatriz();  // 🔥 Llama a la función que genera el reporte
        MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Reporte generado con éxito.");
        dialog.Run();
        dialog.Destroy();
    }
}
