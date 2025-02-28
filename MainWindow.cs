using System;
using Gtk;

public class MainWindow : Window
{
    public MainWindow() : base("AutoGest Pro - Menú Principal")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        DeleteEvent += (o, args) => Application.Quit();

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Bienvenido al sistema AutoGest Pro");

        Button cargaMasivaButton = new Button("Carga Masiva");
        cargaMasivaButton.Clicked += (sender, e) =>
        {
            new CargaMasivaWindow().Show();
        };

        Button ingresoManualButton = new Button("Ingreso Manual");
        ingresoManualButton.Clicked += (sender, e) =>
        {
            new IngresoManualWindow().Show();
        };

        Button gestionUsuariosButton = new Button("Gestión de Usuarios");
        gestionUsuariosButton.Clicked += (sender, e) => {
            new GestionUsuariosWindow().Show();
        };

        Button generarServicioButton = new Button("Generar Servicio");
        generarServicioButton.Clicked += (sender, e) => {
            new GenerarServicioWindow().Show();
        };

        Button generarReporteButton = new Button("Generar Reporte");
        generarReporteButton.Clicked += (sender, e) => {
            new GenerarReporteWindow().Show();
        };


        Button logoutButton = new Button("Cerrar Sesión");
        logoutButton.Clicked += (sender, e) =>
        {
            new LoginWindow().Show();
            Destroy();
        };

        vbox.PackStart(label, false, false, 10);
        vbox.PackStart(cargaMasivaButton, false, false, 10);
        vbox.PackStart(ingresoManualButton, false, false, 10);
        vbox.PackStart(gestionUsuariosButton, false, false, 10);
        vbox.PackStart(generarServicioButton, false, false, 10);
        vbox.PackStart(generarReporteButton, false, false, 10);
        vbox.PackStart(logoutButton, false, false, 10);

        Add(vbox);
        ShowAll();
    }
}
