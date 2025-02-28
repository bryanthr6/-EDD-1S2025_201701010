using System;
using Gtk;

public class EliminarUsuarioWindow : Window
{
    public EliminarUsuarioWindow() : base("Eliminar Usuario")
    {
        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Ingrese el ID del usuario a eliminar:");

        Entry idEntry = new Entry() { PlaceholderText = "ID Usuario" };

        Button eliminarButton = new Button("Eliminar Usuario");
        eliminarButton.Clicked += (sender, e) => OnEliminarUsuario(idEntry.Text);

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(idEntry, false, false, 5);
        vbox.PackStart(eliminarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private unsafe void OnEliminarUsuario(string id)
    {
        if (!int.TryParse(id, out int userId))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID debe ser un número válido.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        if (Program.usuarios.BuscarPorID(userId) == null)
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Usuario no encontrado.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        Program.usuarios.EliminarUsuario(userId);

        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Usuario eliminado correctamente.");
        successDialog.Run();
        successDialog.Destroy();
        Destroy();
    }
}
