using Gtk;
using System;
using System.Text;
using System.Runtime.InteropServices;

public unsafe class WindowVerServicios : Window  // Marcamos la clase como unsafe
{
    private ComboBox comboOrden;
    private TextView textViewServicios;
    private Button regresarButton;
    private int idUsuario;

    public WindowVerServicios(int idUsuario) : base("Mis Servicios")
    {
        this.idUsuario = idUsuario;
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        BorderWidth = 15;

        // Contenedor principal
        var vbox = new Box(Orientation.Vertical, 10);

        // Etiqueta de instrucción
        var lblInstruccion = new Label("Seleccione el tipo de orden:") 
        {
            Halign = Align.Start,
            MarginBottom = 10
        };

        // ComboBox para seleccionar el orden
        comboOrden = new ComboBox(new[] { "PRE-ORDEN", "IN-ORDEN", "POST-ORDEN" });
        comboOrden.Active = 1;
        comboOrden.Changed += OnOrdenChanged;

        // TextView para mostrar los servicios
        textViewServicios = new TextView 
        {
            Editable = false,
            WrapMode = WrapMode.Word,
            MarginTop = 10,
            Expand = true
        };

        // Configurar fuente monoespaciada (forma moderna)
        var cssProvider = new CssProvider();
        cssProvider.LoadFromData("textview { font-family: Monospace; font-size: 10pt; }");
        textViewServicios.StyleContext.AddProvider(cssProvider, StyleProviderPriority.Application);

        // Crear ScrollWindow para el TextView
        var scrolledWindow = new ScrolledWindow 
        {
            ShadowType = ShadowType.EtchedIn,
            Expand = true
        };
        scrolledWindow.Add(textViewServicios);

        // Botón Regresar
        regresarButton = new Button 
        {
            Label = "Regresar",
            MarginTop = 10
        };

        // Añadir elementos al contenedor
        vbox.PackStart(lblInstruccion, false, false, 0);
        vbox.PackStart(comboOrden, false, false, 0);
        vbox.PackStart(scrolledWindow, true, true, 0);
        vbox.PackStart(regresarButton, false, false, 0);

        Add(vbox);
        regresarButton.Clicked += OnRegresarClicked;
        ShowAll();
        MostrarServicios("IN-ORDEN");
    }

    private void OnOrdenChanged(object? sender, EventArgs e)
    {
        string ordenSeleccionado = comboOrden.ActiveText;
        MostrarServicios(ordenSeleccionado);
    }

    private unsafe void MostrarServicios(string orden)
    {
        var buffer = new StringBuilder();
        buffer.AppendLine("ID\tVehículo\tRepuesto\tDetalles\tCosto");
        buffer.AppendLine("--------------------------------------------------");

        // Obtener servicios del usuario según el orden seleccionado
        switch (orden)
        {
            case "PRE-ORDEN":
                MostrarServiciosPreOrden(buffer);
                break;
            case "IN-ORDEN":
                MostrarServiciosInOrden(buffer);
                break;
            case "POST-ORDEN":
                MostrarServiciosPostOrden(buffer);
                break;
        }

        textViewServicios.Buffer.Text = buffer.ToString();
    }

    private unsafe void MostrarServiciosInOrden(StringBuilder buffer)
    {
        // Implementación segura usando el método de Program.arbolServicios
        buffer.AppendLine("=== Servicios en IN-ORDEN ===");
        // Aquí iría la lógica para obtener servicios del usuario
    }

    private unsafe void MostrarServiciosPreOrden(StringBuilder buffer)
    {
        buffer.AppendLine("=== Servicios en PRE-ORDEN ===");
        // Implementación similar para pre-orden
    }

    private unsafe void MostrarServiciosPostOrden(StringBuilder buffer)
    {
        buffer.AppendLine("=== Servicios en POST-ORDEN ===");
        // Implementación similar para post-orden
    }

    private unsafe void OnRegresarClicked(object? sender, EventArgs e)
    {
        var usuario = Program.listaUsuarios.BuscarPorId(idUsuario);
        if (usuario != null)
        {
            WindowUser userWindow = new(usuario);  // Usando new simplificado
            userWindow.Show();
            this.Destroy();
        }
    }

    public class ComboBox : ComboBoxText
    {
        public ComboBox(string[] opciones) : base()
        {
            foreach (var opcion in opciones)
                AppendText(opcion);
        }
    }
}