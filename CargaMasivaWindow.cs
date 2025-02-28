using System;
using Gtk;
using System.IO;

public class CargaMasivaWindow : Window
{
    private FileChooserButton fileChooser;
    private Button cargarButton;
    
    public CargaMasivaWindow() : base("Carga Masiva de Datos")
    {
        SetDefaultSize(400, 200);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label label = new Label("Seleccione un archivo JSON para la carga masiva:");
        fileChooser = new FileChooserButton("Seleccionar archivo", FileChooserAction.Open);
        FileFilter filter = new FileFilter();
        filter.Name = "Archivos JSON";
        filter.AddPattern("*.json");
        fileChooser.Filter = filter;

        
        cargarButton = new Button("Cargar Datos");
        cargarButton.Clicked += OnCargarClicked;
        
        vbox.PackStart(label, false, false, 5);
        vbox.PackStart(fileChooser, false, false, 5);
        vbox.PackStart(cargarButton, false, false, 5);
        
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

        Program.CargarDesdeJson(filePath);
        MessageDialog successDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Datos cargados exitosamente.");
        successDialog.Run();
        successDialog.Destroy();
    }
}
