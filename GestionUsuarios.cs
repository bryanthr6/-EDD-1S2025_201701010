using System;
using Gtk;

public class GestionUsuariosWindow : Window
{
    public GestionUsuariosWindow() : base("Gestión de Usuarios")
    {
        SetDefaultSize(400, 250);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Seleccione una opción:");

        Button verUsuarioButton = new Button("Ver Usuario");
        verUsuarioButton.Clicked += (sender, e) => new VerUsuarioWindow().Show();

        Button editarUsuarioButton = new Button("Editar Usuario");
        editarUsuarioButton.Clicked += (sender, e) => new EditarUsuarioWindow().Show();

        Button eliminarUsuarioButton = new Button("Eliminar Usuario");
        eliminarUsuarioButton.Clicked += (sender, e) => new EliminarUsuarioWindow().Show();

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(verUsuarioButton, false, false, 5);
        vbox.PackStart(editarUsuarioButton, false, false, 5);
        vbox.PackStart(eliminarUsuarioButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }
}
