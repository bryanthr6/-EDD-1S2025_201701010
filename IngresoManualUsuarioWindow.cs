using System;
using Gtk;

public class IngresoManualUsuarioWindow : Window
{
    public IngresoManualUsuarioWindow() : base("Ingreso Manual de Usuario")
    {
        SetDefaultSize(400, 250);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Ingrese los datos del usuario:");
        
        Entry idEntry = new Entry() { PlaceholderText = "ID" };
        Entry nombresEntry = new Entry() { PlaceholderText = "Nombres" };
        Entry apellidosEntry = new Entry() { PlaceholderText = "Apellidos" };
        Entry correoEntry = new Entry() { PlaceholderText = "Correo" };
        Entry contraseniaEntry = new Entry() { PlaceholderText = "Contraseña", Visibility = false };
        
        Button guardarButton = new Button("Guardar Usuario");
        guardarButton.Clicked += (sender, e) => OnGuardarUsuario(idEntry.Text, nombresEntry.Text, apellidosEntry.Text, correoEntry.Text, contraseniaEntry.Text);
        
        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();
        
        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(idEntry, false, false, 5);
        vbox.PackStart(nombresEntry, false, false, 5);
        vbox.PackStart(apellidosEntry, false, false, 5);
        vbox.PackStart(correoEntry, false, false, 5);
        vbox.PackStart(contraseniaEntry, false, false, 5);
        vbox.PackStart(guardarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);
        
        Add(vbox);
        ShowAll();
    }
    
    private void OnGuardarUsuario(string id, string nombres, string apellidos, string correo, string contrasenia)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(apellidos) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasenia))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Todos los campos son obligatorios.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }
        
        if (!int.TryParse(id, out int userId))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID debe ser un número válido.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }
        
        Program.usuarios.Insertar(userId, nombres, apellidos, correo, contrasenia);
        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Usuario guardado exitosamente.");
        successDialog.Run();
        successDialog.Destroy();
        Destroy();
    }
}