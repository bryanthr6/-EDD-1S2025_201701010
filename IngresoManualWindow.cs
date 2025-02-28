using System;
using Gtk;

public class IngresoManualWindow : Window
{
    public IngresoManualWindow() : base("Ingreso Manual de Datos")
    {
        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Seleccione el tipo de ingreso manual:");
        
        Button usuarioButton = new Button("Ingresar Usuario");
        usuarioButton.Clicked += (sender, e) => new IngresoManualUsuarioWindow().Show();

        Button vehiculoButton = new Button("Ingresar Vehículo");
        vehiculoButton.Clicked += (sender, e) => new IngresoManualVehiculoWindow().Show();

        Button repuestoButton = new Button("Ingresar Repuesto");
        repuestoButton.Clicked += (sender, e) => new IngresoManualRepuestoWindow().Show();


        
        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();
        
        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(usuarioButton, false, false, 5);
        vbox.PackStart(vehiculoButton, false, false, 5);
        vbox.PackStart(repuestoButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);
        
        Add(vbox);
        ShowAll();
    }
}
