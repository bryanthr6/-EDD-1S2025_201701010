using Gtk;
using System;
using System.Text;

public class WindowVerRepuestos : Window
{
    private ComboBox comboOrden;
    private TextView textViewRepuestos;
    private Button regresarButton;

    public WindowVerRepuestos() : base("Visualización de Repuestos")
    {
        // Configuración básica de la ventana
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        BorderWidth = 15;

        // Crear contenedor principal
        var vbox = new Box(Orientation.Vertical, 10);

        // Etiqueta de instrucción
        var lblInstruccion = new Label("Seleccione el tipo de orden:") {
            Halign = Align.Start,
            MarginBottom = 10
        };

        // ComboBox para seleccionar el orden
        comboOrden = new ComboBox(new[] { "PRE-ORDEN", "IN-ORDEN", "POST-ORDEN" });
        comboOrden.Active = 1; // Seleccionar IN-ORDEN por defecto
        comboOrden.Changed += OnOrdenChanged;

        // TextView para mostrar los repuestos
        textViewRepuestos = new TextView {
            Editable = false,
            WrapMode = WrapMode.None,
            MarginTop = 10
        };

        // Configurar el font usando CSS
        var cssProvider = new CssProvider();
        cssProvider.LoadFromData("textview { font-family: Monospace; font-size: 10pt; }");
        textViewRepuestos.StyleContext.AddProvider(cssProvider, StyleProviderPriority.Application);

        // Crear ScrollWindow para el TextView
        var scrolledWindow = new ScrolledWindow {
            ShadowType = ShadowType.EtchedIn,
            Expand = true
        };
        scrolledWindow.Add(textViewRepuestos);

        // Botón Regresar
        regresarButton = new Button("Regresar") {
            MarginTop = 10
        };
        regresarButton.Clicked += OnRegresarClicked;

        // Añadir elementos al contenedor
        vbox.PackStart(lblInstruccion, false, false, 0);
        vbox.PackStart(comboOrden, false, false, 0);
        vbox.PackStart(scrolledWindow, true, true, 0);
        vbox.PackStart(regresarButton, false, false, 0);

        // Añadir contenedor a la ventana
        Add(vbox);

        // Mostrar todos los widgets
        ShowAll();

        // Mostrar datos iniciales (In-Orden por defecto)
        MostrarRepuestos("IN-ORDEN");
    }

    private void OnOrdenChanged(object? sender, EventArgs e)
    {
        string ordenSeleccionado = comboOrden.ActiveText;
        MostrarRepuestos(ordenSeleccionado);
    }

    private void MostrarRepuestos(string orden)
    {
        string resultado = "";
        
        // Mostrar según el orden seleccionado
        switch (orden)
        {
            case "PRE-ORDEN":
                resultado = Program.arbolRepuestos.ObtenerRepuestosPreOrden();
                break;
            case "IN-ORDEN":
                resultado = Program.arbolRepuestos.ObtenerRepuestosInOrden();
                break;
            case "POST-ORDEN":
                resultado = Program.arbolRepuestos.ObtenerRepuestosPostOrden();
                break;
        }

        // Mostrar en el TextView
        textViewRepuestos.Buffer.Text = resultado;
    }

    private void OnRegresarClicked(object? sender, EventArgs e)
    {
        // Regresar a la ventana de administrador
        WindowAdmin adminWindow = new WindowAdmin();
        adminWindow.Show();
        this.Destroy();
    }

    // Clase auxiliar para ComboBox
    public class ComboBox : ComboBoxText
    {
        public ComboBox(string[] opciones) : base()
        {
            foreach (var opcion in opciones)
            {
                AppendText(opcion);
            }
        }
    }
}