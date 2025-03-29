using Gtk;
using System;

public class WindowGestion : Window
{
    private Button usuariosButton;
    private Button vehiculosButton;
    private Button volverButton;

    public WindowGestion() : base("Gestión de Entidades")
    {
        // Configuración básica de la ventana
        SetDefaultSize(500, 300);
        SetPosition(WindowPosition.Center);
        BorderWidth = 15;

        // Crear contenedor principal
        Box vbox = new Box(Orientation.Vertical, 10);

        // Título
        vbox.PackStart(new Label("Seleccione el tipo de entidad a gestionar:") { 
            MarginBottom = 20 
        }, false, false, 0);

        // Botón de Usuarios
        usuariosButton = new Button("Gestión de Usuarios")
        {
            MarginBottom = 10
        };
        usuariosButton.Clicked += OnUsuariosClicked;

        // Botón de Vehículos
        vehiculosButton = new Button("Gestión de Vehículos")
        {
            MarginBottom = 10
        };
        vehiculosButton.Clicked += OnVehiculosClicked;

        // Botón de Volver
        volverButton = new Button("Volver al Menú Principal")
        {
            MarginTop = 20
        };
        volverButton.Clicked += OnVolverClicked;

        // Añadir elementos al contenedor
        vbox.PackStart(usuariosButton, false, false, 0);
        vbox.PackStart(vehiculosButton, false, false, 0);
        vbox.PackStart(volverButton, false, false, 0);

        // Añadir contenedor a la ventana
        Add(vbox);

        // Mostrar todos los widgets
        ShowAll();
    }

    private void OnUsuariosClicked(object? sender, EventArgs e)
    {
        // Abrir ventana de edición de usuarios
        WindowEditarUsuario editarUsuarioWindow = new WindowEditarUsuario();
        editarUsuarioWindow.Show();
        this.Hide(); // Ocultar la ventana actual
        }

    private void OnVehiculosClicked(object? sender, EventArgs e)
    {
        // Abrir ventana de edición de vehículos
        WindowEditarVehiculo editarVehiculoWindow = new WindowEditarVehiculo();
        editarVehiculoWindow.Show();
        this.Hide();
    }

    private void OnVolverClicked(object? sender, EventArgs e)
    {
        // Regresar a la ventana de administrador
        WindowAdmin adminWindow = new WindowAdmin();
        adminWindow.Show();
        this.Destroy();
    }

    
}