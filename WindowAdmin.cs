using Gtk;
using System;

public class WindowAdmin : Window
{
    private Button logoutButton;
    private Button cargaMasivaButton;
    private Button gestionEntidadesButton;
    private Button actualizarRepuestosButton;
    private Button verRepuestosButton;
    private Button generarServicioButton;
    private Button generarReportesButton;

    public WindowAdmin() : base("Menú Administrador")
    {
        // Configuración básica de la ventana
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;
        
        // Crear contenedor principal
        Box vbox = new Box(Orientation.Vertical, 10);
        
        // Título
        vbox.PackStart(new Label("Bienvenido al Panel de Administración") { 
            MarginBottom = 20,
            Justify = Justification.Center
        }, false, false, 0);

        // Botones principales
        cargaMasivaButton = new Button("Carga Masiva") { MarginBottom = 10 };
        gestionEntidadesButton = new Button("Gestión de Entidades") { MarginBottom = 10 };
        actualizarRepuestosButton = new Button("Actualización de Repuestos") { MarginBottom = 10 };
        verRepuestosButton = new Button("Visualizar Repuestos") { MarginBottom = 10 };
        generarServicioButton = new Button("Generar Servicio") { MarginBottom = 10 };
        generarReportesButton = new Button("Generar Reportes") { MarginBottom = 10 };
        logoutButton = new Button("Cerrar sesión") { MarginTop = 20 };

        // Añadir elementos al contenedor
        vbox.PackStart(cargaMasivaButton, false, false, 0);
        vbox.PackStart(gestionEntidadesButton, false, false, 0);
        vbox.PackStart(actualizarRepuestosButton, false, false, 0);
        vbox.PackStart(verRepuestosButton, false, false, 0);
        vbox.PackStart(generarServicioButton, false, false, 0);
        vbox.PackStart(generarReportesButton, false, false, 0);
        vbox.PackStart(logoutButton, false, false, 0);
        
        // Añadir contenedor a la ventana
        Add(vbox);

        // Conectar eventos
        cargaMasivaButton.Clicked += OnCargaMasivaClicked;
        gestionEntidadesButton.Clicked += OnGestionEntidadesClicked;
        actualizarRepuestosButton.Clicked += OnActualizarRepuestosClicked;
        verRepuestosButton.Clicked += OnVerRepuestosClicked;
        generarServicioButton.Clicked += OnGenerarServicioClicked;
        generarReportesButton.Clicked += OnGenerarReportesClicked;
        logoutButton.Clicked += OnLogoutClicked;
        
        ShowAll();
    }

    private void OnCargaMasivaClicked(object? sender, EventArgs e)
    {
        WindowCarga cargaWindow = new WindowCarga();
        cargaWindow.Show();
        this.Hide();
    }

    private void OnGestionEntidadesClicked(object? sender, EventArgs e)
    {
        WindowGestion gestionWindow = new WindowGestion();
        gestionWindow.Show();
        this.Hide();
    }

    private void OnActualizarRepuestosClicked(object? sender, EventArgs e)
    {
        WindowActualizacionRepuesto actualizarRepWindow = new WindowActualizacionRepuesto();
        actualizarRepWindow.Show();
        this.Hide();
    }

    private void OnVerRepuestosClicked(object? sender, EventArgs e)
    {
        WindowVerRepuestos verRepuestosWindow = new WindowVerRepuestos();
        verRepuestosWindow.Show();
        this.Hide();
    }

    private void OnGenerarServicioClicked(object? sender, EventArgs e)
    {
        WindowGenerarServicio generarServicioWindow = new WindowGenerarServicio();
        generarServicioWindow.Show();
        this.Hide();
    }

    private void OnGenerarReportesClicked(object? sender, EventArgs e)
    {
        WindowReportes reportesWindow = new WindowReportes();
        reportesWindow.Show();
        this.Hide();
    }

    private void OnLogoutClicked(object? sender, EventArgs e)
    {
        WindowLogin loginWindow = new WindowLogin();
        loginWindow.Show();
        this.Destroy();
    }
}