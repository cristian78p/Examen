using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Examen
{
    public partial class Form1 : Form
    {
        List<Producto> productos;
        public Form1()
        {
            InitializeComponent();
            productos = new List<Producto>();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombreProducto.Text))
                {
                    MessageBox.Show("El nombre del producto no puede estar vacío.", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPrecioProducto.Text))
                {
                    MessageBox.Show("El precio del producto no puede estar vacío.", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCantidadStock.Text))
                {
                    MessageBox.Show("La cantidad en stock no puede estar vacía.", "Error");
                    return;
                }

                if (!decimal.TryParse(txtPrecioProducto.Text, out decimal precio) || precio < 0)
                {
                    MessageBox.Show("El precio debe ser un número válido y mayor o igual a 0.", "Error");
                    return;
                }

                if (!int.TryParse(txtCantidadStock.Text, out int cantidad) || cantidad < 0)
                {
                    MessageBox.Show("La cantidad debe ser un número entero válido y mayor o igual a 0.", "Error");
                    return;
                }
                Producto productoExistente = productos.Find(p => p.Nombre.Equals(txtNombreProducto.Text, StringComparison.OrdinalIgnoreCase));

                if (productoExistente != null)
                {
                    
                    productoExistente.Cantidad += cantidad;

                    if (productoExistente.Precio != precio)
                    {
                        DialogResult resultado = MessageBox.Show(
                            $"El producto '{productoExistente.Nombre}' ya existe con un precio diferente ({productoExistente.Precio:C}). ¿Desea actualizar el precio al nuevo valor ({precio:C})?",
                            "Confirmar cambio de precio",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (resultado == DialogResult.Yes)
                        {
                            productoExistente.Precio = precio; 
                        }
                    }
                }
                else
                {
                    
                    Producto nuevoProducto = new Producto(txtNombreProducto.Text, precio, cantidad);
                    productos.Add(nuevoProducto);
                }
                ActualizarInventario();
                MessageBox.Show("Producto agregado exitosamente.", "Éxito");
                limpiarAgrProducto();
            }
            catch (FormatException)
            {
                MessageBox.Show("ingrese datos validos");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void txtBuscarProducto_TextChanged(object sender, EventArgs e)
        {
            ActualizarInventario(txtBuscarProducto.Text) ;
        }

        private void listBoxInventario_SelectionChanged(object sender, EventArgs e)
        {
            if (listBoxInventario.SelectedRows.Count > 0)
            {
                Producto productoSeleccionado = (Producto)listBoxInventario.SelectedRows[0].DataBoundItem;
                txtProductoSeleccionado.Text = productoSeleccionado.Nombre;
            }
        }

        private void btnVenderProducto_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtProductoSeleccionado.Text))
                {
                    MessageBox.Show("Debe seleccionar un producto a vender.", "Error");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCantidadVenta.Text))
                {
                    MessageBox.Show("La cantidad a vender no puede estar vacía.", "Error");
                    return;
                }

                if (!int.TryParse(txtCantidadVenta.Text, out int cantidadVenta) || cantidadVenta <= 0)
                {
                    MessageBox.Show("La cantidad a vender debe ser un número entero mayor a 0.", "Error");
                    return;
                }

                Producto producto = productos.Find(p => p.Nombre.Equals(txtProductoSeleccionado.Text, StringComparison.OrdinalIgnoreCase));
                if (producto == null)
                {
                    MessageBox.Show("El producto no se encuentra en el inventario.", "Error");
                    return;
                }

                if (producto.Cantidad < cantidadVenta)
                {
                    MessageBox.Show("No hay suficiente stock para realizar la venta.", "Error");
                    return;
                }

                producto.Cantidad -= cantidadVenta;

                ActualizarInventario();
                limpiarVenderProducto();

                MessageBox.Show("Venta realizada exitosamente.", "Éxito");
            }
            catch(FormatException)
            {
                MessageBox.Show("ingrese datos validos");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        private void limpiarAgrProducto()
        {
            txtNombreProducto.Clear();
            txtCantidadStock.Clear();
            txtPrecioProducto.Clear();
        }

        private void limpiarVenderProducto()
        {
            txtCantidadVenta.Clear();
            txtBuscarProducto.Clear();
        }
        private void ActualizarInventario(string filtro ="")
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                listBoxInventario.DataSource = null;
                listBoxInventario.DataSource = productos;
            }
            else
            {
                var productosFiltrados = productos
                    .Where(p => p.Nombre.Contains(filtro))
                    .ToList();
                listBoxInventario.DataSource = null;
                listBoxInventario.DataSource = productosFiltrados;
            }
            listBoxInventario.Columns["Precio"].DefaultCellStyle.Format = "C";
        }

    }
}
