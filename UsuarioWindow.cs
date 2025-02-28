using System;
using Gtk;

unsafe class UsuarioWindow : Window
{
    public unsafe UsuarioWindow(NodoUsuario* usuario) : base("Bienvenido")
    {
        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label nameLabel = new Label($"Bienvenido, {GetString(usuario->Nombres)} {GetString(usuario->Apellidos)}");

        Button logoutButton = new Button("Cerrar Sesión");
        logoutButton.Clicked += (sender, e) =>
        {
            new LoginWindow().Show();
            Destroy();
        };

        vbox.PackStart(nameLabel, false, false, 10);
        vbox.PackStart(logoutButton, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    private string GetString(char* charArray)
    {
        return new string(charArray).TrimEnd('\0');
    }
}
