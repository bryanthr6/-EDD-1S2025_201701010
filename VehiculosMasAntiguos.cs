using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;

public unsafe class VehiculosMasAntiguos : Window
{
    public VehiculosMasAntiguos() : base("Top 5 Vehículos Más Antiguos")
    {
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label titleLabel = new Label("Top 5 Vehículos Más Antiguos");

        TreeView treeView = new TreeView();
        CargarTopVehiculosAntiguos(treeView);

        Button cerrarButton = new Button("Cerrar");
        cerrarButton.Clicked += (sender, e) => Destroy();

        vbox.PackStart(titleLabel, false, false, 10);
        vbox.PackStart(treeView, true, true, 5);
        vbox.PackStart(cerrarButton, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    private void CargarTopVehiculosAntiguos(TreeView treeView)
    {
        treeView.AppendColumn("ID", new CellRendererText(), "text", 0);
        treeView.AppendColumn("Marca", new CellRendererText(), "text", 1);
        treeView.AppendColumn("Modelo", new CellRendererText(), "text", 2);

        ListStore store = new ListStore(typeof(string), typeof(string), typeof(string));

        int count = 0;
        NodoVehiculo* actual = Program.vehiculos.cabeza;
        while (actual != null)
        {
            count++;
            actual = actual->siguiente;
        }

        if (count == 0) 
        { 
            treeView.Model = store;
            return; // Si no hay vehículos, salir
        }

        IntPtr[] vehiculosArray = new IntPtr[count];
        actual = Program.vehiculos.cabeza;
        for (int i = 0; i < count; i++)
        {
            vehiculosArray[i] = (IntPtr)actual;
            actual = actual->siguiente;
        }

        Array.Sort(vehiculosArray, (a, b) => ((NodoVehiculo*)a)->ID_Vehiculo.CompareTo(((NodoVehiculo*)b)->ID_Vehiculo));

        for (int i = 0; i < Math.Min(5, count); i++)
        {
            NodoVehiculo* v = (NodoVehiculo*)vehiculosArray[i];
            store.AppendValues(v->ID_Vehiculo.ToString(), GetString(v->Marca), GetString(v->Modelo));
        }

        treeView.Model = store;
    }

    private unsafe string GetString(char* charArray)
    {
        return new string(charArray).TrimEnd('\0');
    }

}