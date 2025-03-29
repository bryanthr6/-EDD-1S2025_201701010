using Gtk;
using System;

public class WindowGenerarServicio : Window
{
    private Entry entryIdServicio;
    private Entry entryIdRepuesto;
    private Entry entryIdVehiculo;
    private Entry entryDetalles;
    private Entry entryCosto;

    public WindowGenerarServicio() : base("Generar Nuevo Servicio")
    {
        SetDefaultSize(500, 400);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;

        // Usar Box en lugar de VBox/HBox obsoletos
        var vbox = new Box(Orientation.Vertical, 5);

        // ID Servicio
        var hboxIdServicio = new Box(Orientation.Horizontal, 5);
        hboxIdServicio.PackStart(new Label("ID Servicio:"), false, false, 0);
        entryIdServicio = new Entry();
        hboxIdServicio.PackStart(entryIdServicio, true, true, 0);
        vbox.PackStart(hboxIdServicio, false, false, 0);

        // ID Repuesto
        var hboxIdRepuesto = new Box(Orientation.Horizontal, 5);
        hboxIdRepuesto.PackStart(new Label("ID Repuesto:"), false, false, 0);
        entryIdRepuesto = new Entry();
        hboxIdRepuesto.PackStart(entryIdRepuesto, true, true, 0);
        vbox.PackStart(hboxIdRepuesto, false, false, 0);

        // ID Vehículo
        var hboxIdVehiculo = new Box(Orientation.Horizontal, 5);
        hboxIdVehiculo.PackStart(new Label("ID Vehículo:"), false, false, 0);
        entryIdVehiculo = new Entry();
        hboxIdVehiculo.PackStart(entryIdVehiculo, true, true, 0);
        vbox.PackStart(hboxIdVehiculo, false, false, 0);

        // Detalles
        var hboxDetalles = new Box(Orientation.Horizontal, 5);
        hboxDetalles.PackStart(new Label("Detalles:"), false, false, 0);
        entryDetalles = new Entry();
        hboxDetalles.PackStart(entryDetalles, true, true, 0);
        vbox.PackStart(hboxDetalles, false, false, 0);

        // Costo
        var hboxCosto = new Box(Orientation.Horizontal, 5);
        hboxCosto.PackStart(new Label("Costo:"), false, false, 0);
        entryCosto = new Entry();
        hboxCosto.PackStart(entryCosto, true, true, 0);
        vbox.PackStart(hboxCosto, false, false, 0);

        // Botones
        var hboxBotones = new Box(Orientation.Horizontal, 5) { Homogeneous = true };
        var btnGenerar = new Button("Generar");
        btnGenerar.Clicked += OnGenerarClicked;
        var btnRegresar = new Button("Regresar");
        btnRegresar.Clicked += OnRegresarClicked;
        
        hboxBotones.PackStart(btnGenerar, true, true, 0);
        hboxBotones.PackStart(btnRegresar, true, true, 0);
        vbox.PackStart(hboxBotones, false, false, 0);

        Add(vbox);
        ShowAll();
    }

    private void OnGenerarClicked(object? sender, EventArgs e)
    {
        // Validar campos
        if (!int.TryParse(entryIdServicio.Text, out int idServicio))
        {
            MostrarError("ID de servicio inválido");
            return;
        }

        if (!int.TryParse(entryIdRepuesto.Text, out int idRepuesto))
        {
            MostrarError("ID de repuesto inválido");
            return;
        }

        if (!int.TryParse(entryIdVehiculo.Text, out int idVehiculo))
        {
            MostrarError("ID de vehículo inválido");
            return;
        }

        if (string.IsNullOrWhiteSpace(entryDetalles.Text))
        {
            MostrarError("Los detalles no pueden estar vacíos");
            return;
        }

        if (!int.TryParse(entryCosto.Text, out int costo))
        {
            MostrarError("Costo inválido");
            return;
        }

        // Verificar si el servicio ya existe
        unsafe
        {
            if (Program.arbolServicios.Buscar(idServicio) != null)
            {
                MostrarError("Ya existe un servicio con este ID");
                return;
            }

            // Verificar que exista el repuesto y el vehículo
            if (Program.arbolRepuestos.Buscar(idRepuesto) == null)
            {
                MostrarError("No existe un repuesto con este ID");
                return;
            }

            if (Program.listaVehiculos.BuscarPorId(idVehiculo) == null)
            {
                MostrarError("No existe un vehículo con este ID");
                return;
            }
        }

        // Generar el servicio
        bool resultado = Program.arbolServicios.GenerarServicioConFactura(
            Program.arbolFacturas,
            idServicio,
            idRepuesto,
            idVehiculo,
            entryDetalles.Text,
            costo
        );

        if (resultado)
        {
            var dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                "Servicio generado exitosamente"
            );
            dialog.Run();
            dialog.Destroy();
            
            // Limpiar campos
            entryIdServicio.Text = "";
            entryIdRepuesto.Text = "";
            entryIdVehiculo.Text = "";
            entryDetalles.Text = "";
            entryCosto.Text = "";
        }
    }

    private void OnRegresarClicked(object? sender, EventArgs e)
    {
        var adminWindow = new WindowAdmin();
        adminWindow.Show();
        this.Destroy();
    }

    private void MostrarError(string mensaje)
    {
        var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Error,
            ButtonsType.Ok,
            mensaje
        );
        dialog.Run();
        dialog.Destroy();
    }
}