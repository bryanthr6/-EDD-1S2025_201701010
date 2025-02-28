using System;
using Gtk;

public class LoginWindow : Window
{
    public LoginWindow() : base("Inicio de Sesión")
    {
        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Entry emailEntry = new Entry() { PlaceholderText = "Correo" };
        Entry passwordEntry = new Entry() { Visibility = false, PlaceholderText = "Contraseña" };
        Button loginButton = new Button("Ingresar");

        loginButton.Clicked += (sender, e) => OnLogin(emailEntry.Text, passwordEntry.Text);

        vbox.PackStart(emailEntry, false, false, 5);
        vbox.PackStart(passwordEntry, false, false, 5);
        vbox.PackStart(loginButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    void OnLogin(string email, string password)
    {
        if (email == "root@gmail.com" && password == "root123")
        {
            Console.WriteLine("Inicio de sesión exitoso.");
            new MainWindow().Show();
            Destroy();
        }
        else
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Credenciales incorrectas");
            dialog.Run();
            dialog.Destroy();
        }
    }
}
