using System;
using Gtk;

public unsafe class CancelarFacturaWindow : Window
{
    private Label idLabel;
    private Label ordenLabel;
    private Label totalLabel;
    private Button cancelarButton;

    public CancelarFacturaWindow() : base("Cancelar Factura")
    {
        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);

        idLabel = new Label("");
        ordenLabel = new Label("");
        totalLabel = new Label("");

        cancelarButton = new Button("Cancelar Factura");
        cancelarButton.Clicked += (sender, e) => CancelarFactura();

        vbox.PackStart(new Label("Detalles de la Factura:"), false, false, 5);
        vbox.PackStart(idLabel, false, false, 5);
        vbox.PackStart(ordenLabel, false, false, 5);
        vbox.PackStart(totalLabel, false, false, 5);
        vbox.PackStart(cancelarButton, false, false, 10);

        Add(vbox);
        MostrarUltimaFactura(); // Cargar la última factura al abrir la ventana
        ShowAll();
    }

    private void MostrarUltimaFactura()
    {
        NodoFactura* factura = Program.facturas.tope;
        if (factura == null)
        {
            idLabel.Text = "No hay facturas pendientes.";
            ordenLabel.Text = "";
            totalLabel.Text = "";
            cancelarButton.Sensitive = false; // Deshabilitar botón si no hay facturas
            return;
        }

        idLabel.Text = $"📜 ID Factura: {factura->ID}";
        ordenLabel.Text = $"🔄 ID Orden: {factura->ID_Orden}";
        totalLabel.Text = $"💰 Total: Q{factura->Total:0.00}";
    }

    private void CancelarFactura()
    {
        if (Program.facturas.tope == null)
        {
            Console.WriteLine("No hay facturas pendientes.");
            return;
        }

        Program.facturas.CancelarFactura();
        MostrarUltimaFactura(); // Actualiza la vista con la nueva última factura
    }
}
