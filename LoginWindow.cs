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

    unsafe void OnLogin(string email, string password)
    {
        // Verificar si es el usuario root
        if (email == "root@gmail.com" && password == "root123")
        {
            Console.WriteLine("Inicio de sesión exitoso. (ROOT)");
            new MainWindow().Show();
            Destroy();
            return;
        }

        // Buscar el usuario en la lista
        NodoUsuario* usuario = Program.usuarios.BuscarPorCorreo(email);

        if (usuario != null && GetString(usuario->Contrasenia) == password)
        {
            Console.WriteLine($"Inicio de sesión exitoso. Bienvenido {GetString(usuario->Nombres)}");
            new UsuarioWindow(usuario).Show();  // 🔥 Abre la ventana del usuario
            Destroy();
        }
        else
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Credenciales incorrectas");
            dialog.Run();
            dialog.Destroy();
        }
    }

    // Función para convertir `char*` en `string`
    private unsafe string GetString(char* charArray)
    {
        return new string(charArray).TrimEnd('\0');
    }


}
