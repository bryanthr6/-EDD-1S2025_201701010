using Gtk;
using System;
using System.Runtime.InteropServices;

public class WindowUser : Window
{
    private Button logoutButton;

    public unsafe WindowUser(NodoUsuario* usuario) : base("Panel de Usuario")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;

        var vbox = new Box(Orientation.Vertical, 10);
        
        string nombres = Program.listaUsuarios.PtrToString(usuario->Nombres);
        string apellidos = Program.listaUsuarios.PtrToString(usuario->Apellidos);

        var welcomeLabel = new Label($"Bienvenido {nombres} {apellidos}")
        {
            MarginBottom = 20,
            Halign = Align.Center // Centrar el texto
        };

        // Agregar más información del usuario
        var infoLabel = new Label($"Correo: {Program.listaUsuarios.PtrToString(usuario->Correo)}\nEdad: {usuario->Edad}")
        {
            Halign = Align.Center,
            MarginBottom = 20
        };

        logoutButton = new Button("Cerrar sesión")
        {
            MarginTop = 20,
            WidthRequest = 150 // Ancho fijo para el botón
        };

        vbox.PackStart(welcomeLabel, false, false, 0);
        vbox.PackStart(infoLabel, false, false, 0);
        vbox.PackStart(new Label(""), true, true, 0); // Espaciador
        vbox.PackStart(logoutButton, false, false, 0);

        Add(vbox);
        logoutButton.Clicked += OnLogoutClicked;
        ShowAll();
    }

    private void OnLogoutClicked(object? sender, EventArgs e)
    {
        WindowLogin loginWindow = new WindowLogin();
        loginWindow.Show();
        this.Destroy();
    }
}