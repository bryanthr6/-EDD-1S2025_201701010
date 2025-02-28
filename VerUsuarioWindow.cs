using System;
using Gtk;

public class VerUsuarioWindow : Window
{
    public VerUsuarioWindow() : base("Ver Usuario")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Ingrese el ID del usuario:");

        Entry idEntry = new Entry() { PlaceholderText = "ID Usuario" };

        Button buscarButton = new Button("Buscar");
        buscarButton.Clicked += (sender, e) => OnBuscarUsuario(idEntry.Text);

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(idEntry, false, false, 5);
        vbox.PackStart(buscarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private unsafe void OnBuscarUsuario(string id)
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

        // Obtener información del usuario
        string infoUsuario = $"ID: {userId}\n" +
                             $"Nombre: {GetString(usuario->Nombres, 50)} {GetString(usuario->Apellidos, 50)}\n" +
                             $"Correo: {GetString(usuario->Correo, 50)}\n\n";

        // Buscar los vehículos del usuario
        string infoVehiculos = "Vehículos:\n";
        bool tieneVehiculos = false;

        NodoVehiculo* vehiculoActual = Program.vehiculos.cabeza; // Usar la cabeza de la lista
        while (vehiculoActual != null)
        {
            if (vehiculoActual->ID_Usuario == userId)
            {
                tieneVehiculos = true;
                infoVehiculos += $"- {GetString(vehiculoActual->Marca, 50)} {GetString(vehiculoActual->Modelo, 50)} (Placa: {GetString(vehiculoActual->Placa, 20)})\n";
            }
            vehiculoActual = vehiculoActual->siguiente; // Avanzar en la lista
        }

        if (!tieneVehiculos)
        {
            infoVehiculos += "Este usuario no tiene vehículos registrados.";
        }

        // Mostrar el mensaje en un cuadro de diálogo
        MessageDialog infoDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, infoUsuario + infoVehiculos);
        infoDialog.Run();
        infoDialog.Destroy();
    }

    private static unsafe string GetString(char* charArray, int length)
    {
        return new string(charArray, 0, length).Split('\0')[0]; // Elimina caracteres nulos
    }
}
