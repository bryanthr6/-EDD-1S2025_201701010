using System;
using Gtk;

public class IngresoManualVehiculoWindow : Window
{
    public IngresoManualVehiculoWindow() : base("Ingreso Manual de Vehículo")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Ingrese los datos del vehículo:");

        Entry idVehiculoEntry = new Entry() { PlaceholderText = "ID Vehículo" };
        Entry idUsuarioEntry = new Entry() { PlaceholderText = "ID Usuario" };
        Entry marcaEntry = new Entry() { PlaceholderText = "Marca" };
        Entry modeloEntry = new Entry() { PlaceholderText = "Modelo" };
        Entry placaEntry = new Entry() { PlaceholderText = "Placa" };

        Button guardarButton = new Button("Guardar Vehículo");
        guardarButton.Clicked += (sender, e) => 
            OnGuardarVehiculo(idVehiculoEntry.Text, idUsuarioEntry.Text, marcaEntry.Text, modeloEntry.Text, placaEntry.Text);

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(idVehiculoEntry, false, false, 5);
        vbox.PackStart(idUsuarioEntry, false, false, 5);
        vbox.PackStart(marcaEntry, false, false, 5);
        vbox.PackStart(modeloEntry, false, false, 5);
        vbox.PackStart(placaEntry, false, false, 5);
        vbox.PackStart(guardarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private void OnGuardarVehiculo(string idVehiculo, string idUsuario, string marca, string modelo, string placa)
    {
        if (string.IsNullOrWhiteSpace(idVehiculo) || string.IsNullOrWhiteSpace(idUsuario) ||
            string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo) || string.IsNullOrWhiteSpace(placa))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Todos los campos son obligatorios.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        if (!int.TryParse(idVehiculo, out int vehiculoId) || !int.TryParse(idUsuario, out int usuarioId))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID Vehículo e ID Usuario deben ser números válidos.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        // Insertar el vehículo en la lista de vehículos
        Program.vehiculos.Insertar(vehiculoId, usuarioId, marca, modelo, placa, Program.usuarios);

        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Vehículo guardado exitosamente.");
        successDialog.Run();
        successDialog.Destroy();
        Destroy();
    }
}
