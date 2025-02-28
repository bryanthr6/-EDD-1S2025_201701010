using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

public unsafe class VehiculosMasServicios : Window
{
    public VehiculosMasServicios() : base("Top 5 Vehículos con Más Servicios")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label titleLabel = new Label("Top 5 Vehículos con Más Servicios");

        TreeView treeView = new TreeView();
        CargarTopVehiculosServicios(treeView);

        Button cerrarButton = new Button("Cerrar");
        cerrarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(titleLabel, false, false, 10);
        vbox.PackStart(treeView, true, true, 5);
        vbox.PackStart(cerrarButton, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    private void CargarTopVehiculosServicios(TreeView treeView)
    {
        treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
        treeView.AppendColumn("Marca", new CellRendererText(), "text", 1);
        treeView.AppendColumn("Modelo", new CellRendererText(), "text", 2);
        treeView.AppendColumn("Servicios", new CellRendererText(), "text", 3);

        ListStore store = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
        Dictionary<int, int> contadorServicios = new Dictionary<int, int>();

        // Contar servicios por vehículo
        NodoServicio* servicio = Program.servicios.frente;
        while (servicio != null)
        {
            if (contadorServicios.ContainsKey(servicio->ID_Vehiculo))
                contadorServicios[servicio->ID_Vehiculo]++;
            else
                contadorServicios[servicio->ID_Vehiculo] = 1;

            servicio = servicio->siguiente;
        }

        var top5 = contadorServicios.OrderByDescending(v => v.Value).Take(5);

        foreach (var vehiculo in top5)
        {
            NodoVehiculo* v = Program.vehiculos.BuscarPorID(vehiculo.Key);
            if (v != null)
            {
                store.AppendValues(v->ID_Vehiculo.ToString(), GetString(v->Marca), GetString(v->Modelo), vehiculo.Value.ToString());
            }
        }

        treeView.Model = store;
    }

    private unsafe string GetString(char* charArray)
    {
        return new string(charArray).TrimEnd('\0');
    }
}
