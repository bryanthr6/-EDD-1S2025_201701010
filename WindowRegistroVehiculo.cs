using Gtk;
using System;

public class WindowRegistroVehiculo : Window
{
    private Entry entryId;
    private Entry entryMarca;
    private Entry entryModelo;
    private Entry entryPlaca;
    private Button registrarButton;
    private Button regresarButton;
    private int idUsuario;

    public WindowRegistroVehiculo(int idUsuario) : base("Registro de Vehículo")
    {
        this.idUsuario = idUsuario;
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;

        var vbox = new Box(Orientation.Vertical, 10);

        // ID Vehículo
        var hboxId = new Box(Orientation.Horizontal, 5);
        hboxId.PackStart(new Label("ID Vehículo:"), false, false, 0);
        entryId = new Entry();
        hboxId.PackStart(entryId, true, true, 0);
        vbox.PackStart(hboxId, false, false, 0);

        // Marca
        var hboxMarca = new Box(Orientation.Horizontal, 5);
        hboxMarca.PackStart(new Label("Marca:"), false, false, 0);
        entryMarca = new Entry();
        hboxMarca.PackStart(entryMarca, true, true, 0);
        vbox.PackStart(hboxMarca, false, false, 0);

        // Modelo
        var hboxModelo = new Box(Orientation.Horizontal, 5);
        hboxModelo.PackStart(new Label("Modelo:"), false, false, 0);
        entryModelo = new Entry();
        hboxModelo.PackStart(entryModelo, true, true, 0);
        vbox.PackStart(hboxModelo, false, false, 0);

        // Placa
        var hboxPlaca = new Box(Orientation.Horizontal, 5);
        hboxPlaca.PackStart(new Label("Placa:"), false, false, 0);
        entryPlaca = new Entry();
        hboxPlaca.PackStart(entryPlaca, true, true, 0);
        vbox.PackStart(hboxPlaca, false, false, 0);

        // Botones
        var hboxBotones = new Box(Orientation.Horizontal, 5) { Homogeneous = true };
        registrarButton = new Button("Registrar");
        regresarButton = new Button("Regresar");
        
        hboxBotones.PackStart(registrarButton, true, true, 0);
        hboxBotones.PackStart(regresarButton, true, true, 0);
        vbox.PackStart(hboxBotones, false, false, 0);

        Add(vbox);

        // Conectar eventos
        registrarButton.Clicked += OnRegistrarClicked;
        regresarButton.Clicked += OnRegresarClicked;

        ShowAll();
    }

    private void OnRegistrarClicked(object? sender, EventArgs e)
    {
        // Validar campos
        if (!int.TryParse(entryId.Text, out int idVehiculo))
        {
            MostrarError("ID de vehículo inválido");
            return;
        }

        if (string.IsNullOrWhiteSpace(entryMarca.Text))
        {
            MostrarError("La marca no puede estar vacía");
            return;
        }

        if (!int.TryParse(entryModelo.Text, out int modelo))
        {
            MostrarError("Modelo debe ser un número");
            return;
        }

        if (string.IsNullOrWhiteSpace(entryPlaca.Text))
        {
            MostrarError("La placa no puede estar vacía");
            return;
        }

        // Verificar si el vehículo ya existe
        unsafe
        {
            if (Program.listaVehiculos.BuscarPorId(idVehiculo) != null)
            {
                MostrarError("Ya existe un vehículo con este ID");
                return;
            }

            // Registrar el vehículo
            Program.listaVehiculos.AgregarVehiculo(
                idVehiculo,
                idUsuario,
                entryMarca.Text,
                modelo,
                entryPlaca.Text
            );
        }

        // Mostrar confirmación
        using (var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Info,
            ButtonsType.Ok,
            "Vehículo registrado exitosamente"))
        {
            dialog.Run();
        }

        // Limpiar campos
        entryId.Text = "";
        entryMarca.Text = "";
        entryModelo.Text = "";
        entryPlaca.Text = "";
    }

    private unsafe void OnRegresarClicked(object? sender, EventArgs e)
    {
        // Buscar el usuario actual
        var usuario = Program.listaUsuarios.BuscarPorId(idUsuario);
        if (usuario != null)
        {
            var userWindow = new WindowUser(usuario);
            userWindow.Show();
            this.Destroy();
        }
    }

    private void MostrarError(string mensaje)
    {
        using (var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Error,
            ButtonsType.Ok,
            mensaje))
        {
            dialog.Title = "Error de registro";
            dialog.Run();
        }
    }
}