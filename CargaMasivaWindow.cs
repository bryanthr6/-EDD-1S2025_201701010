using System;
using Gtk;
using System.IO;

public class CargaMasivaWindow : Window
{
    private FileChooserButton fileChooser;
    private ComboBoxText tipoComboBox;
    private Button cargarButton;
    
    public CargaMasivaWindow() : base("Carga Masiva de Datos")
    {
        SetDefaultSize(400, 250);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Seleccione el tipo de datos y el archivo JSON:");
        
        // ComboBox para elegir el tipo de datos a cargar
        tipoComboBox = new ComboBoxText();
        tipoComboBox.AppendText("Usuarios");
        tipoComboBox.AppendText("Vehículos");
        tipoComboBox.AppendText("Repuestos");
        tipoComboBox.Active = 0; // Por defecto, seleccionamos "Usuarios"

        // FileChooser para seleccionar el archivo JSON
        fileChooser = new FileChooserButton("Seleccionar archivo", FileChooserAction.Open);
        FileFilter filter = new FileFilter();
        filter.Name = "Archivos JSON";
        filter.AddPattern("*.json");
        fileChooser.Filter = filter;

        // Botón para iniciar la carga
        cargarButton = new Button("Cargar Datos");
        cargarButton.Clicked += OnCargarClicked;
        
        // Botón para cerrar la ventana
        Button regresarButton = new Button("Regresar");
        regresarButton.Clicked += (sender, e) => Destroy();

        // Agregar los elementos a la ventana
        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(tipoComboBox, false, false, 5);
        vbox.PackStart(fileChooser, false, false, 5);
        vbox.PackStart(cargarButton, false, false, 5);
        vbox.PackStart(regresarButton, false, false, 5);
        
        Add(vbox);
        ShowAll();
    }
    
    private void OnCargarClicked(object? sender, EventArgs e)
    {
        string filePath = fileChooser.Filename;
        if (!File.Exists(filePath))
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Archivo no encontrado.");
            errorDialog.Run();
            errorDialog.Destroy();
            return;
        }

        string tipoSeleccionado = tipoComboBox.ActiveText;

        // Ejecutar la carga según el tipo seleccionado
        switch (tipoSeleccionado)
        {
            case "Usuarios":
                Program.CargarUsuariosDesdeJson(filePath);
                break;
            case "Vehículos":
                Program.CargarVehiculosDesdeJson(filePath);
                break;
            case "Repuestos":
                Program.CargarRepuestosDesdeJson(filePath);
                break;
            default:
                MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Seleccione un tipo de datos válido.");
                errorDialog.Run();
                errorDialog.Destroy();
                return;
        }

        // Mostrar mensaje de éxito
        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, $"{tipoSeleccionado} cargados exitosamente.");
        successDialog.Run();
        successDialog.Destroy();
    }
}
