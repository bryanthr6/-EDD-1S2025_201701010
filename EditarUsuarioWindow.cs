using System;
using Gtk;

public class EditarUsuarioWindow : Window
{
    public EditarUsuarioWindow() : base("Editar Usuario")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Ingrese el ID del usuario a editar:");

        Entry idEntry = new Entry() { PlaceholderText = "ID Usuario" };
        Entry nombreEntry = new Entry() { PlaceholderText = "Nuevo Nombre" };
        Entry apellidosEntry = new Entry() { PlaceholderText = "Nuevos Apellidos" };
        Entry correoEntry = new Entry() { PlaceholderText = "Nuevo Correo" };

        Button editarButton = new Button("Guardar Cambios");
        editarButton.Clicked += (sender, e) => OnEditarUsuario(idEntry.Text, nombreEntry.Text, apellidosEntry.Text, correoEntry.Text);

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(idEntry, false, false, 5);
        vbox.PackStart(nombreEntry, false, false, 5);
        vbox.PackStart(apellidosEntry, false, false, 5);
        vbox.PackStart(correoEntry, false, false, 5);
        vbox.PackStart(editarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private unsafe void OnEditarUsuario(string id, string nuevoNombre, string nuevosApellidos, string nuevoCorreo)
    {
        if (!int.TryParse(id, out int userId))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID debe ser un número válido.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        NodoUsuario* usuario = Program.usuarios.BuscarPorID(userId);
        if (usuario == null)
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Usuario no encontrado.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        // Actualizar datos si el usuario ingresó un valor
        if (!string.IsNullOrEmpty(nuevoNombre)) CopyString(usuario->Nombres, nuevoNombre);
        if (!string.IsNullOrEmpty(nuevosApellidos)) CopyString(usuario->Apellidos, nuevosApellidos);
        if (!string.IsNullOrEmpty(nuevoCorreo)) CopyString(usuario->Correo, nuevoCorreo);

        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Usuario actualizado correctamente.");
        successDialog.Run();
        successDialog.Destroy();
        Destroy();
    }

    private static unsafe void CopyString(char* destination, string source)
    {
        int i;
        for (i = 0; i < source.Length && i < 49; i++)
        {
            destination[i] = source[i];
        }
        destination[i] = '\0';
    }
}
