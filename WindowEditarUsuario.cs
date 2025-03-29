using Gtk;
using System;
using System.Runtime.InteropServices;

public class WindowEditarUsuario : Window
{
    private Entry entryId;
    private Entry entryNombre;
    private Entry entryApellido;
    private Entry entryEdad;
    private Entry entryCorreo;
    private Button buscarButton;
    private Button borrarButton;
    private Button regresarButton;

    public WindowEditarUsuario() : base("Editar Usuario")
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
        var titleLabel = new Label("Editar Usuario") {
            Hexpand = true,
            Halign = Align.Center,
            MarginBottom = 20
        };
        grid.Attach(titleLabel, 0, 0, 3, 1);

        // Fila ID
        var lblId = new Label("ID de usuario:") {
            Halign = Align.End
        };
        entryId = new Entry();
        grid.Attach(lblId, 0, 1, 1, 1);
        grid.Attach(entryId, 1, 1, 3, 1);

        // Fila Nombre
        var lblNombre = new Label("Nombre:") {
            Halign = Align.End
        };
        entryNombre = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblNombre, 0, 2, 1, 1);
        grid.Attach(entryNombre, 1, 2, 3, 1);

        // Fila Apellido
        var lblApellido = new Label("Apellido:") {
            Halign = Align.End
        };
        entryApellido = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblApellido, 0, 3, 1, 1);
        grid.Attach(entryApellido, 1, 3, 3, 1);

        // Fila Edad
        var lblEdad = new Label("Edad:") {
            Halign = Align.End
        };
        entryEdad = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblEdad, 0, 4, 1, 1);
        grid.Attach(entryEdad, 1, 4, 3, 1);

        // Fila Correo
        var lblCorreo = new Label("Correo:") {
            Halign = Align.End
        };
        entryCorreo = new Entry() {
            Sensitive = false
        };
        grid.Attach(lblCorreo, 0, 5, 1, 1);
        grid.Attach(entryCorreo, 1, 5, 3, 1);

        // Botón Buscar
        buscarButton = new Button("Buscar") {
            MarginTop = 20
        };
        buscarButton.Clicked += OnBuscarClicked;
        grid.Attach(buscarButton, 0, 6, 1, 1);

        // Botón Borrar
        borrarButton = new Button("Borrar Usuario") {
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
        if (int.TryParse(entryId.Text, out int idUsuario))
        {
            // Buscar usuario en la lista
            var usuario = Program.listaUsuarios.BuscarPorId(idUsuario);
            
            if (usuario != null)
            {
                // Habilitar campos y llenar con datos
                entryNombre.Sensitive = true;
                entryApellido.Sensitive = true;
                entryEdad.Sensitive = true;
                entryCorreo.Sensitive = true;
                borrarButton.Sensitive = true;
                
                entryNombre.Text = Program.listaUsuarios.PtrToString(usuario->Nombres);
                entryApellido.Text = Program.listaUsuarios.PtrToString(usuario->Apellidos);
                entryEdad.Text = usuario->Edad.ToString();
                entryCorreo.Text = Program.listaUsuarios.PtrToString(usuario->Correo);
            }
            else
            {
                using var md = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Error,
                    ButtonsType.Ok,
                    "Usuario no encontrado");
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
        if (int.TryParse(entryId.Text, out int idUsuario))
        {
            // Diálogo de confirmación
            using var confirmDialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Question,
                ButtonsType.YesNo,
                "¿Está seguro que desea eliminar este usuario?");
            
            if (confirmDialog.Run() == (int)ResponseType.Yes)
            {
                // Eliminar usuario
                Program.listaUsuarios.EliminarPorId(idUsuario);
                
                // Mostrar confirmación
                using var successDialog = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Info,
                    ButtonsType.Ok,
                    "Usuario eliminado correctamente");
                successDialog.Run();
                
                // Limpiar formulario
                entryId.Text = "";
                entryNombre.Text = "";
                entryApellido.Text = "";
                entryEdad.Text = "";
                entryCorreo.Text = "";
                
                // Deshabilitar campos
                entryNombre.Sensitive = false;
                entryApellido.Sensitive = false;
                entryEdad.Sensitive = false;
                entryCorreo.Sensitive = false;
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