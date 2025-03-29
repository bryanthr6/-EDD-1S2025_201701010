using Gtk;
using System;

public class WindowLogin : Window
{
    private Entry emailEntry;
    private Entry passwordEntry;
    private Button loginButton;

    public WindowLogin() : base("Login")
    {
        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;
        
        var vbox = new Box(Orientation.Vertical, 10);
        
        Label emailLabel = new Label("Correo electrónico:");
        Label passwordLabel = new Label("Contraseña:");
        
        emailEntry = new Entry() { PlaceholderText = "usuario@ejemplo.com" };
        passwordEntry = new Entry() 
        { 
            PlaceholderText = "contraseña", 
            Visibility = false,
            InvisibleChar = '•'
        };
        
        loginButton = new Button("Iniciar sesión") { MarginTop = 20 };

        vbox.PackStart(emailLabel, false, false, 0);
        vbox.PackStart(emailEntry, false, false, 0);
        vbox.PackStart(passwordLabel, false, false, 0);
        vbox.PackStart(passwordEntry, false, false, 0);
        vbox.PackStart(loginButton, false, false, 0);

        Add(vbox);
        loginButton.Clicked += OnLoginClicked;
        ShowAll();
    }

    private unsafe void OnLoginClicked(object? sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string password = passwordEntry.Text;

        // Validar admin
        if (email == "admin@usac.com" && password == "admin123")
        {
            WindowAdmin adminWindow = new WindowAdmin();
            adminWindow.Show();
            this.Destroy();
            return;
        }

        // Validar usuario normal
        var usuario = Program.listaUsuarios.BuscarPorCorreo(email);
        if (usuario != null)
        {
            string contraseniaAlmacenada = Program.listaUsuarios.PtrToString(usuario->Contrasenia);
            if (password == contraseniaAlmacenada)
            {
                WindowUser userWindow = new WindowUser(usuario);
                userWindow.Show();
                this.Destroy();
                return;
            }
        }

        // Si no coincide ninguna credencial
        using (MessageDialog errorDialog = new MessageDialog(
            this, 
            DialogFlags.Modal, 
            MessageType.Error, 
            ButtonsType.Ok, 
            "Credenciales incorrectas"))
        {
            errorDialog.Run();
        }
    }
}