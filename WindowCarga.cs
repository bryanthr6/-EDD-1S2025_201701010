using Gtk;
using System;
using System.IO;
using System.Text.Json;

public class WindowCarga : Window
{
    private ComboBox comboBoxTipo;
    private Button cargarButton;
    private Button volverButton;
    private Label statusLabel;

    public WindowCarga() : base("Carga Masiva de Datos")
    {
        // Configuración básica de la ventana
        SetDefaultSize(500, 350);
        SetPosition(WindowPosition.Center);
        BorderWidth = 15;

        // Crear contenedor principal
        Box vbox = new Box(Orientation.Vertical, 10);

        // Etiqueta de instrucción
        vbox.PackStart(new Label("Seleccione el tipo de datos a cargar:") { 
            MarginBottom = 10 
        }, false, false, 0);

        // Crear ComboBox con las opciones
        comboBoxTipo = new ComboBox(new[] { "Usuarios", "Vehículos", "Repuestos" });
        comboBoxTipo.Active = 0; // Seleccionar primera opción por defecto

        // Etiqueta de estado
        statusLabel = new Label("Seleccione un archivo JSON para cargar") {
            MarginTop = 10
        };

        // Botón de Cargar
        cargarButton = new Button("Seleccionar Archivo JSON") {
            MarginTop = 20
        };
        cargarButton.Clicked += OnCargarClicked;

        // Botón de Volver
        volverButton = new Button("Volver al Menú") {
            MarginTop = 10
        };
        volverButton.Clicked += OnVolverClicked;

        // Añadir elementos al contenedor
        vbox.PackStart(comboBoxTipo, false, false, 0);
        vbox.PackStart(statusLabel, false, false, 0);
        vbox.PackStart(cargarButton, false, false, 0);
        vbox.PackStart(volverButton, false, false, 0);

        // Añadir contenedor a la ventana
        Add(vbox);

        // Mostrar todos los widgets
        ShowAll();
    }

    private void OnCargarClicked(object? sender, EventArgs e)
    {
        // Crear diálogo para seleccionar archivo
        using var fileChooser = new FileChooserDialog(
            "Seleccione el archivo JSON",
            this,
            FileChooserAction.Open,
            "Cancelar", ResponseType.Cancel,
            "Abrir", ResponseType.Accept);
        
        // Filtro para archivos JSON
        FileFilter filter = new FileFilter();
        filter.Name = "Archivos JSON";
        filter.AddPattern("*.json");
        fileChooser.AddFilter(filter);

        if (fileChooser.Run() == (int)ResponseType.Accept)
        {
            string rutaArchivo = fileChooser.Filename;
            string tipoSeleccionado = comboBoxTipo.ActiveText;
            
            try
            {
                string json = File.ReadAllText(rutaArchivo);
                var datos = JsonSerializer.Deserialize<DatosJson>(json) ?? new DatosJson();
                
                int cantidad = 0;
                string mensaje = "";
                
                switch (tipoSeleccionado)
                {
                    case "Usuarios":
                        cantidad = datos.Usuarios.Count;
                        foreach (var usuario in datos.Usuarios) 
                        {
                            Program.listaUsuarios.AgregarUsuario(
                                usuario.ID, usuario.Nombres, usuario.Apellidos, 
                                usuario.Correo, usuario.Edad, usuario.Contrasenia);
                        }
                        mensaje = $"Se cargaron {cantidad} usuarios correctamente";
                        break;
                        
                    case "Vehículos":
                        cantidad = datos.Vehiculos.Count;
                        foreach (var vehiculo in datos.Vehiculos) 
                        {
                            Program.listaVehiculos.AgregarVehiculo(
                                vehiculo.ID, vehiculo.ID_Usuario, 
                                vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
                        }
                        mensaje = $"Se cargaron {cantidad} vehículos correctamente";
                        break;
                        
                    case "Repuestos":
                        cantidad = datos.Repuestos.Count;
                        foreach (var repuesto in datos.Repuestos) 
                        {
                            Program.arbolRepuestos.Insertar(
                                repuesto.ID, repuesto.Repuesto, 
                                repuesto.Detalles, (int)repuesto.Costo);
                        }
                        mensaje = $"Se cargaron {cantidad} repuestos correctamente";
                        break;
                }
                
                // Mostrar mensaje de éxito
                statusLabel.Text = mensaje;
                Console.WriteLine(mensaje); // También en consola
                
                using var md = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Info,
                    ButtonsType.Ok,
                    mensaje);
                md.Run();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Error al cargar el archivo: {ex.Message}";
                statusLabel.Text = errorMsg;
                Console.WriteLine(errorMsg);
                
                using var errorDialog = new MessageDialog(
                    this,
                    DialogFlags.DestroyWithParent,
                    MessageType.Error,
                    ButtonsType.Ok,
                    errorMsg);
                errorDialog.Run();
            }
        }
        
        fileChooser.Destroy();
    }

    private void OnVolverClicked(object? sender, EventArgs e)
    {
        // Regresar a la ventana de administrador
        WindowAdmin adminWindow = new WindowAdmin();
        adminWindow.Show();
        this.Destroy();
    }
}

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