using Gtk;
using System;
using System.Text;

public class WindowVerBlockchain : Window
{
    private TextView textViewBlockchain;
    
    public WindowVerBlockchain() : base("Blockchain de Usuarios")
    {
        SetDefaultSize(800, 600);
        SetPosition(WindowPosition.Center);
        BorderWidth = 10;
        
        var vbox = new Box(Orientation.Vertical, 10);
        
        var lblTitulo = new Label("<big><b>Blockchain de Usuarios</b></big>")
        {
            UseMarkup = true,
            Halign = Align.Center,
            MarginBottom = 10
        };
        
        textViewBlockchain = new TextView
        {
            Editable = false,
            WrapMode = WrapMode.Word,
            Expand = true
        };
        
        var scroll = new ScrolledWindow
        {
            ShadowType = ShadowType.EtchedIn,
            Expand = true
        };
        scroll.Add(textViewBlockchain);
        
        var btnRegresar = new Button("Regresar")
        {
            MarginTop = 10
        };
        btnRegresar.Clicked += (s, e) => 
        {
            new WindowAdmin().Show();
            this.Destroy();
        };
        
        vbox.PackStart(lblTitulo, false, false, 0);
        vbox.PackStart(scroll, true, true, 0);
        vbox.PackStart(btnRegresar, false, false, 0);
        
        Add(vbox);
        ShowAll();
        MostrarBlockchain();
    }
    
    private void MostrarBlockchain()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== BLOCKCHAIN DE USUARIOS ===");
        sb.AppendLine($"¿Es válida? {(Program.blockchainUsuarios.EsValida() ? "SÍ" : "NO")}");
        sb.AppendLine("==============================");
        
        foreach (var bloque in Program.blockchainUsuarios.Cadena)
        {
            sb.AppendLine($"\n[Bloque #{bloque.Index}]");
            sb.AppendLine($"Timestamp: {bloque.Timestamp:dd-MM-yy HH:mm:ss}");
            sb.AppendLine($"Hash: {bloque.Hash}");
            sb.AppendLine($"Hash anterior: {bloque.PreviousHash}");
            sb.AppendLine($"Nonce: {bloque.Nonce}");
            sb.AppendLine("Datos:");
            sb.AppendLine($"  ID: {bloque.Data.ID}");
            sb.AppendLine($"  Nombre: {bloque.Data.Nombres} {bloque.Data.Apellidos}");
            sb.AppendLine($"  Correo: {bloque.Data.Correo}");
            sb.AppendLine($"  Edad: {bloque.Data.Edad}");
            sb.AppendLine("------------------------------");
        }
        
        textViewBlockchain.Buffer.Text = sb.ToString();
    }
}