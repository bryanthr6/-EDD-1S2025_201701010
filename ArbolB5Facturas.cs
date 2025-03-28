using System;
using System.Collections.Generic;

public class Factura 
{
    public int Id { get; set; }
    public int IdServicio { get; set; }
    public double Total { get; set; }
    public DateTime Fecha { get; set; }

    public Factura(int id, int idServicio, double total)
    {
        Id = id;
        IdServicio = idServicio;
        Total = total;
        Fecha = DateTime.Now;
    }
}

public class NodoB 
{
    public const int Orden = 5;
    public List<Factura> Facturas { get; set; } = new List<Factura>(Orden - 1);
    public List<NodoB> Hijos { get; set; } = new List<NodoB>(Orden);
    public bool EsHoja => Hijos.Count == 0;
}

public class ArbolB5Facturas 
{
    private NodoB raiz;
    private static int ultimoIdFactura = 0;

    public ArbolB5Facturas() 
    {
        raiz = new NodoB();
    }

    public void Insertar(Factura factura) 
    {
        if (Buscar(factura.Id) != null) 
        {
            Console.WriteLine($"Error: Ya existe una factura con ID {factura.Id}");
            return;
        }

        if (raiz.Facturas.Count == NodoB.Orden - 1) 
        {
            var nuevaRaiz = new NodoB();
            nuevaRaiz.Hijos.Add(raiz);
            DividirHijo(nuevaRaiz, 0);
            raiz = nuevaRaiz;
        }
        InsertarNoLleno(raiz, factura);
    }
    public int GenerarNuevoIdFactura()
        {
            ultimoIdFactura++;
            return ultimoIdFactura;
        }

    public void MostrarFacturaPorId(int id)
    {
        Factura? factura = Buscar(id);
        if (factura == null)
        {
            Console.WriteLine("No se encontró ninguna factura con ese ID.");
            return;
        }

        Console.WriteLine("\n--- Factura Encontrada ---");
        Console.WriteLine($"ID: {factura.Id}");
        Console.WriteLine($"ID Servicio: {factura.IdServicio}");
        Console.WriteLine($"Total: Q{factura.Total:F2}");
        Console.WriteLine($"Fecha: {factura.Fecha:yyyy-MM-dd HH:mm:ss}");
    }

    private void InsertarNoLleno(NodoB nodo, Factura factura) 
    {
        int i = nodo.Facturas.Count - 1;

        if (nodo.EsHoja) 
        {
            while (i >= 0 && factura.Id < nodo.Facturas[i].Id) 
            {
                i--;
            }
            nodo.Facturas.Insert(i + 1, factura);
        } 
        else 
        {
            while (i >= 0 && factura.Id < nodo.Facturas[i].Id) 
            {
                i--;
            }
            i++;

            if (nodo.Hijos[i].Facturas.Count == NodoB.Orden - 1) 
            {
                DividirHijo(nodo, i);
                if (factura.Id > nodo.Facturas[i].Id) 
                {
                    i++;
                }
            }
            InsertarNoLleno(nodo.Hijos[i], factura);
        }
    }

    private void DividirHijo(NodoB padre, int indiceHijo) 
    {
        var hijoADividir = padre.Hijos[indiceHijo];
        var nuevoHijo = new NodoB();

        int inicio = NodoB.Orden / 2;
        for (int i = inicio; i < NodoB.Orden - 1; i++) 
        {
            nuevoHijo.Facturas.Add(hijoADividir.Facturas[i]);
        }
        hijoADividir.Facturas.RemoveRange(inicio, NodoB.Orden - 1 - inicio);

        if (!hijoADividir.EsHoja) 
        {
            for (int i = inicio; i < NodoB.Orden; i++) 
            {
                nuevoHijo.Hijos.Add(hijoADividir.Hijos[i]);
            }
            hijoADividir.Hijos.RemoveRange(inicio, NodoB.Orden - inicio);
        }

        padre.Facturas.Insert(indiceHijo, hijoADividir.Facturas[inicio - 1]);
        hijoADividir.Facturas.RemoveAt(inicio - 1);

        padre.Hijos.Insert(indiceHijo + 1, nuevoHijo);
    }

    public Factura? Buscar(int id)
    {
        return BuscarRec(raiz, id);
    }

    private Factura? BuscarRec(NodoB nodo, int id) 
    {
        int i = 0;
        while (i < nodo.Facturas.Count && id > nodo.Facturas[i].Id) 
        {
            i++;
        }

        if (i < nodo.Facturas.Count && id == nodo.Facturas[i].Id) 
        {
            return nodo.Facturas[i];
        }

        return nodo.EsHoja ? null : BuscarRec(nodo.Hijos[i], id);
    }

    public void MostrarFacturas() 
    {
        Console.WriteLine("\n=== FACTURAS REGISTRADAS ===");
        MostrarRec(raiz);
        Console.WriteLine("============================");
    }

    private void MostrarRec(NodoB nodo) 
    {
        int i;
        for (i = 0; i < nodo.Facturas.Count; i++) 
        {
            if (!nodo.EsHoja) 
            {
                MostrarRec(nodo.Hijos[i]);
            }
            var f = nodo.Facturas[i];
            Console.WriteLine($"ID: {f.Id}");
            Console.WriteLine($"  Servicio: {f.IdServicio}");
            Console.WriteLine($"  Total: Q{f.Total:F2}");
            Console.WriteLine($"  Fecha: {f.Fecha:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine("----------------------------");
        }

        if (!nodo.EsHoja) 
        {
            MostrarRec(nodo.Hijos[i]);
        }
    }
}