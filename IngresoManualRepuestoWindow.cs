using System;
using Gtk;

public class IngresoManualRepuestoWindow : Window
{
    public IngresoManualRepuestoWindow() : base("Ingreso Manual de Repuesto")
    {
        SetDefaultSize(400, 250);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Ingrese los datos del repuesto:");

        Entry idRepuestoEntry = new Entry() { PlaceholderText = "ID Repuesto" };
        Entry nombreEntry = new Entry() { PlaceholderText = "Nombre del Repuesto" };
        Entry detallesEntry = new Entry() { PlaceholderText = "Detalles" };
        Entry costoEntry = new Entry() { PlaceholderText = "Costo" };

        Button guardarButton = new Button("Guardar Repuesto");
        guardarButton.Clicked += (sender, e) => 
            OnGuardarRepuesto(idRepuestoEntry.Text, nombreEntry.Text, detallesEntry.Text, costoEntry.Text);

        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(idRepuestoEntry, false, false, 5);
        vbox.PackStart(nombreEntry, false, false, 5);
        vbox.PackStart(detallesEntry, false, false, 5);
        vbox.PackStart(costoEntry, false, false, 5);
        vbox.PackStart(guardarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);

        Add(vbox);
        ShowAll();
    }

    private void OnGuardarRepuesto(string id, string nombre, string detalles, string costo)
    {
        if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(detalles) || string.IsNullOrWhiteSpace(costo))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Todos los campos son obligatorios.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        if (!int.TryParse(id, out int repuestoId) || !float.TryParse(costo, out float repuestoCosto))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID debe ser un número entero y el Costo debe ser un número válido.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        // Insertar el repuesto en la lista de repuestos
        Program.repuestos.Insertar(repuestoId, nombre, repuestoCosto);

        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Repuesto guardado exitosamente.");
        successDialog.Run();
        successDialog.Destroy();
        Destroy();
    }
}
