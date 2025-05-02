using Gtk;
using System;

public class WindowUser : Window
{
    private Button logoutButton;
    private Button misServiciosButton;
    private Button registrarVehiculoButton;
    private UsuarioData usuario;

    // Cambiar el constructor para recibir UsuarioData en lugar de NodoUsuario*
    public WindowUser(UsuarioData usuario) : base("Panel de Usuario")
    {
        this.usuario = usuario;
        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);
        BorderWidth = 15;

        // Contenedor principal
        var vbox = new Box(Orientation.Vertical, 10);
        
        // Encabezado
        var welcomeLabel = new Label($"<big><b>Bienvenido, {usuario.Nombres} {usuario.Apellidos}</b></big>")
        {
            UseMarkup = true,
            Halign = Align.Center,
            MarginBottom = 20
        };

        // Información del usuario
        var userInfoFrame = new Frame("Información de tu cuenta");
        var userInfoBox = new Box(Orientation.Vertical, 5) { Margin = 10 };
        
        var correoLabel = new Label($"<b>Correo:</b> {usuario.Correo}") { UseMarkup = true, Halign = Align.Start };
        var edadLabel = new Label($"<b>Edad:</b> {usuario.Edad}") { UseMarkup = true, Halign = Align.Start };
        
        userInfoBox.PackStart(correoLabel, false, false, 0);
        userInfoBox.PackStart(edadLabel, false, false, 0);
        
        userInfoFrame.Add(userInfoBox);

        // Botones de acción
        var buttonBox = new Box(Orientation.Vertical, 5) 
        {
            Halign = Align.Center,
            Valign = Align.End
        };

        misServiciosButton = new Button("Mis Servicios") 
        {
            WidthRequest = 200,
            MarginBottom = 5
        };

        registrarVehiculoButton = new Button("Registrar vehículo") 
        {
            WidthRequest = 200,
            MarginBottom = 5
        };

        logoutButton = new Button("Cerrar sesión") 
        {
            WidthRequest = 200
        };

        buttonBox.PackStart(misServiciosButton, false, false, 0);
        buttonBox.PackStart(registrarVehiculoButton, false, false, 0);
        buttonBox.PackStart(logoutButton, false, false, 0);

        // Ensamblar la interfaz
        vbox.PackStart(welcomeLabel, false, false, 0);
        vbox.PackStart(userInfoFrame, false, false, 0);
        vbox.PackStart(new Box(Orientation.Vertical, 0) { Expand = true }, true, true, 0);
        vbox.PackStart(buttonBox, false, false, 0);

        Add(vbox);

        // Conectar eventos
        logoutButton.Clicked += OnLogoutClicked;
        misServiciosButton.Clicked += OnMisServiciosClicked;
        registrarVehiculoButton.Clicked += OnRegistrarVehiculoClicked;

        ShowAll();
    }

    private void OnMisServiciosClicked(object? sender, EventArgs e)
    {
        var serviciosWindow = new WindowVerServicios(usuario.ID);
        serviciosWindow.Show();
        this.Destroy();
    }

    private void OnRegistrarVehiculoClicked(object? sender, EventArgs e)
    {
        var registroWindow = new WindowRegistroVehiculo(usuario.ID);
        registroWindow.Show();
        this.Destroy();
    }

    private void OnLogoutClicked(object? sender, EventArgs e)
    {
        WindowLogin loginWindow = new WindowLogin();
        loginWindow.Show();
        this.Destroy();
    }
}