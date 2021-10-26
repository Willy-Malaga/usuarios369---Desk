using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using usuarios369.Datos;
using usuarios369.Logica;

namespace usuarios369.Presentacion
{
    public partial class Usuarios : Form
    {
        public Usuarios()
        {
            InitializeComponent();
        }
        int idusuario;

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            panelUsuario.Visible = true;
            panelUsuario.Dock = DockStyle.Fill;
            btnGuardarcambios.Visible = true;
            btnGuardarcambios.Visible = false;
            txtUsuario.Clear();
            txtPass.Clear();
        }

        private void Icono_Click(object sender, EventArgs e)
        {
            dlg.InitialDirectory = "";
            dlg.Filter = "Imagenes|*.jpg;*.png";
            dlg.FilterIndex = 2;
            dlg.Title = "Cargador de Imagenes";
            if (dlg.ShowDialog () == DialogResult.OK)
            {
                Icono.BackgroundImage = null;
                Icono.Image = new Bitmap(dlg.FileName);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text != "")
            {
                if(txtPass.Text != "")
                {
                    insertar_usuario();
                    mostrar_usuarios();
                }
                else
                {
                    MessageBox.Show("Ingrese una Contraseña", "Sin Contraseña", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Ingrese un Usuario", "Sin Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void insertar_usuario()
        {
            lusuarios dt = new lusuarios();
            dusuarios function = new dusuarios();
            dt.Usuario = txtUsuario.Text;
            dt.Pass = txtPass.Text;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Icono.Image.Save(ms, Icono.Image.RawFormat);
            dt.Icono = ms.GetBuffer();
            dt.Estado = "ACTIVO";
            if (function.insertar(dt))
            {
                MessageBox.Show("Usuario Registrado", "Registro Correcto");
                panelUsuario.Visible = false;
                panelUsuario.Dock = DockStyle.None;
            }
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            mostrar_usuarios();
        }
        private void mostrar_usuarios()
        {
            DataTable dt;
            dusuarios funcion = new dusuarios();
            dt = funcion.mostrar_usuarios();
            datalistado.DataSource = dt;
        }

        private void eliminar_usuarios()
        {
            lusuarios dt = new lusuarios();
            dusuarios funcion = new dusuarios();
            dt.Idusuario = idusuario;
            if (funcion.eliminar_usuarios(dt))
            {
                MessageBox.Show("Usuario Eliminado", "Eliminacion Correcta");
                panelUsuario.Visible = false;
                panelUsuario.Dock = DockStyle.None;
            }
        }

        private void datalistado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            idusuario = Convert.ToInt32(datalistado.SelectedCells[2].Value.ToString());
            if (e.ColumnIndex == this.datalistado.Columns["Eliminar"].Index)
            {
                DialogResult result;
                result = MessageBox.Show("¿Realmente desea eliminar este registro?", "Eliminando Registros", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    eliminar_usuarios();
                    mostrar_usuarios();
                }
            }

            if (e.ColumnIndex == this.datalistado.Columns["Editar"].Index)
            {
                
                txtUsuario.Text = datalistado.SelectedCells[3].Value.ToString();
                txtPass.Text = datalistado.SelectedCells[4].Value.ToString();
                Icono.BackgroundImage = null;
                byte[] b = (Byte[])datalistado.SelectedCells[5].Value;
                System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                Icono.Image = Image.FromStream(ms);

                panelUsuario.Visible = true;
                panelUsuario.Dock = DockStyle.Fill;
                btnGuardar.Visible = false;
                btnGuardarcambios.Visible = true;
            }
        }

        private void btnvolver_Click(object sender, EventArgs e)
        {
            panelUsuario.Visible = false;
            panelUsuario.Dock = DockStyle.None;
        }

        private void btnGuardarcambios_Click(object sender, EventArgs e)
        {
            editar_usuario();
            mostrar_usuarios();
        }

        private void editar_usuario()
        {
            lusuarios dt = new lusuarios();
            dusuarios function = new dusuarios();
            dt.Idusuario = idusuario;
            dt.Usuario = txtUsuario.Text;
            dt.Pass = txtPass.Text;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Icono.Image.Save(ms, Icono.Image.RawFormat);
            dt.Icono = ms.GetBuffer();
            dt.Estado = "ACTIVO";
            if (function.editar(dt))
            {
                MessageBox.Show("Usuario Modificado", "Registro Correcto");
                panelUsuario.Visible = false;
                panelUsuario.Dock = DockStyle.None;
            }
        }

        private void txtbuscador_TextChanged(object sender, EventArgs e)
        {
            buscar_usuarios();
        }
        private void buscar_usuarios()
        {
            DataTable dt;
            dusuarios funcion = new dusuarios();
            dt = funcion.buscar_usuarios(txtbuscador.Text);
            datalistado.DataSource = dt;
        }
    }
}
