using Gtk;
using System;
using System.Runtime.InteropServices;

public class WindowEditarVehiculo : Window
{
    private Entry entryId;
    private Entry entryIdUsuario;
    private Entry entryMarca;
    private Entry entryModelo;
    private Entry entryPlaca;
    private Button buscarButton;
    private Button borrarButton;
    private Button regresarButton;

    public WindowEditarVehiculo() : base("Editar Vehículo")
    {
        // Configuración básica de la ventana
        SetDefaultSize(600, 400);
        SetPosition(WindowPosition.Center);
        BorderWidth = 15;

        // Crear contenedor principal
        var grid = new Grid();
        grid.ColumnSpacing = 10;
        grid.RowSpacing = 10;

        // Título
        var titleLabel = new Label("Editar Vehículo") {
            Hexpand = true,
            Halign = Align.Center,
            MarginBottom = 20
        };
        grid.Attach(titleLabel, 0, 0, 3, 1);

        // Fila ID Vehículo
        var lblId = new Label("ID del vehículo:") {
            Halign = Align.End
        };
        entryId = new Entry();
        grid.Attach(lblId, 0, 1, 1, 1);
        grid.Attach(entryId, 1, 1, 3, 1);

        // Fila ID Usuario
        var lblIdUsuario = new Label("ID del dueño:") {
            Halign = Align.End
        };
        entryIdUsuario = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblIdUsuario, 0, 2, 1, 1);
        grid.Attach(entryIdUsuario, 1, 2, 3, 1);

        // Fila Marca
        var lblMarca = new Label("Marca:") {
            Halign = Align.End
        };
        entryMarca = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblMarca, 0, 3, 1, 1);
        grid.Attach(entryMarca, 1, 3, 3, 1);

        // Fila Modelo
        var lblModelo = new Label("Modelo:") {
            Halign = Align.End
        };
        entryModelo = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblModelo, 0, 4, 1, 1);
        grid.Attach(entryModelo, 1, 4, 3, 1);

        // Fila Placa
        var lblPlaca = new Label("Placa:") {
            Halign = Align.End
        };
        entryPlaca = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblPlaca, 0, 5, 1, 1);
        grid.Attach(entryPlaca, 1, 5, 3, 1);

        // Botón Buscar
        buscarButton = new Button("Buscar") {
            MarginTop = 20
        };
        buscarButton.Clicked += OnBuscarClicked;
        grid.Attach(buscarButton, 0, 6, 1, 1);

        // Botón Borrar
        borrarButton = new Button("Borrar Vehículo") {
            MarginTop = 20,
            Sensitive = false
        };
        borrarButton.Clicked += OnBorrarClicked;
        grid.Attach(borrarButton, 1, 6, 1, 1);

        // Botón Regresar
        regresarButton = new Button("Regresar") {
            MarginTop = 20
        };
        regresarButton.Clicked += OnRegresarClicked;
        grid.Attach(regresarButton, 2, 6, 1, 1);

        // Añadir contenedor a la ventana
        Add(grid);

        // Mostrar todos los widgets
        ShowAll();
    }

    private unsafe void OnBuscarClicked(object? sender, EventArgs e)
    {
        if (int.TryParse(entryId.Text, out int idVehiculo))
        {
            // Buscar vehículo en la lista
            var vehiculo = Program.listaVehiculos.BuscarPorId(idVehiculo);
            
            if (vehiculo != null)
            {
                // Habilitar campos y llenar con datos
                entryIdUsuario.Sensitive = true;
                entryMarca.Sensitive = true;
                entryModelo.Sensitive = true;
                entryPlaca.Sensitive = true;
                borrarButton.Sensitive = true;
                
                entryIdUsuario.Text = vehiculo->IdUsuario.ToString();
                entryMarca.Text = Program.listaVehiculos.PtrToString(vehiculo->Marca);
                entryModelo.Text = vehiculo->Modelo.ToString();
                entryPlaca.Text = Program.listaVehiculos.PtrToString(vehiculo->Placa);
            }
            else
            {
                using var md = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Error,
                    ButtonsType.Ok,
                    "Vehículo no encontrado");
                md.Run();
                borrarButton.Sensitive = false;
            }
        }
        else
        {
            using var md = new MessageDialog(
                this,
                DialogFlags.DestroyWithParent,
                MessageType.Error,
                ButtonsType.Ok,
                "ID inválido. Ingrese un número válido");
            md.Run();
            borrarButton.Sensitive = false;
        }
    }

    private void OnBorrarClicked(object? sender, EventArgs e)
    {
        if (int.TryParse(entryId.Text, out int idVehiculo))
        {
            // Diálogo de confirmación
            using var confirmDialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Question,
                ButtonsType.YesNo,
                "¿Está seguro que desea eliminar este vehículo?");
            
            if (confirmDialog.Run() == (int)ResponseType.Yes)
            {
                // Eliminar vehículo
                Program.listaVehiculos.EliminarPorId(idVehiculo);
                
                // Mostrar confirmación
                using var successDialog = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Info,
                    ButtonsType.Ok,
                    "Vehículo eliminado correctamente");
                successDialog.Run();
                
                // Limpiar formulario
                entryId.Text = "";
                entryIdUsuario.Text = "";
                entryMarca.Text = "";
                entryModelo.Text = "";
                entryPlaca.Text = "";
                
                // Deshabilitar campos
                entryIdUsuario.Sensitive = false;
                entryMarca.Sensitive = false;
                entryModelo.Sensitive = false;
                entryPlaca.Sensitive = false;
                borrarButton.Sensitive = false;
            }
            confirmDialog.Destroy();
        }
        else
        {
            using var md = new MessageDialog(
                this,
                DialogFlags.DestroyWithParent,
                MessageType.Error,
                ButtonsType.Ok,
                "ID inválido");
            md.Run();
        }
    }

    private void OnRegresarClicked(object? sender, EventArgs e)
    {
        // Regresar a la ventana de gestión
        WindowGestion gestionWindow = new WindowGestion();
        gestionWindow.Show();
        this.Destroy();
    }
}