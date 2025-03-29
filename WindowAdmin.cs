using Gtk;
using System;

public class WindowAdmin : Window
{
    private Button logoutButton;
    private Button cargaMasivaButton;
    private Button gestionEntidadesButton;
    private Button actualizarRepuestosButton;
    private Button verRepuestosButton;
    private Button generarServicioButton; // Nuevo botón

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
            MarginBottom = 20 
        }, false, false, 0);

        // Botón de Carga Masiva
        cargaMasivaButton = new Button("Carga Masiva")
        {
            MarginBottom = 10
        };
        cargaMasivaButton.Clicked += OnCargaMasivaClicked;

        // Botón de Gestión de Entidades
        gestionEntidadesButton = new Button("Gestión de Entidades")
        {
            MarginBottom = 10
        };
        gestionEntidadesButton.Clicked += OnGestionEntidadesClicked;

        // Botón de Actualización de Repuestos
        actualizarRepuestosButton = new Button("Actualización de Repuestos")
        {
            MarginBottom = 10
        };
        actualizarRepuestosButton.Clicked += OnActualizarRepuestosClicked;

        // Botón de Visualizar Repuestos
        verRepuestosButton = new Button("Visualizar Repuestos")
        {
            MarginBottom = 10
        };
        verRepuestosButton.Clicked += OnVerRepuestosClicked;

        // Nuevo Botón de Generar Servicio
        generarServicioButton = new Button("Generar Servicio")
        {
            MarginBottom = 10
        };
        generarServicioButton.Clicked += OnGenerarServicioClicked;

        // Botón de Cerrar Sesión
        logoutButton = new Button("Cerrar sesión")
        {
            MarginTop = 20
        };

        // Añadir elementos al contenedor
        vbox.PackStart(cargaMasivaButton, false, false, 0);
        vbox.PackStart(gestionEntidadesButton, false, false, 0);
        vbox.PackStart(actualizarRepuestosButton, false, false, 0);
        vbox.PackStart(verRepuestosButton, false, false, 0);
        vbox.PackStart(generarServicioButton, false, false, 0); // Agregar nuevo botón
        vbox.PackStart(logoutButton, false, false, 0);
        
        // Añadir contenedor a la ventana
        Add(vbox);

        // Conectar eventos
        logoutButton.Clicked += OnLogoutClicked;
        
        // Mostrar todos los widgets
        ShowAll();
    }

    private void OnCargaMasivaClicked(object? sender, EventArgs e)
    {
        // Abrir ventana de carga masiva
        WindowCarga cargaWindow = new WindowCarga();
        cargaWindow.Show();
        this.Hide();
    }

    private void OnGestionEntidadesClicked(object? sender, EventArgs e)
    {
        // Abrir ventana de gestión de entidades
        WindowGestion gestionWindow = new WindowGestion();
        gestionWindow.Show();
        this.Hide();
    }

    private void OnActualizarRepuestosClicked(object? sender, EventArgs e)
    {
        // Abrir ventana de actualización de repuestos
        WindowActualizacionRepuesto actualizarRepWindow = new WindowActualizacionRepuesto();
        actualizarRepWindow.Show();
        this.Hide();
    }

    private void OnVerRepuestosClicked(object? sender, EventArgs e)
    {
        // Abrir ventana de visualización de repuestos
        WindowVerRepuestos verRepuestosWindow = new WindowVerRepuestos();
        verRepuestosWindow.Show();
        this.Hide();
    }

    private void OnGenerarServicioClicked(object? sender, EventArgs e)
    {
        // Abrir ventana de generación de servicio
        WindowGenerarServicio generarServicioWindow = new WindowGenerarServicio();
        generarServicioWindow.Show();
        this.Hide();
    }

    private void OnLogoutClicked(object? sender, EventArgs e)
    {
        // Al hacer clic en cerrar sesión, se regresa a la ventana de login
        WindowLogin loginWindow = new WindowLogin();
        loginWindow.Show();
        this.Destroy();
    }
}