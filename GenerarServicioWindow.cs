using System;
using Gtk;

public class GenerarServicioWindow : Window
{
    public GenerarServicioWindow() : base("Generar Servicio")
    {
        SetDefaultSize(400, 350);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Ingrese los datos del servicio:");

        Entry idServicioEntry = new Entry() { PlaceholderText = "ID Servicio" };
        Entry idVehiculoEntry = new Entry() { PlaceholderText = "ID Vehículo" };
        Entry idRepuestoEntry = new Entry() { PlaceholderText = "ID Repuesto" };
        Entry detallesEntry = new Entry() { PlaceholderText = "Detalles del Servicio" };
        Entry costoEntry = new Entry() { PlaceholderText = "Costo del Servicio" };

        Button generarButton = new Button("Generar Servicio");
        generarButton.Clicked += (sender, e) => OnGenerarServicio(
            idServicioEntry.Text, idVehiculoEntry.Text, idRepuestoEntry.Text, detallesEntry.Text, costoEntry.Text);

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(idServicioEntry, false, false, 5);
        vbox.PackStart(idVehiculoEntry, false, false, 5);
        vbox.PackStart(idRepuestoEntry, false, false, 5);
        vbox.PackStart(detallesEntry, false, false, 5);
        vbox.PackStart(costoEntry, false, false, 5);
        vbox.PackStart(generarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private unsafe void OnGenerarServicio(string idServicio, string idVehiculo, string idRepuesto, string detalles, string costoServicio)
    {
        if (!int.TryParse(idServicio, out int servicioId) ||
            !int.TryParse(idVehiculo, out int vehiculoId) ||
            !int.TryParse(idRepuesto, out int repuestoId) ||
            !float.TryParse(costoServicio, out float costo))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
                "Los campos ID Servicio, ID Vehículo e ID Repuesto deben ser números enteros. El costo debe ser un número decimal.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        if (string.IsNullOrWhiteSpace(detalles))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
                "El campo 'Detalles' no puede estar vacío.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        // Validar existencia del vehículo y repuesto
        if (Program.vehiculos.BuscarPorID(vehiculoId) == null)
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, 
                "Error: Vehículo no encontrado.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        NodoRepuesto* repuesto = Program.repuestos.BuscarPorID(repuestoId);
        if (repuesto == null)
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, 
                "Error: Repuesto no encontrado.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        // Insertar servicio en la lista de servicios
        Program.servicios.Insertar(servicioId, repuestoId, vehiculoId, detalles, costo, Program.repuestos, Program.vehiculos);

        // Generar factura con el costo del servicio + costo del repuesto
        float total = costo + repuesto->Precio;
        Program.facturas.GenerarFactura(servicioId, servicioId, total);

        // Aquí se debería insertar el detalle en la matriz dispersa cuando se implemente

        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok,
            $"Servicio registrado exitosamente.\nFactura Generada: ID {servicioId}, Total: {total}");
        successDialog.Run();
        successDialog.Destroy();
        Destroy();
    }
}
