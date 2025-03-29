using Gtk;
using System;
using System.Runtime.InteropServices;

public class WindowActualizacionRepuesto : Window
{
    private Entry entryId;
    private Entry entryNombreActual;
    private Entry entryNuevoNombre;
    private Entry entryDetallesActuales;
    private Entry entryNuevosDetalles;
    private Entry entryPrecioActual;
    private Entry entryNuevoPrecio;
    private Button buscarButton;
    private Button actualizarButton;
    private Button regresarButton;

    public WindowActualizacionRepuesto() : base("Actualización de Repuestos")
    {
        // Configuración básica de la ventana
        SetDefaultSize(800, 500);
        SetPosition(WindowPosition.Center);
        BorderWidth = 15;

        // Crear contenedor principal
        var grid = new Grid();
        grid.ColumnSpacing = 10;
        grid.RowSpacing = 10;

        // Título
        var titleLabel = new Label("Actualización de Repuestos") {
            Hexpand = true,
            Halign = Align.Center,
            MarginBottom = 20
        };
        grid.Attach(titleLabel, 0, 0, 4, 1);

        // Encabezados
        grid.Attach(new Label("Campo") { Halign = Align.Center }, 0, 1, 1, 1);
        grid.Attach(new Label("Valor Actual") { Halign = Align.Center }, 1, 1, 1, 1);
        grid.Attach(new Label("Nuevo Valor") { Halign = Align.Center }, 2, 1, 1, 1);

        // Fila ID
        var lblId = new Label("ID:") {
            Halign = Align.End
        };
        entryId = new Entry();
        grid.Attach(lblId, 0, 2, 1, 1);
        grid.Attach(entryId, 1, 2, 3, 1);

        // Fila Nombre
        var lblNombre = new Label("Nombre:") {
            Halign = Align.End
        };
        entryNombreActual = new Entry() { Sensitive = false };
        entryNuevoNombre = new Entry();
        grid.Attach(lblNombre, 0, 3, 1, 1);
        grid.Attach(entryNombreActual, 1, 3, 1, 1);
        grid.Attach(entryNuevoNombre, 2, 3, 1, 1);

        // Fila Detalles
        var lblDetalles = new Label("Detalles:") {
            Halign = Align.End
        };
        entryDetallesActuales = new Entry() { Sensitive = false };
        entryNuevosDetalles = new Entry();
        grid.Attach(lblDetalles, 0, 4, 1, 1);
        grid.Attach(entryDetallesActuales, 1, 4, 1, 1);
        grid.Attach(entryNuevosDetalles, 2, 4, 1, 1);

        // Fila Precio
        var lblPrecio = new Label("Precio:") {
            Halign = Align.End
        };
        entryPrecioActual = new Entry() { Sensitive = false };
        entryNuevoPrecio = new Entry();
        grid.Attach(lblPrecio, 0, 5, 1, 1);
        grid.Attach(entryPrecioActual, 1, 5, 1, 1);
        grid.Attach(entryNuevoPrecio, 2, 5, 1, 1);

        // Botón Buscar
        buscarButton = new Button("Buscar") {
            MarginTop = 20
        };
        buscarButton.Clicked += OnBuscarClicked;
        grid.Attach(buscarButton, 0, 6, 1, 1);

        // Botón Actualizar
        actualizarButton = new Button("Actualizar") {
            MarginTop = 20,
            Sensitive = false
        };
        actualizarButton.Clicked += OnActualizarClicked;
        grid.Attach(actualizarButton, 1, 6, 1, 1);

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
        if (int.TryParse(entryId.Text, out int idRepuesto))
        {
            // Buscar repuesto en el árbol
            var repuesto = Program.arbolRepuestos.Buscar(idRepuesto);
            
            if (repuesto != null)
            {
                // Mostrar datos actuales
                entryNombreActual.Text = Program.arbolRepuestos.PtrToString(repuesto->Nombre);
                entryDetallesActuales.Text = Program.arbolRepuestos.PtrToString(repuesto->Detalles);
                entryPrecioActual.Text = repuesto->Precio.ToString();

                // Limpiar campos de nuevos valores
                entryNuevoNombre.Text = "";
                entryNuevosDetalles.Text = "";
                entryNuevoPrecio.Text = "";

                // Habilitar botón de actualizar
                actualizarButton.Sensitive = true;
            }
            else
            {
                using var md = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Error,
                    ButtonsType.Ok,
                    "Repuesto no encontrado");
                md.Run();
                actualizarButton.Sensitive = false;
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
            actualizarButton.Sensitive = false;
        }
    }

    private unsafe void OnActualizarClicked(object? sender, EventArgs e)
    {
        if (int.TryParse(entryId.Text, out int idRepuesto))
        {
            // Buscar repuesto nuevamente para asegurarnos que existe
            var repuesto = Program.arbolRepuestos.Buscar(idRepuesto);
            
            if (repuesto != null)
            {
                // Obtener nuevos valores (usar actuales si no se especificó nuevo valor)
                string nuevoNombre = !string.IsNullOrEmpty(entryNuevoNombre.Text) ? 
                    entryNuevoNombre.Text : entryNombreActual.Text;
                
                string nuevosDetalles = !string.IsNullOrEmpty(entryNuevosDetalles.Text) ? 
                    entryNuevosDetalles.Text : entryDetallesActuales.Text;
                
                int nuevoPrecio;
                if (!int.TryParse(entryNuevoPrecio.Text, out nuevoPrecio))
                {
                    // Si no es un número válido, mantener el precio actual
                    nuevoPrecio = repuesto->Precio;
                }

                // Actualizar los campos del repuesto (sin usar fixed adicional)
                for (int i = 0; i < 50; i++)
                {
                    repuesto->Nombre[i] = i < nuevoNombre.Length ? nuevoNombre[i] : '\0';
                }

                for (int i = 0; i < 100; i++)
                {
                    repuesto->Detalles[i] = i < nuevosDetalles.Length ? nuevosDetalles[i] : '\0';
                }

                repuesto->Precio = nuevoPrecio;

                // Mostrar mensaje de éxito
                using var md = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Info,
                    ButtonsType.Ok,
                    "Repuesto actualizado correctamente");
                md.Run();

                // Actualizar la vista con los nuevos valores actuales
                entryNombreActual.Text = nuevoNombre;
                entryDetallesActuales.Text = nuevosDetalles;
                entryPrecioActual.Text = nuevoPrecio.ToString();
                
                // Limpiar campos de nuevos valores
                entryNuevoNombre.Text = "";
                entryNuevosDetalles.Text = "";
                entryNuevoPrecio.Text = "";
            }
        }
    }

    private void OnRegresarClicked(object? sender, EventArgs e)
    {
        // Regresar a la ventana de administrador
        WindowAdmin adminWindow = new WindowAdmin();
        adminWindow.Show();
        this.Destroy();
    }
}